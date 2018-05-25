using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;
using Granular.Collections;
using Granular.Extensions;
using System.Windows.Input;

namespace System.Windows
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right,
        Stretch
    }

    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom,
        Stretch
    }

    [RuntimeNameProperty("Name")]
    [Bridge.Reflectable(Bridge.MemberAccessibility.PublicInstanceProperty)]
    public class FrameworkElement : UIElement, IResourceContainer
    {
        public event EventHandler<ResourcesChangedEventArgs> ResourcesChanged;
        public event DependencyPropertyChangedEventHandler DataContextChanged;

        public static readonly RoutedEvent InitializedEvent = EventManager.RegisterRoutedEvent("Initialized", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(FrameworkElement));
        public event RoutedEventHandler Initialized
        {
            add { AddHandler(InitializedEvent, value); }
            remove { RemoveHandler(InitializedEvent, value); }
        }

        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register("HorizontalAlignment", typeof(HorizontalAlignment), typeof(FrameworkElement), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).OnAlignmentChanged(e)));
        public HorizontalAlignment HorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", typeof(VerticalAlignment), typeof(FrameworkElement), new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).OnAlignmentChanged(e)));
        public VerticalAlignment VerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(FrameworkElement), new FrameworkPropertyMetadata(Thickness.Zero, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata(Double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).SetSize()));
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata(Double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).SetSize()));
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).SetMinSize()));
        public double MinWidth
        {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).SetMinSize()));
        public double MinHeight
        {
            get { return (double)GetValue(MinHeightProperty); }
            set { SetValue(MinHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata(Double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).SetMaxSize()));
        public double MaxWidth
        {
            get { return (double)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }

        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata(Double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).SetMaxSize()));
        public double MaxHeight
        {
            get { return (double)GetValue(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }

        private static readonly DependencyPropertyKey ActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("ActualWidth", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ActualWidthProperty = ActualWidthPropertyKey.DependencyProperty;
        private IDependencyPropertyValueEntry actualWidthValueEntry;
        public double ActualWidth
        {
            get { return (double)GetValue(ActualWidthPropertyKey); }
            private set { actualWidthValueEntry.SetBaseValue((int)BaseValueSource.Local, value); }
        }

        private static readonly DependencyPropertyKey ActualHeightPropertyKey = DependencyProperty.RegisterReadOnly("ActualHeight", typeof(double), typeof(FrameworkElement), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ActualHeightProperty = ActualHeightPropertyKey.DependencyProperty;
        private IDependencyPropertyValueEntry actualHeightValueEntry;
        public double ActualHeight
        {
            get { return (double)GetValue(ActualHeightPropertyKey); }
            private set { actualHeightValueEntry.SetBaseValue((int)BaseValueSource.Local, value); }
        }

        public Size ActualSize { get; private set; }

        private Size _size;
        public Size Size
        {
            get { return _size; }
            private set
            {
                if (_size != value)
                {
                    _size = value;
                    OnSizeChanged();
                }
            }
        }
        public Size MinSize { get; private set; }
        public Size MaxSize { get; private set; }

        public bool IsInitialized { get; private set; }

        public FrameworkElement TemplatedParent { get; internal set; }

        private UIElement templateChild;
        public UIElement TemplateChild
        {
            get { return templateChild; }
            set
            {
                if (templateChild == value)
                {
                    return;
                }

                if (templateChild != null)
                {
                    RemoveVisualChild(templateChild);
                }

                templateChild = value;

                if (templateChild != null)
                {
                    AddVisualChild(templateChild);
                }

                InvalidateMeasure();

                OnTemplateChildChanged();
            }
        }

        public static readonly DependencyProperty StyleProperty = DependencyProperty.Register("Style", typeof(Style), typeof(FrameworkElement), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure, propertyChangedCallback: (sender, e) => (sender as FrameworkElement).OnStyleChanged(e)));
        public Style Style
        {
            get { return (Style)GetValue(StyleProperty); }
            set { SetValue(StyleProperty, value); }
        }

        public static readonly DependencyProperty DefaultStyleKeyProperty = DependencyProperty.Register("DefaultStyleKey", typeof(object), typeof(FrameworkElement), new FrameworkPropertyMetadata());
        public object DefaultStyleKey
        {
            get { return (object)GetValue(DefaultStyleKeyProperty); }
            set { SetValue(DefaultStyleKeyProperty, value); }
        }

        public static readonly DependencyProperty FocusVisualStyleProperty = DependencyProperty.Register("FocusVisualStyle", typeof(Style), typeof(FrameworkElement), new FrameworkPropertyMetadata());
        public Style FocusVisualStyle
        {
            get { return (Style)GetValue(FocusVisualStyleProperty); }
            set { SetValue(FocusVisualStyleProperty, value); }
        }

        private ResourceDictionary resources;
        public ResourceDictionary Resources
        {
            get { return resources; }
            set
            {
                if (resources == value)
                {
                    return;
                }

                if (resources != null)
                {
                    resources.ResourcesChanged -= OnResourceDictionaryChanged;
                }

                resources = value;

                if (resources != null)
                {
                    resources.ResourcesChanged += OnResourceDictionaryChanged;
                }

                RaiseResourcesChanged(ResourcesChangedEventArgs.Reset);
            }
        }

        private IResourceContainer resourceInheritanceParent;
        private IResourceContainer ResourceInheritanceParent
        {
            get { return resourceInheritanceParent; }
            set
            {
                if (resourceInheritanceParent == value)
                {
                    return;
                }

                IResourceContainer oldResourceInheritanceParent = resourceInheritanceParent;

                if (resourceInheritanceParent != null)
                {
                    resourceInheritanceParent.ResourcesChanged -= OnParentResourcesChanged;
                }

                resourceInheritanceParent = value;

                if (resourceInheritanceParent != null)
                {
                    resourceInheritanceParent.ResourcesChanged += OnParentResourcesChanged;
                }

                if (oldResourceInheritanceParent != null && !oldResourceInheritanceParent.IsEmpty || resourceInheritanceParent != null && !resourceInheritanceParent.IsEmpty)
                {
                    RaiseResourcesChanged(ResourcesChangedEventArgs.Reset);
                }
            }
        }

        bool IResourceContainer.IsEmpty { get { return (ResourceInheritanceParent == null || ResourceInheritanceParent.IsEmpty) && (Resources == null || Resources.IsEmpty); } }

        public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), typeof(FrameworkElement), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).OnDataContextChanged(e)));
        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        public static readonly DependencyProperty CursorProperty = DependencyProperty.Register("Cursor", typeof(Cursor), typeof(FrameworkElement), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).UpdateCursor()));
        public Cursor Cursor
        {
            get { return (Cursor)GetValue(CursorProperty); }
            set { SetValue(CursorProperty, value); }
        }

        public static readonly DependencyProperty ForceCursorProperty = DependencyProperty.Register("ForceCursor", typeof(bool), typeof(FrameworkElement), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).UpdateCursor()));
        public bool ForceCursor
        {
            get { return (bool)GetValue(ForceCursorProperty); }
            set { SetValue(ForceCursorProperty, value); }
        }

        public static readonly DependencyProperty LayoutTransformProperty = DependencyProperty.Register("LayoutTransform", typeof(Transform), typeof(FrameworkElement), new FrameworkPropertyMetadata(Transform.Identity, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, propertyChangedCallback: (sender, e) => ((FrameworkElement)sender).OnLayoutTransformChanged(e)));
        public Transform LayoutTransform
        {
            get { return (Transform)GetValue(LayoutTransformProperty); }
            set { SetValue(LayoutTransformProperty, value); }
        }

        public ObservableCollection<ITrigger> Triggers { get; private set; }

        public string Name { get; set; }

        private IFrameworkTemplate appliedTemplate;
        private Matrix layoutTransformValue;
        private bool isDefaultAlignment;

        public FrameworkElement()
        {
            Triggers = new ObservableCollection<ITrigger>();
            Triggers.CollectionChanged += OnTriggersCollectionChanged;

            actualWidthValueEntry = GetValueEntry(ActualWidthPropertyKey);
            actualHeightValueEntry = GetValueEntry(ActualHeightPropertyKey);

            ActualSize = Size.Empty;
            _size = Size.Empty;
            MinSize = Size.Zero;
            MaxSize = Size.Infinity;

            isDefaultAlignment = true;
        }

        public override string ToString()
        {
            return Name.IsNullOrEmpty() ?
                String.Format("{0}", GetType().Name) :
                String.Format("{0} ({1})", GetType().Name, Name);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            FrameworkPropertyMetadata metadata = e.Property.GetMetadata(GetType()) as FrameworkPropertyMetadata;

            if (metadata != null)
            {
                if (metadata.AffectsMeasure)
                {
                    InvalidateMeasure();
                }

                if (metadata.AffectsArrange)
                {
                    InvalidateArrange();
                }

                if (metadata.AffectsRender)
                {
                    InvalidateVisual();
                }

                if (metadata.AffectsParentMeasure && VisualParent != null)
                {
                    ((UIElement)VisualParent).InvalidateMeasure();
                }

                if (metadata.AffectsParentArrange && VisualParent != null)
                {
                    ((UIElement)VisualParent).InvalidateArrange();
                }
            }

            if (!e.IsSubPropertyChange)
            {
                BaseValueSource baseValueSource = GetBaseValueSource(e.Property);
                if (baseValueSource != BaseValueSource.Default && baseValueSource != BaseValueSource.Inherited)
                {
                    if (e.OldValue is IContextElement)
                    {
                        ((IContextElement)e.OldValue).TrySetContextParent(null);
                    }

                    if (e.NewValue is IContextElement)
                    {
                        ((IContextElement)e.NewValue).TrySetContextParent(this);
                    }
                }
            }
        }

        protected sealed override Size MeasureCore(Size availableSize)
        {
            availableSize -= Margin.Size;

            if (!layoutTransformValue.IsNullOrIdentity())
            {
                availableSize = layoutTransformValue.GetContainedSize(availableSize);
            }

            availableSize = Size.Combine(availableSize).Bounds(MinSize, MaxSize);

            Size measuredSize = MeasureOverride(availableSize);

            measuredSize = Size.Combine(measuredSize).Bounds(MinSize, MaxSize);

            if (!layoutTransformValue.IsNullOrIdentity())
            {
                measuredSize = layoutTransformValue.GetContainingSize(measuredSize);
            }

            measuredSize += Margin.Size;

            return measuredSize;
        }

        protected virtual Size MeasureOverride(Size availableSize)
        {
            return Size.Zero;
        }

        protected sealed override void ArrangeCore(Rect finalRect)
        {
            Size finalSize = isDefaultAlignment ? finalRect.Size : new Size(
                HorizontalAlignment != HorizontalAlignment.Stretch ? DesiredSize.Width.Min(finalRect.Width) : finalRect.Width,
                VerticalAlignment != VerticalAlignment.Stretch ? DesiredSize.Height.Min(finalRect.Height) : finalRect.Height);

            finalSize -= Margin.Size;

            if (!layoutTransformValue.IsNullOrIdentity())
            {
                finalSize = layoutTransformValue.GetContainedSize(finalSize);
            }

            finalSize = Size.Combine(finalSize).Bounds(MinSize, MaxSize);

            Size arrangedSize = ArrangeOverride(finalSize);

            Rect containingRect = new Rect(arrangedSize);

            if (!layoutTransformValue.IsNullOrIdentity())
            {
                containingRect = layoutTransformValue.GetContainingRect(containingRect);
            }

            containingRect = containingRect.AddMargin(Margin);

            Point alignedOffset = GetAlignmentOffset(finalRect, containingRect.Size, HorizontalAlignment, VerticalAlignment);

            Point visualOffset = alignedOffset - containingRect.Location;

            VisualBounds = new Rect(visualOffset, arrangedSize);

            ActualWidth = arrangedSize.Width;
            ActualHeight = arrangedSize.Height;
            ActualSize = arrangedSize;
        }

        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        protected virtual IFrameworkTemplate GetTemplate()
        {
            return null;
        }

        public bool ApplyTemplate()
        {
            IFrameworkTemplate template = GetTemplate();

            if (appliedTemplate == template)
            {
                return false;
            }

            if (appliedTemplate != null)
            {
                appliedTemplate.Detach(this);
            }

            appliedTemplate = template;

            if (appliedTemplate != null)
            {
                appliedTemplate.Attach(this);
            }

            OnApplyTemplate();

            return true;
        }

        /// <summary>
        /// This virtual is called by FE.ApplyTemplate before it does work to generate the template tree.
        /// </summary>
        /// <remarks>
        /// This virtual is overridden for the following three reasons
        /// 1. By ContentPresenter/ItemsPresenter to choose the template to be applied in this case.
        /// 2. By RowPresenter/ColumnHeaderPresenter/InkCanvas to build custom visual trees
        /// 3. By ScrollViewer/TickBar/ToolBarPanel/Track to hookup bindings to their TemplateParent
        /// </remarks>
        internal virtual void OnPreApplyTemplate()
        {
        }

        protected virtual void OnApplyTemplate()
        {
            // Notify the ContentPresenter/ItemsPresenter that we are about to generate the
            // template tree and allow them to choose the right template to be applied.
            OnPreApplyTemplate();
        }

        protected override void OnVisualParentChanged(Visual oldVisualParent, Visual newVisualParent)
        {
            base.OnVisualParentChanged(oldVisualParent, newVisualParent);

            Initialize();
            ResourceInheritanceParent = (LogicalParent ?? VisualParent) as FrameworkElement;
        }

        protected override void OnLogicalParentChanged(UIElement oldLogicalParent, UIElement newLogicalParent)
        {
            ResourceInheritanceParent = (LogicalParent ?? VisualParent) as FrameworkElement;
        }

        private void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            IsInitialized = true;

            OnInitialized(EventArgs.Empty);
            RaiseEvent(new RoutedEventArgs(InitializedEvent, this));
        }

        protected virtual void OnInitialized(EventArgs e)
        {
            //
        }

        private void OnStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                (e.OldValue as Style).Detach(this);
            }

            if (e.NewValue != null)
            {
                (e.NewValue as Style).Attach(this);
            }
        }

        public bool TryGetResource(object resourceKey, out object value)
        {
            if ((Resources == null || !Resources.TryGetValue(resourceKey, out value)) &&
                (ResourceInheritanceParent == null || !ResourceInheritanceParent.TryGetResource(resourceKey, out value)))
            {
                value = null;
                return false;
            }

            return true;
        }

        private void OnParentResourcesChanged(object sender, ResourcesChangedEventArgs e)
        {
            RaiseResourcesChanged(e);
        }

        private void OnResourceDictionaryChanged(object sender, ResourcesChangedEventArgs e)
        {
            RaiseResourcesChanged(e);
        }

        private void RaiseResourcesChanged(ResourcesChangedEventArgs e)
        {
            OnResourcesChanged(e);
            ResourcesChanged.Raise(this, e);
        }

        protected virtual void OnResourcesChanged(ResourcesChangedEventArgs e)
        {
            if (DefaultStyleKey == null || !e.Contains(DefaultStyleKey))
            {
                return;
            }

            object value;
            if (!TryGetResource(DefaultStyleKey, out value))
            {
                value = null;
            }

            SetValue(StyleProperty, value, BaseValueSource.Default);
        }

        protected void SetResourceInheritanceParent(IResourceContainer parent)
        {
            this.ResourceInheritanceParent = parent;
        }

        protected virtual void OnTemplateChildChanged()
        {
            //
        }

        private void OnTriggersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ITrigger trigger in e.OldItems)
            {
                trigger.Detach(this, BaseValueSource.Local);

                if (trigger is IContextElement)
                {
                    ((IContextElement)trigger).TrySetContextParent(null);
                }
            }

            foreach (ITrigger trigger in e.NewItems)
            {
                if (trigger is IContextElement)
                {
                    ((IContextElement)trigger).TrySetContextParent(this);
                }

                trigger.Attach(this, BaseValueSource.Local);
            }
        }

        private void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
        {
            DataContextChanged.Raise(this, e);
        }

        private void UpdateCursor()
        {
            if (IsMouseOver)
            {
                ApplicationHost.Current.GetMouseDeviceFromElement(this).UpdateCursor();
            }
        }

        protected override void OnQueryCursor(QueryCursorEventArgs e)
        {
            if (Cursor != null && (ForceCursor || !e.Handled))
            {
                e.Cursor = Cursor;
                e.Handled = true;
            }
        }

        private void OnLayoutTransformChanged(DependencyPropertyChangedEventArgs e)
        {
            Transform newLayoutTransform = (Transform)e.NewValue;
            layoutTransformValue = newLayoutTransform.IsNullOrIdentity() ? null : newLayoutTransform.Value;
            InvalidateVisualTransform();
        }

        private void OnAlignmentChanged(DependencyPropertyChangedEventArgs e)
        {
            isDefaultAlignment = HorizontalAlignment == HorizontalAlignment.Stretch && VerticalAlignment == VerticalAlignment.Stretch;
        }

        protected override Matrix GetVisualTransformOverride()
        {
            if (layoutTransformValue.IsNullOrIdentity())
            {
                return base.GetVisualTransformOverride();
            }

            return layoutTransformValue * base.GetVisualTransformOverride();
        }

        private void SetSize()
        {
            Size = new Size(Width, Height);
        }

        protected virtual void OnSizeChanged()
        {

        }

        private void SetMinSize()
        {
            MinSize = new Size(MinWidth, MinHeight);
        }

        private void SetMaxSize()
        {
            MaxSize = new Size(MaxWidth, MaxHeight);
        }

        private static Point GetAlignmentOffset(Rect container, Size alignedRectSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            double alignedLeft = container.Left;
            double alignedTop = container.Top;

            if (horizontalAlignment == HorizontalAlignment.Right)
            {
                alignedLeft = container.Left + container.Width - alignedRectSize.Width;
            }

            if (horizontalAlignment == HorizontalAlignment.Center || horizontalAlignment == HorizontalAlignment.Stretch)
            {
                alignedLeft = container.Left + (container.Width - alignedRectSize.Width) / 2;
            }

            if (verticalAlignment == VerticalAlignment.Bottom)
            {
                alignedTop = container.Top + container.Height - alignedRectSize.Height;
            }

            if (verticalAlignment == VerticalAlignment.Center || verticalAlignment == VerticalAlignment.Stretch)
            {
                alignedTop = container.Top + (container.Height - alignedRectSize.Height) / 2;
            }

            return alignedLeft == 0 && alignedTop == 0 ? Point.Zero : new Point(alignedLeft, alignedTop);
        }
    }
}
