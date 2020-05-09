// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// BSON serializer with optional configuration type.
    /// </summary>
    public class ObcBsonSerializer : ObcSerializerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonSerializer"/> class.
        /// </summary>
        /// <param name="bsonSerializationConfigurationType">Optional <see cref="BsonSerializationConfigurationBase"/> implementation to use; default is <see cref="NullBsonSerializationConfiguration"/>.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        public ObcBsonSerializer(
            BsonSerializationConfigurationType bsonSerializationConfigurationType = null,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
            : base(bsonSerializationConfigurationType ?? typeof(NullBsonSerializationConfiguration).ToBsonSerializationConfigurationType(), unregisteredTypeEncounteredStrategy)
        {
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.Bson;

        /// <inheritdoc />
        public override byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            var result = objectToSerialize?.SerializeToBytes();

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            byte[] serializedBytes)
        {
            var objectType = typeof(T);

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Deserialize, null);

            var result = serializedBytes == null
                ? default
                : serializedBytes.Deserialize<T>();

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            var result = serializedBytes?.Deserialize(type);

            return result;
        }

        /// <inheritdoc />
        public override string SerializeToString(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            string result;

            if (objectToSerialize == null)
            {
                result = SerializationConfigurationBase.NullSerializedStringValue;
            }
            else
            {
                var document = objectToSerialize.SerializeToDocument();

                result = document.ToJson();
            }

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            string serializedString)
        {
            var objectType = typeof(T);

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Deserialize, null);

            T result;

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                result = default;
            }
            else
            {
                var document = serializedString.ToBsonDocument();

                result = document.DeserializeFromDocument<T>();
            }

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            string serializedString,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            object result;

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                result = null;
            }
            else
            {
                var document = serializedString.ToBsonDocument();

                result = document.DeserializeFromDocument(type);
            }

            return result;
        }

        private void InternalBsonThrowOnUnregisteredTypeIfAppropriate(
            Type objectType,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            if (objectType == typeof(string))
            {
                throw new NotSupportedException("String is not supported as a root type by the underlying BSON Serializer.");
            }

            this.ThrowOnUnregisteredTypeIfAppropriate(objectType, serializationDirection, objectToSerialize);
        }
    }
}
