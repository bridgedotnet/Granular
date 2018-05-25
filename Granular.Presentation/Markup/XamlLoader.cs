using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace System.Windows.Markup
{
    public static class XamlLoader
    {
        public static object Load(XamlElement resource)
        {
            IElementFactory factory = ElementFactory.FromXamlElement(resource, null);
            return factory.CreateElement(new InitializeContext(resource.Namespaces));
        }

        public static void Load(object target, XamlElement resource)
        {
            IElementInitializer initializer = new ElementInitializer(resource);

            initializer.InitializeElement(target, new InitializeContext(resource.Namespaces));
        }
    }
}
