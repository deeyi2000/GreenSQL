using System;
using System.Windows.Input;

namespace GreenSQL.Infrastructure.MVVM
{
    public class Command : ICommand
    {
        protected Func<object, bool> _canExecute;
        protected Action<object> _execute;

        public event EventHandler CanExecuteChanged;
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        public bool CanExecute(object parameter = null) => (null != _canExecute) ? _canExecute(parameter) : true;

        public void Execute(object parameter = null)
        {
            if (null != _execute &&
                CanExecute(parameter))
            {
                _execute(parameter);
            }
        }

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected Command() { }
    }

    public class Command<T> : Command
    {
        public Command(Action<T> execute, Func<T, bool> canExecute = null) : base()
        {
            if (null != execute)
                _execute = new Action<object>(o => execute((T)o));
            if (null != canExecute)
                _canExecute = new Func<object, bool>(o => canExecute((T)o));
        }
    }
}
