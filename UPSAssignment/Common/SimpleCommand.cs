using System;
using System.Windows.Input;

namespace UPSAssignment.Common
{
    public class SimpleCommand : ICommand
    {
        #region Private fields
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        #endregion

        #region Constructor
        public SimpleCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
        #endregion

        #region Event Handler
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        } 
        #endregion

        #region ICommand Implementation
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        } 
        #endregion
    }
}
