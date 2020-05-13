// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcCompressingSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression;

    /// <summary>
    /// A serializer that compresses after serialization and decompresses before de-serialization.
    /// </summary>
    public class ObcCompressingSerializer : ISerializeAndDeserialize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcCompressingSerializer"/> class.
        /// </summary>
        /// <param name="serializer">The underlying serializer to use.</param>
        /// <param name="compressor">The compressor to use.</param>
        protected ObcCompressingSerializer(
            ISerializeAndDeserialize serializer,
            ICompressAndDecompress compressor)
        {
            new { serializer }.AsArg().Must().NotBeNull();
            new { compressor }.AsArg().Must().NotBeNull();

            this.Serializer = serializer;
            this.Compressor = compressor;
        }

        /// <summary>
        /// Gets the underlying serializer to use.
        /// </summary>
        public ISerializeAndDeserialize Serializer { get; }

        /// <summary>
        /// Gets the compressor to use.
        /// </summary>
        public ICompressAndDecompress Compressor { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => this.Serializer.SerializationConfigurationType;

        /// <inheritdoc />
        public SerializationKind SerializationKind => this.Serializer.SerializationKind;

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var bytes = this.Serializer.SerializeToBytes(objectToSerialize);

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

            var result = this.Serializer.Deserialize<T>(bytes);

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            var bytes = this.Compressor.DecompressBytes(serializedBytes);

            var result = this.Serializer.Deserialize(bytes, type);

            return result;
        }
    }
}
