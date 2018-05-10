using System.Reflection;

namespace System.Windows.Markup
{
    [MarkupExtensionParameter("Member")]
    [Bridge.Reflectable(Bridge.MemberAccessibility.PublicInstanceProperty)]
    public class StaticExtension : IMarkupExtension
    {
        private string _member;

        public string Member
        {
            get
            {
                return _member;
            }
            set
            {
                _member = value;
            }
        }

        public object ProvideValue(InitializeContext context)
        {
            if (_member == null)
                throw new Granular.Exception("MarkupExtensionStaticMember");

            int length = _member.IndexOf('.');
            if (length < 0)
                throw new Granular.Exception("MarkupExtensionBadStatic");

            string qualifiedTypeName = _member.Substring(0, length);
            if (qualifiedTypeName == string.Empty)
                throw new Granular.Exception("MarkupExtensionBadStatic");

            Type type = TypeParser.ParseType(qualifiedTypeName, context.XamlNamespaces);

            var name = _member.Substring(length + 1, _member.Length - length - 1);
            if (name == string.Empty)
                throw new Granular.Exception("MarkupExtensionBadStatic");

            if (type.IsEnum)
                return Enum.Parse(type, name);

            object obj;
            if (GetFieldOrPropertyValue(type, name, out obj))
                return obj;

            throw new Granular.Exception("MarkupExtensionBadStatic");
        }

        private bool GetFieldOrPropertyValue(Type type, string name, out object value)
        {
            Type type1 = type;
            do
            {
                FieldInfo field = type1.GetField(name, BindingFlags.Static | BindingFlags.Public);
                if (field != (FieldInfo)null)
                {
                    value = field.GetValue((object)null);
                    return true;
                }
                type1 = type1.BaseType;
            }
            while (type1 != (Type)null);
            Type type2 = type;

            do
            {
                PropertyInfo property = type2.GetProperty(name, BindingFlags.Static | BindingFlags.Public);
                if (property != (PropertyInfo)null)
                {
                    value = property.GetValue((object)null, (object[])null);
                    return true;
                }
                type2 = type2.BaseType;
            }
            while (type2 != (Type)null);
            value = (object)null;
            return false;
        }
    }
}