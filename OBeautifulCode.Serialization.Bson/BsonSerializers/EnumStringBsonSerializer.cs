// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumStringBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    /// <summary>
    /// Represents a serializer for enums, including support for <see cref="FlagsAttribute"/> ones.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    public class EnumStringBsonSerializer<TEnum> : StructSerializerBase<TEnum>
        where TEnum : struct
    {
        /// <inheritdoc />
        public override TEnum Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var bsonReader = context.Reader;

            var stringValue = bsonReader.ReadString();

            var result = (TEnum)Enum.Parse(typeof(TEnum), stringValue);

            return result;
        }

        /// <inheritdoc />
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            TEnum value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var bsonWriter = context.Writer;

            bsonWriter.WriteString(value.ToString());
        }
    }
}