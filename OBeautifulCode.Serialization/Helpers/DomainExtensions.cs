﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Representation.System;
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
        /// <param name="objectToPackageIntoDescribedSerializationBase">Object to serialize.</param>
        /// <param name="serializerRepresentation">Representation of the serializer to use.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="serializationFormat">The serialization format to use.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Self described serialization.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static DescribedSerializationBase ToDescribedSerializationUsingSpecificFactory<T>(
            this T objectToPackageIntoDescribedSerializationBase,
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
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(serializationFormat)}' == '{SerializationFormat.Invalid}'"), (Exception)null);
            }

            var serializer = serializerFactory.BuildSerializer(serializerRepresentation, assemblyMatchStrategy);

            var ret = objectToPackageIntoDescribedSerializationBase.ToDescribedSerializationUsingSpecificSerializer(serializer, serializationFormat);

            return ret;
        }

        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerializationBase">Object to serialize.</param>
        /// <param name="serializer">Serializer to use.</param>
        /// <param name="serializationFormat">The serialization format to use.</param>
        /// <returns>
        /// Self described serialization.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static DescribedSerializationBase ToDescribedSerializationUsingSpecificSerializer<T>(
            this T objectToPackageIntoDescribedSerializationBase,
            ISerializer serializer,
            SerializationFormat serializationFormat)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (serializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(serializationFormat)}' == '{SerializationFormat.Invalid}'"), (Exception)null);
            }

            var payloadType = objectToPackageIntoDescribedSerializationBase?.GetType() ?? typeof(T);
            if (payloadType.IsClosedAnonymousType())
            {
                payloadType = typeof(DynamicTypePlaceholder);
            }

            DescribedSerializationBase result;
            switch (serializationFormat)
            {
                case SerializationFormat.Binary:
                    var serializedBytes = serializer.SerializeToBytes(objectToPackageIntoDescribedSerializationBase);
                    result = new BinaryDescribedSerialization(payloadType.ToRepresentation(), serializer.SerializerRepresentation, serializedBytes);
                    break;
                case SerializationFormat.String:
                    var serializedString = serializer.SerializeToString(objectToPackageIntoDescribedSerializationBase);
                    result = new StringDescribedSerialization(payloadType.ToRepresentation(), serializer.SerializerRepresentation, serializedString);
                    break;
                default: throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {serializationFormat} is not supported."));
            }

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="describedSerializationBase">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        public static T DeserializePayloadUsingSpecificFactory<T>(
            this DescribedSerializationBase describedSerializationBase,
            ISerializerFactory serializerFactory,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            var result = (T)DeserializePayloadUsingSpecificFactory(describedSerializationBase, serializerFactory, assemblyMatchStrategy);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerializationBase">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        public static object DeserializePayloadUsingSpecificFactory(
            this DescribedSerializationBase describedSerializationBase,
            ISerializerFactory serializerFactory,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            if (describedSerializationBase == null)
            {
                throw new ArgumentNullException(nameof(describedSerializationBase));
            }

            if (serializerFactory == null)
            {
                throw new ArgumentNullException(nameof(serializerFactory));
            }

            var serializer = serializerFactory.BuildSerializer(describedSerializationBase.SerializerRepresentation, assemblyMatchStrategy);

            var result = describedSerializationBase.DeserializePayloadUsingSpecificSerializer(serializer, assemblyMatchStrategy);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="describedSerializationBase">Self described serialized object.</param>
        /// <param name="deserializer">Deserializer to use.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "deserializer", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static T DeserializePayloadUsingSpecificSerializer<T>(
            this DescribedSerializationBase describedSerializationBase,
            IDeserialize deserializer,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            var result = (T)DeserializePayloadUsingSpecificSerializer(describedSerializationBase, deserializer, assemblyMatchStrategy);

            return result;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerializationBase">Self described serialized object.</param>
        /// <param name="deserializer">Deserializer to use.</param>
        /// <param name="assemblyMatchStrategy">Optional assembly match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="AssemblyMatchStrategy.AnySingleVersion" />.</param>
        /// <returns>
        /// Originally serialized object.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "deserializer", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static object DeserializePayloadUsingSpecificSerializer(
            this DescribedSerializationBase describedSerializationBase,
            IDeserialize deserializer,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion)
        {
            if (describedSerializationBase == null)
            {
                throw new ArgumentNullException(nameof(describedSerializationBase));
            }

            if (deserializer == null)
            {
                throw new ArgumentNullException(nameof(deserializer));
            }

            var targetType = describedSerializationBase.PayloadTypeRepresentation.ResolveFromLoadedTypes(assemblyMatchStrategy);

            object result;

            var serializationFormat = describedSerializationBase.GetSerializationFormat();

            switch (serializationFormat)
            {
                case SerializationFormat.Binary:
                    var describedSerializationBinary = (BinaryDescribedSerialization)describedSerializationBase;
                    var serializedBytes = describedSerializationBinary.SerializedPayload;
                    result = serializedBytes == null ? null : deserializer.Deserialize(serializedBytes, targetType);
                    break;
                case SerializationFormat.String:
                    var describedSerializationString = (StringDescribedSerialization)describedSerializationBase;
                    var serializedString = describedSerializationString.SerializedPayload;
                    result = serializedString == null ? null : deserializer.Deserialize(serializedString, targetType);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {serializationFormat} is not supported."));
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

            // Is an abstract class or an interface (IsAbstract returns true for interfaces)
            var result = type.IsAbstract
                ? RelatedTypesToInclude.Descendants
                : RelatedTypesToInclude.None;

            return result;
        }
    }
}