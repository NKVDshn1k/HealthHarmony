using System;
using System.Windows.Input;

namespace HealthHarmony.Base
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute)
           : this(execute, null) =>
            _execute = execute;


        public static RelayCommand Get(Action<object> execute, ref RelayCommand property)
        {
            if (property == null)
                property = new RelayCommand(execute);
            return property;
        }


        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) =>
            _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) =>
            _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        private event EventHandler CanExecuteChangedInternal;

        public void RaiseCanExecuteChanged() =>
            CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
    }
}
