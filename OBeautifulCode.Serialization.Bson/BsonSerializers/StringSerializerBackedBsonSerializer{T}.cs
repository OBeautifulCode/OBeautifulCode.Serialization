// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializerBackedBsonSerializer{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A BSON serializer backed by a <see cref="IStringSerializeAndDeserialize"/>.
    /// </summary>
    /// <typeparam name="T">The type that the serializer is registered for.</typeparam>
    public class StringSerializerBackedBsonSerializer<T> : SerializerBase<T>
    {
        private readonly IStringSerializeAndDeserialize backingSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSerializerBackedBsonSerializer{T}"/> class.
        /// </summary>
        /// <param name="backingSerializer">The backing serializer.</param>
        public StringSerializerBackedBsonSerializer(
            IStringSerializeAndDeserialize backingSerializer)
        {
            this.backingSerializer = backingSerializer;
        }

        /// <inheritdoc />
        public override T Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();

            string serializedPayload;

            var bsonType = context.Reader.GetCurrentBsonType();

            if (bsonType == BsonType.Null)
            {
                context.Reader.ReadNull();

                serializedPayload = null;
            }
            else if (bsonType == BsonType.String)
            {
                serializedPayload = context.Reader.ReadString();
            }
            else
            {
                throw new NotSupportedException(Invariant($"Cannot convert a {bsonType} to a {this.ValueType.ToStringReadable()}."));
            }

            var result = (T)this.backingSerializer.Deserialize(serializedPayload, this.ValueType);

            return result;
        }

        /// <inheritdoc />
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            T value)
        {
            new { context }.AsArg().Must().NotBeNull();

            var serializedString = this.backingSerializer.SerializeToString(value);

            if (serializedString == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                context.Writer.WriteString(serializedString);
            }
        }
    }
}