namespace Nova.Dnug.Domain.Model
{
    using System;

    using ProtoBuf;

    /// <summary>
    /// Extension of <see cref="ModelEntity"/> specifying properties for <see cref="Student"/>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class Student : ModelEntity
    {
        /// <summary>
        /// Gets or sets the student number of the student
        /// </summary>
        [ProtoMember(100)]
        public Guid StudentNumber { get; set; }

        /// <summary>
        /// Gets or sets the forename of the student
        /// </summary>
        [ProtoMember(101)]
        public string Forename { get; set; }

        /// <summary>
        /// Gets or sets the surname of the student
        /// </summary>
        [ProtoMember(102)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the address of the student
        /// </summary>
        [ProtoMember(103)]
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the student.
        /// </summary>
        [ProtoMember(104)]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("Forename: {0}, Surname: {1}", this.Forename, this.Surname);
        }
    }
}
