// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressIfConfiguredSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Builds an <see cref="ObcCompressingSerializer"/> using a specified backing factory to build the
    /// backing serializer, if and only if the specified <see cref="SerializerRepresentation"/> specifies
    /// compression.  If not, this factory just uses the backing serializer.
    /// </summary>
    public class CompressIfConfiguredSerializerFactory : SerializerFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressIfConfiguredSerializerFactory"/> class.
        /// </summary>
        /// <param name="backingSerializerFactory">A factory that builds the backing serializer to use.</param>
        /// <param name="compressorFactory">The compressor factory to use.</param>
        public CompressIfConfiguredSerializerFactory(
            ISerializerFactory backingSerializerFactory,
            ICompressorFactory compressorFactory)
        {
            if (backingSerializerFactory == null)
            {
                throw new ArgumentNullException(nameof(backingSerializerFactory));
            }

            if (compressorFactory == null)
            {
                throw new ArgumentNullException(nameof(compressorFactory));
            }

            this.BackingSerializerFactory = backingSerializerFactory;
            this.CompressorFactory = compressorFactory;
        }

        /// <summary>
        /// Gets a factory that builds the backing serializer to use.
        /// </summary>
        public ISerializerFactory BackingSerializerFactory { get; }

        /// <summary>
        /// Gets the compressor factory to use.
        /// </summary>
        public ICompressorFactory CompressorFactory { get; }

        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            // Strip off compression before building the backing serializer.
            // The serializer returned by this method is responsible for compression.
            var serializerRepresentationWithNoCompression = serializerRepresentation.DeepCloneWithCompressionKind(
                CompressionKind.None);

            var backingSerializer = this.BackingSerializerFactory.BuildSerializer(
                serializerRepresentationWithNoCompression,
                assemblyVersionMatchStrategy);

            // Wrap the backing serializer in a compressing serializer if appropriate,
            // otherwise just return the backing serializer.
            var result = this.WrapInCompressingSerializerIfAppropriate(
                backingSerializer,
                serializerRepresentation.CompressionKind);

            return result;
        }

        private ISerializer WrapInCompressingSerializerIfAppropriate(
            ISerializer backingSerializer,
            CompressionKind compressionKind)
        {
            ISerializer result;

            switch (compressionKind)
            {
                case CompressionKind.None:
                    result = backingSerializer;
                    break;
                case CompressionKind.DotNetZip:
                    var compressor = this.CompressorFactory.BuildCompressor(compressionKind);
                    result = new ObcCompressingSerializer(backingSerializer, compressor);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(CompressionKind)} is not supported: {compressionKind}."));
            }

            return result;
        }
    }
}
