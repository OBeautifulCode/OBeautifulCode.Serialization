﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcLambdaBackedSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Serializer that is backed by <see cref="Func{T1,T2,T3,T4,T5,T6,T7,T8,T9,TResult}" />.
    /// </summary>
    public class ObcLambdaBackedSerializer : ISerializeAndDeserialize
    {
        private readonly Func<object, string> serializeString;
        private readonly Func<string, Type, object> deserializeString;
        private readonly Func<object, byte[]> serializeBytes;
        private readonly Func<byte[], Type, object> deserializeBytes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcLambdaBackedSerializer"/> class.
        /// </summary>
        /// <param name="serializeString">Serialize object to string.</param>
        /// <param name="deserializeString">Deserialize object from string.</param>
        /// <param name="serializeBytes">Serialize object to bytes.</param>
        /// <param name="deserializeBytes">Deserialize object from bytes.</param>
        public ObcLambdaBackedSerializer(
            Func<object, string> serializeString,
            Func<string, Type, object> deserializeString,
            Func<object, byte[]> serializeBytes,
            Func<byte[], Type, object> deserializeBytes)
        {
            new { serializeString }.AsArg().Must().NotBeNull();
            new { deserializeString }.AsArg().Must().NotBeNull();
            new { serializeBytes }.AsArg().Must().NotBeNull();
            new { deserializeBytes }.AsArg().Must().NotBeNull();

            this.serializeString = serializeString;
            this.deserializeString = deserializeString;
            this.serializeBytes = serializeBytes;
            this.deserializeBytes = deserializeBytes;
        }

        /// <inheritdoc />
        public Type ConfigurationType => null;

        /// <inheritdoc />
        public SerializationKind SerializationKind => SerializationKind.LambdaBacked;

        /// <inheritdoc />
        public byte[] SerializeToBytes(object objectToSerialize)
        {
            return this.serializeBytes(objectToSerialize);
        }

        /// <inheritdoc />
        public string SerializeToString(object objectToSerialize)
        {
            return this.serializeString(objectToSerialize);
        }

        /// <inheritdoc />
        public T Deserialize<T>(string serializedString)
        {
            return (T)this.Deserialize(serializedString, typeof(T));
        }

        /// <inheritdoc />
        public object Deserialize(string serializedString, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            return this.deserializeString(serializedString, type);
        }

        /// <inheritdoc />
        public T Deserialize<T>(byte[] serializedBytes)
        {
            return (T)this.Deserialize(serializedBytes, typeof(T));
        }

        /// <inheritdoc />
        public object Deserialize(byte[] serializedBytes, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            return this.deserializeBytes(serializedBytes, type);
        }
    }
}