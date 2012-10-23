namespace Nova.Dnug.Domain.Model
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// Extension of <see cref="ModelEntity"/> specifying properties for <see cref="Address"/>
    /// </summary>
    [ProtoContract]
    [Serializable]
    public class Address : ModelEntity
    {
        /// <summary>
        /// Gets or sets the house name or number.
        /// </summary>
        [ProtoMember(100)]
        public string HouseNameOrNumber { get; set; }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        [ProtoMember(101)]
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [ProtoMember(102)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        [ProtoMember(103)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        [ProtoMember(104)]
        public string ZipCode { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "HouseNameOrNumber: {0}, Street: {1}, City: {2}, State: {3}, ZipCode: {4}", 
                this.HouseNameOrNumber, 
                this.Street, 
                this.City, 
                this.State, 
                this.ZipCode);
        }
    }
}