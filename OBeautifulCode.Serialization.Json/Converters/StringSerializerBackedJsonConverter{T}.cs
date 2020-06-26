// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializerBackedJsonConverter{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    /// <summary>
    /// A converter that is backed by a specified <see cref="ISerializeAndDeserialize"/>.
    /// </summary>
    /// <typeparam name="T">The type that this converter is registered for.</typeparam>
    public class StringSerializerBackedJsonConverter<T> : StringSerializerBackedJsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSerializerBackedJsonConverter{T}"/> class.
        /// </summary>
        /// <param name="backingSerializer">The backing serializer.</param>
        /// <param name="canConvertTypeMatchStrategy">OPTIONAL strategy to use to match the incoming type-to-consider with <typeparamref name="T"/>.  DEFAULT is match when the types are equal.</param>
        public StringSerializerBackedJsonConverter(
            IStringSerializeAndDeserialize backingSerializer,
            CanConvertTypeMatchStrategy canConvertTypeMatchStrategy = CanConvertTypeMatchStrategy.TypeToConsiderEqualsRegisteredType)
        : base(typeof(T), backingSerializer, canConvertTypeMatchStrategy)
        {
        }
    }
}
