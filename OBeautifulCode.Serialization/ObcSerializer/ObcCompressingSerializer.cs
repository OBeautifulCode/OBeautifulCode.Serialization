// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcCompressingSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// A serializer that compresses after serialization with a backing serializer
    /// and decompresses before de-serialization using a backing serializer.
    /// </summary>
    public class ObcCompressingSerializer : ISerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcCompressingSerializer"/> class.
        /// </summary>
        /// <param name="backingSerializer">The backing serializer to use.</param>
        /// <param name="compressor">The compressor to use.</param>
        public ObcCompressingSerializer(
            ISerializer backingSerializer,
            ICompressor compressor)
        {
            if (backingSerializer == null)
            {
                throw new ArgumentNullException(nameof(backingSerializer));
            }

            if (compressor == null)
            {
                throw new ArgumentNullException(nameof(compressor));
            }

            this.BackingSerializer = backingSerializer;
            this.Compressor = compressor;
            this.SerializerRepresentation = new SerializerRepresentation(backingSerializer.SerializationKind, backingSerializer.SerializationConfigurationType?.ConcreteSerializationConfigurationDerivativeType.ToRepresentation(), compressor.CompressionKind);
        }

        /// <summary>
        /// Gets the backing serializer to use.
        /// </summary>
        public ISerializer BackingSerializer { get; }

        /// <summary>
        /// Gets the compressor to use.
        /// </summary>
        public ICompressor Compressor { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => this.BackingSerializer.SerializationConfigurationType;

        /// <inheritdoc />
        public SerializationKind SerializationKind => this.BackingSerializer.SerializationKind;

        /// <inheritdoc />
        public SerializerRepresentation SerializerRepresentation { get; }

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var bytes = this.BackingSerializer.SerializeToBytes(objectToSerialize);

            var result = this.Compressor.CompressBytes(bytes);

            return result;
        }

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            var compressedBytes = this.SerializeToBytes(objectToSerialize);

            var result = Convert.ToBase64String(compressedBytes);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            var compressedBytes = Convert.FromBase64String(serializedString);

            var result = this.Deserialize<T>(compressedBytes);

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            var compressedBytes = Convert.FromBase64String(serializedString);

            var result = this.Deserialize(compressedBytes, type);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            byte[] serializedBytes)
        {
            var bytes = this.Compressor.DecompressBytes(serializedBytes);

            var result = this.BackingSerializer.Deserialize<T>(bytes);

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            var bytes = this.Compressor.DecompressBytes(serializedBytes);

            var result = this.BackingSerializer.Deserialize(bytes, type);

            return result;
        }
    }
}
