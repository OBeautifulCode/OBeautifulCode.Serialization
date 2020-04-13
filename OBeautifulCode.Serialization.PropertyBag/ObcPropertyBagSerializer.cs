﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.String.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializer for moving in and out of a <see cref="Dictionary{TKey,TValue} "/> for string, string.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is not a problem.")]
    public class ObcPropertyBagSerializer : ConfiguredSerializerBase, IPropertyBagSerializeAndDeserialize
    {
        /// <summary>
        /// Reserved key for storing <see cref="Type.FullName" />.
        /// </summary>
        public const string ReservedKeyTypeFullName = nameof(object.GetType);

        /// <summary>
        /// Reserved key for storing <see cref="object.ToString" />.
        /// </summary>
        public const string ReservedKeyToString = nameof(object.ToString);

        /// <summary>
        /// Encoding to use for conversion in and out of bytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Is not mutable.")]
        public static readonly Encoding SerializationEncoding = Encoding.UTF8;

        private readonly ObcDictionaryStringStringSerializer dictionaryStringSerializer;

        private readonly IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> configuredTypeToSerializerMap;

        private readonly Dictionary<Type, IStringSerializeAndDeserialize> cachedAttributeSerializerTypeToObjectMap;

        private readonly PropertyBagSerializationConfigurationBase propertyBagConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcPropertyBagSerializer"/> class.
        /// </summary>
        /// <param name="propertyBagSerializationConfigurationType">Type of configuration to use.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        public ObcPropertyBagSerializer(
            PropertyBagSerializationConfigurationType propertyBagSerializationConfigurationType = null,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
            : base(propertyBagSerializationConfigurationType ?? typeof(NullPropertyBagSerializationConfiguration).ToPropertyBagSerializationConfigurationType(), unregisteredTypeEncounteredStrategy)
        {
            this.propertyBagConfiguration = (PropertyBagSerializationConfigurationBase)this.SerializationConfiguration;
            this.dictionaryStringSerializer = new ObcDictionaryStringStringSerializer(
                this.propertyBagConfiguration.StringSerializationKeyValueDelimiter,
                this.propertyBagConfiguration.StringSerializationLineDelimiter,
                this.propertyBagConfiguration.StringSerializationNullValueEncoding);
            this.configuredTypeToSerializerMap = this.propertyBagConfiguration.BuildConfiguredTypeToSerializerMap();
            this.cachedAttributeSerializerTypeToObjectMap = new Dictionary<Type, IStringSerializeAndDeserialize>();
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.PropertyBag;

        /// <summary>
        /// Converts string into a byte array.
        /// </summary>
        /// <param name="stringRepresentation">String representation.</param>
        /// <returns>Byte array.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = "Name/spelling is correct.")]
        public static byte[] ConvertStringToByteArray(string stringRepresentation)
        {
            var ret = SerializationEncoding.GetBytes(stringRepresentation);
            return ret;
        }

        /// <summary>
        /// Converts string representation byte array into a string.
        /// </summary>
        /// <param name="stringRepresentationAsBytes">String representation as bytes.</param>
        /// <returns>JSON string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = "I like this name...")]
        public static string ConvertByteArrayToString(byte[] stringRepresentationAsBytes)
        {
            var ret = SerializationEncoding.GetString(stringRepresentationAsBytes);
            return ret;
        }

        /// <inheritdoc />
        public override byte[] SerializeToBytes(object objectToSerialize)
        {
            var stringRepresentation = this.SerializeToString(objectToSerialize);
            var stringRepresentationBytes = ConvertStringToByteArray(stringRepresentation);
            return stringRepresentationBytes;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(byte[] serializedBytes)
        {
            var ret = this.Deserialize(serializedBytes, typeof(T));
            return (T)ret;
        }

        /// <inheritdoc />
        public override object Deserialize(byte[] serializedBytes, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var stringRepresentation = ConvertByteArrayToString(serializedBytes);
            return this.Deserialize(stringRepresentation, type);
        }

        /// <inheritdoc />
        public override string SerializeToString(object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();
            if (objectType == typeof(string))
            {
                throw new NotSupportedException("String is not supported as a type for this serializer.");
            }

            this.ThrowOnUnregisteredTypeIfAppropriate(objectType);

            if (objectToSerialize == null)
            {
                return SerializationConfigurationBase.NullSerializedStringValue;
            }

            var serializedObject = this.SerializeToPropertyBag(objectToSerialize);
            var ret = this.dictionaryStringSerializer.SerializeDictionaryToString(serializedObject);

            return ret;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(string serializedString)
        {
            var objectType = typeof(T);

            this.ThrowOnUnregisteredTypeIfAppropriate(objectType);

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return default(T);
            }

            var dictionary = this.dictionaryStringSerializer.DeserializeToDictionary(serializedString);
            var ret = this.Deserialize<T>(dictionary);

            return ret;
        }

        /// <inheritdoc cref="IStringSerializeAndDeserialize"/>
        public override object Deserialize(string serializedString, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.ThrowOnUnregisteredTypeIfAppropriate(type);

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return null;
            }

            var dictionary = this.dictionaryStringSerializer.DeserializeToDictionary(serializedString);
            var ret = this.Deserialize(dictionary, type);

            return ret;
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> SerializeToPropertyBag(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var specificType = objectToSerialize.GetType();
            var bindingFlags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;
            var propertyNames = specificType.GetProperties(bindingFlags);
            var properties = propertyNames.ToDictionary(
                k => k.Name,
                v =>
                    {
                        var propertyInfo = specificType.GetProperty(v.Name, bindingFlags);
                        propertyInfo.AsArg(Invariant($"Could not find {nameof(PropertyInfo)} on type: {specificType} by name: {v.Name}")).Must().NotBeNull();

                        var propertyValue = propertyInfo.GetValue(objectToSerialize);

                        return propertyValue == null ? null : this.MakeStringFromPropertyValue(propertyValue);
                    });

            properties.Add(ReservedKeyToString, objectToSerialize.ToString());
            properties.Add(ReservedKeyTypeFullName, specificType.FullName);

            return properties;
        }

        /// <inheritdoc />
        public T Deserialize<T>(IReadOnlyDictionary<string, string> serializedPropertyBag)
        {
            var ret = this.Deserialize(serializedPropertyBag, typeof(T));

            return (T)ret;
        }

        /// <inheritdoc />
        public object Deserialize(IReadOnlyDictionary<string, string> serializedPropertyBag, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            if (serializedPropertyBag == null)
            {
                return SerializationConfigurationBase.NullSerializedStringValue;
            }

            if (!serializedPropertyBag.Any())
            {
                if (type.HasParameterlessConstructor())
                {
                    return type.Construct();
                }
                else
                {
                    throw new SerializationException(Invariant($"Found no properties for type {type} and it also does not have a parameterless constructor."));
                }
            }

            var ret = this.ConstructAndFillProperties(type, serializedPropertyBag);
            return ret;
        }

        private static IReadOnlyCollection<Type> GetAllLoadedTypes()
        {
            var loadedAssemblies = AssemblyLoader.GetLoadedAssemblies().Distinct().ToList();
            var allTypes = new List<Type>();
            var reflectionTypeLoadExceptions = new List<ReflectionTypeLoadException>(); // suppress for now, maybe throw later
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

            return allTypes;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Prefer this for readability.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Prefer this for readability.")]
        private object ConstructAndFillProperties(Type objectType, IReadOnlyDictionary<string, string> properties)
        {
            var specificType = objectType;
            if (properties.ContainsKey(ReservedKeyTypeFullName))
            {
                var specificTypeFullName = properties[ReservedKeyTypeFullName];
                var loadedTypeMatches = GetAllLoadedTypes().Where(_ => _.FullName == specificTypeFullName).ToList();
                if (loadedTypeMatches.Any())
                {
                    if (loadedTypeMatches.Count > 1)
                    {
                        throw new ArgumentException(Invariant($"Found multiple loaded matches for ({specificTypeFullName}); {string.Join(",", loadedTypeMatches)}"));
                    }
                    else
                    {
                        specificType = loadedTypeMatches.Single();
                    }
                }
            }

            var propertyNameToObjectMap = new Dictionary<string, object>();
            var bindingFlags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;
            foreach (var property in properties)
            {
                if (property.Key == ReservedKeyTypeFullName || property.Key == ReservedKeyToString)
                {
                    // reserved and not assigned to properties
                    continue;
                }

                var propertyInfo = specificType.GetProperty(property.Key, bindingFlags);
                var missingPropertyExceptionMessage = Invariant($"Could not find {nameof(PropertyInfo)} on type: {specificType} by name: {property.Key}");
                propertyInfo.AsArg(missingPropertyExceptionMessage).Must().NotBeNull();
                var propertyType = propertyInfo?.PropertyType ?? throw new ArgumentNullException(missingPropertyExceptionMessage);

                var targetValue = property.Value == null ? null : this.MakeObjectFromString(property.Value, propertyType);

                propertyNameToObjectMap.Add(property.Key, targetValue);
            }

            var propertyNamesUpper = propertyNameToObjectMap.Keys.Select(_ => _.ToUpperInvariant()).ToList();
            var discoveredConstructorToUse = specificType.GetConstructors().Select(c => new { Parameters = c.GetParameters(), Constructor = c })
                .Where(t => t.Parameters.Select(p => p.Name.ToUpperInvariant()).Intersect(propertyNamesUpper).Count() == t.Parameters.Length)
                .OrderByDescending(t => t.Parameters.Length).FirstOrDefault()?.Constructor;

            if (discoveredConstructorToUse == null)
            {
                throw new SerializationException(Invariant($"Could not find a parameterless constructor or a constructor whose parameter names matched the properties provided; type: {specificType}, properties: {string.Join(",", properties.Keys)}."));
            }

            var propertyNameUpperToObjectsMap = propertyNameToObjectMap.ToDictionary(k => k.Key.ToUpperInvariant(), v => v.Value);
            var parameterNameUpperAndObjects = discoveredConstructorToUse.GetParameters().Select(
                _ => new { NameUpper = _.Name.ToUpperInvariant(), Value = propertyNameUpperToObjectsMap[_.Name.ToUpperInvariant()] }).ToList();

            var parameterObjects = parameterNameUpperAndObjects.Select(_ => _.Value).ToArray();
            var ret = discoveredConstructorToUse.Invoke(parameterObjects);
            foreach (var nameToPropertyInfoAndObject in propertyNameToObjectMap)
            {
                var propertyInfo = specificType.GetProperty(nameToPropertyInfoAndObject.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(ret, nameToPropertyInfoAndObject.Value);
                }
            }

            return ret;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Name/spelling is correct.")]
        private object MakeObjectFromString(string serializedString, Type type)
        {
            new { serializedString }.AsArg().Must().NotBeNull();

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return null;
            }

            if (this.configuredTypeToSerializerMap.ContainsKey(type))
            {
                var serializer = this.configuredTypeToSerializerMap[type];
                var ret = serializer.Deserialize(serializedString, type);
                return ret;
            }
            else if (type.IsEnum)
            {
                return Enum.Parse(type, serializedString);
            }
            else if (type.IsArray)
            {
                var arrayItemType = type.GetElementType() ?? throw new ArgumentException(Invariant($"Found array type that cannot extract element type: {type}"));
                var asList = (IList)this.MakeObjectFromString(
                    serializedString,
                    typeof(List<>).MakeGenericType(arrayItemType));
                var asArrayList = new ArrayList(asList);
                return asArrayList.ToArray(arrayItemType);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return ObcDateTimeStringSerializer.DeserializeToDateTime(serializedString);
            }
            else if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type))
            {
                var itemType = type.GenericTypeArguments.SingleOrDefault() ?? throw new ArgumentException(Invariant($"Found {typeof(IEnumerable)} type that cannot extract element type: {type}"));
                var stringValues = serializedString.FromCsv(this.dictionaryStringSerializer.NullValueEncoding);
                var values = (IList)typeof(List<>).MakeGenericType(itemType).Construct();
                foreach (var stringValue in stringValues)
                {
                    var itemValue = stringValue == null
                        ? null
                        : this.MakeObjectFromString(stringValue, itemType);

                    values.Add(itemValue);
                }

                return values;
            }
            else
            {
                var bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance;
                var typeToSearchForParse = type;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeToSearchForParse = type.GenericTypeArguments.Single();
                }

                var parseMethod = typeToSearchForParse.GetMethods(bindingFlags).SingleOrDefault(
                    _ =>
                        {
                            var parameters = _.GetParameters();
                            return _.Name == "Parse" && parameters.Length == 1 && parameters.Single().ParameterType == typeof(string);
                        });

                if (parseMethod == null)
                {
                    // nothing we can do here so return the string and hope...
                    return serializedString;
                }
                else
                {
                    var ret = parseMethod.Invoke(null, new object[] { serializedString });
                    return ret;
                }
            }
        }

        private string MakeStringFromPropertyValue(object propertyValue)
        {
            new { propertyValue }.AsArg().Must().NotBeNull();

            var propertyType = propertyValue.GetType();

            if (this.configuredTypeToSerializerMap.ContainsKey(propertyType))
            {
                var serializer = this.configuredTypeToSerializerMap[propertyType];
                var ret = serializer.SerializeToString(propertyValue);
                return ret;
            }
            else if (propertyValue is DateTime propertyValueAsDateTime)
            {
                return ObcDateTimeStringSerializer.SerializeToString(propertyValueAsDateTime);
            }
            else
            {
                if (propertyType != typeof(string) && propertyValue is IEnumerable propertyValueAsEnumerable)
                {
                    var values = new List<string>();
                    foreach (var item in propertyValueAsEnumerable)
                    {
                        var serializedItem = item == null
                            ? null
                            : this.MakeStringFromPropertyValue(item);

                        values.Add(serializedItem);
                    }

                    return values.ToCsv(this.dictionaryStringSerializer.NullValueEncoding);
                }
                else if (propertyValue is ISerializeToString propertyValueAsSerializeToString)
                {
                    return propertyValueAsSerializeToString.SerializeToString();
                }
                else
                {
                    return propertyValue.ToString();
                }
            }
        }
    }

    /// <inheritdoc />
    public sealed class ObcPropertyBagSerializer<TPropertyBagConfiguration> : ObcPropertyBagSerializer
        where TPropertyBagConfiguration : PropertyBagSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcPropertyBagSerializer{TPropertyBagConfiguration}"/> class.
        /// </summary>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        public ObcPropertyBagSerializer(
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
            : base(typeof(TPropertyBagConfiguration).ToPropertyBagSerializationConfigurationType(), unregisteredTypeEncounteredStrategy)
        {
        }
    }
}
