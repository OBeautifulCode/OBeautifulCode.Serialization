// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullDescribedSerialization.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Null-object pattern implementation of a <see cref="DescribedSerializationBase"/>.
    /// </summary>
    public partial class NullDescribedSerialization : DescribedSerializationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullDescribedSerialization"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">The type of object serialized.</param>
        /// <param name="serializerRepresentation">The serializer used to generate the payload.</param>
        public NullDescribedSerialization(
            TypeRepresentation payloadTypeRepresentation,
            SerializerRepresentation serializerRepresentation)
            : base(payloadTypeRepresentation, serializerRepresentation)
        {
        }

        /// <inheritdoc />
        public override SerializationFormat GetSerializationFormat()
        {
            var result = SerializationFormat.Null;

            return result;
        }

        /// <inheritdoc />
        public override string GetSerializedPayloadAsEncodedString()
        {
            return null;
        }

        /// <inheritdoc />
        public override byte[] GetSerializedPayloadAsEncodedBytes()
        {
            return null;
        }
    }
}