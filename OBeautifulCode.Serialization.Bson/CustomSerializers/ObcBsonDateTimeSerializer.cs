﻿// <copyright file="ObcBsonDateTimeSerializer.cs" company="OBeautifulCode">
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
}