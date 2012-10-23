namespace Nova.Dnug.UI.Wpf.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Nova.Dnug.Data.Repository;
    using Nova.Dnug.Domain.Model;

    /// <summary>
    /// Command to clear data from repositories
    /// </summary>
    public class RetrieveCommand : BackgroundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveCommand"/> class.
        /// </summary>
        /// <param name="repositories">
        /// Collection of repositories to create data in
        /// </param>
        /// <param name="universities">
        /// Collection of universities to retrieve from the repository
        /// </param>
        /// <param name="notifyProgress">
        /// Delegate to call back progress from each repository
        /// </param>
        public RetrieveCommand(IEnumerable<IRepository> repositories, IEnumerable<University> universities, Action<IRepository> notifyProgress)
            : base(() => Parallel.ForEach(
                repositories, 
                delegate(IRepository repository)
                    {
                        foreach (var id in universities.Select(x => x.Id))
                        {
                            University university = repository.Retrieve(id);
                            
                            if (university != null)
                            {
                                notifyProgress(repository);
                            }
                        }
                    }))
        {
        }
    }
}
