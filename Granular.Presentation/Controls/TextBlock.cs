using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Documents;

namespace System.Windows.Controls
{
    [ContentProperty("Text")]
    public class TextBlock : FrameworkElement
    {
        static TextBlock()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(new StyleKey(typeof(TextBlock))));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnTextChanged(e)));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnForegroundChanged(e)));
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnFontFamilyChanged(e)));
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnFontSizeChanged(e)));
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnFontStyleChanged(e)));
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnFontWeightChanged(e)));
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnFontStretchChanged(e)));
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnTextAlignmentChanged(e)));
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(TextBlock), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnTextTrimmingChanged(e)));
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBlock), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback: (sender, e) => ((TextBlock)sender).OnTextWrappingChanged(e)));
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        private ITextBlockRenderElement textBlockRenderElement;
        private Size noWrapSize;

        public TextBlock()
        {
            //
        }

        protected override object CreateRenderElementContentOverride(IRenderElementFactory factory)
        {
            if (textBlockRenderElement == null)
            {
                textBlockRenderElement = factory.CreateTextBlockRenderElement(this);

                textBlockRenderElement.Bounds = new Rect(VisualBounds.Size);
                textBlockRenderElement.FontFamily = this.FontFamily;
                textBlockRenderElement.Foreground = this.Foreground;
                textBlockRenderElement.FontSize = this.FontSize;
                textBlockRenderElement.FontStyle = this.FontStyle;
                textBlockRenderElement.FontStretch = this.FontStretch;
                textBlockRenderElement.FontWeight = this.FontWeight;
                textBlockRenderElement.Text = this.Text;
                textBlockRenderElement.TextAlignment = this.TextAlignment;
                textBlockRenderElement.TextTrimming = this.TextTrimming;
                textBlockRenderElement.TextWrapping = this.TextWrapping;
            }

            return textBlockRenderElement;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (noWrapSize == null)
            {
                noWrapSize = ApplicationHost.Current.TextMeasurementService.Measure(Text ?? String.Empty, FontSize, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), Double.PositiveInfinity);
            }

            if (TextWrapping == TextWrapping.NoWrap || noWrapSize.Width <= availableSize.Width)
            {
                return noWrapSize;
            }

            return ApplicationHost.Current.TextMeasurementService.Measure(Text ?? String.Empty, FontSize, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), availableSize.Width);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.Bounds = new Rect(finalSize);
            }

            return finalSize;
        }

        private void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.Text = Text;
            }

            noWrapSize = null;
        }

        private void OnFontFamilyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.FontFamily = FontFamily;
            }

            noWrapSize = null;
        }

        private void OnFontSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.FontSize = FontSize;
            }

            noWrapSize = null;
        }

        private void OnFontStyleChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.FontStyle = FontStyle;
            }

            noWrapSize = null;
        }

        private void OnFontWeightChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.FontWeight = FontWeight;
            }

            noWrapSize = null;
        }

        private void OnFontStretchChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.FontStretch = FontStretch;
            }

            noWrapSize = null;
        }

        private void OnForegroundChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.Foreground = (Brush)e.NewValue;
            }
        }

        private void OnTextAlignmentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.TextAlignment = (TextAlignment)e.NewValue;
            }
        }

        private void OnTextTrimmingChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.TextTrimming = (TextTrimming)e.NewValue;
            }
        }

        private void OnTextWrappingChanged(DependencyPropertyChangedEventArgs e)
        {
            if (textBlockRenderElement != null)
            {
                textBlockRenderElement.TextWrapping = (TextWrapping)e.NewValue;
            }
        }
    }
}
