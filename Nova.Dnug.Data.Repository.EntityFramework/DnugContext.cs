namespace Nova.Dnug.Data.Repository.EntityFramework
{
    using System.Data.Entity;

    using Nova.Dnug.Domain.Model;

    /// <summary>
    /// Extension of <see cref="DbContext"/> exposing <see cref="DbSet"/> instances for the domain model
    /// </summary>
    public class DnugContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DnugContext"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string of the SQL Server database to connect to
        /// </param>
        public DnugContext(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet"/> for managing <see cref="Address"/> instances.
        /// </summary>        
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet"/> for managing <see cref="Course"/> instances.
        /// </summary>        
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet"/> for managing <see cref="Faculty"/> instances.
        /// </summary>        
        public DbSet<Faculty> Faculties { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet"/> for managing <see cref="Student"/> instances.
        /// </summary>        
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet"/> for managing <see cref="University"/> instances.
        /// </summary>
        public DbSet<University> Universities { get; set; }
    }
}
