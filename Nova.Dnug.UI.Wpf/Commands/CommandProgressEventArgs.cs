namespace Nova.Dnug.UI.Wpf.Commands
{
    using System;

    using Nova.Dnug.Data.Repository;
    using Nova.Dnug.Domain.Model;

    /// <summary>
    /// Extension of <see cref="EventArgs"/> composing data for a <see cref="IRepository"/> and <see cref="University"/>
    /// </summary>
    public class CommandProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        public IRepository Repository { get; set; }

        /// <summary>
        /// Gets or sets the university.
        /// </summary>
        public University University { get; set; }
    }
}