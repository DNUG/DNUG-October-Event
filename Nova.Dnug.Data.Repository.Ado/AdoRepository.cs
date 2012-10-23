namespace Nova.Dnug.Data.Repository.Ado
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Nova.Dnug.Domain.Model;

    /// <summary>
    /// ADO.NET specific implementation of <see cref="IRepository"/>
    /// </summary>
    public class AdoRepository : IRepository
    {
        /// <summary>
        /// The connection string of the sql server to connect to
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdoRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string of the sql server to connect to.
        /// </param>
        public AdoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Builds the repository from scratch to ensure a cold start
        /// </summary>
        public void Build()
        {
            using (var connection = new SqlConnection(this.connectionString))
            using (var command = new SqlCommand("Clear", connection))
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Creates a <see cref="University"/> instance in the database
        /// </summary>
        /// <param name="university">
        /// The <see cref="University"/> instance to persist to the database
        /// </param>
        public void Create(University university)
        {
            var universities = new DataTable("Univserities");
            var faculties = new DataTable("Faculties");
            var courses = new DataTable("Courses");
            var students = new DataTable("Students");
            var addresses = new DataTable("Addresses");

            universities.Columns.Add("Id", typeof(Guid));
            universities.Columns.Add("Name", typeof(string));
            universities.Columns.Add("LocationId", typeof(Guid));

            addresses.Columns.Add("Id", typeof(Guid));
            addresses.Columns.Add("HouseNameOrNumber", typeof(string));
            addresses.Columns.Add("Street", typeof(string));
            addresses.Columns.Add("City", typeof(string));
            addresses.Columns.Add("State", typeof(string));
            addresses.Columns.Add("ZipCode", typeof(string));

            faculties.Columns.Add("Id", typeof(Guid));
            faculties.Columns.Add("Name", typeof(string));
            faculties.Columns.Add("UniversityId", typeof(Guid));
            faculties.Columns.Add("AddressId", typeof(Guid));

            courses.Columns.Add("Id", typeof(Guid));
            courses.Columns.Add("Code", typeof(string));
            courses.Columns.Add("Description", typeof(string));
            courses.Columns.Add("StartDate", typeof(DateTime));
            courses.Columns.Add("EndDate", typeof(DateTime));
            courses.Columns.Add("FacultyID", typeof(Guid));

            students.Columns.Add("Id", typeof(Guid));
            students.Columns.Add("StudentNumber", typeof(Guid));
            students.Columns.Add("Forename", typeof(string));
            students.Columns.Add("Surname", typeof(string));
            students.Columns.Add("AddressId", typeof(Guid));
            students.Columns.Add("DateOfBirth", typeof(DateTime));
            students.Columns.Add("CourseId", typeof(Guid));

            universities.Rows.Add(university.Id, university.Name, university.Location.Id);
            
            addresses.Rows.Add(
                university.Location.Id,
                university.Location.HouseNameOrNumber,
                university.Location.Street,
                university.Location.City,
                university.Location.State,
                university.Location.ZipCode);

            foreach (var faculty in university.Faculties)
            {
                faculties.Rows.Add(faculty.Id, faculty.Name, university.Id, faculty.Address.Id);
                addresses.Rows.Add(
                    faculty.Address.Id,
                    faculty.Address.HouseNameOrNumber,
                    faculty.Address.Street,
                    faculty.Address.City,
                    faculty.Address.State,
                    faculty.Address.ZipCode);

                foreach (var course in faculty.CoursesOffered)
                {
                    courses.Rows.Add(
                        course.Id,
                        course.Code,
                        course.Description.Length > 100 ? course.Description.Substring(0, 100) : course.Description,
                        course.StartDate,
                        course.EndDate,
                        faculty.Id);

                    foreach (var student in course.RegisteredStudents)
                    {
                        students.Rows.Add(
                            student.Id,
                            student.StudentNumber,
                            student.Forename,
                            student.Surname,
                            student.Address.Id,
                            student.DateOfBirth,
                            course.Id);

                        addresses.Rows.Add(
                            student.Address.Id,
                            student.Address.HouseNameOrNumber,
                            student.Address.Street,
                            student.Address.City,
                            student.Address.State,
                            student.Address.ZipCode);
                    }
                }
            }

            using (var connection = new SqlConnection(this.connectionString))
            using (var command = new SqlCommand("CreateUniversity", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Universities", universities));
                command.Parameters.Add(new SqlParameter("@Faculties", faculties));
                command.Parameters.Add(new SqlParameter("@Courses", courses));
                command.Parameters.Add(new SqlParameter("@Students", students));
                command.Parameters.Add(new SqlParameter("@Addresses", addresses));
                command.ExecuteNonQuery();
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
            using (var connection = new SqlConnection(this.connectionString))
            using (var command = new SqlCommand("RetrieveUniversity", connection))
            {
                connection.Open();

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Id", id));
                var adapter = new SqlDataAdapter(command);
                var dataset = new DataSet();
                adapter.Fill(dataset);

                DataTable universityDataTable = dataset.Tables[0];
                DataTable facultiesDataTable = dataset.Tables[1];
                DataTable coursesDataTable = dataset.Tables[2];
                DataTable studentsDataTable = dataset.Tables[3];

                var university = new University
                {
                    Id = universityDataTable.Rows[0].Field<Guid>("Id"),
                    Name = universityDataTable.Rows[0].Field<string>("Name"),
                    Location = new Address
                    {
                        Id = universityDataTable.Rows[0].Field<Guid>("AddressId"),
                        HouseNameOrNumber = universityDataTable.Rows[0].Field<string>("HouseNameOrNumber"),
                        Street = universityDataTable.Rows[0].Field<string>("Street"),
                        City = universityDataTable.Rows[0].Field<string>("City"),
                        State = universityDataTable.Rows[0].Field<string>("State"),
                        ZipCode = universityDataTable.Rows[0].Field<string>("ZipCode")
                    },
                    Faculties = new List<Faculty>()
                };

                foreach (DataRow facultyRow in facultiesDataTable.Rows.Cast<DataRow>())
                {
                    var faculty = new Faculty
                    {
                        Id = facultyRow.Field<Guid>("Id"),
                        Name = facultyRow.Field<string>("Name"),
                        Address = new Address
                        {
                            Id = facultyRow.Field<Guid>("AddressId"),
                            HouseNameOrNumber = facultyRow.Field<string>("HouseNameOrNumber"),
                            Street = facultyRow.Field<string>("Street"),
                            City = facultyRow.Field<string>("City"),
                            State = facultyRow.Field<string>("State"),
                            ZipCode = facultyRow.Field<string>("ZipCode")
                        },
                        CoursesOffered = new List<Course>()
                    };

                    foreach (DataRow courseRow in coursesDataTable.Rows.Cast<DataRow>())
                    {
                        var course = new Course
                        {
                            Id = courseRow.Field<Guid>("Id"),
                            Code = courseRow.Field<int>("Code"),
                            Description = courseRow.Field<string>("Description"),
                            StartDate = courseRow.Field<DateTime>("StartDate"),
                            EndDate = courseRow.Field<DateTime>("EndDate"),
                            RegisteredStudents = new List<Student>()
                        };

                        foreach (DataRow studentRow in studentsDataTable.Rows.Cast<DataRow>())
                        {
                            var student = new Student
                            {
                                Id = studentRow.Field<Guid>("Id"),
                                StudentNumber = studentRow.Field<Guid>("StudentNumber"),
                                Forename = studentRow.Field<string>("Forename"),
                                Surname = studentRow.Field<string>("Surname"),
                                DateOfBirth = studentRow.Field<DateTime>("DateOfBirth"),
                                Address = new Address
                                {
                                    Id = studentRow.Field<Guid>("AddressId"),
                                    HouseNameOrNumber = studentRow.Field<string>("HouseNameOrNumber"),
                                    Street = studentRow.Field<string>("Street"),
                                    City = studentRow.Field<string>("City"),
                                    State = studentRow.Field<string>("State"),
                                    ZipCode = studentRow.Field<string>("ZipCode")
                                }
                            };

                            course.RegisteredStudents.Add(student);
                        }

                        faculty.CoursesOffered.Add(course);
                    }

                    university.Faculties.Add(faculty);
                }

                return university;
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
            using (var connection = new SqlConnection(this.connectionString))
            using (var command = new SqlCommand("UpdateUniversity", connection))
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("Id", id);
                command.Parameters.AddWithValue("Name", name);
                command.ExecuteNonQuery();
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
            using (var connection = new SqlConnection(this.connectionString))
            using (var command = new SqlCommand("DeleteUniversity", connection))
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("Id", id);
                command.ExecuteNonQuery();
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
            using (var connection = new SqlConnection(this.connectionString))
            using (var command = new SqlCommand("GetUniversityCountByForename", connection))
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("Forename", forename);
                var parameter = new SqlParameter("Count", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                return (int)parameter.Value;
            }
        }
    }
}