using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Input
{
    public interface ICommand
    {
        event EventHandler CanExecuteChanged;
        bool CanExecute(object parameter);
        void Execute(object parameter);
    }
}
