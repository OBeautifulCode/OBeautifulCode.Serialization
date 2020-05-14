// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerialization.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Represents a serialized object along with a description of the type of the object.
    /// </summary>
    public partial class DescribedSerialization : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescribedSerialization"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">A description of the type of object serialized.</param>
        /// <param name="serializedPayload">The object serialized to a string.</param>
        /// <param name="serializerRepresentation">The serializer used to generate the payload.</param>
        /// <param name="serializationFormat">The format that the object was serialized into.</param>
        /// <exception cref="ArgumentNullException"><paramref name="payloadTypeRepresentation"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="serializedPayload"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="serializedPayload"/> is whitespace.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="serializerRepresentation"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="serializerRepresentation"/> is whitespace.</exception>
        public DescribedSerialization(
            TypeRepresentation payloadTypeRepresentation,
            string serializedPayload,
            SerializerRepresentation serializerRepresentation,
            SerializationFormat serializationFormat)
        {
            new { payloadTypeRepresentation }.AsArg().Must().NotBeNull();
            new { serializerRepresentation }.AsArg().Must().NotBeNull();
            new { serializationFormat }.AsArg().Must().NotBeEqualTo(SerializationFormat.Invalid);

            this.PayloadTypeRepresentation = payloadTypeRepresentation;
            this.SerializedPayload = serializedPayload;
            this.SerializerRepresentation = serializerRepresentation;
            this.SerializationFormat = serializationFormat;
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
        /// Gets the representation of the serializer used to generate the payload.
        /// </summary>
        public SerializerRepresentation SerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the format that the object was serialized into.
        /// </summary>
        public SerializationFormat SerializationFormat { get; private set; }
    }
}
