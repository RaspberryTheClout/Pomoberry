using System;
using System.Windows.Input;

namespace Pomoberry.UI.Models
{
    /// <summary>
    /// reusable RelayCommand class for handling command execution
    /// </summary>
    public class RelayCommand : ICommand
    {
        // declare a variable that can hold execute function
        private readonly Action<object?> _execute;
        public event EventHandler? CanExecuteChanged;     // required by ICommand

        //Constructor
        public RelayCommand(Action<object?> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));  // raise an exception if 'execute' is null.
        }

        //Method to execute the relevant function
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public bool CanExecute(object? parameter) => true;  // always return true. Required by ICommand

    }
}
