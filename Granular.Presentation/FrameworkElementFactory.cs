using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace System.Windows
{
    public interface IFrameworkElementFactory
    {
        FrameworkElement CreateElement(FrameworkElement templatedParent);
    }

    public class FrameworkElementFactory : IFrameworkElementFactory
    {
        private IElementFactory elementFactory;
        private InitializeContext context;

        public FrameworkElementFactory(IElementFactory elementFactory, InitializeContext context)
        {
            this.elementFactory = elementFactory;
            this.context = context;
        }

        public FrameworkElement CreateElement(FrameworkElement templatedParent)
        {
            InitializeContext elementContext = new InitializeContext(null, context, new NameScope(context.NameScope), templatedParent, BaseValueSource.ParentTemplate, context.XamlNamespaces);

            FrameworkElement element = elementFactory.CreateElement(elementContext) as FrameworkElement;

            NameScope.SetNameScope(element, elementContext.NameScope);

            return element;
        }
    }
}
