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
    public class ObcBsonSerializer : ConfiguredSerializerBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Keeping for easy extension.")]
        private readonly BsonSerializationConfigurationBase bsonConfiguration;

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
            this.bsonConfiguration = (BsonSerializationConfigurationBase)this.SerializationConfiguration;
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.Bson;

        /// <inheritdoc />
        public override byte[] SerializeToBytes(object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType);

            if (objectToSerialize == null)
            {
                return null;
            }

            return ObcBsonSerializerHelper.SerializeToBytes(objectToSerialize);
        }

        /// <inheritdoc />
        public override T Deserialize<T>(byte[] serializedBytes)
        {
            var objectType = typeof(T);

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType);

            if (serializedBytes == null)
            {
                return default(T);
            }

            return ObcBsonSerializerHelper.Deserialize<T>(serializedBytes);
        }

        /// <inheritdoc />
        public override object Deserialize(byte[] serializedBytes, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(type);

            if (serializedBytes == null)
            {
                return null;
            }

            return ObcBsonSerializerHelper.Deserialize(serializedBytes, type);
        }

        /// <inheritdoc />
        public override string SerializeToString(object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType);

            if (objectToSerialize == null)
            {
                return SerializationConfigurationBase.NullSerializedStringValue;
            }

            var document = ObcBsonSerializerHelper.SerializeToDocument(objectToSerialize);
            var json = document.ToJson();
            return json;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(string serializedString)
        {
            var objectType = typeof(T);

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(objectType);

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return default(T);
            }

            var document = serializedString.ToBsonDocument();
            return ObcBsonSerializerHelper.DeserializeFromDocument<T>(document);
        }

        /// <inheritdoc />
        public override object Deserialize(string serializedString, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.InternalBsonThrowOnUnregisteredTypeIfAppropriate(type);

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return null;
            }

            var document = serializedString.ToBsonDocument();
            return ObcBsonSerializerHelper.DeserializeFromDocument(document, type);
        }

        private void InternalBsonThrowOnUnregisteredTypeIfAppropriate(Type objectType)
        {
            if (objectType == typeof(string))
            {
                throw new NotSupportedException("String is not supported as a root type by the underlying BSON Serializer.");
            }

            this.ThrowOnUnregisteredTypeIfAppropriate(objectType);
        }
    }

    /// <inheritdoc />
    public sealed class ObcBsonSerializer<TBsonSerializationConfiguration> : ObcBsonSerializer
        where TBsonSerializationConfiguration : BsonSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonSerializer{TBsonSerializationConfiguration}"/> class.
        /// </summary>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        public ObcBsonSerializer(
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
            : base(typeof(TBsonSerializationConfiguration).ToBsonSerializationConfigurationType(), unregisteredTypeEncounteredStrategy)
        {
        }
    }
}
