// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializerBackedJsonConverter{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A converter that is backed by a specified <see cref="ISerializeAndDeserialize"/>.
    /// </summary>
    /// <typeparam name="T">The type that this converter is registered for.</typeparam>
    public class StringSerializerBackedJsonConverter<T> : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSerializerBackedJsonConverter{T}"/> class.
        /// </summary>
        /// <param name="backingSerializer">The backing serializer.</param>
        /// <param name="canConvertTypeMatchStrategy">OPTIONAL strategy to use to match the incoming type-to-consider with <typeparamref name="T"/>.  DEFAULT is match when the types are equal.</param>
        public StringSerializerBackedJsonConverter(
            IStringSerializeAndDeserialize backingSerializer,
            CanConvertTypeMatchStrategy canConvertTypeMatchStrategy = CanConvertTypeMatchStrategy.TypeToConsiderEqualsRegisteredType)
        {
            new { backingSerializer }.AsArg().Must().NotBeNull();

            this.BackingSerializer = backingSerializer;
            this.CanConvertTypeMatchStrategy = canConvertTypeMatchStrategy;
        }

        /// <summary>
        /// Gets the underlying serializer.
        /// </summary>
        public IStringSerializeAndDeserialize BackingSerializer { get; }

        /// <summary>
        /// Gets the strategy to use to match the incoming type-to-consider with <typeparamref name="T"/>.
        /// </summary>
        public CanConvertTypeMatchStrategy CanConvertTypeMatchStrategy { get; }

        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            new { writer }.AsArg().Must().NotBeNull();

            var stringToWrite = this.BackingSerializer.SerializeToString(value);

            if (stringToWrite == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(stringToWrite);
            }
        }

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            new { reader }.AsArg().Must().NotBeNull();

            var result = this.BackingSerializer.Deserialize(reader.Value?.ToString(), objectType);

            return result;
        }

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType)
        {
            new { objectType }.Must().NotBeNull();

            bool result;

            switch (this.CanConvertTypeMatchStrategy)
            {
                case CanConvertTypeMatchStrategy.TypeToConsiderEqualsRegisteredType:
                    result = objectType == typeof(T);
                    break;
                case CanConvertTypeMatchStrategy.TypeToConsiderIsAssignableToRegisteredType:
                    result = typeof(T).IsAssignableFrom(objectType);
                    break;
                case CanConvertTypeMatchStrategy.RegisteredTypeIsAssignableToTypeToConsider:
                    result = objectType.IsAssignableFrom(typeof(T));
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(this.CanConvertTypeMatchStrategy)} is not supported: {this.CanConvertTypeMatchStrategy}."));
            }

            return result;
        }
    }
}
