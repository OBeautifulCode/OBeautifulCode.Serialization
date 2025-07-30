// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachingSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Concurrent;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets a cached serializer or if not found, builds a serializer using a
    /// specified backing serializer factory and add that serializer to the cache.
    /// </summary>
    public class CachingSerializerFactory : SerializerFactoryBase
    {
        private readonly ConcurrentDictionary<SerializerRepresentation, ConcurrentDictionary<VersionMatchStrategy, ISerializer>>
            cachedSerializerRepresentationToSerializerMap = new ConcurrentDictionary<SerializerRepresentation, ConcurrentDictionary<VersionMatchStrategy, ISerializer>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingSerializerFactory"/> class.
        /// </summary>
        /// <param name="backingSerializerFactory">A factory that builds the backing serializer to use.</param>
        public CachingSerializerFactory(
            ISerializerFactory backingSerializerFactory)
        {
            if (backingSerializerFactory == null)
            {
                throw new ArgumentNullException(nameof(backingSerializerFactory));
            }

            this.BackingSerializerFactory = backingSerializerFactory;
        }

        /// <summary>
        /// Gets a factory that builds the backing serializer to use.
        /// </summary>
        public ISerializerFactory BackingSerializerFactory { get; }

        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            ISerializer result;

            if (this.cachedSerializerRepresentationToSerializerMap.TryGetValue(
                    serializerRepresentation,
                    out ConcurrentDictionary<VersionMatchStrategy, ISerializer> assemblyVersionMatchStrategyToSerializerMap))
            {
                if (assemblyVersionMatchStrategyToSerializerMap.TryGetValue(assemblyVersionMatchStrategy, out result))
                {
                    return result;
                }
            }

            result = this.BackingSerializerFactory.BuildSerializer(
                serializerRepresentation,
                assemblyVersionMatchStrategy);

            this.cachedSerializerRepresentationToSerializerMap.TryAdd(
                serializerRepresentation,
                new ConcurrentDictionary<VersionMatchStrategy, ISerializer>());

            this.cachedSerializerRepresentationToSerializerMap[serializerRepresentation].TryAdd(
                assemblyVersionMatchStrategy,
                result);

            return result;
        }
    }
}
