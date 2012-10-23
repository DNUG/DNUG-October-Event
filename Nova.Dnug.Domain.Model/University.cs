namespace Nova.Dnug.Domain.Model
{
    using System;
    using System.Collections.Generic;
    
    using ProtoBuf;

    /// <summary>
    /// Extension of <see cref="ModelEntity"/> specifying properties for <see cref="University"/>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class University : ModelEntity
    {
        /// <summary>
        /// Gets or sets the name of the university.
        /// </summary>
        [ProtoMember(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the university
        /// </summary>
        [ProtoMember(101)]
        public Address Location { get; set; }

        /// <summary>
        /// Gets or sets the collection of faculties that make up the university
        /// </summary>
        [ProtoMember(102)]
        public List<Faculty> Faculties { get; set; }

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