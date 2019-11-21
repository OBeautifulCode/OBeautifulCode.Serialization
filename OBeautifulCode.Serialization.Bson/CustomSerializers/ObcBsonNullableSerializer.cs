// <copyright file="ObcBsonNullableSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Represents a serializer for enums, including support for <see cref="FlagsAttribute"/> ones.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    public class ObcBsonNullableSerializer<T> : SerializerBase<T?>
        where T : struct
    {
        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T? value)
        {
            new { context }.AsArg().Must().NotBeNull();

            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                var serializer = BsonConfigurationBase.GetAppropriateSerializer(typeof(T));

                serializer.Serialize(context, args, value.Value);
            }
        }

        /// <inheritdoc />
        public override T? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();
            new { context.Reader }.AsArg().Must().NotBeNull();

            if (context.Reader.State != BsonReaderState.Type && context.Reader.CurrentBsonType == BsonType.Null)
            {
                context.Reader.ReadNull();

                return null;
            }

            var type = context.Reader.GetCurrentBsonType();

            if (type == BsonType.Null)
            {
                return null;
            }

            var serializer = BsonConfigurationBase.GetAppropriateSerializer(typeof(T));

            var result = serializer.Deserialize(context, args);

            return (T?)result;
        }
    }
}