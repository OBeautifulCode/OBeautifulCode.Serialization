// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcStringSerializerBackedSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Serializer that is backed by a <see cref="IStringSerializeAndDeserialize"/>.
    /// </summary>
    /// <remarks>
    /// Binary serialization will be the UTF-8 byte representation of the resulting string of the backing serializer.
    /// </remarks>
    public class ObcStringSerializerBackedSerializer : ISerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcStringSerializerBackedSerializer"/> class.
        /// </summary>
        /// <param name="backingStringSerializer">Backing string serializer.</param>
        /// <param name="id">Optional identifier to be stored in metadata of <see cref="SerializerRepresentation"/>.  DEFAULT is null.</param>
        public ObcStringSerializerBackedSerializer(
            IStringSerializeAndDeserialize backingStringSerializer,
            string id = null)
        {
            if (backingStringSerializer == null)
            {
                throw new ArgumentNullException(nameof(backingStringSerializer));
            }

            this.BackingStringSerializer = backingStringSerializer;

            this.SerializerRepresentation = new SerializerRepresentation(SerializationKind.StringSerializerBacked, metadata: new Dictionary<string, string> { { nameof(id), id } });
        }

        /// <summary>
        /// Gets the encoding for binary serialization.
        /// </summary>
        public static Encoding Encoding => Encoding.UTF8;

        /// <summary>
        /// Gets the backing string serializer.
        /// </summary>
        public IStringSerializeAndDeserialize BackingStringSerializer { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => null;

        /// <inheritdoc />
        public SerializationKind SerializationKind => SerializationKind.StringSerializerBacked;

        /// <inheritdoc />
        public SerializerRepresentation SerializerRepresentation { get; }

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var serializedString = this.BackingStringSerializer.SerializeToString(objectToSerialize);

            var result = Encoding.GetBytes(serializedString);

            return result;
        }

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            var result = this.BackingStringSerializer.SerializeToString(objectToSerialize);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            var result = this.BackingStringSerializer.Deserialize<T>(serializedString);

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = this.BackingStringSerializer.Deserialize(serializedString, type);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            byte[] serializedBytes)
        {
            var serializedString = Encoding.GetString(serializedBytes);

            var result = this.BackingStringSerializer.Deserialize<T>(serializedString);

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

            var serializedString = Encoding.GetString(serializedBytes);

            var result = this.BackingStringSerializer.Deserialize(serializedString, type);

            return result;
        }
    }
}