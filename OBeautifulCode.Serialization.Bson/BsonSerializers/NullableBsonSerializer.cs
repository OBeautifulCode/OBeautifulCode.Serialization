// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Globalization;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                // ObjectSerializer handles values types (confirmed by browsing BSON source for that type),
                // so we want to default to that if a serializer is not found
                var serializer = typeof(T).GetAppropriateSerializer(defaultToObjectSerializer: true);

                serializer.Serialize(context, args, value.Value);
            }
        }

        /// <inheritdoc />
        public override T? Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Reader == null)
            {
                throw new ArgumentNullException(nameof(context.Reader));
            }

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
                    // ObjectSerializer handles values types (confirmed by browsing BSON source for that type),
                    // so we want to default to that if a serializer is not found
                    var serializer = typeof(T).GetAppropriateSerializer(defaultToObjectSerializer: true);

                    var deserialized = serializer.Deserialize(context, args);

                    var expectedType = typeof(T);

                    if (deserialized == null)
                    {
                        throw new InvalidOperationException(Invariant($"When deserializing BSON Type '{bsonType}' into '{expectedType.ToStringReadable()}', got null which was unexpected."));
                    }

                    var deserializedType = deserialized.GetType();

                    if (deserializedType == expectedType)
                    {
                        result = (T?)deserialized;
                    }
                    else if (deserializedType == typeof(string))
                    {
                        // We have observed that 'decimal?' and 'int?' are stored as a string when a document is persisted
                        // to MongoDB, despite it being a 'decimal?' in the BSON format.  This code is thus
                        // needed to convert the string to the expected type.
                        // We have observed that 'bool?' is stored as a boolean.
                        result = (T?)Convert.ChangeType(deserialized, expectedType, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        throw new InvalidOperationException(Invariant($"When deserializing BSON Type '{bsonType}' into '{expectedType.ToStringReadable()}', BSON returned the following unexpected type: '{deserializedType.ToStringReadable()}'."));
                    }
                }
            }

            return result;
        }
    }
}