using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Utils
{
    public interface IDelegateCommand
    {
        void RaiseCanExecuteChanged();
    }

    public class DelegateCommand<T> : ICommand, IDelegateCommand
    {
        Action<T> _executeCommand;
        Func<T, bool> _canExecuteCommand;

        public DelegateCommand(Action<T> executeCommand)
            : this(executeCommand, x => true)
        { }

        public DelegateCommand(Action<T> executeCommand, Func<T, bool> canExecuteCommand)
        {
            Check.NotNull(executeCommand, "executeCommand");
            Check.NotNull(canExecuteCommand, "canExecuteCommand");

            _executeCommand = executeCommand;
            _canExecuteCommand = canExecuteCommand;
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
        {
            return _canExecuteCommand((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            _executeCommand((T)parameter);
        }

        #endregion
    }

}
