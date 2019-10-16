// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredJsonConverter.cs" company="OBeautifulCode">
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
    /// Json converter to use.
    /// </summary>
    public class RegisteredJsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredJsonConverter"/> class.
        /// </summary>
        /// <param name="serializingConverterBuilderFunction">Builder function for the converter to use when serializing.</param>
        /// <param name="deserializingConverterBuilderFunction">Builder function for the converter to use when deserializing.</param>
        /// <param name="outputKind"><see cref="RegisteredJsonConverterOutputKind" /> of this converter.</param>
        /// <param name="handledTypes"><see cref="Type" />'s handled by this converter.</param>
        public RegisteredJsonConverter(Func<JsonConverter> serializingConverterBuilderFunction, Func<JsonConverter> deserializingConverterBuilderFunction, RegisteredJsonConverterOutputKind outputKind, IReadOnlyCollection<Type> handledTypes)
        {
            new { serializingConverterBuilderFunction }.AsArg().Must().NotBeNull();
            new { deserializingConverterBuilderFunction }.AsArg().Must().NotBeNull();
            new { handledTypes }.AsArg().Must().NotBeNull().And().NotBeEmptyEnumerable();

            this.SerializingConverterBuilderFunction = serializingConverterBuilderFunction;
            this.DeserializingConverterBuilderFunction = deserializingConverterBuilderFunction;
            this.OutputKind = outputKind;
            this.HandledTypes = handledTypes;
        }

        /// <summary>
        /// Gets the builder function for the converter to use when serializing.
        /// </summary>
        public Func<JsonConverter> SerializingConverterBuilderFunction { get; private set; }

        /// <summary>
        /// Gets the builder function for the converter to use when deserializing.
        /// </summary>
        public Func<JsonConverter> DeserializingConverterBuilderFunction { get; private set; }

        /// <summary>
        /// Gets the <see cref="RegisteredJsonConverterOutputKind" />.
        /// </summary>
        public RegisteredJsonConverterOutputKind OutputKind { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type" /> that this converter will handle.
        /// </summary>
        public IReadOnlyCollection<Type> HandledTypes { get; private set; }
    }

#pragma warning disable SA1201 // Elements should appear in the correct order

    /// <summary>
    /// Enumeration of the outputs of the <see cref="RegisteredJsonConverter" />.
    /// </summary>
    public enum RegisteredJsonConverterOutputKind
    {
        /// <summary>
        /// Completely unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Outputs what JSON will consider a string.
        /// </summary>
        String,

        /// <summary>
        /// Outputs what JSON will consider an object (i.e. a start object is emitted.)
        /// </summary>
        Object,
    }

#pragma warning restore SA1201 // Elements should appear in the correct order
}