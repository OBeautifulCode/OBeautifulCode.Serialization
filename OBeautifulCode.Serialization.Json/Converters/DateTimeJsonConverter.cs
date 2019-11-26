// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Custom <see cref="DateTime"/> converter to do the right thing.
    /// </summary>
    internal class DateTimeJsonConverter : JsonConverter
    {
        private static readonly IStringSerializeAndDeserialize UnderlyingSerializer = new ObcDateTimeStringSerializer();

        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            var payload = value == null ? null : UnderlyingSerializer.SerializeToString((DateTime)value);

            var payloadObject = new JValue(payload);

            payloadObject.WriteTo(writer);
        }

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            new { reader }.AsArg().Must().NotBeNull();
            new { serializer }.AsArg().Must().NotBeNull();

            object result;

            if (reader.TokenType == JsonToken.Null)
            {
                result = null;
            }
            else
            {
                var payload = reader.Value;

                result = payload == null ? null : UnderlyingSerializer.Deserialize(payload.ToString(), typeof(DateTime));
            }

            return result;
        }

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType)
        {
            var result = (objectType != null) && ((objectType == typeof(DateTime)) || (objectType == typeof(DateTime?)));

            return result;
        }
    }
}
