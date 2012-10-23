namespace Nova.Dnug.Domain.Model.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Nova.Dnug.Domain.Model.Properties;

    /// <summary>
    /// Builder class for creating instances of the <see cref="University"/> class
    /// </summary>
    public class UniversityBuilder
    {
        /// <summary>
        /// Embedded instance of <see cref="AddressBuilder"/> to create a random instances of <see cref="Address"/>
        /// </summary>
        private readonly AddressBuilder addressBuilder;

        /// <summary>
        /// Embedded instance of <see cref="FacultyBuilder"/> to create a random instances of <see cref="Faculty"/>
        /// </summary>
        private readonly FacultyBuilder facultyBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniversityBuilder"/> class.
        /// </summary>
        public UniversityBuilder()
        {
            this.addressBuilder = new AddressBuilder();
            this.facultyBuilder = new FacultyBuilder();
        }

        /// <summary>
        /// Builds a randomly generated collection of <see cref="University"/> entities
        /// </summary>
        /// <param name="complexity">
        /// The value of the complexity of the <see cref="University"/> to create
        /// </param>
        /// <param name="count">
        /// The number of instances of <see cref="University"/> to create
        /// </param>
        /// <returns>
        /// A randomly generated <see cref="University"/> instance
        /// </returns>
        public IEnumerable<University> Build(int complexity, int count)
        {
                        var universities = from name in Resources.Universities.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Take(count)
                               select new University
                                   {
                                       Id = Guid.NewGuid(),
                                       Name = name,
                                       Faculties = Enumerable.Range(0, complexity).Select(x => this.facultyBuilder.Build(complexity)).ToList(),
                                       Location = this.addressBuilder.Build()
                                   };

            return universities.ToList();
        }

        /// <summary>
        /// Builds a randomly generated collection of <see cref="University"/> entities
        /// </summary>
        /// <param name="complexity">
        /// The value of the complexity of the <see cref="University"/> to create
        /// </param>
        /// <returns>
        /// A randomly generated <see cref="University"/> instance
        /// </returns>
        public IEnumerable<University> Build(int complexity)
        {
            return this.Build(
                complexity,
                Resources.Universities.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Count());
        }
    }
}
