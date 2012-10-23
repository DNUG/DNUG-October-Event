namespace Nova.Dnug.UI.Wpf.Commands
{
    using System;
    using System.Windows.Input;

    using Nova.Dnug.UI.Wpf.Models;

    /// <summary>
    /// Implementation of <see cref="ICommand"/> for toggling the selected state of a respository
    /// </summary>
    public class ToggleCommand : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            var repository = parameter as ChartableRepository;

            if (repository != null)
            {
                repository.Selected = !repository.Selected;
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
