using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantWPF.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Func<T?, bool>? _canExecute;

        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute(Cast(parameter));

        public void Execute(object? parameter)
            => _execute(Cast(parameter));

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private static T? Cast(object? p)
        {
            if (p == null) return default;
            if (p is T t) return t;
            // Trường hợp parameter là Window nhưng binding truyền FrameworkElement.DataContext, v.v…
            try { return (T?)Convert.ChangeType(p, typeof(T)); }
            catch { return default; }
        }
    }
}
