namespace Nova.Dnug.UI.Wpf.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Nova.Dnug.Data.Repository;

    /// <summary>
    /// Command to clear data from repositories
    /// </summary>
    public class BuildCommand : BackgroundCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildCommand"/> class.
        /// </summary>
        /// <param name="repositories">
        /// Collection of repositories to clear data from
        /// </param>
        public BuildCommand(IEnumerable<IRepository> repositories)
            : base(() => Parallel.ForEach(repositories, x => x.Build()))
        {
        }
    }
}
