// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerRepresentation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    using static System.FormattableString;

    /// <summary>
    /// Model object that represents a serializer so you can persist and share the definition and rehydrate the serializer later.
    /// </summary>
    public partial class SerializerRepresentation : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerRepresentation"/> class.
        /// </summary>
        /// <param name="serializationKind">The <see cref="SerializationKind" /> to serialize into.</param>
        /// <param name="compressionKind">Optional <see cref="CompressionKind" /> to use; DEFAULT is None.</param>
        /// <param name="serializationConfigType">Optional representation of the type of the serialization configuration to use; DEFAULT is null.</param>
        /// <param name="metadata">Optional metadata to put, especially useful for custom serializer factory; DEFAULT is empty.</param>
        public SerializerRepresentation(
            SerializationKind serializationKind,
            TypeRepresentation serializationConfigType = null,
            CompressionKind compressionKind = CompressionKind.None,
            IReadOnlyDictionary<string, string> metadata = null)
        {
            if (serializationKind == SerializationKind.Invalid)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(serializationKind)}' == '{SerializationKind.Invalid}'"), (Exception)null);
            }

            if (compressionKind == CompressionKind.Invalid)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(compressionKind)}' == '{CompressionKind.Invalid}'"), (Exception)null);
            }

            this.SerializationKind = serializationKind;
            this.SerializationConfigType = serializationConfigType;
            this.CompressionKind = compressionKind;
            this.Metadata = metadata ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the <see cref="SerializationKind" /> to serialize into.
        /// </summary>
        public SerializationKind SerializationKind { get; private set; }

        /// <summary>
        /// Gets the <see cref="TypeRepresentation" /> of the type of the serialization configuration.
        /// </summary>
        public TypeRepresentation SerializationConfigType { get; private set; }

        /// <summary>
        /// Gets the <see cref="CompressionKind" /> to use.
        /// </summary>
        public CompressionKind CompressionKind { get; private set; }

        /// <summary>
        /// Gets a map of metadata for custom use.
        /// </summary>
        public IReadOnlyDictionary<string, string> Metadata { get; private set; }
    }
}