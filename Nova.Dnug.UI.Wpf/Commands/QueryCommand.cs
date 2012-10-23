namespace Nova.Dnug.UI.Wpf.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Nova.Dnug.Data.Repository;

    /// <summary>
    /// Command to clear data from repositories
    /// </summary>
    public class QueryCommand : BackgroundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCommand"/> class.
        /// </summary>
        /// <param name="repositories">
        /// Collection of repositories to query data from
        /// </param>
        /// <param name="forenames">
        /// Collection of forenames to query
        /// </param>
        /// <param name="notifyProgress">
        /// Delegate to call back progress from each repository
        /// </param>
        public QueryCommand(IEnumerable<IRepository> repositories, IEnumerable<string> forenames, Action<IRepository> notifyProgress)
            : base(() => Parallel.ForEach(
                repositories, 
                delegate(IRepository repository)
                    {
                        foreach (var forename in forenames)
                        {
                            repository.Query(forename);
                            notifyProgress(repository);
                        }
                    }))
        {
        }
    }
}
