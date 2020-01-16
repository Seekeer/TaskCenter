using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Utils
{
    public class ViewModelBase : NotifyPropertyChanged
    {
        #region Commands
        private Dictionary<string, ICommand> _commands;
        public IDictionary<string, ICommand> Commands
        {
            get
            {
                if (_commands == null)
                {
                    _commands = new Dictionary<string, ICommand>();
                }
                return _commands;
            }
        }

        protected virtual void RegisterCommand<T>(string name, Action<T> executeCommand, Func<T, bool> canExecuteCommand)
        {
            Check.NotNull(name, "name");
            Check.NotNull(executeCommand, "executeCommand");
            Check.NotNull(canExecuteCommand, "canExecuteCommand");

            var command = new DelegateCommand<T>(executeCommand, canExecuteCommand);
            Commands.Add(name, command);
        }

        protected void RegisterCommand<T>(string name, Action<T> executeCommand)
        {
            RegisterCommand<T>(name, executeCommand, x => true);
        }

        protected void RegisterCommand(string name, Action<object> executeCommand, Func<object, bool> canExecuteCommand)
        {
            RegisterCommand<object>(name, executeCommand, canExecuteCommand);
        }

        protected void RegisterCommand(string name, Action<object> executeCommand)
        {
            RegisterCommand<object>(name, executeCommand);
        }

        protected virtual void RaiseCanExecuteChanged(string name)
        {
            Check.NotNull(name, "name");
            ICommand command = null;
            Commands.TryGetValue(name, out command);

            if (command != null && command is IDelegateCommand)
            {
                ((IDelegateCommand)command).RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
