// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcNullSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Null object pattern implementation of an <see cref="ISerializer"/>.
    /// </summary>
    public class ObcNullSerializer : ISerializer
    {
        private static readonly SerializerRepresentation CachedSerializerRepresentation = new SerializerRepresentation(SerializationKind.Proprietary);

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => null;

        /// <inheritdoc />
        public SerializationKind SerializationKind => CachedSerializerRepresentation.SerializationKind;

        /// <inheritdoc />
        public SerializerRepresentation SerializerRepresentation => CachedSerializerRepresentation;

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize) => null;

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize) => null;

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString) => default;

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type) => null;

        /// <inheritdoc />
        public T Deserialize<T>(
            byte[] serializedBytes) => default;

        /// <inheritdoc />
        public object Deserialize(
            byte[] serializedBytes,
            Type type) => null;
    }
}