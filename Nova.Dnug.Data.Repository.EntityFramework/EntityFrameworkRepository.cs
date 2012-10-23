namespace Nova.Dnug.Data.Repository.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Nova.Dnug.Domain.Model;

    /// <summary>
    /// Entity Framework specific implementation of <see cref="IRepository"/>
    /// </summary>
    public class EntityFrameworkRepository : IRepository
    {
        /// <summary>
        /// The connection string of the sql server database to connect to
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string of the sql server database to connect to.
        /// </param>
        public EntityFrameworkRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Builds the repository from scratch to ensure a cold start
        /// </summary>
        public void Build()
        {
            new DropCreateDatabaseAlways<DnugContext>().InitializeDatabase(
                new DnugContext(this.connectionString));
        }

        /// <summary>
        /// Creates a <see cref="University"/> instance in the database
        /// </summary>
        /// <param name="university">
        /// The <see cref="University"/> instance to persist to the database
        /// </param>
        public void Create(University university)
        {
            using (var context = new DnugContext(this.connectionString))
            {
                context.Universities.Add(university);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves an instance of <see cref="University"/> for the given id
        /// </summary>
        /// <param name="id">
        /// The id of the <see cref="University"/> to load
        /// </param>
        /// <returns>
        /// A <see cref="University"/> instance matching the requested id
        /// </returns>
        public University Retrieve(Guid id)
        {
            using (var context = new DnugContext(this.connectionString))
            {
                return (from u in context.Universities
                            .Include("Location")
                            .Include("Faculties")
                            .Include("Faculties.Address")
                            .Include("Faculties.CoursesOffered")
                            .Include("Faculties.CoursesOffered.RegisteredStudents")
                            .Include("Faculties.CoursesOffered.RegisteredStudents.Address")
                        where u.Id == id
                        select u).First();
            }
        }

        /// <summary>
        /// Updates a scalar property on the <see cref="University"/> instance associated to the <see cref="id"/> parameter
        /// </summary>
        /// <param name="id">
        /// The id of the <see cref="University"/> to load
        /// </param>
        /// <param name="name">
        /// The new name to apply to the <see cref="University"/>
        /// </param>
        public void Update(Guid id, string name)
        {
            using (var context = new DnugContext(this.connectionString))
            {
                University university = (from u in context.Universities
                                         where u.Id == id
                                         select u).First();
                university.Name = name;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the <see cref="University"/> instance with the specified <see cref="id"/>
        /// </summary>
        /// <param name="id">
        /// The id of the <see cref="University"/> to delete
        /// </param>
        public void Delete(Guid id)
        {
            using (var context = new DnugContext(this.connectionString))
            {
                University university = (from u in context.Universities
                                         where u.Id == id
                                         select u).First();
                context.Universities.Remove(university);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Queries the repository to establish how many <see cref="University"/> instances contain a student registered with a given forename
        /// </summary>
        /// <param name="forename">
        /// The forename to search for
        /// </param>
        /// <returns>
        /// The count of <see cref="University"/> instances containing a <see cref="Student"/> with the matching forename
        /// </returns>
        public int Query(string forename)
        {
            using (var context = new DnugContext(this.connectionString))
            {
                return (from university in context.Universities
                        from faculty in university.Faculties
                        from course in faculty.CoursesOffered
                        from student in course.RegisteredStudents
                        where student.Forename.Equals(forename)
                        select student).Count();
            }
        }
    }
}