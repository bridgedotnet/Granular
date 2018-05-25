using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
    public class ItemsPresenter : FrameworkElement
    {
        public static readonly DependencyProperty ItemContainerGeneratorProperty = DependencyProperty.Register("ItemContainerGenerator", typeof(IItemContainerGenerator), typeof(ItemsPresenter), new FrameworkPropertyMetadata(null, (sender, e) => ((ItemsPresenter)sender).OnItemContainerGeneratorChanged(e)));
        public IItemContainerGenerator ItemContainerGenerator
        {
            get { return (IItemContainerGenerator)GetValue(ItemContainerGeneratorProperty); }
            set { SetValue(ItemContainerGeneratorProperty, value); }
        }

        public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(IFrameworkTemplate), typeof(ItemsPresenter), new FrameworkPropertyMetadata(null, (sender, e) => ((ItemsPresenter)sender).OnTemplateChanged(e)));
        public IFrameworkTemplate Template
        {
            get { return (IFrameworkTemplate)GetValue(TemplateProperty); }
            set { SetValue(TemplateProperty, value); }
        }

        private Panel panel;
        public Panel Panel
        {
            get { return panel; }
            private set
            {
                if (panel == value)
                {
                    return;
                }

                if (panel != null)
                {
                    panel.ItemContainerGenerator = null;
                }

                panel = value;

                if (panel != null)
                {
                    panel.ItemContainerGenerator = ItemContainerGenerator;
                }
            }
        }

        internal ItemsControl Owner
        {
            get
            {
                ItemsControl owner = TemplatedParent as ItemsControl;
                return owner;
            }
        }

        public ItemsPresenter()
        {
            //
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Panel == null)
            {
                return Size.Zero;
            }

            Panel.Measure(availableSize);
            return Panel.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Panel != null)
            {
                Panel.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        protected override IFrameworkTemplate GetTemplate()
        {
            return Template;
        }

        private void OnItemContainerGeneratorChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Panel != null)
            {
                Panel.ItemContainerGenerator = ItemContainerGenerator;
            }
        }

        private void OnTemplateChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ItemContainerGenerator != null)
            {
                ItemContainerGenerator.RemoveRange(0, ItemContainerGenerator.ItemsCount);
            }

            ApplyTemplate();

            Panel = VisualChildren.FirstOrDefault() as Panel;
        }

        internal static ItemsPresenter FromPanel(Panel panel)
        {
            if (panel == null)
                return null;

            return panel.TemplatedParent as ItemsPresenter;
        }
    }
}
