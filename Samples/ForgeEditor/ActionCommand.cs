using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ForgeEditor
{
    public abstract class Command : ICommand
    {
        public string Name { get; set; }

        public abstract bool CanExecute(object parameter);

        public event EventHandler CanExecuteChanged;

        public abstract void Execute(object parameter);

        public void OnCanExecuteChanged()
        {
            EventHandler temp = CanExecuteChanged;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }
    }

    public delegate bool ActionCanExecuteHandler(object sender, object parameter);

    public class ActionCommand : Command
    {
        readonly Func<bool> _canExecute;
        readonly Action _actionexecute;

        public event EventHandler<object> ExecutedCalled;

        public event ActionCanExecuteHandler CanExecuteCalled;


        public ActionCommand(Action execute, Func<bool> canExecute = null)
        {
            this._actionexecute = execute;
            this._canExecute = canExecute;
        }
        public override bool CanExecute(object parameter)
        {
            return (_canExecute == null || _canExecute()) && OnCanExecuted(parameter);
        }

        private bool OnCanExecuted(object parameter)
        {
            if (CanExecuteCalled != null)
                return CanExecuteCalled(this, parameter);
            return true;
        }

        public override void Execute(object parameter)
        {
            _actionexecute();

            if (ExecutedCalled != null)
            {
                ExecutedCalled(this, parameter);
            }
        }
    }
}
