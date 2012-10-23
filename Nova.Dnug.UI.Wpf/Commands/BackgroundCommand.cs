namespace Nova.Dnug.UI.Wpf.Commands
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Implementation of <see cref="ICommand"/> for running actions ona background thread
    /// and reporting completion back to the caller.
    /// Completion will be reported back on a background thread
    /// </summary>
    public class BackgroundCommand : ICommand
    {
        /// <summary>
        /// The command to run in the background
        /// </summary>
        private readonly Action command;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundCommand"/> class.
        /// </summary>
        /// <param name="command">
        /// The command to run in the background
        /// </param>
        public BackgroundCommand(Action command)
        {
            this.command = command;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Occurs when the command has completed processing
        /// </summary>
        public event EventHandler CommandComplete;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += delegate
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => Mouse.OverrideCursor = Cursors.Wait));
                    this.command();
                };

            worker.RunWorkerCompleted += delegate
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => Mouse.OverrideCursor = Cursors.Arrow));
                  
                    if (this.CommandComplete != null)
                    {
                        this.CommandComplete(this, new EventArgs());
                    }
                };

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
