// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Compression.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Internal;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serialization-related extension methods.
    /// </summary>
    public static class DomainExtensions
    {
        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerialization">Object to serialize.</param>
        /// <param name="serializerDescription">Description of the serializer to use.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="compressorFactory">Implementation of <see cref="ICompressorFactory" /> that can resolve the compressor.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <returns>
        /// Self described serialization.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static DescribedSerialization ToDescribedSerializationUsingSpecificFactory<T>(
            this T objectToPackageIntoDescribedSerialization,
            SerializerDescription serializerDescription,
            ISerializerFactory serializerFactory,
            ICompressorFactory compressorFactory,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            new { serializerDescription }.AsArg().Must().NotBeNull();
            new { serializerFactory }.AsArg().Must().NotBeNull();
            new { compressorFactory }.AsArg().Must().NotBeNull();

            var serializer = serializerFactory.BuildSerializer(serializerDescription, assemblyMatchStrategy, unregisteredTypeEncounteredStrategy);

            var compressor = compressorFactory.BuildCompressor(serializerDescription.CompressionKind);

            var ret = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificSerializer(serializerDescription, serializer, compressor);

            return ret;
        }

        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerialization">Object to serialize.</param>
        /// <param name="serializerDescription">Description of the serializer to use.</param>
        /// <param name="serializer">Serializer to use.</param>
        /// <param name="compressor">Optional compressor to use; DEFAULT is null.</param>
        /// <returns>
        /// Self described serialization.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static DescribedSerialization ToDescribedSerializationUsingSpecificSerializer<T>(
            this T objectToPackageIntoDescribedSerialization,
            SerializerDescription serializerDescription,
            ISerialize serializer,
            ICompress compressor = null)
        {
            new { serializerDescription }.AsArg().Must().NotBeNull();
            new { serializer }.AsArg().Must().NotBeNull();

            var localCompressor = compressor ?? new NullCompressor();

            localCompressor.CompressionKind.AsArg(Invariant($"{nameof(serializerDescription)}.{nameof(serializerDescription.CompressionKind)}-Must-match-{nameof(compressor)}.{nameof(compressor.CompressionKind)}")).Must().BeEqualTo(serializerDescription.CompressionKind);

            string payload;

            switch (serializerDescription.SerializationFormat)
            {
                case SerializationFormat.Binary:
                    var rawBytes = serializer.SerializeToBytes(objectToPackageIntoDescribedSerialization);
                    var compressedBytes = localCompressor.CompressBytes(rawBytes);
                    payload = Convert.ToBase64String(compressedBytes);
                    break;
                case SerializationFormat.String:
                    payload = serializer.SerializeToString(objectToPackageIntoDescribedSerialization);
                    break;
                default: throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {serializerDescription.SerializationFormat} is not supported."));
            }

            var payloadType = objectToPackageIntoDescribedSerialization?.GetType() ?? typeof(T);

            if (payloadType.IsClosedAnonymousType())
            {
                payloadType = typeof(DynamicTypePlaceholder);
            }

            var result = new DescribedSerialization(payloadType.ToRepresentation(), payload, serializerDescription);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="compressorFactory">Implementation of <see cref="ICompressorFactory" /> that can resolve the compressor.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        public static object DeserializePayloadUsingSpecificFactory(
            this DescribedSerialization describedSerialization,
            ISerializerFactory serializerFactory,
            ICompressorFactory compressorFactory,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            new { describedSerialization }.AsArg().Must().NotBeNull();
            new { serializerFactory }.AsArg().Must().NotBeNull();
            new { compressorFactory }.AsArg().Must().NotBeNull();

            var serializer = serializerFactory.BuildSerializer(describedSerialization.SerializerDescription, assemblyMatchStrategy, unregisteredTypeEncounteredStrategy);

            var compressor = compressorFactory.BuildCompressor(describedSerialization.SerializerDescription.CompressionKind);

            var result = describedSerialization.DeserializePayloadUsingSpecificSerializer(serializer, compressor, assemblyMatchStrategy);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="deserializer">Deserializer to use.</param>
        /// <param name="decompressor">Optional compressor to use; DEFAULT is null.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "deserializer", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "decompressor", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static object DeserializePayloadUsingSpecificSerializer(
            this DescribedSerialization describedSerialization,
            IDeserialize deserializer,
            IDecompress decompressor = null,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            new { describedSerialization }.AsArg().Must().NotBeNull();
            new { deserializer }.AsArg().Must().NotBeNull();

            var localDecompressor = decompressor ?? new NullCompressor();

            var targetType = describedSerialization.PayloadTypeRepresentation.ResolveFromLoadedTypes(assemblyMatchStrategy);

            object result;

            switch (describedSerialization.SerializerDescription.SerializationFormat)
            {
                case SerializationFormat.Binary:
                    var rawBytes = Convert.FromBase64String(describedSerialization.SerializedPayload);
                    var decompressedBytes = localDecompressor.DecompressBytes(rawBytes);
                    result = deserializer.Deserialize(decompressedBytes, targetType);
                    break;
                case SerializationFormat.String:
                    result = deserializer.Deserialize(describedSerialization.SerializedPayload, targetType);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {describedSerialization.SerializerDescription.SerializationFormat} is not supported."));
            }

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="compressorFactory">Implementation of <see cref="ICompressorFactory" /> that can resolve the compressor.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        public static T DeserializePayloadUsingSpecificFactory<T>(
            this DescribedSerialization describedSerialization,
            ISerializerFactory serializerFactory,
            ICompressorFactory compressorFactory,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            var result = (T)DeserializePayloadUsingSpecificFactory(describedSerialization, serializerFactory, compressorFactory, assemblyMatchStrategy, unregisteredTypeEncounteredStrategy);

            return result;
        }
    }
}