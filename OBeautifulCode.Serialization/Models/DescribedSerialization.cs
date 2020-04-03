// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerialization.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Represents a serialized object along with a description of the type of the object.
    /// </summary>
    public class DescribedSerialization : IEquatable<DescribedSerialization>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescribedSerialization"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">A description of the type of object serialized.</param>
        /// <param name="serializedPayload">The object serialized to a string.</param>
        /// <param name="serializerDescription">The serializer used to generate the payload.</param>
        /// <exception cref="ArgumentNullException"><paramref name="payloadTypeRepresentation"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="serializedPayload"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="serializedPayload"/> is whitespace.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="serializerDescription"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="serializerDescription"/> is whitespace.</exception>
        public DescribedSerialization(TypeRepresentation payloadTypeRepresentation, string serializedPayload, SerializerDescription serializerDescription)
        {
            new { payloadTypeRepresentation }.AsArg().Must().NotBeNull();
            new { serializerDescription }.AsArg().Must().NotBeNull();

            this.PayloadTypeRepresentation = payloadTypeRepresentation;
            this.SerializedPayload = serializedPayload;
            this.SerializerDescription = serializerDescription;
        }

        /// <summary>
        /// Gets a description of the type of object serialized.
        /// </summary>
        public TypeRepresentation PayloadTypeRepresentation { get; private set; }

        /// <summary>
        /// Gets the object serialized to a string (bytes will be Base64 encoded here).
        /// </summary>
        public string SerializedPayload { get; private set; }

        /// <summary>
        /// Gets the description of the serializer used to generate the payload.
        /// </summary>
        public SerializerDescription SerializerDescription { get; private set; }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are equal.</returns>
        public static bool operator ==(DescribedSerialization first, DescribedSerialization second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            return first.PayloadTypeRepresentation == second.PayloadTypeRepresentation
                   && first.SerializedPayload == second.SerializedPayload
                   && first.SerializerDescription == second.SerializerDescription;
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are inequal.</returns>
        public static bool operator !=(DescribedSerialization first, DescribedSerialization second) => !(first == second);

        /// <inheritdoc />
        public bool Equals(DescribedSerialization other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as DescribedSerialization);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.PayloadTypeRepresentation).Hash(this.SerializedPayload).Hash(this.SerializerDescription).Value;
    }
}
