// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerDescription.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Representation.System;

    using static System.FormattableString;

    /// <summary>
    /// Model object to describe a serializer so you can persist and share the definition and rehydrate the serializer later.
    /// </summary>
    public class SerializerDescription : IEquatable<SerializerDescription>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerDescription"/> class.
        /// </summary>
        /// <param name="serializationKind">The <see cref="SerializationKind" /> to serialize into.</param>
        /// <param name="serializationFormat">The <see cref="SerializationFormat" /> to serialize into.</param>
        /// <param name="compressionKind">Optional <see cref="CompressionKind" /> to use; DEFAULT is None.</param>
        /// <param name="configurationTypeRepresentation">Optional configuration to use; DEFAULT is null.</param>
        /// <param name="metadata">Optional metadata to put, especially useful for customer serializer factory; DEFAULT is empty.</param>
        public SerializerDescription(SerializationKind serializationKind, SerializationFormat serializationFormat, TypeRepresentation configurationTypeRepresentation = null, CompressionKind compressionKind = CompressionKind.None, IReadOnlyDictionary<string, string> metadata = null)
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

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are equal.</returns>
        public static bool operator ==(SerializerDescription first, SerializerDescription second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
            {
                return false;
            }

            var metadataEqual = first.Metadata.IsEqualTo(second.Metadata);

            return first.SerializationKind == second.SerializationKind && first.SerializationFormat == second.SerializationFormat
                                                                       && first.CompressionKind == second.CompressionKind
                                                                       && first.ConfigurationTypeRepresentation == second.ConfigurationTypeRepresentation
                                                                       && metadataEqual;
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="first">First parameter.</param>
        /// <param name="second">Second parameter.</param>
        /// <returns>A value indicating whether or not the two items are inequal.</returns>
        public static bool operator !=(SerializerDescription first, SerializerDescription second) => !(first == second);

        /// <inheritdoc />
        public override string ToString()
        {
            var metadataString = string.Join(",", this.Metadata.Select(_ => Invariant($"[{_.Key}={_.Value}]")));
            var result = Invariant($"{nameof(SerializerDescription)}: {nameof(this.SerializationKind)}={this.SerializationKind}, {nameof(this.SerializationFormat)}={this.SerializationFormat}, {nameof(this.CompressionKind)}={this.CompressionKind}, {nameof(this.ConfigurationTypeRepresentation)}={this.ConfigurationTypeRepresentation}, {nameof(this.Metadata)}={metadataString},");
            return result;
        }

        /// <inheritdoc />
        public bool Equals(SerializerDescription other) => this == other;

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as SerializerDescription);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.SerializationKind)
            .Hash(this.SerializationFormat)
            .Hash(this.CompressionKind)
            .Hash(this.ConfigurationTypeRepresentation)
            .Hash(this.Metadata).Value;
    }
}