using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
    public enum ClickMode
    {
        Release,
        Press,
        //Hover,
    }

    [TemplateVisualState(VisualStates.CommonStates, VisualStates.NormalState)]
    [TemplateVisualState(VisualStates.CommonStates, VisualStates.MouseOverState)]
    [TemplateVisualState(VisualStates.CommonStates, VisualStates.PressedState)]
    [TemplateVisualState(VisualStates.CommonStates, VisualStates.DisabledState)]
    [TemplateVisualState(VisualStates.FocusStates, VisualStates.FocusedState)]
    [TemplateVisualState(VisualStates.FocusStates, VisualStates.UnfocusedState)]
    public class ButtonBase : ContentControl
    {
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ButtonBase));
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonBase), new FrameworkPropertyMetadata());
        public ClickMode ClickMode
        {
            get { return (ClickMode)GetValue(ClickModeProperty); }
            set { SetValue(ClickModeProperty, value); }
        }

        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(ButtonBase), new ControlPropertyMetadata(false, (sender, e) => ((ButtonBase)sender).OnIsPressedChanged(e), affectsVisualState: true));
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonBase), 
            new FrameworkPropertyMetadata(null, (sender, e) => ((ButtonBase)sender).OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue)));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameterProperty", typeof(object), typeof(ButtonBase), new FrameworkPropertyMetadata());
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        private IDisposable keyboardFocus;

        static ButtonBase()
        {
            EventManager.RegisterClassHandler(typeof(ButtonBase), ClickEvent, (RoutedEventHandler)((sender, e) => ((ButtonBase)sender).OnClick(e)), false);

            UIElement.IsEnabledProperty.OverrideMetadata(typeof(ButtonBase), new ControlPropertyMetadata(inherits: true, affectsVisualState: true));
            UIElement.IsMouseOverProperty.OverrideMetadata(typeof(ButtonBase), new ControlPropertyMetadata(affectsVisualState: true));
            UIElement.IsFocusedProperty.OverrideMetadata(typeof(ButtonBase), new ControlPropertyMetadata(affectsVisualState: true));
        }

        public ButtonBase()
        {
            //
        }

        protected bool RaiseClick()
        {
            RoutedEventArgs e = new RoutedEventArgs(ClickEvent, this);
            RaiseEvent(e);
            return e.Handled;
        }

        protected virtual void OnClick(RoutedEventArgs e)
        {
            var command = Command;
            if (command != null)
            {
                command.Execute(CommandParameter);
            }
        }

        protected virtual void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            //
        }

        protected virtual void OnPressStarted()
        {
            //
        }

        protected virtual void OnPressEnded()
        {
            //
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Focus();

                IsPressed = true;
                OnPressStarted();

                e.MouseDevice.Capture(this);

                if (ClickMode == ClickMode.Press)
                {
                    e.Handled = RaiseClick();
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.MouseDevice.CaptureTarget == this)
            {
                IsPressed = false;
                OnPressEnded();

                e.MouseDevice.ReleaseCapture();

                if (IsMouseOver && ClickMode == ClickMode.Release)
                {
                    e.Handled = RaiseClick();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.MouseDevice.CaptureTarget == this)
            {
                IsPressed = e.MouseDevice.CaptureTarget == this && IsVisualChild(this, e.MouseDevice.HitTarget as Visual);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space && !IsPressed)
            {
                IsPressed = true;
                OnPressStarted();

                if (ClickMode == ClickMode.Press)
                {
                    e.Handled = RaiseClick();
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                IsPressed = false;
                OnPressEnded();

                if (ClickMode == ClickMode.Release)
                {
                    e.Handled = RaiseClick();
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            keyboardFocus = Keyboard.Focus(this);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (keyboardFocus != null)
            {
                keyboardFocus.Dispose();
                keyboardFocus = null;
            }
        }

        protected override void UpdateVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, GetCommonState(), useTransitions);
            VisualStateManager.GoToState(this, GetFocusState(), useTransitions);
        }

        private string GetCommonState()
        {
            if (!IsEnabled)
            {
                return VisualStates.DisabledState;
            }

            if (IsPressed)
            {
                return VisualStates.PressedState;
            }

            if (IsMouseOver)
            {
                return VisualStates.MouseOverState;
            }

            return VisualStates.NormalState;
        }

        private string GetFocusState()
        {
            return IsFocused ? VisualStates.FocusedState : VisualStates.UnfocusedState;
        }

        private static bool IsVisualChild(Visual parent, Visual child)
        {
            return child != null && (parent == child || IsVisualChild(parent, child.VisualParent));
        }

        private void OnCommandChanged(ICommand oldValue, ICommand newValue)
        {
            if (oldValue != null)
            {
                oldValue.CanExecuteChanged += UpdateCommandStatus;
            }
            if (newValue != null)
            {
                newValue.CanExecuteChanged += UpdateCommandStatus;
                UpdateCommandStatus(this, EventArgs.Empty);
            }
        }
        private void UpdateCommandStatus(object sender, EventArgs e)
        {
            IsEnabled = Command.CanExecute(CommandParameter);
        }
    }
}
