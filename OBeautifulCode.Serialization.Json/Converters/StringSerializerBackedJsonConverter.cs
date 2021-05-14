// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializerBackedJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;

    using NewtonsoftFork.Json;

    using static System.FormattableString;

    /// <summary>
    /// A converter that is backed by a specified <see cref="ISerializeAndDeserialize"/>.
    /// </summary>
    public class StringSerializerBackedJsonConverter : JsonConverter
    {
        private readonly ConcurrentDictionary<Type, bool> cachedTypeToCanCovertMap = new ConcurrentDictionary<Type, bool>();

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
            if (registeredType == null)
            {
                throw new ArgumentNullException(nameof(registeredType));
            }

            if (backingSerializer == null)
            {
                throw new ArgumentNullException(nameof(backingSerializer));
            }

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
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

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
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var result = this.BackingSerializer.Deserialize(reader.Value?.ToString(), objectType);

            return result;
        }

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            if (this.cachedTypeToCanCovertMap.TryGetValue(objectType, out bool result))
            {
                return result;
            }

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

            this.cachedTypeToCanCovertMap.TryAdd(objectType, result);

            return result;
        }
    }
}
