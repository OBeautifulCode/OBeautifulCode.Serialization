// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConverterBuilder.cs" company="OBeautifulCode">
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
    /// Builds a serializing and deserializing <see cref="JsonConverter"/>.
    /// </summary>
    public class JsonConverterBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConverterBuilder"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the builder.</param>
        /// <param name="serializingConverterBuilderFunc">A func that builds the serializing <see cref="JsonConverter"/>.</param>
        /// <param name="deserializingConverterBuilderFunc">A func that builds the deserializing <see cref="JsonConverter"/>.</param>
        /// <param name="outputKind">The output kind of the converter.</param>
        public JsonConverterBuilder(
            string id,
            Func<JsonConverter> serializingConverterBuilderFunc,
            Func<JsonConverter> deserializingConverterBuilderFunc,
            JsonConverterOutputKind outputKind)
        {
            new { id }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { serializingConverterBuilderFunc }.AsArg().Must().NotBeNull();
            new { deserializingConverterBuilderFunc }.AsArg().Must().NotBeNull();
            new { outputKind }.AsArg().Must().NotBeEqualTo(JsonConverterOutputKind.Unknown);

            this.Id = id;
            this.SerializingConverterBuilderFunc = serializingConverterBuilderFunc;
            this.DeserializingConverterBuilderFunc = deserializingConverterBuilderFunc;
            this.OutputKind = outputKind;
        }

        /// <summary>
        /// Gets the unique identifier of the builder.
        /// </summary>
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
        /// Gets the output kind of the converter.
        /// </summary>
        /// <remarks>
        /// If a custom converter outputs a string, then Dictionaries that are keyed on the type can be written
        /// using a standard JSON format for dictionaries.  If, however, the converter does not output a string,
        /// then any dictionaries keyed on the type must be written as a list of key/value pairs.
        /// </remarks>
        public JsonConverterOutputKind OutputKind { get; }

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