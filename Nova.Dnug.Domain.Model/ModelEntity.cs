namespace Nova.Dnug.Domain.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProtoBuf;

    /// <summary>
    /// Base class for domain model entities
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class ModelEntity
    {
        /// <summary>
        /// Gets or sets the id of the entity.
        /// </summary>
        [Key]
        [ProtoMember(1)]
        public Guid Id { get; set; }

        /// <summary>
        /// Strongly typed implementation of <see cref="Equals(object)"/>
        /// </summary>
        /// <param name="other">
        /// The <see cref="ModelEntity"/> to compare with the current <see cref="ModelEntity"/>.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="ModelEntity"/> is equal to the current <see cref="ModelEntity"/>; otherwise, false.
        /// </returns>
        public bool Equals(ModelEntity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Id == this.Id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(ModelEntity))
            {
                return false;
            }

            return this.Equals((ModelEntity)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
