// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Serialization.PropertyBag.Internal;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializer for moving in and out of a <see cref="Dictionary{TKey,TValue} "/> for string, string.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
    public class ObcPropertyBagSerializer : ObcSerializerBase, IPropertyBagSerializeAndDeserialize
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
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = ObcSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly Encoding SerializationEncoding = Encoding.UTF8;

        private readonly ObcDictionaryStringStringSerializer dictionaryStringSerializer;

        private readonly IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> configuredTypeToSerializerMap;

        private readonly PropertyBagSerializationConfigurationBase propertyBagConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcPropertyBagSerializer"/> class.
        /// </summary>
        /// <param name="propertyBagSerializationConfigurationType">Type of configuration to use.</param>
        public ObcPropertyBagSerializer(
            PropertyBagSerializationConfigurationType propertyBagSerializationConfigurationType = null)
            : base(propertyBagSerializationConfigurationType ?? typeof(NullPropertyBagSerializationConfiguration).ToPropertyBagSerializationConfigurationType())
        {
            this.propertyBagConfiguration = (PropertyBagSerializationConfigurationBase)this.SerializationConfiguration;

            this.dictionaryStringSerializer = new ObcDictionaryStringStringSerializer(
                this.propertyBagConfiguration.StringSerializationKeyValueDelimiter,
                this.propertyBagConfiguration.StringSerializationLineDelimiter,
                this.propertyBagConfiguration.StringSerializationNullValueEncoding);

            this.configuredTypeToSerializerMap = this.propertyBagConfiguration.BuildConfiguredTypeToSerializerMap();
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.PropertyBag;

        /// <summary>
        /// Converts string into a byte array.
        /// </summary>
        /// <param name="stringRepresentation">String representation.</param>
        /// <returns>
        /// Byte array.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static byte[] ConvertStringToByteArray(
            string stringRepresentation)
        {
            var result = SerializationEncoding.GetBytes(stringRepresentation);

            return result;
        }

        /// <summary>
        /// Converts string representation byte array into a string.
        /// </summary>
        /// <param name="stringRepresentationAsBytes">String representation as bytes.</param>
        /// <returns>
        /// JSON string.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static string ConvertByteArrayToString(
            byte[] stringRepresentationAsBytes)
        {
            var result = SerializationEncoding.GetString(stringRepresentationAsBytes);

            return result;
        }

        /// <inheritdoc />
        public override byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var stringRepresentation = this.SerializeToString(objectToSerialize);

            var result = ConvertStringToByteArray(stringRepresentation);

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            byte[] serializedBytes)
        {
            var result = (T)this.Deserialize(serializedBytes, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var stringRepresentation = ConvertByteArrayToString(serializedBytes);

            var result = this.Deserialize(stringRepresentation, type);

            return result;
        }

        /// <inheritdoc />
        public override string SerializeToString(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            if (objectType == typeof(string))
            {
                throw new NotSupportedException("String is not supported as a type for this serializer.");
            }

            this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            if (objectToSerialize == null)
            {
                return SerializationConfigurationBase.NullSerializedStringValue;
            }

            var serializedObject = this.SerializeToPropertyBag(objectToSerialize);

            var result = this.dictionaryStringSerializer.SerializeDictionaryToString(serializedObject);

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            string serializedString)
        {
            var objectType = typeof(T);

            this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Deserialize, null);

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return default(T);
            }

            var dictionary = this.dictionaryStringSerializer.DeserializeToDictionary(serializedString);

            var result = this.Deserialize<T>(dictionary);

            return result;
        }

        /// <inheritdoc cref="IStringSerializeAndDeserialize"/>
        public override object Deserialize(
            string serializedString,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return null;
            }

            var dictionary = this.dictionaryStringSerializer.DeserializeToDictionary(serializedString);

            var result = this.Deserialize(dictionary, type);

            return result;
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

            var result = propertyNames.ToDictionary(
                k => k.Name,
                v =>
                    {
                        var propertyInfo = specificType.GetProperty(v.Name, bindingFlags);
                        propertyInfo.AsArg(Invariant($"Could not find {nameof(PropertyInfo)} on type: {specificType} by name: {v.Name}")).Must().NotBeNull();

                        var propertyValue = propertyInfo.GetValue(objectToSerialize);

                        return propertyValue == null ? null : this.MakeStringFromPropertyValue(propertyValue);
                    });

            result.Add(ReservedKeyToString, objectToSerialize.ToString());

            result.Add(ReservedKeyTypeFullName, specificType.FullName);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<string, string> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<string, string> serializedPropertyBag,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            if (serializedPropertyBag == null)
            {
                return SerializationConfigurationBase.NullSerializedStringValue;
            }

            if (!serializedPropertyBag.Any())
            {
                if (type.HasDefaultConstructor())
                {
                    return type.Construct();
                }
                else
                {
                    throw new SerializationException(Invariant($"Found no properties for type {type} and it also does not have a parameterless constructor."));
                }
            }

            var result = this.ConstructAndFillProperties(type, serializedPropertyBag);

            return result;
        }

        private static IReadOnlyCollection<Type> GetAllLoadedTypes()
        {
            var loadedAssemblies = AssemblyLoader.GetLoadedAssemblies().Distinct().ToList();

            var result = new List<Type>();

            var reflectionTypeLoadExceptions = new List<ReflectionTypeLoadException>(); // suppress for now, maybe throw later

            foreach (var assembly in loadedAssemblies)
            {
                try
                {
                    result.AddRange(new[] { assembly }.GetTypesFromAssemblies());
                }
                catch (TypeLoadException ex) when (ex.InnerException?.GetType() == typeof(ReflectionTypeLoadException))
                {
                    var reflectionTypeLoadException = (ReflectionTypeLoadException)ex.InnerException;

                    result.AddRange(reflectionTypeLoadException.Types);

                    reflectionTypeLoadExceptions.Add(reflectionTypeLoadException);
                }
            }

            return result;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private object ConstructAndFillProperties(
            Type objectType,
            IReadOnlyDictionary<string, string> properties)
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

            var result = discoveredConstructorToUse.Invoke(parameterObjects);

            foreach (var nameToPropertyInfoAndObject in propertyNameToObjectMap)
            {
                var propertyInfo = specificType.GetProperty(nameToPropertyInfoAndObject.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty);

                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(result, nameToPropertyInfoAndObject.Value);
                }
            }

            return result;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private object MakeObjectFromString(
            string serializedString,
            Type type)
        {
            new { serializedString }.AsArg().Must().NotBeNull();

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                return null;
            }

            if (this.configuredTypeToSerializerMap.ContainsKey(type))
            {
                var serializer = this.configuredTypeToSerializerMap[type];

                var result = serializer.Deserialize(serializedString, type);

                return result;
            }
            else if (type.IsEnum)
            {
                var result = Enum.Parse(type, serializedString);

                return result;
            }
            else if (type.IsArray)
            {
                var arrayItemType = type.GetElementType() ?? throw new ArgumentException(Invariant($"Found array type that cannot extract element type: {type}"));

                var asList = (IList)this.MakeObjectFromString(serializedString, typeof(List<>).MakeGenericType(arrayItemType));

                var asArrayList = new ArrayList(asList);

                var result = asArrayList.ToArray(arrayItemType);

                return result;
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                var result = ObcDateTimeStringSerializer.DeserializeToDateTime(serializedString);

                return result;
            }
            else if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type))
            {
                var itemType = type.GenericTypeArguments.SingleOrDefault() ?? throw new ArgumentException(Invariant($"Found {typeof(IEnumerable)} type that cannot extract element type: {type}"));

                var stringValues = serializedString.FromCsv(this.dictionaryStringSerializer.NullValueEncoding);

                var result = (IList)typeof(List<>).MakeGenericType(itemType).Construct();

                foreach (var stringValue in stringValues)
                {
                    var itemValue = stringValue == null
                        ? null
                        : this.MakeObjectFromString(stringValue, itemType);

                    result.Add(itemValue);
                }

                return result;
            }
            else
            {
                var bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance;

                var typeToSearchForParse = type;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeToSearchForParse = type.GenericTypeArguments.Single();
                }

                var parseMethod = typeToSearchForParse.GetMethods(bindingFlags).SingleOrDefault(_ =>
                {
                    var parameters = _.GetParameters();
                    return _.Name == "Parse" && parameters.Length == 1 && parameters.Single().ParameterType == typeof(string);
                });

                object result;

                if (parseMethod == null)
                {
                    // nothing we can do here so return the string and hope...
                    result = serializedString;
                }
                else
                {
                    result = parseMethod.Invoke(null, new object[] { serializedString });
                }

                return result;
            }
        }

        private string MakeStringFromPropertyValue(
            object propertyValue)
        {
            new { propertyValue }.AsArg().Must().NotBeNull();

            var propertyType = propertyValue.GetType();

            if (this.configuredTypeToSerializerMap.ContainsKey(propertyType))
            {
                var serializer = this.configuredTypeToSerializerMap[propertyType];

                var result = serializer.SerializeToString(propertyValue);

                return result;
            }
            else if (propertyValue is DateTime propertyValueAsDateTime)
            {
                var result = ObcDateTimeStringSerializer.SerializeToString(propertyValueAsDateTime);

                return result;
            }
            else
            {
                string result;

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

                    result = values.ToCsv(this.dictionaryStringSerializer.NullValueEncoding);
                }
                else if (propertyValue is ISerializeToString propertyValueAsSerializeToString)
                {
                    result = propertyValueAsSerializeToString.SerializeToString();
                }
                else
                {
                    result = propertyValue.ToString();
                }

                return result;
            }
        }
    }
}
