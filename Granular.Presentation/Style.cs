using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using Granular.Collections;

namespace System.Windows
{
    [ContentProperty("Setters")]
    [DictionaryKeyProperty("Key")]
    [DeferredValueKeyProvider(typeof(StyleKeyProvider))]
    [Bridge.Reflectable(Bridge.MemberAccessibility.PublicInstanceProperty)]
    public class Style
    {
        private Type targetType;
        public Type TargetType
        {
            get { return targetType; }
            set
            {
                if (targetType == value)
                {
                    return;
                }

                targetType = value;
            }
        }

        private object key;
        public object Key
        {
            get { return key ?? new StyleKey(TargetType); }
            set { key = value; }
        }

        public Style BasedOn { get; set; }
        public ObservableCollection<ITrigger> Triggers { get; private set; }
        public ObservableCollection<ITriggerAction> Setters { get; private set; }
        //public ResourceDictionary Resources { get; set; }

        public Style()
        {
            Triggers = new ObservableCollection<ITrigger>();
            Setters = new ObservableCollection<ITriggerAction>();
        }

        public void Attach(FrameworkElement element)
        {
            Attach(element, setter => true);
        }

        public void Attach(FrameworkElement element, Func<ITriggerAction, bool> settersFilter)
        {
            if (BasedOn != null)
            {
                BasedOn.Attach(element, setter => !Setters.Any(action => action.IsActionOverlaps(setter)) && settersFilter(setter));
            }

            foreach (ITriggerAction action in Setters.Where(settersFilter))
            {
                action.Apply(element, BaseValueSource.Style);
            }

            foreach (ITrigger trigger in Triggers)
            {
                trigger.Attach(element, BaseValueSource.StyleTrigger);
            }
        }

        public void Detach(FrameworkElement element)
        {
            Detach(element, setter => true);
        }

        public void Detach(FrameworkElement element, Func<ITriggerAction, bool> settersFilter)
        {
            if (BasedOn != null)
            {
                BasedOn.Detach(element, setter => !Setters.Any(action => action.IsActionOverlaps(setter)) && settersFilter(setter));
            }

            foreach (ITriggerAction action in Setters.Where(settersFilter))
            {
                action.Clean(element, BaseValueSource.Style);
            }

            foreach (ITrigger trigger in Triggers)
            {
                trigger.Detach(element, BaseValueSource.StyleTrigger);
            }
        }
    }

    public sealed class StyleKey : IResourceKey
    {
        public Assembly Assembly { get { return TargetType != null ? TargetType.Assembly : null; } }

        public Type TargetType { get; private set; }

        private int hashCode;

        public StyleKey(Type targetType)
        {
            this.TargetType = targetType;

            this.hashCode = TargetType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StyleKey other = obj as StyleKey;

            return Object.ReferenceEquals(this, other) || !Object.ReferenceEquals(other, null) &&
                Object.Equals(this.TargetType, other.TargetType);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override string ToString()
        {
            return String.Format("StyleKey({0})", TargetType.Name);
        }
    }

    public class StyleKeyProvider : IDeferredValueKeyProvider
    {
        public object GetValueKey(XamlElement element)
        {
            XamlMember keyMember = element.Members.SingleOrDefault(member => member.Name.LocalName == "Key");
            if (keyMember != null)
            {
                return ElementFactory.FromValue(keyMember.Values.Single(), typeof(object), element.Namespaces, element.SourceUri).CreateElement(new InitializeContext(element.Namespaces));
            }

            XamlMember targetTypeMember = element.Members.SingleOrDefault(member => member.Name.LocalName == "TargetType");
            if (targetTypeMember != null)
            {
                return new StyleKey((Type)ElementFactory.FromValue(targetTypeMember.Values.Single(), typeof(Type), element.Namespaces, element.SourceUri).CreateElement(new InitializeContext(element.Namespaces)));
            }

            throw new Granular.Exception($"Can't create value key from \"{element.Name}\"");
        }
    }
}
