// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConverterBuilder.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using Newtonsoft.Json;

    using static System.FormattableString;

    /// <summary>
    /// Builds a serializing and deserializing <see cref="JsonConverter"/>.
    /// </summary>
    public class JsonConverterBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConverterBuilder"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the builder.  This is used to de-dupe builders.  We want to avoid adding the same converters to the converter stack multiple times when multiple <see cref="TypeToRegisterForJson"/> objects have the same builder, because it degrades performance.</param>
        /// <param name="serializingConverterBuilderFunc">A func that builds the serializing <see cref="JsonConverter"/>.</param>
        /// <param name="deserializingConverterBuilderFunc">A func that builds the deserializing <see cref="JsonConverter"/>.</param>
        public JsonConverterBuilder(
            string id,
            Func<JsonConverter> serializingConverterBuilderFunc,
            Func<JsonConverter> deserializingConverterBuilderFunc)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(Invariant($"'{nameof(id)}' is white space"));
            }

            if (serializingConverterBuilderFunc == null)
            {
                throw new ArgumentNullException(nameof(serializingConverterBuilderFunc));
            }

            if (deserializingConverterBuilderFunc == null)
            {
                throw new ArgumentNullException(nameof(deserializingConverterBuilderFunc));
            }

            this.Id = id;
            this.SerializingConverterBuilderFunc = serializingConverterBuilderFunc;
            this.DeserializingConverterBuilderFunc = deserializingConverterBuilderFunc;
        }

        /// <summary>
        /// Gets the unique identifier of the builder.
        /// </summary>
        /// <remarks>
        /// This is used to de-dupe builders.
        /// We want to avoid adding the same converters to the converter stack multiple times when multiple
        /// <see cref="TypeToRegisterForJson"/> objects have the same builder, because it degrades performance.
        /// </remarks>
        public string Id { get; }

        /// <summary>
        /// Gets a func that builds the serializing <see cref="JsonConverter"/>.
        /// </summary>
        public Func<JsonConverter> SerializingConverterBuilderFunc { get; }

        /// <summary>
        /// Gets a func that builds the deserializing <see cref="JsonConverter"/>.
        /// </summary>
        public Func<JsonConverter> DeserializingConverterBuilderFunc { get; }

        /// <summary>
        /// Gets the func that builds a <see cref="JsonConverter"/> for the specified <see cref="SerializationDirection"/>.
        /// </summary>
        /// <param name="serializationDirection">The serialization direction.</param>
        /// <returns>
        /// The func that builds a <see cref="JsonConvert"/> for the specified <see cref="SerializationDirection"/>.
        /// </returns>
        public Func<JsonConverter> GetJsonConverterBuilderFuncBySerializationDirection(
            SerializationDirection serializationDirection)
        {
            switch (serializationDirection)
            {
                case SerializationDirection.Serialize:
                    return this.SerializingConverterBuilderFunc;
                case SerializationDirection.Deserialize:
                    return this.DeserializingConverterBuilderFunc;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(SerializationDirection)} is not supported: {serializationDirection}"));
            }
        }
    }
}