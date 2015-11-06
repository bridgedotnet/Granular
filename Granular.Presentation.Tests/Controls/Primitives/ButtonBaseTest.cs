using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Granular.Presentation.Tests.Controls.Primitives
{
    [TestClass]
    public class ButtonBaseTest
    {
        protected ButtonBase target;

        [TestInitialize]
        public void SetUp()
        {
            target = new ButtonBase();
        }

        [TestClass]
        public class Command: ButtonBaseTest
        {
            [TestMethod]
            public void WhenAttachingNewCommand_OldIsDetached()
            {
                bool canExecuteCalled;
                RelayCommand firstCommand = new RelayCommand(p => { }, p => true);
                target.Command = firstCommand;
                Mock<ICommand> secondCommand = new Mock<ICommand>();
                secondCommand.Setup(mq => mq.CanExecute(It.IsAny<object>())).Returns(() => { canExecuteCalled = true; return false; });
                target.Command = secondCommand.Object;

                canExecuteCalled = false;
                // call to detached ICommand shouldn't trigger CanExecute changed on ButtonBase
                firstCommand.OnCanExecuteChanged();

                Assert.IsFalse(canExecuteCalled);
            }

            [TestMethod]
            public void AfterAssigningCommandThatDisables_IsEnabledIsFalse()
            {
                RelayCommand command = new RelayCommand(p => { }, p => false);
                target.Command = command;

                Assert.IsFalse(target.IsEnabled);
            }
            [TestMethod]
            public void AfterAssigningCommandThatEnables_IsEnabledIsTrue()
            {
                RelayCommand command = new RelayCommand(p => { }, p => true);
                target.Command = command;

                Assert.IsTrue(target.IsEnabled);
            }
            [TestMethod]
            public void CommandCanExecuteChanged_IsHandled()
            {
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
                target.CommandParameter = 57;
                Mock<ICommand> command = new Mock<ICommand>();
                target.Command = command.Object;

                command.Verify(mq => mq.CanExecute(It.Is<object>(o => (int)o == 57)), Times.Once);
            }
        }
    }
}
