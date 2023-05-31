using System.Windows.Input;

namespace crud_wpf_mvvm_origin.Codes;

public class Command<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Predicate<T>? _canExecute;

    public Command(Action<T> execute)
        : this(execute, null)
    {
    }

    public Command(Action<T> execute, Predicate<T>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));

        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        if (_canExecute == null)
        {
            return true;
        }

        if (parameter is T typedParameter)
        {
            return _canExecute(typedParameter);
        }

        return false;
    }

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            if (_canExecute != null) CommandManager.RequerySuggested += value;
        }
        
        remove
        {
            if (_canExecute != null) CommandManager.RequerySuggested -= value;
        }
    }

    public void Execute(object? parameter)
    {
        if (parameter is T typedParameter)
        {
            _execute(typedParameter);
        }
        else if (default(T) == null)
        {
            _execute(default!);
        }        
    }
}
