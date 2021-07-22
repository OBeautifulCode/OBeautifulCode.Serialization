// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Abstract factory interface for building serializers.
    /// </summary>
    public interface ISerializerFactory
    {
        /// <summary>
        /// Builds the correct implementation of <see cref="ISerializer" /> based on the description.
        /// </summary>
        /// <param name="serializerRepresentation">Representation of the serializer.</param>
        /// <param name="assemblyVersionMatchStrategy">Optional assembly version match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="VersionMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Correct implementation of <see cref="ISerializer" /> based on the description.
        /// </returns>
        ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion);
    }
}