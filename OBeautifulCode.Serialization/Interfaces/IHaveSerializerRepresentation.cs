// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHaveSerializerRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Represents a serializer that has a <see cref="SerializerRepresentation"/>.
    /// </summary>
    public interface IHaveSerializerRepresentation
    {
        /// <summary>
        /// Gets the <see cref="SerializerRepresentation"/>.
        /// </summary>
        SerializerRepresentation SerializerRepresentation { get; }
    }
}
