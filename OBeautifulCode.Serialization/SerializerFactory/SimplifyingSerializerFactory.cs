// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimplifyingSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Builds an <see cref="ObcSimplifyingSerializer"/> by using a specified fallback factory to builds a
    /// serializer to use when <see cref="ObcSimplifyingSerializer"/> cannot shortcut a type.
    /// </summary>
    public class SimplifyingSerializerFactory : SerializerFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimplifyingSerializerFactory"/> class.
        /// </summary>
        /// <param name="fallbackSerializerFactory">Builds the serializer to use when the simplifying serializer cannot shortcut a type.</param>
        public SimplifyingSerializerFactory(
            ISerializerFactory fallbackSerializerFactory)
        {
            if (fallbackSerializerFactory == null)
            {
                throw new ArgumentNullException(nameof(fallbackSerializerFactory));
            }

            this.FallbackSerializerFactory = fallbackSerializerFactory;
        }

        /// <summary>
        /// Gets a factory that builds the serializer to use when the simplifying serializer cannot shortcut a type.
        /// </summary>
        public ISerializerFactory FallbackSerializerFactory { get; }

        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            var fallbackSerializer = this.FallbackSerializerFactory.BuildSerializer(serializerRepresentation, assemblyVersionMatchStrategy);

            var result = new ObcSimplifyingSerializer(fallbackSerializer);

            return result;
        }
    }
}
