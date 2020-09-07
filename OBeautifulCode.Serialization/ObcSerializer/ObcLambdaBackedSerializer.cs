// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcLambdaBackedSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Serializer that is backed by <see cref="Func{T1,TResult}" />.
    /// </summary>
    public class ObcLambdaBackedSerializer : ISerializer
    {
        private readonly Func<object, byte[]> serializeBytes;

        private readonly Func<byte[], Type, object> deserializeBytes;

        private readonly ObcLambdaBackedStringSerializer stringSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcLambdaBackedSerializer"/> class.
        /// </summary>
        /// <param name="serializeString">Serialize object to string.</param>
        /// <param name="deserializeString">Deserialize object from string.</param>
        /// <param name="serializeBytes">Serialize object to bytes.</param>
        /// <param name="deserializeBytes">Deserialize object from bytes.</param>
        /// <param name="id">Optional identifier to be stored in metadata of <see cref="SerializerRepresentation"/>.  DEFAULT is null.</param>
        public ObcLambdaBackedSerializer(
            Func<object, string> serializeString,
            Func<string, Type, object> deserializeString,
            Func<object, byte[]> serializeBytes,
            Func<byte[], Type, object> deserializeBytes,
            string id = null)
        {
            this.stringSerializer = new ObcLambdaBackedStringSerializer(serializeString, deserializeString);

            if (serializeBytes == null)
            {
                throw new ArgumentNullException(nameof(serializeBytes));
            }

            if (deserializeBytes == null)
            {
                throw new ArgumentNullException(nameof(deserializeBytes));
            }

            this.serializeBytes = serializeBytes;
            this.deserializeBytes = deserializeBytes;

            this.SerializerRepresentation = new SerializerRepresentation(SerializationKind.LambdaBacked, metadata: new Dictionary<string, string> { { nameof(id), id } });
        }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => null;

        /// <inheritdoc />
        public SerializationKind SerializationKind => SerializationKind.LambdaBacked;

        /// <inheritdoc />
        public SerializerRepresentation SerializerRepresentation { get; }

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var result = this.serializeBytes(objectToSerialize);

            return result;
        }

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            var result = this.stringSerializer.SerializeToString(objectToSerialize);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            var result = this.stringSerializer.Deserialize<T>(serializedString);

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            var result = this.stringSerializer.Deserialize(serializedString, type);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            byte[] serializedBytes)
        {
            var result = (T)this.Deserialize(serializedBytes, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = this.deserializeBytes(serializedBytes, type);

            return result;
        }
    }
}