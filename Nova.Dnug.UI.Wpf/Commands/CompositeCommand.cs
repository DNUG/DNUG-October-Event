namespace Nova.Dnug.UI.Wpf.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Implementation of <see cref="ICommand"/> composing multiple other <see cref="ICommand"/> instances
    /// </summary>
    public class CompositeCommand : ICommand
    {
        /// <summary>
        /// Collection of commands being composed
        /// </summary>
        private readonly IEnumerable<ICommand> commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand"/> class.
        /// </summary>
        /// <param name="commands">
        /// The commands to compose
        /// </param>
        public CompositeCommand(params ICommand[] commands)
        {
            this.commands = commands;
        }

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
            foreach (var command in this.commands)
            {
                command.Execute(parameter);
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
            return this.commands.All(x => x.CanExecute(parameter));
        }
    }
}
