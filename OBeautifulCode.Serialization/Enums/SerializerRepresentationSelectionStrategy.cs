// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerRepresentationSelectionStrategy.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Strategy for selecting the <see cref="SerializerRepresentation"/>.
    /// </summary>
    public enum SerializerRepresentationSelectionStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Use the specified representation.
        /// </summary>
        UseSpecifiedRepresentation,

        /// <summary>
        /// Use the representation of the serializer that was built by the serializer factory.
        /// </summary>
        UseRepresentationOfSerializerBuiltByFactory,
    }
}
