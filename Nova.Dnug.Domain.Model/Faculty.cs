namespace Nova.Dnug.Domain.Model
{
    using System;
    using System.Collections.Generic;

    using ProtoBuf;

    /// <summary>
    /// Extension of <see cref="ModelEntity"/> specifying properties for <see cref="Faculty"/>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class Faculty : ModelEntity
    {
        /// <summary>
        /// Gets or sets the name of the faculty
        /// </summary>
        [ProtoMember(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the faculty
        /// </summary>
        [ProtoMember(101)]
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the courses offered by the faculty
        /// </summary>
        [ProtoMember(102)]
        public List<Course> CoursesOffered { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("Name: {0}", this.Name);
        }
    }
}
