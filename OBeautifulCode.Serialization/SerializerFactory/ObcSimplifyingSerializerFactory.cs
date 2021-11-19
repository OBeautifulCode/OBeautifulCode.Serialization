// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSimplifyingSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using OBeautifulCode.Type;

    /// <summary>
    /// A serializer factory that wraps the serializers built by a backing factory with an <see cref="ObcSimplifyingSerializer"/>.
    /// </summary>
    public class ObcSimplifyingSerializerFactory : ISerializerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSimplifyingSerializerFactory"/> class.
        /// </summary>
        /// <param name="backingSerializerFactory">The backing serializer factory.</param>
        public ObcSimplifyingSerializerFactory(
            ISerializerFactory backingSerializerFactory)
        {
            this.BackingSerializerFactory = backingSerializerFactory;
        }

        /// <summary>
        /// Gets the backing serializer factory.
        /// </summary>
        public ISerializerFactory BackingSerializerFactory { get; }

        /// <inheritdoc />
        public ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            var fallbackSerializer = this.BackingSerializerFactory.BuildSerializer(serializerRepresentation, assemblyVersionMatchStrategy);

            var result = new ObcSimplifyingSerializer(fallbackSerializer);

            return result;
        }
    }
}
