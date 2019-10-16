// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableColorSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Drawing;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;

    using static System.FormattableString;

    /// <inheritdoc />
    internal class NullableColorSerializer : SerializerBase<Color?>
    {
        /// <inheritdoc />
        public override Color? Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();

            var type = context.Reader.GetCurrentBsonType();

            Color? result;
            switch (type)
            {
                case BsonType.Null:
                    context.Reader.ReadNull();
                    result = null;
                    break;
                case BsonType.String:
                    result = ColorTranslator.FromHtml(context.Reader.ReadString());
                    break;
                default:
                    throw new NotSupportedException(Invariant($"Cannot convert a {type} to a {typeof(Color).Name}."));
            }

            return result;
        }

        /// <inheritdoc />
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            Color? value)
        {
            new { context }.AsArg().Must().NotBeNull();

            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                var colorHtml = ColorTranslator.ToHtml((Color)value);
                context.Writer.WriteString(colorHtml);
            }
        }
    }
}
