namespace Nova.Dnug.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using ProtoBuf;

    /// <summary>
    /// Extension of <see cref="ModelEntity"/> specifying properties for <see cref="Course"/>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class Course : ModelEntity
    {
        /// <summary>
        /// Gets or sets the code of the course
        /// </summary>
        [ProtoMember(100)]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the description of the course
        /// </summary>
        [ProtoMember(101)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start date of the course
        /// </summary>
        [ProtoMember(102)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the course
        /// </summary>
        [ProtoMember(103)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of students registered on the course
        /// </summary>
        [ProtoMember(104)]
        public List<Student> RegisteredStudents { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("Code: {0}, Description: {1}", this.Code, this.Description);
        }
    }
}
