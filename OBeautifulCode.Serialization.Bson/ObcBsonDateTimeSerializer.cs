// <copyright file="ObcBsonDateTimeSerializer.cs" company="OBeautifulCode">
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

    using static System.FormattableString;

    /// <summary>
    /// Custom <see cref="DateTime"/> serializer to do the right thing.
    /// </summary>
    public class ObcBsonDateTimeSerializer : SerializerBase<DateTime>
    {
        private static readonly IStringSerializeAndDeserialize StringSerializer = new ObcDateTimeStringSerializer();

        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            new { context }.AsArg().Must().NotBeNull();

            context.Writer.WriteString(StringSerializer.SerializeToString(value));
        }

        /// <inheritdoc />
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();

            var type = context.Reader.GetCurrentBsonType();
            switch (type)
            {
                case BsonType.String:
                    return StringSerializer.Deserialize<DateTime>(context.Reader.ReadString());
                default:
                    throw new NotSupportedException(Invariant($"Cannot convert a {type} to a {nameof(DateTime)}."));
            }
        }
    }

    /// <summary>
    /// Custom <see cref="Nullable{DateTime}"/> serializer to do the right thing.
    /// </summary>
    public class ObcBsonNullableDateTimeSerializer : SerializerBase<DateTime?>
    {
        private static readonly IStringSerializeAndDeserialize StringSerializer = new ObcDateTimeStringSerializer();

        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime? value)
        {
            new { context }.AsArg().Must().NotBeNull();

            var stringValue = StringSerializer.SerializeToString(value);

            if (stringValue == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                context.Writer.WriteString(stringValue);
            }
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Is checked.")]
        public override DateTime? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();
            new { context.Reader }.AsArg().Must().NotBeNull();

            if (context.Reader.State != BsonReaderState.Type && context.Reader.CurrentBsonType == BsonType.Null)
            {
                context.Reader.ReadNull();
                return null;
            }

            var type = context.Reader.GetCurrentBsonType();
            switch (type)
            {
                case BsonType.Null:
                    return null;
                case BsonType.String:
                    return StringSerializer.Deserialize<DateTime?>(context.Reader.ReadString());
                default:
                    throw new NotSupportedException(Invariant($"Cannot convert a {type} to a {nameof(Nullable<DateTime>)}."));
            }
        }
    }
}