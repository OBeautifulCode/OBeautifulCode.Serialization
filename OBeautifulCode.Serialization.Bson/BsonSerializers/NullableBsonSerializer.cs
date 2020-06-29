// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Represents a serializer for <see cref="Nullable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    public class NullableBsonSerializer<T> : SerializerBase<T?>
        where T : struct
    {
        /// <inheritdoc />
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            T? value)
        {
            new { context }.AsArg().Must().NotBeNull();

            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                var serializer = typeof(T).GetAppropriateSerializer();

                serializer.Serialize(context, args, value.Value);
            }
        }

        /// <inheritdoc />
        public override T? Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();
            new { context.Reader }.AsArg().Must().NotBeNull();

            T? result;

            if ((context.Reader.State != BsonReaderState.Type) && (context.Reader.CurrentBsonType == BsonType.Null))
            {
                context.Reader.ReadNull();

                result = null;
            }
            else
            {
                var bsonType = context.Reader.GetCurrentBsonType();

                if (bsonType == BsonType.Null)
                {
                    result = null;
                }
                else
                {
                    var serializer = typeof(T).GetAppropriateSerializer();

                    result = (T?)serializer.Deserialize(context, args);
                }
            }

            return result;
        }
    }
}