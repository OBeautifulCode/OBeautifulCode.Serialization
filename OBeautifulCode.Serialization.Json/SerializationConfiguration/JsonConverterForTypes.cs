// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConverterForTypes.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Specifies a serializing and deserializing <see cref="JsonConverter"/> that should be used for a specified set of types.
    /// </summary>
    public class JsonConverterForTypes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConverterForTypes"/> class.
        /// </summary>
        /// <param name="serializingConverterBuilderFunc">A func that builds the serializing <see cref="JsonConverter"/>.</param>
        /// <param name="deserializingConverterBuilderFunc">A func that builds the deserializing <see cref="JsonConverter"/>.</param>
        /// <param name="outputKind">The output kind of the converter.</param>
        /// <param name="handledTypes">The set of types that should be handled by the serializing and deserializing <see cref="JsonConverter"/>.</param>
        public JsonConverterForTypes(
            Func<JsonConverter> serializingConverterBuilderFunc,
            Func<JsonConverter> deserializingConverterBuilderFunc,
            JsonConverterOutputKind outputKind,
            IReadOnlyCollection<Type> handledTypes)
        {
            new { serializingConverterBuilderFunc }.AsArg().Must().NotBeNull();
            new { deserializingConverterBuilderFunc }.AsArg().Must().NotBeNull();
            new { handledTypes }.AsArg().Must().NotBeNull().And().NotBeEmptyEnumerable();

            this.SerializingConverterBuilderFunc = serializingConverterBuilderFunc;
            this.DeserializingConverterBuilderFunc = deserializingConverterBuilderFunc;
            this.OutputKind = outputKind;
            this.HandledTypes = handledTypes;
        }

        /// <summary>
        /// Gets a func that builds the serializing <see cref="JsonConverter"/>.
        /// </summary>
        public Func<JsonConverter> SerializingConverterBuilderFunc { get; }

        /// <summary>
        /// Gets a func that builds the deserializing <see cref="JsonConverter"/>.
        /// </summary>
        public Func<JsonConverter> DeserializingConverterBuilderFunc { get; }

        /// <summary>
        /// Gets a value that specifies the kind of the converter.
        /// </summary>
        public JsonConverterOutputKind OutputKind { get; }

        /// <summary>
        /// Gets the set of types that should be handled by the serializing and deserializing <see cref="JsonConverter"/>.
        /// </summary>
        public IReadOnlyCollection<Type> HandledTypes { get; }
    }
}