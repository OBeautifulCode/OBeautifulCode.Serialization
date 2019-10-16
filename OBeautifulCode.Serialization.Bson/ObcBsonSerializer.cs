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

    using static System.FormattableString;

    /// <summary>
    /// Mongo BSON serializer with optional configuration type.
    /// </summary>
    public class ObcBsonSerializer : SerializerBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Keeping for easy extension.")]
        private readonly BsonConfigurationBase bsonConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonSerializer"/> class.
        /// </summary>
        /// <param name="configurationType">Optional <see cref="BsonConfigurationBase"/> implementation to use; default is <see cref="NullBsonConfiguration"/>.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        public ObcBsonSerializer(
            Type configurationType = null,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
            : base(configurationType ?? typeof(NullBsonConfiguration), unregisteredTypeEncounteredStrategy)
        {
            if (configurationType != null)
            {
                configurationType.IsSubclassOf(typeof(BsonConfigurationBase)).AsArg(
                    Invariant($"Configuration type - {configurationType.FullName} - must derive from {nameof(BsonConfigurationBase)}.")).Must().BeTrue();

                configurationType.HasParameterlessConstructor().AsArg(
                    Invariant($"{nameof(configurationType)} must contain a default constructor to use in {nameof(ObcBsonSerializer)}.")).Must().BeTrue();
            }

            this.bsonConfiguration = (BsonConfigurationBase)this.configuration;
        }

        /// <inheritdoc />
        public override SerializationKind Kind => SerializationKind.Bson;

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
    public sealed class ObcBsonSerializer<TBsonConfiguration> : ObcBsonSerializer
        where TBsonConfiguration : BsonConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonSerializer{TBsonConfiguration}"/> class.
        /// </summary>
        public ObcBsonSerializer()
            : base(typeof(TBsonConfiguration))
        {
        }
    }
}
