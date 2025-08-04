// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlwaysThrowingSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets a serializer factory that always throws (<see cref="NotSupportedException"/>).
    /// </summary>
    public class AlwaysThrowingSerializerFactory : SerializerFactoryBase
    {
        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            throw new NotSupportedException();
        }
    }
}
