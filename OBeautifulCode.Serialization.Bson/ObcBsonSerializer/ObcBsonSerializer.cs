// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson;

    using OBeautifulCode.Reflection.Recipes;
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

        private readonly BsonSerializationConfigurationBase bsonSerializationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonSerializer"/> class.
        /// </summary>
        /// <param name="bsonSerializationConfigurationType">Optional <see cref="BsonSerializationConfigurationBase"/> implementation to use; default is <see cref="NullBsonSerializationConfiguration"/>.</param>
        public ObcBsonSerializer(
            BsonSerializationConfigurationType bsonSerializationConfigurationType = null)
            : base(bsonSerializationConfigurationType ?? typeof(NullBsonSerializationConfiguration).ToBsonSerializationConfigurationType())
        {
            this.bsonSerializationConfiguration = (BsonSerializationConfigurationBase)this.SerializationConfiguration;

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
            objectToSerialize = this.WrapRootObjectThatSerializesToStringIfAppropriate(objectToSerialize);

            var objectType = objectToSerialize?.GetType();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            var result = objectToSerialize?.SerializeToBytes();

            return result;
        }

        /// <inheritdoc />
        public override string SerializeToString(
            object objectToSerialize)
        {
            objectToSerialize = this.WrapRootObjectThatSerializesToStringIfAppropriate(objectToSerialize);

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
            byte[] serializedBytes)
        {
            var result = (T)this.Deserialize(serializedBytes, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            type = this.GetRootObjectThatSerializesToStringWrapperTypeIfAppropriate(type);

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            var result = serializedBytes == null
                ? null
                : this.DeserializeSettingSerializationConfigurationInUse(() => serializedBytes.Deserialize(type));

            result = UnwrapRootObjectIfAppropriate(result);

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            string serializedString)
        {
            var result = (T)this.Deserialize(serializedString, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            string serializedString,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            type = this.GetRootObjectThatSerializesToStringWrapperTypeIfAppropriate(type);

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

                result = UnwrapRootObjectIfAppropriate(result);
            }

            return result;
        }

        private static Type GetRootObjectThatSerializesToStringWrapperType(
            Type objectType)
        {
            var result = typeof(RootObjectThatSerializesToStringWrapper<>).MakeGenericType(objectType);

            return result;
        }

        private static object UnwrapRootObjectIfAppropriate(
            object deserializedObject)
        {
            object result;

            if (deserializedObject is IWrapRootObject rootObjectWrapper)
            {
                result = rootObjectWrapper.UntypedRootObject;
            }
            else
            {
                result = deserializedObject;
            }

            return result;
        }

        private void InternalBsonThrowOnUnregisteredTypeIfAppropriate(
            Type objectType,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
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

        private object WrapRootObjectThatSerializesToStringIfAppropriate(
            object objectToSerialize)
        {
            object result;

            if (objectToSerialize == null)
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                result = objectToSerialize;
            }
            else
            {
                var objectType = objectToSerialize.GetType();

                if (this.IsTypeThatSerializesToString(objectType))
                {
                    var wrapperType = GetRootObjectThatSerializesToStringWrapperType(objectType);

                    result = wrapperType.Construct(objectToSerialize);
                }
                else
                {
                    result = objectToSerialize;
                }
            }

            return result;
        }

        private Type GetRootObjectThatSerializesToStringWrapperTypeIfAppropriate(
            Type objectType)
        {
            var result = this.IsTypeThatSerializesToString(objectType)
                ? GetRootObjectThatSerializesToStringWrapperType(objectType)
                : objectType;

            return result;
        }

        private bool IsTypeThatSerializesToString(
            Type objectType)
        {
            var result = (objectType == typeof(string)) || this.bsonSerializationConfiguration.TypesWithCustomStringSerializers.ContainsKey(objectType);

            return result;
        }
    }
}
