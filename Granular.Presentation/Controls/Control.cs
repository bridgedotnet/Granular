using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;

namespace System.Windows.Controls
{
    [Bridge.Reflectable(Bridge.MemberAccessibility.PublicInstanceProperty)]
    public class Control : FrameworkElement
    {
        public static readonly RoutedEvent PreviewMouseDoubleClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseDoubleClick", RoutingStrategy.Tunnel, typeof(MouseButtonEventHandler), typeof(Control));
        public event MouseButtonEventHandler PreviewMouseDoubleClick
        {
            add { AddHandler(PreviewMouseDoubleClickEvent, value); }
            remove { RemoveHandler(PreviewMouseDoubleClickEvent, value); }
        }

        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(Control));
        public event MouseButtonEventHandler MouseDoubleClick
        {
            add { AddHandler(MouseDoubleClickEvent, value); }
            remove { RemoveHandler(MouseDoubleClickEvent, value); }
        }

        public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(Control), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((Control)sender).ApplyTemplate()));
        public ControlTemplate Template
        {
            get { return (ControlTemplate)GetValue(TemplateProperty); }
            set { SetValue(TemplateProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Control));
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(Control));
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(Control));
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty = Border.BorderBrushProperty.AddOwner(typeof(Control));
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty = Border.BorderThicknessProperty.AddOwner(typeof(Control));
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(Control), new FrameworkPropertyMetadata(HorizontalAlignment.Left));
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(Control), new FrameworkPropertyMetadata(VerticalAlignment.Top));
        public VerticalAlignment VerticalContentAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty = Border.PaddingProperty.AddOwner(typeof(Control));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty IsTabStopProperty = KeyboardNavigation.IsTabStopProperty.AddOwner(typeof(Control));
        public bool IsTabStop
        {
            get { return (bool)GetValue(IsTabStopProperty); }
            set { SetValue(IsTabStopProperty, value); }
        }

        public static readonly DependencyProperty TabIndexProperty = KeyboardNavigation.TabIndexProperty.AddOwner(typeof(Control));
        public int TabIndex
        {
            get { return (int)GetValue(TabIndexProperty); }
            set { SetValue(TabIndexProperty, value); }
        }

        static Control()
        {
            UIElement.FocusableProperty.OverrideMetadata(typeof(Control), new FrameworkPropertyMetadata(true));

            EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewMouseDownEvent, (MouseButtonEventHandler)OnPreviewMouseDown, true);
            EventManager.RegisterClassHandler(typeof(Control), UIElement.MouseDownEvent, (MouseButtonEventHandler)OnMouseDown, true);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            FrameworkPropertyMetadata metadata = e.Property.GetMetadata(GetType()) as FrameworkPropertyMetadata;

            if (metadata != null && metadata.AffectsVisualState)
            {
                UpdateVisualState(true);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TemplateChild == null)
            {
                return Size.Zero;
            }

            TemplateChild.Measure(availableSize);
            return TemplateChild.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (TemplateChild != null)
            {
                TemplateChild.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            //
        }

        protected override void OnApplyTemplate()
        {
            UpdateVisualState(false);
        }

        protected override IFrameworkTemplate GetTemplate()
        {
            return Template;
        }

        private bool RaiseMouseButtonEvent(MouseButtonEventArgs e, RoutedEvent routedEvent)
        {
            MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(routedEvent, e.OriginalSource, e.MouseDevice, e.Timestamp, e.AbsolutePosition, e.ChangedButton, e.ButtonState, e.ClickCount);
            RaiseEvent(eventArgs);
            return eventArgs.Handled;
        }

        private static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && ((Control)sender).RaiseMouseButtonEvent(e, PreviewMouseDoubleClickEvent))
            {
                e.Handled = true;
            }
        }

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && ((Control)sender).RaiseMouseButtonEvent(e, MouseDoubleClickEvent))
            {
                e.Handled = true;
            }
        }
    }
}
