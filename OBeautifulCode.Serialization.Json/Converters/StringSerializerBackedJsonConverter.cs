// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializerBackedJsonConverter.cs" company="OBeautifulCode">
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
    public class StringSerializerBackedJsonConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSerializerBackedJsonConverter"/> class.
        /// </summary>
        /// <param name="registeredType">The type that the converter is registered for.</param>
        /// <param name="backingSerializer">The backing serializer.</param>
        /// <param name="canConvertTypeMatchStrategy">OPTIONAL strategy to use to match the incoming type-to-consider with <paramref name="registeredType"/>.  DEFAULT is match when the types are equal.</param>
        public StringSerializerBackedJsonConverter(
            Type registeredType,
            IStringSerializeAndDeserialize backingSerializer,
            CanConvertTypeMatchStrategy canConvertTypeMatchStrategy = CanConvertTypeMatchStrategy.TypeToConsiderEqualsRegisteredType)
        {
            new { registeredType }.AsArg().Must().NotBeNull();
            new { backingSerializer }.AsArg().Must().NotBeNull();

            this.RegisteredType = registeredType;
            this.BackingSerializer = backingSerializer;
            this.CanConvertTypeMatchStrategy = canConvertTypeMatchStrategy;
        }

        /// <summary>
        /// Gets the type that the converter is registered for.
        /// </summary>
        public Type RegisteredType { get; }

        /// <summary>
        /// Gets the underlying serializer.
        /// </summary>
        public IStringSerializeAndDeserialize BackingSerializer { get; }

        /// <summary>
        /// Gets the strategy to use to match the incoming type-to-consider with <see cref="RegisteredType"/>.
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
                    result = objectType == this.RegisteredType;
                    break;
                case CanConvertTypeMatchStrategy.TypeToConsiderIsAssignableToRegisteredType:
                    result = this.RegisteredType.IsAssignableFrom(objectType);
                    break;
                case CanConvertTypeMatchStrategy.TypeToConsiderIsAssignableFromRegisteredType:
                    result = objectType.IsAssignableFrom(this.RegisteredType);
                    break;
                case CanConvertTypeMatchStrategy.TypeToConsiderIsAssignableToOrFromRegisteredType:
                    result = this.RegisteredType.IsAssignableFrom(objectType) || objectType.IsAssignableFrom(this.RegisteredType);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(this.CanConvertTypeMatchStrategy)} is not supported: {this.CanConvertTypeMatchStrategy}."));
            }

            return result;
        }
    }
}
