namespace Nova.Dnug.Data.Repository.Redis
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using BookSleeve;
    using Nova.Dnug.Domain.Model;
    using ProtoBuf;

    /// <summary>
    /// Redis specific implementation of <see cref="IRepository"/>
    /// </summary>
    public class RedisRepository : IRepository
    {
        /// <summary>
        /// The connection string of the Redis server to connect to
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// A connection to the Redis server
        /// </summary>
        private RedisConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string of the Redis server to connect to
        /// </param>
        public RedisRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Builds the repository from scratch to ensure a cold start
        /// </summary>
        public void Build()
        {
            this.connection = new RedisConnection(this.connectionString.Split(':')[0], Convert.ToInt32(this.connectionString.Split(':')[1]), allowAdmin: true);
            Serializer.PrepareSerializer<University>();
            this.connection.Open();
            Task task = this.connection.Server.FlushDb(0);
            task.Wait();
        }

        /// <summary>
        /// Creates a <see cref="University"/> instance in the database
        /// </summary>
        /// <param name="university">
        /// The <see cref="University"/> instance to persist to the database
        /// </param>
        public void Create(University university)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, university);
                Task task = this.connection.Strings.Set(0, university.Id.ToString(), stream.GetBuffer());
                task.Wait();
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
            Task<byte[]> task = this.connection.Strings.Get(0, id.ToString());
            task.Wait();

            using (var stream = new MemoryStream(task.Result))
            {
                return Serializer.Deserialize<University>(stream);
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
            University university = this.Retrieve(id);
            university.Name = name;

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, university);
                Task task = this.connection.Strings.Set(0, university.Id.ToString(), stream.GetBuffer());
                task.Wait();
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
            this.connection.Keys.Remove(0, id.ToString());
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
            // no implementation as not feasible due to the nature of Redis
            return 0;
        }
    }
}