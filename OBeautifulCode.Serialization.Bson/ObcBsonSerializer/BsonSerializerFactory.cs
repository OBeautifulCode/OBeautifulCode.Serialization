// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public sealed class BsonSerializerFactory : SerializerFactoryBase
    {
        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            if (serializerRepresentation.CompressionKind != CompressionKind.None)
            {
                throw new ArgumentOutOfRangeException(nameof(serializerRepresentation), Invariant($"{nameof(serializerRepresentation)}.{nameof(SerializerRepresentation.CompressionKind)} is not {nameof(CompressionKind)}.{nameof(CompressionKind.None)}.  Consider wrapping this factory in a {nameof(CompressIfConfiguredSerializerFactory)} if compression is required."));
            }

            // ReSharper disable once RedundantArgumentDefaultValue
            var configurationType = serializerRepresentation.SerializationConfigType?.ResolveFromLoadedTypes(assemblyVersionMatchStrategy, throwIfCannotResolve: true);

            ISerializer result;

            switch (serializerRepresentation.SerializationKind)
            {
                case SerializationKind.Bson:
                    result = new ObcBsonSerializer(configurationType?.ToBsonSerializationConfigurationType());
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(serializerRepresentation)} from enumeration {nameof(SerializationKind)} of {serializerRepresentation.SerializationKind} is not supported."));
            }

            return result;
        }
    }
}
