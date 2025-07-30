// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerFactoryBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class implementation of an <see cref="ISerializerFactory" />.
    /// </summary>
    public abstract class SerializerFactoryBase : ISerializerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerFactoryBase"/> class.
        /// </summary>
        protected SerializerFactoryBase()
        {
        }

        /// <inheritdoc />
        public abstract ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion);
    }
}
