using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Windows
{
    public delegate void DependencyPropertyChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e);

    public class DependencyPropertyChangedEventArgs : EventArgs
    {
        public DependencyProperty Property { get; private set; }
        public object NewValue { get; private set; }
        public object OldValue { get; private set; }
        public bool IsSubPropertyChange { get; private set; }

        public DependencyPropertyChangedEventArgs(DependencyProperty property, object oldValue, object newValue) :
            this(property, oldValue, newValue, false)
        {
            //
        }

        public DependencyPropertyChangedEventArgs(DependencyProperty property, object value) :
            this(property, value, value, true)
        {
            //
        }

        public DependencyPropertyChangedEventArgs() :
            this(null, null, null, true)
        {
            //
        }

        private DependencyPropertyChangedEventArgs(DependencyProperty property, object oldValue, object newValue, bool isSubPropertyChange)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.IsSubPropertyChange = isSubPropertyChange;
        }
    }

    [DebuggerNonUserCode]
    public static class DependencyPropertyChangedEventHandlerExtensions
    {
        public static void Raise(this DependencyPropertyChangedEventHandler handler, object sender, DependencyPropertyChangedEventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
