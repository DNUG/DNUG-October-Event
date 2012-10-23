namespace Nova.Dnug.Data.Repository
{
    using System;
    using Nova.Dnug.Domain.Model;

    /// <summary>
    /// Core contract for databases implementations providing CRUD and complex query functionality
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Builds the repository from scratch to ensure a cold start
        /// </summary>
        void Build();

        /// <summary>
        /// Creates a <see cref="University"/> instance in the database
        /// </summary>
        /// <param name="university">
        /// The <see cref="University"/> instance to persist to the database
        /// </param>
        void Create(University university);

        /// <summary>
        /// Retrieves an instance of <see cref="University"/> for the given id
        /// </summary>
        /// <param name="id">
        /// The id of the <see cref="University"/> to load
        /// </param>
        /// <returns>
        /// A <see cref="University"/> instance matching the requested id
        /// </returns>
        University Retrieve(Guid id);

        /// <summary>
        /// Updates a scalar property on the <see cref="University"/> instance associated to the <see cref="id"/> parameter
        /// </summary>
        /// <param name="id">
        /// The id of the <see cref="University"/> to load
        /// </param>
        /// <param name="name">
        /// The new name to apply to the <see cref="University"/>
        /// </param>
        void Update(Guid id, string name);

        /// <summary>
        /// Deletes the <see cref="University"/> instance with the specified <see cref="id"/>
        /// </summary>
        /// <param name="id">
        /// The id of the <see cref="University"/> to delete
        /// </param>
        void Delete(Guid id);

        /// <summary>
        /// Queries the repository to establish how many <see cref="University"/> instances contain a student registered with a given forename
        /// </summary>
        /// <param name="forename">
        /// The forename to search for
        /// </param>
        /// <returns>
        /// The count of <see cref="University"/> instances containing a <see cref="Student"/> with the matching forename
        /// </returns>
        int Query(string forename);
    }
}
