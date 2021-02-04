// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSimplifyingSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// A serializer factory that wraps the serializers built by a backing factory with an <see cref="ObcSimplifyingSerializer"/>.
    /// </summary>
    public class ObcSimplifyingSerializerFactory : ISerializerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSimplifyingSerializerFactory"/> class.
        /// </summary>
        /// <param name="backingSerializerFactory">The backing serializer factory.</param>
        protected ObcSimplifyingSerializerFactory(
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
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            var fallbackSerializer = this.BackingSerializerFactory.BuildSerializer(serializerRepresentation, assemblyMatchStrategy);

            var result = new ObcSimplifyingSerializer(fallbackSerializer);

            return result;
        }
    }
}
