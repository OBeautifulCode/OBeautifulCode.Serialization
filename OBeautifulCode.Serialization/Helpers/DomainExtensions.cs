﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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
        /// Gets the representation of the specified serializer.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <returns>
        /// The representation.
        /// </returns>
        public static SerializerRepresentation ToRepresentation(
            this IHaveSerializerRepresentation serializer)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            var result = serializer.SerializerRepresentation;

            return result;
        }

        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerialization">Object to serialize.</param>
        /// <param name="serializerRepresentation">Representation of the serializer to use.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="serializationFormat">The serialization format to use.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Self described serialization.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static DescribedSerialization ToDescribedSerializationUsingSpecificFactory<T>(
            this T objectToPackageIntoDescribedSerialization,
            SerializerRepresentation serializerRepresentation,
            ISerializerFactory serializerFactory,
            SerializationFormat serializationFormat,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            if (serializerFactory == null)
            {
                throw new ArgumentNullException(nameof(serializerFactory));
            }

            if (serializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(serializationFormat)}' is equal to '{SerializationFormat.Invalid}'"), (Exception)null);
            }

            var serializer = serializerFactory.BuildSerializer(serializerRepresentation, assemblyMatchStrategy);

            var ret = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificSerializer(serializer, serializationFormat);

            return ret;
        }

        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerialization">Object to serialize.</param>
        /// <param name="serializer">Serializer to use.</param>
        /// <param name="serializationFormat">The serialization format to use.</param>
        /// <returns>
        /// Self described serialization.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static DescribedSerialization ToDescribedSerializationUsingSpecificSerializer<T>(
            this T objectToPackageIntoDescribedSerialization,
            ISerializer serializer,
            SerializationFormat serializationFormat)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (serializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(serializationFormat)}' is equal to '{SerializationFormat.Invalid}'"), (Exception)null);
            }

            string payload;

            switch (serializationFormat)
            {
                case SerializationFormat.Binary:
                    var bytes = serializer.SerializeToBytes(objectToPackageIntoDescribedSerialization);
                    payload = bytes == null ? null : Convert.ToBase64String(bytes);
                    break;
                case SerializationFormat.String:
                    payload = serializer.SerializeToString(objectToPackageIntoDescribedSerialization);
                    break;
                default: throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {serializationFormat} is not supported."));
            }

            var payloadType = objectToPackageIntoDescribedSerialization?.GetType() ?? typeof(T);

            if (payloadType.IsClosedAnonymousType())
            {
                payloadType = typeof(DynamicTypePlaceholder);
            }

            var result = new DescribedSerialization(payloadType.ToRepresentation(), payload, serializer.SerializerRepresentation, serializationFormat);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        public static T DeserializePayloadUsingSpecificFactory<T>(
            this DescribedSerialization describedSerialization,
            ISerializerFactory serializerFactory,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            var result = (T)DeserializePayloadUsingSpecificFactory(describedSerialization, serializerFactory, assemblyMatchStrategy);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        public static object DeserializePayloadUsingSpecificFactory(
            this DescribedSerialization describedSerialization,
            ISerializerFactory serializerFactory,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            if (describedSerialization == null)
            {
                throw new ArgumentNullException(nameof(describedSerialization));
            }

            if (serializerFactory == null)
            {
                throw new ArgumentNullException(nameof(serializerFactory));
            }

            var serializer = serializerFactory.BuildSerializer(describedSerialization.SerializerRepresentation, assemblyMatchStrategy);

            var result = describedSerialization.DeserializePayloadUsingSpecificSerializer(serializer, assemblyMatchStrategy);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="deserializer">Deserializer to use.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "deserializer", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static T DeserializePayloadUsingSpecificSerializer<T>(
            this DescribedSerialization describedSerialization,
            IDeserialize deserializer,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            var result = (T)DeserializePayloadUsingSpecificSerializer(describedSerialization, deserializer, assemblyMatchStrategy);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="deserializer">Deserializer to use.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "deserializer", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static object DeserializePayloadUsingSpecificSerializer(
            this DescribedSerialization describedSerialization,
            IDeserialize deserializer,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            if (describedSerialization == null)
            {
                throw new ArgumentNullException(nameof(describedSerialization));
            }

            if (deserializer == null)
            {
                throw new ArgumentNullException(nameof(deserializer));
            }

            var targetType = describedSerialization.PayloadTypeRepresentation.ResolveFromLoadedTypes(assemblyMatchStrategy);

            object result;

            switch (describedSerialization.SerializationFormat)
            {
                case SerializationFormat.Binary:
                    var bytes = describedSerialization.SerializedPayload == null ? null : Convert.FromBase64String(describedSerialization.SerializedPayload);
                    result = deserializer.Deserialize(bytes, targetType);
                    break;
                case SerializationFormat.String:
                    result = deserializer.Deserialize(describedSerialization.SerializedPayload, targetType);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {describedSerialization.SerializationFormat} is not supported."));
            }

            return result;
        }

        /// <summary>
        /// Resolves <see cref="RelatedTypesToInclude.Default"/> into
        /// an actionable <see cref="RelatedTypesToInclude"/> for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// An actionable <see cref="RelatedTypesToInclude"/> resolved for the specified type,
        /// when <see cref="RelatedTypesToInclude.Default"/> is specified.
        /// </returns>
        public static RelatedTypesToInclude ResolveDefaultIntoActionableRelatedTypesToInclude(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            RelatedTypesToInclude result;

            if (type.IsInterface)
            {
                result = RelatedTypesToInclude.Descendants;
            }
            else if (type.IsAbstract)
            {
                result = RelatedTypesToInclude.Descendants;
            }
            else
            {
                result = RelatedTypesToInclude.None;
            }

            return result;
        }
    }
}