using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Markup
{
    public class InitializeContext
    {
        public object Target { get; private set; }
        public InitializeContext ParentContext { get; private set; }

        public BaseValueSource ValueSource { get; private set; }
        public INameScope NameScope { get; private set; }
        public FrameworkElement TemplatedParent { get; private set; }

        public XamlNamespaces XamlNamespaces { get; private set; }

        public object Root { get; private set; }

        public InitializeContext(XamlNamespaces xamlNamespaces) :
            this(null, null, new NameScope(), null, BaseValueSource.Local, xamlNamespaces)
        {
            //
        }

        public InitializeContext(object target, InitializeContext parentContext, INameScope nameScope, FrameworkElement templatedParent, BaseValueSource valueSource, XamlNamespaces xamlNamespaces)
        {
            this.Target = target;
            this.ParentContext = parentContext;

            this.NameScope = nameScope;
            this.TemplatedParent = templatedParent;
            this.ValueSource = valueSource;
            this.XamlNamespaces = xamlNamespaces;

            this.Root = parentContext != null && parentContext.Root != null ? parentContext.Root : Target;
        }
    }

    public static class InitializeContextExtensions
    {
        public static InitializeContext SetTarget(this InitializeContext context, object target)
        {
            return new InitializeContext(target, context.ParentContext, context.NameScope, context.TemplatedParent, context.ValueSource, context.XamlNamespaces);
        }

        public static InitializeContext SetNameScope(this InitializeContext context, INameScope nameScope)
        {
            return new InitializeContext(context.Target, context.ParentContext, nameScope, context.TemplatedParent, context.ValueSource, context.XamlNamespaces);
        }

        public static InitializeContext CreateChildContext(this InitializeContext context, object child)
        {
            return new InitializeContext(child, context, context.NameScope, context.TemplatedParent, context.ValueSource, context.XamlNamespaces);
        }
    }
}
