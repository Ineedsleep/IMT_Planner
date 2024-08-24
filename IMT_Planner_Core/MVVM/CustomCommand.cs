using System;
using System.Windows.Input;

namespace MVVM
{
    public class CustomCommand : ICommand

    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        

        public CustomCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute?.Invoke(parameter);
    }
}