// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Model object to describe a serializer so you can persist and share the definition and rehydrate the serializer later.
    /// </summary>
    public partial class SerializerDescription : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerDescription"/> class.
        /// </summary>
        /// <param name="serializationKind">The <see cref="SerializationKind" /> to serialize into.</param>
        /// <param name="serializationFormat">The <see cref="SerializationFormat" /> to serialize into.</param>
        /// <param name="compressionKind">Optional <see cref="CompressionKind" /> to use; DEFAULT is None.</param>
        /// <param name="configurationTypeRepresentation">Optional configuration to use; DEFAULT is null.</param>
        /// <param name="metadata">Optional metadata to put, especially useful for customer serializer factory; DEFAULT is empty.</param>
        public SerializerDescription(
            SerializationKind serializationKind,
            SerializationFormat serializationFormat,
            TypeRepresentation configurationTypeRepresentation = null,
            CompressionKind compressionKind = CompressionKind.None,
            IReadOnlyDictionary<string, string> metadata = null)
        {
            new { serializationKind }.AsArg().Must().NotBeEqualTo(SerializationKind.Invalid);
            new { serializationFormat }.AsArg().Must().NotBeEqualTo(SerializationFormat.Invalid);
            new { compressionKind }.AsArg().Must().NotBeEqualTo(CompressionKind.Invalid);

            this.SerializationKind = serializationKind;
            this.SerializationFormat = serializationFormat;
            this.ConfigurationTypeRepresentation = configurationTypeRepresentation;
            this.CompressionKind = compressionKind;
            this.Metadata = metadata ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the <see cref="SerializationKind" /> to serialize into.
        /// </summary>
        public SerializationKind SerializationKind { get; private set; }

        /// <summary>
        /// Gets the <see cref="SerializationFormat" /> to serialize into.
        /// </summary>
        public SerializationFormat SerializationFormat { get; private set; }

        /// <summary>
        /// Gets the <see cref="CompressionKind" /> to use.
        /// </summary>
        public CompressionKind CompressionKind { get; private set; }

        /// <summary>
        /// Gets the <see cref="TypeRepresentation" /> of the configuration.
        /// </summary>
        public TypeRepresentation ConfigurationTypeRepresentation { get; private set; }

        /// <summary>
        /// Gets a map of metadata for custom use.
        /// </summary>
        public IReadOnlyDictionary<string, string> Metadata { get; private set; }
    }
}