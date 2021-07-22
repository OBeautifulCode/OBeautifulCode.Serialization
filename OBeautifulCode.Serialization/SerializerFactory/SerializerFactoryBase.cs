// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerFactoryBase.cs" company="OBeautifulCode">
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
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public abstract class SerializerFactoryBase : ISerializerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerFactoryBase"/> class.
        /// </summary>
        /// <param name="compressorFactory">The compressor factory to use.  Set to null to use <see cref="OBeautifulCode.Compression.Recipes.CompressorFactory.Instance"/>.</param>
        protected SerializerFactoryBase(
            ICompressorFactory compressorFactory)
        {
            this.CompressorFactory = compressorFactory ?? OBeautifulCode.Compression.Recipes.CompressorFactory.Instance;
        }

        /// <summary>
        /// Gets the compressor factory to use.
        /// </summary>
        public ICompressorFactory CompressorFactory { get; }

        /// <inheritdoc />
        public abstract ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion);

        /// <summary>
        /// Wraps the specified serializer in a <see cref="ObcCompressingSerializer"/> if appropriate.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="compressionKind">The kind of compression to use.</param>
        /// <returns>
        /// The specified serializer, wrapped in a <see cref="ObcCompressingSerializer"/> if it's appropriate given
        /// the specified <paramref name="compressionKind"/>.
        /// </returns>
        protected ISerializer WrapInCompressingSerializerIfAppropriate(
            ISerializer serializer,
            CompressionKind compressionKind)
        {
            ISerializer result;

            switch (compressionKind)
            {
                case CompressionKind.None:
                    result = serializer;
                    break;
                case CompressionKind.DotNetZip:
                    var compressor = this.CompressorFactory.BuildCompressor(compressionKind);
                    result = new ObcCompressingSerializer(serializer, compressor);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(CompressionKind)} is not supported: {compressionKind}."));
            }

            return result;
        }
    }
}
