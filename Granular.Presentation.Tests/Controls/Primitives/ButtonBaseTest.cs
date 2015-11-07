using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls.Primitives;

namespace Granular.Presentation.Tests.Controls.Primitives
{
    [TestClass]
    public class ButtonBaseTest
    {
        [TestClass]
        public class Command
        {
            [TestMethod]
            public void WhenAttachingNewCommand_OldIsDetached()
            {
                ButtonBase target = new ButtonBase();
                bool canExecuteCalled;
                RelayCommand firstCommand = new RelayCommand(p => { }, p => true);
                target.Command = firstCommand;
                RelayCommand secondCommand = new RelayCommand(p => { }, p => { canExecuteCalled = true; return false; });
                target.Command = secondCommand;

                canExecuteCalled = false;
                // call to detached ICommand shouldn't trigger CanExecute changed on ButtonBase
                firstCommand.OnCanExecuteChanged();

                Assert.IsFalse(canExecuteCalled);
            }

            [TestMethod]
            public void AfterAssigningCommandThatDisables_IsEnabledIsFalse()
            {
                ButtonBase target = new ButtonBase();
                RelayCommand command = new RelayCommand(p => { }, p => false);
                target.Command = command;

                Assert.IsFalse(target.IsEnabled);
            }
            [TestMethod]
            public void AfterAssigningCommandThatEnables_IsEnabledIsTrue()
            {
                ButtonBase target = new ButtonBase();
                RelayCommand command = new RelayCommand(p => { }, p => true);
                target.Command = command;

                Assert.IsTrue(target.IsEnabled);
            }
            [TestMethod]
            public void CommandCanExecuteChanged_IsHandled()
            {
                ButtonBase target = new ButtonBase();
                bool isEnabled = false;
                RelayCommand command = new RelayCommand(p => { }, p => isEnabled);
                target.Command = command;

                Assert.IsFalse(target.IsEnabled);
                isEnabled = true;
                command.OnCanExecuteChanged();

                Assert.IsTrue(target.IsEnabled);
            }
            [TestMethod]
            public void WhenCommandParameterIsAssignedAndCommandIsAssigned_CommandParameterIsPassedToCanExecute()
            {
                ButtonBase target = new ButtonBase();
                bool canExecuteCalled = false;
                target.CommandParameter = 57;
                RelayCommand command = new RelayCommand(p => { }, p => canExecuteCalled = true);
                target.Command = command;

                Assert.IsTrue(canExecuteCalled);
            }
        }
    }
}
