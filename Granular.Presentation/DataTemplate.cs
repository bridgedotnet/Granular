using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace System.Windows
{
    [DictionaryKeyProperty("Key")]
    [DeferredValueKeyProvider(typeof(DataTemplateKeyProvider))]
    [Bridge.Reflectable(Bridge.MemberAccessibility.PublicInstanceProperty)]
    public class DataTemplate : FrameworkTemplate
    {
        private Type dataType;
        public Type DataType
        {
            get { return dataType; }
            set
            {
                if (dataType == value)
                {
                    return;
                }

                dataType = value;
            }
        }

        private object key;
        public object Key
        {
            get { return key ?? new TemplateKey(DataType); }
            set { key = value; }
        }

        public DataTemplate()
        {
            //
        }
    }

    public class DataTemplateKeyProvider : IDeferredValueKeyProvider
    {
        public object GetValueKey(XamlElement element)
        {
            XamlMember keyMember = element.Members.SingleOrDefault(member => member.Name.LocalName == "Key");
            if (keyMember != null)
            {
                return ElementFactory.FromValue(keyMember.Values.Single(), typeof(object), element.Namespaces, element.SourceUri).CreateElement(new InitializeContext(element.Namespaces));
            }

            XamlMember dataTypeMember = element.Members.SingleOrDefault(member => member.Name.LocalName == "DataType");
            if (dataTypeMember != null)
            {
                return new TemplateKey((Type)ElementFactory.FromValue(dataTypeMember.Values.Single(), typeof(Type), element.Namespaces, element.SourceUri).CreateElement(new InitializeContext(element.Namespaces)));
            }

            throw new Granular.Exception($"Can't create value key from \"{element.Name}\"");
        }
    }
}
