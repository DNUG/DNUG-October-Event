namespace Nova.Dnug.Data.Repository.MongoDb
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Nova.Dnug.Domain.Model;
    using Builders = MongoDB.Driver.Builders;

    /// <summary>
    /// MongoDB specific implementation of <see cref="IRepository"/>
    /// </summary>
    public class MongoDbRepository : IRepository
    {
        /// <summary>
        /// The connection string of the MongoDB server to connect to
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The mongo collection to load and save documents to
        /// </summary>
        private MongoCollection<BsonDocument> mongoCollection;

        /// <summary>
        /// The database instance containing the <see cref="mongoCollection"/>
        /// </summary>
        private MongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string of the MongoDB server to connect to
        /// </param>
        public MongoDbRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Builds the repository from scratch to ensure a cold start
        /// </summary>
        public void Build()
        {
            this.database = MongoServer.Create(this.connectionString).GetDatabase("DNUG");
            this.database.Drop();

            this.mongoCollection = this.database.GetCollection("Universities");

            // index on forename to mirror applied to the sql server db
            this.mongoCollection.EnsureIndex("Faculties.CoursesOffered.RegisteredStudents.Forename");
        }

        /// <summary>
        /// Creates a <see cref="University"/> instance in the database
        /// </summary>
        /// <param name="university">
        /// The <see cref="University"/> instance to persist to the database
        /// </param>
        public void Create(University university)
        {
            this.mongoCollection.Insert(university);
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
            return this.mongoCollection.FindOneAs<University>(Builders.Query.EQ("_id", id));
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
            this.mongoCollection.FindAndModify(Builders.Query.EQ("_id", id), null, Builders.Update.Set("Name", name));
        }

        /// <summary>
        /// Deletes the <see cref="University"/> instance with the specified <see cref="id"/>
        /// </summary>
        /// <param name="id">
        /// The id of the <see cref="University"/> to delete
        /// </param>
        public void Delete(Guid id)
        {
            this.mongoCollection.FindAndRemove(Builders.Query.EQ("_id", id), null);
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
            return (int)this.mongoCollection.Count(
                Builders.Query.EQ("Faculties.CoursesOffered.RegisteredStudents.Forename", forename));
        }
    }
}