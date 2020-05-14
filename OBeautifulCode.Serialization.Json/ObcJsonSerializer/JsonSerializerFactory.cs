﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;

    using static System.FormattableString;

    /// <summary>
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public sealed class JsonSerializerFactory : SerializerFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializerFactory"/> class.
        /// </summary>
        /// <param name="compressorFactory">Optional compressor factory to use.  DEFAULT is to use OBeautifulCode.Compression.Recipes.CompressorFactory.Instance.</param>
        public JsonSerializerFactory(
            ICompressorFactory compressorFactory = null)
            : base(compressorFactory)
        {
        }

        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            new { serializerRepresentation }.AsArg().Must().NotBeNull();

            // ReSharper disable once RedundantArgumentDefaultValue
            var configurationType = serializerRepresentation.SerializationConfigType?.ResolveFromLoadedTypes(assemblyMatchStrategy, throwIfCannotResolve: true);

            ISerializer serializer;

            switch (serializerRepresentation.SerializationKind)
            {
                case SerializationKind.Json:
                    serializer = new ObcJsonSerializer(configurationType?.ToJsonSerializationConfigurationType());
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(serializerRepresentation)} from enumeration {nameof(SerializationKind)} of {serializerRepresentation.SerializationKind} is not supported."));
            }

            var result = this.WrapInCompressingSerializerIfAppropriate(serializer, serializerRepresentation.CompressionKind);

            return result;
        }
    }
}
