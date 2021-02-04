// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcAlwaysThrowingSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Serializer that always throws (<see cref="NotSupportedException"/>).
    /// </summary>
    /// <remarks>
    /// This is useful in testing to prove that a serializer is never called.
    /// </remarks>
    public class ObcAlwaysThrowingSerializer : ISerializer
    {
        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => throw new NotSupportedException();

        /// <inheritdoc />
        public SerializationKind SerializationKind => throw new NotSupportedException();

        /// <inheritdoc />
        public SerializerRepresentation SerializerRepresentation => throw new NotSupportedException();

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize) => throw new NotSupportedException();

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize) => throw new NotSupportedException();

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString) => throw new NotSupportedException();

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type) => throw new NotSupportedException();

        /// <inheritdoc />
        public T Deserialize<T>(
            byte[] serializedBytes) => throw new NotSupportedException();

        /// <inheritdoc />
        public object Deserialize(
            byte[] serializedBytes,
            Type type) => throw new NotSupportedException();
    }
}