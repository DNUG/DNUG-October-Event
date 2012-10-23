namespace Nova.Dnug.UI.Wpf.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Nova.Dnug.Data.Repository;
    using Nova.Dnug.Domain.Model;

    /// <summary>
    /// Command to clear data from repositories
    /// </summary>
    public class CreateCommand : BackgroundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommand"/> class.
        /// </summary>
        /// <param name="repositories">
        /// Collection of repositories to create data in
        /// </param>
        /// <param name="universities">
        /// Collection of universities to persist in each repository
        /// </param>
        /// <param name="notifyProgress">
        /// Delegate to call back progress from each repository
        /// </param>
        public CreateCommand(IEnumerable<IRepository> repositories, IEnumerable<University> universities, Action<IRepository> notifyProgress)
            : base(() => Parallel.ForEach(
                repositories, 
                delegate(IRepository repository)
                    {
                        foreach (var university in universities)
                        {
                            repository.Create(university);
                            notifyProgress(repository);
                        }
                    }))
        {
        }
    }
}
