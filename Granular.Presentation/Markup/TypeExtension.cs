using System.Reflection;

namespace System.Windows.Markup
{
    [MarkupExtensionParameter("TypeName")]
    [Bridge.Reflectable(Bridge.MemberAccessibility.PublicInstanceProperty)]
    public class TypeExtension : IMarkupExtension
    {
        private string _typeName;

        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }

        public object ProvideValue(InitializeContext context)
        {
            Console.WriteLine(_typeName);

            if (_typeName == null)
                throw new Granular.Exception("Type name must be specified");

            Type type = TypeParser.ParseType(_typeName, context.XamlNamespaces);
            if (type == null)
                throw new Granular.Exception($"Unknown type \"{_typeName}\"");

            return type;
        }
    }
}