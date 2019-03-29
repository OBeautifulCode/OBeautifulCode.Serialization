﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Serialization.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Naos.Compression.Domain;

    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Validation.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class DomainExtensions
    {
        /// <summary>
        /// Resolve the <see cref="TypeDescription" /> from the loaded types.
        /// </summary>
        /// <param name="typeDescription">Type description to look for.</param>
        /// <param name="typeMatchStrategy">Strategy to use for equality when matching.</param>
        /// <param name="multipleMatchStrategy">Strategy to use with collisions when matching.</param>
        /// <returns>Matched type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Keeping all together.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Keeping all together.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Want to swallow that specific exception.")]
        public static Type ResolveFromLoadedTypes(this TypeDescription typeDescription, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            new { typeDescription }.Must().NotBeNull();

            // first deal with special hack implementation of array types...
            if (typeDescription.Name.Contains("[]") || typeDescription.AssemblyQualifiedName.Contains("[]"))
            {
                var arrayItemTypeDescription = new TypeDescription
                {
                    AssemblyQualifiedName = typeDescription.AssemblyQualifiedName.Replace("[]", string.Empty),
                    Namespace = typeDescription.Namespace,
                    Name = typeDescription.Name.Replace("[]", string.Empty),
                };

                var arrayItemType = arrayItemTypeDescription.ResolveFromLoadedTypes(typeMatchStrategy, multipleMatchStrategy);
                return arrayItemType?.MakeArrayType();
            }

            // if it's not an array type then run normal logic
            var loadedAssemblies = AssemblyLoader.GetLoadedAssemblies().Distinct().ToList();
            var allTypes = new List<Type>();
            var reflectionTypeLoadExceptions = new List<ReflectionTypeLoadException>();
            foreach (var assembly in loadedAssemblies)
            {
                try
                {
                    allTypes.AddRange(new[] { assembly }.GetTypesFromAssemblies());
                }
                catch (TypeLoadException ex) when (ex.InnerException?.GetType() == typeof(ReflectionTypeLoadException))
                {
                    var reflectionTypeLoadException = (ReflectionTypeLoadException)ex.InnerException;
                    allTypes.AddRange(reflectionTypeLoadException.Types);
                    reflectionTypeLoadExceptions.Add(reflectionTypeLoadException);
                }
            }

            AggregateException accumulatedReflectionTypeLoadExceptions = reflectionTypeLoadExceptions.Any()
                ? new AggregateException(Invariant($"Getting types from assemblies threw one or more {nameof(ReflectionTypeLoadException)}.  See inner exceptions."), reflectionTypeLoadExceptions)
                : null;

            allTypes = allTypes.Where(_ => _ != null).Distinct().ToList();
            var typeComparer = new TypeComparer(typeMatchStrategy);
            var allMatchingTypes = allTypes.Where(_ =>
            {
                TypeDescription description = null;

                try
                {
                    /* For types that have dependent assemblies that are not found on disk this will fail when it tries to get properties from the type.
                     * Added because we encountered a FileNotFoundException for an assembly that was not on disk when taking a loaded type and calling
                     * ToTypeDescription on it (specifically it threw on the type.Namespace getter call).
                     */

                    description = _.ToTypeDescription();
                }
                catch (Exception)
                {
                    return false;
                }

                if (description == null)
                {
                    return false;
                }

                return typeComparer.Equals(description, typeDescription);
            }).ToList();

            Type result;
            switch (multipleMatchStrategy)
            {
                case MultipleMatchStrategy.ThrowOnMultiple:
                    if (allMatchingTypes.Count > 1)
                    {
                        var message = "Found multiple versions and multiple match strategy was: " + multipleMatchStrategy;
                        var types = string.Join(",", allMatchingTypes.Select(_ => _.AssemblyQualifiedName + " at " + _.Assembly.CodeBase));
                        throw new InvalidOperationException(message + "; types found: " + types, accumulatedReflectionTypeLoadExceptions);
                    }
                    else
                    {
                        result = allMatchingTypes.SingleOrDefault();
                    }

                    break;
                case MultipleMatchStrategy.NewestVersion:
                    result = allMatchingTypes.OrderByDescending(_ => (_.Assembly.GetName().Version ?? new Version(0, 0, 0, 1)).ToString()).FirstOrDefault();
                    break;
                case MultipleMatchStrategy.OldestVersion:
                    result = allMatchingTypes.OrderBy(_ => (_.Assembly.GetName().Version ?? new Version(0, 0, 0, 1)).ToString()).FirstOrDefault();
                    break;
                default:
                    throw new NotSupportedException("Multiple match strategy not supported: " + multipleMatchStrategy);
            }

            if ((accumulatedReflectionTypeLoadExceptions != null) && (result == null))
            {
                throw accumulatedReflectionTypeLoadExceptions;
            }

            return result;
        }

        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerialization">Object to serialize.</param>
        /// <param name="serializationDescription">Description of the serializer to use.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="compressorFactory">Implementation of <see cref="ICompressorFactory" /> that can resolve the compressor.</param>
        /// <param name="typeMatchStrategy">Optional type match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="TypeMatchStrategy.NamespaceAndName" />.</param>
        /// <param name="multipleMatchStrategy">Optional multiple match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="MultipleMatchStrategy.ThrowOnMultiple" />.</param>
        /// <returns>Self decribed serialization.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "3", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "Spelling/name is correct.")]
        public static DescribedSerialization ToDescribedSerializationUsingSpecificFactory<T>(
            this T objectToPackageIntoDescribedSerialization,
            SerializationDescription serializationDescription,
            ISerializerFactory serializerFactory,
            ICompressorFactory compressorFactory,
            TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName,
            MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            new { serializationDescription }.Must().NotBeNull();
            new { serializerFactory }.Must().NotBeNull();
            new { compressorFactory }.Must().NotBeNull();

            var serializer = serializerFactory.BuildSerializer(serializationDescription, typeMatchStrategy, multipleMatchStrategy);
            var compressor = compressorFactory.BuildCompressor(serializationDescription.CompressionKind);

            var ret = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificSerializer(
                serializationDescription,
                serializer,
                compressor);

            return ret;
        }

        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerialization">Object to serialize.</param>
        /// <param name="serializationDescription">Description of the serializer to use.</param>
        /// <param name="serializer">Serializer to use.</param>
        /// <param name="compressor">Optional compressor to use; DEFAULT is null.</param>
        /// <returns>Self decribed serialization.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "3", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "Spelling/name is correct.")]
        public static DescribedSerialization ToDescribedSerializationUsingSpecificSerializer<T>(
            this T objectToPackageIntoDescribedSerialization,
            SerializationDescription serializationDescription,
            ISerialize serializer,
            ICompress compressor = null)
        {
            new { serializationDescription }.Must().NotBeNull();
            new { serializer }.Must().NotBeNull();

            var localCompressor = compressor ?? new NullCompressor();

            localCompressor.CompressionKind.Named(Invariant($"{nameof(serializationDescription)}.{nameof(serializationDescription.CompressionKind)}-Must-match-{nameof(compressor)}.{nameof(compressor.CompressionKind)}")).Must().BeEqualTo(serializationDescription.CompressionKind);

            string payload;
            switch (serializationDescription.SerializationFormat)
            {
                case SerializationFormat.Binary:
                    var rawBytes = serializer.SerializeToBytes(objectToPackageIntoDescribedSerialization);
                    var compressedBytes = localCompressor.CompressBytes(rawBytes);
                    payload = Convert.ToBase64String(compressedBytes);
                    break;
                case SerializationFormat.String:
                    payload = serializer.SerializeToString(objectToPackageIntoDescribedSerialization);
                    break;
                default: throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {serializationDescription.SerializationFormat} is not supported."));
            }

            var payloadType = objectToPackageIntoDescribedSerialization?.GetType() ?? typeof(T);
            if (payloadType.IsAnonymous())
            {
                payloadType = typeof(DynamicTypePlaceholder);
            }

            var ret = new DescribedSerialization(
                payloadType.ToTypeDescription(),
                payload,
                serializationDescription);

            return ret;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="compressorFactory">Implementation of <see cref="ICompressorFactory" /> that can resolve the compressor.</param>
        /// <param name="typeMatchStrategy">Optional type match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="TypeMatchStrategy.NamespaceAndName" />.</param>
        /// <param name="multipleMatchStrategy">Optional multiple match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="MultipleMatchStrategy.ThrowOnMultiple" />.</param>
        /// <returns>Originally serialized object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Checked with Must and tested.")]
        public static object DeserializePayloadUsingSpecificFactory(
            this DescribedSerialization describedSerialization,
            ISerializerFactory serializerFactory,
            ICompressorFactory compressorFactory,
            TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName,
            MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            new { describedSerialization }.Must().NotBeNull();
            new { serializerFactory }.Must().NotBeNull();
            new { compressorFactory }.Must().NotBeNull();

            var serializer = serializerFactory.BuildSerializer(describedSerialization.SerializationDescription, typeMatchStrategy, multipleMatchStrategy);
            var compressor = compressorFactory.BuildCompressor(describedSerialization.SerializationDescription.CompressionKind);

            var ret = describedSerialization.DeserializePayloadUsingSpecificSerializer(serializer, compressor, typeMatchStrategy, multipleMatchStrategy);
            return ret;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="deserializer">Deserializer to use.</param>
        /// <param name="decompressor">Optional compressor to use; DEFAULT is null.</param>
        /// <param name="typeMatchStrategy">Optional type match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="TypeMatchStrategy.NamespaceAndName" />.</param>
        /// <param name="multipleMatchStrategy">Optional multiple match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="MultipleMatchStrategy.ThrowOnMultiple" />.</param>
        /// <returns>Orginally serialized object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "deserializer", Justification = "It's a better name than serializer.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "decompressor", Justification = "It's a better name than compressor.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Checked with Must and tested.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Checked with Must and tested.")]
        public static object DeserializePayloadUsingSpecificSerializer(
            this DescribedSerialization describedSerialization,
            IDeserialize deserializer,
            IDecompress decompressor = null,
            TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName,
            MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            new { describedSerialization }.Must().NotBeNull();
            new { deserializer }.Must().NotBeNull();

            var localDecompressor = decompressor ?? new NullCompressor();

            var targetType = describedSerialization.PayloadTypeDescription.ResolveFromLoadedTypes(typeMatchStrategy, multipleMatchStrategy);

            object ret;
            switch (describedSerialization.SerializationDescription.SerializationFormat)
            {
                case SerializationFormat.Binary:
                    var rawBytes = Convert.FromBase64String(describedSerialization.SerializedPayload);
                    var decompressedBytes = localDecompressor.DecompressBytes(rawBytes);
                    ret = deserializer.Deserialize(decompressedBytes, targetType);
                    break;
                case SerializationFormat.String:
                    ret = deserializer.Deserialize(describedSerialization.SerializedPayload, targetType);
                    break;
                default: throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} - {describedSerialization.SerializationDescription.SerializationFormat} is not supported."));
            }

            return ret;
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="serializerFactory">Implementation of <see cref="ISerializerFactory" /> that can resolve the serializer.</param>
        /// <param name="compressorFactory">Implementation of <see cref="ICompressorFactory" /> that can resolve the compressor.</param>
        /// <param name="typeMatchStrategy">Optional type match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="TypeMatchStrategy.NamespaceAndName" />.</param>
        /// <param name="multipleMatchStrategy">Optional multiple match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="MultipleMatchStrategy.ThrowOnMultiple" />.</param>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <returns>Orginally serialized object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Checked with Must and tested.")]
        public static T DeserializePayloadUsingSpecificFactory<T>(
            this DescribedSerialization describedSerialization,
            ISerializerFactory serializerFactory,
            ICompressorFactory compressorFactory,
            TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName,
            MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple)
        {
            return (T)DeserializePayloadUsingSpecificFactory(describedSerialization, serializerFactory, compressorFactory, typeMatchStrategy, multipleMatchStrategy);
        }

        /// <summary>
        /// Interrogates the type for a parameterless constructor.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>A value indicating whether or not the type has a parameterless constructor.</returns>
        public static bool HasParameterlessConstructor(this Type type)
        {
            new { type }.Must().NotBeNull();
            var paramterlessConstructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).SingleOrDefault(_ => _.GetParameters().Length == 0);
            return paramterlessConstructor != null;
        }

        /// <summary>
        /// Interrogates the type to see if it implements a specified interface.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <typeparam name="T">Type of interface to check for.</typeparam>
        /// <returns>A value indicating whether or not the type implements the interface.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Like this usage.")]
        public static bool ImplementsInterface<T>(this Type type)
            where T : class
        {
            new { type }.Must().NotBeNull();

            var interfaceType = typeof(T);
            return ImplementsInterface(type, interfaceType);
        }

        /// <summary>
        /// Interrogates the type to see if it implements a specified interface.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="interfaceType">Type to check.Type of interface to check for.</param>
        /// <returns>A value indicating whether or not the type implements the interface.</returns>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            new { type }.Must().NotBeNull();
            new { interfaceType }.Must().NotBeNull();

            var iteratingType = type;
            while (iteratingType != null)
            {
                if (type.GetInterfaces().Any(_ => _ == interfaceType))
                {
                    return true;
                }

                iteratingType = iteratingType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Get the serializer by inspecting attribute on the property and optionally the specific type if known.
        /// </summary>
        /// <param name="propertyInfo">Property info.</param>
        /// <param name="specificType">Specific type if known.</param>
        /// <returns>Type of serializer if found, otherwise null.</returns>
        public static Type GetSerializerTypeFromAttribute(this PropertyInfo propertyInfo, Type specificType = null)
        {
            new { propertyInfo }.Must().NotBeNull();

            var propertyType = specificType ?? propertyInfo.PropertyType;
            var ret = ((NaosStringSerializerAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(NaosStringSerializerAttribute)))?.SerializerType
                      ?? ((NaosStringSerializerAttribute)Attribute.GetCustomAttribute(propertyType, typeof(NaosStringSerializerAttribute)))?.SerializerType;

            return ret;
        }

        /// <summary>
        /// Get the serializer by inspecting attribute on the type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Type of serializer if found, otherwise null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Prefer this for clarity of purpose.")]
        public static Type GetSerializerTypeFromAttribute(this Type type)
        {
            new { type }.Must().NotBeNull();

            var ret = ((NaosStringSerializerAttribute)Attribute.GetCustomAttribute(type, typeof(NaosStringSerializerAttribute)))?.SerializerType;
            return ret;
        }

        /// <summary>
        /// Get the element serializer by inspecting attribute on the property.
        /// </summary>
        /// <param name="propertyInfo">Property info.</param>
        /// <returns>Type of element serializer if found, otherwise null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Prefer this for clarity of purpose.")]
        public static Type GetElementSerializerTypeFromAttribute(this PropertyInfo propertyInfo)
        {
            new { propertyInfo }.Must().NotBeNull();

            var ret = ((NaosElementStringSerializerAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(NaosElementStringSerializerAttribute)))
                ?.ElementSerializerType;
            return ret;
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to the <see cref="DateTimeKind"/> of <see cref="DateTimeKind.Unspecified"/>.
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/> to convert.</param>
        /// <returns>Converted <see cref="DateTime"/>.</returns>
        public static DateTime ToUnspecified(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Converts a nullable <see cref="DateTime"/> to the <see cref="DateTimeKind"/> of <see cref="DateTimeKind.Unspecified"/>.
        /// </summary>
        /// <param name="dateTime">Nullable <see cref="DateTime"/> to convert.</param>
        /// <returns>Converted nullable <see cref="DateTime"/>.</returns>
        public static DateTime? ToUnspecified(this DateTime? dateTime)
        {
            return dateTime is DateTime notNull ? (DateTime?)notNull.ToUnspecified() : null;
        }
    }
}