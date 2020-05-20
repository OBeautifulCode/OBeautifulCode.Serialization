// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MongoDB.Bson;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// BSON serializer with optional configuration type.
    /// </summary>
    public class ObcBsonSerializer : ObcSerializerBase
    {
        /// <summary>
        /// Gets the serialization configuration of the serializer being used for deserialization on the current thread.
        /// </summary>
        /// <remarks>
        /// This is a hack to compensate for our inability to pass a context object into Mongo that gets passed
        /// around during the lifecycle of a deserialization operation.  ObcBsonDiscriminatorConvention needs
        /// the serialization configuration of the serializer that is performing the deserialization operation
        /// so that it can call ThrowOnUnregisteredTypeIfAppropriate().
        /// </remarks>
        [ThreadStatic]
        #pragma warning disable SA1401
        private static SerializationConfigurationBase serializationConfigurationInUseForDeserialization;
        #pragma warning restore SA1401

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonSerializer"/> class.
        /// </summary>
        /// <param name="bsonSerializationConfigurationType">Optional <see cref="BsonSerializationConfigurationBase"/> implementation to use; default is <see cref="NullBsonSerializationConfiguration"/>.</param>
        public ObcBsonSerializer(
            BsonSerializationConfigurationType bsonSerializationConfigurationType = null)
            : base(bsonSerializationConfigurationType ?? typeof(NullBsonSerializationConfiguration).ToBsonSerializationConfigurationType())
        {
            this.SerializerRepresentation = new SerializerRepresentation(SerializationKind.Bson, bsonSerializationConfigurationType?.ConcreteSerializationConfigurationDerivativeType.ToRepresentation());
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.Bson;

        /// <inheritdoc />
        public override SerializerRepresentation SerializerRepresentation { get; }

        /// <summary>
        /// Gets the serialization configuration of the serializer being used for deserialization.
        /// </summary>
        /// <returns>
        /// The serialization configuration of the serializer being used for deserialization.
        /// </returns>
        public static SerializationConfigurationBase GetSerializationConfigurationInUseForDeserialization()
        {
            var result = serializationConfigurationInUseForDeserialization;

            return result;
        }

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
                : this.DeserializeSettingSerializationConfigurationInUse(serializedBytes.Deserialize<T>);

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            var result = serializedBytes == null
                ? null
                : this.DeserializeSettingSerializationConfigurationInUse(() => serializedBytes.Deserialize(type));

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

                result = this.DeserializeSettingSerializationConfigurationInUse(() => document.DeserializeFromDocument<T>());
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

                result = this.DeserializeSettingSerializationConfigurationInUse(() => document.DeserializeFromDocument(type));
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

            this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(objectType, serializationDirection, objectToSerialize);
        }

        private T DeserializeSettingSerializationConfigurationInUse<T>(
            Func<T> deserializationOperation)
        {
            try
            {
                serializationConfigurationInUseForDeserialization = this.SerializationConfiguration;

                var result = deserializationOperation();

                return result;
            }
            finally
            {
                serializationConfigurationInUseForDeserialization = null;
            }
        }
    }
}
