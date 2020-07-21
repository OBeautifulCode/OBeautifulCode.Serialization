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

    using static System.FormattableString;

    /// <summary>
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public sealed class BsonSerializerFactory : SerializerFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializerFactory"/> class.
        /// </summary>
        /// <param name="compressorFactory">Optional compressor factory to use.  DEFAULT is to use OBeautifulCode.Compression.Recipes.CompressorFactory.Instance.</param>
        public BsonSerializerFactory(
            ICompressorFactory compressorFactory = null)
            : base(compressorFactory)
        {
        }

        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            // ReSharper disable once RedundantArgumentDefaultValue
            var configurationType = serializerRepresentation.SerializationConfigType?.ResolveFromLoadedTypes(assemblyMatchStrategy, throwIfCannotResolve: true);

            ISerializer serializer;

            switch (serializerRepresentation.SerializationKind)
            {
                case SerializationKind.Bson:
                    serializer = new ObcBsonSerializer(configurationType?.ToBsonSerializationConfigurationType());
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(serializerRepresentation)} from enumeration {nameof(SerializationKind)} of {serializerRepresentation.SerializationKind} is not supported."));
            }

            var result = this.WrapInCompressingSerializerIfAppropriate(serializer, serializerRepresentation.CompressionKind);

            return result;
        }
    }
}
