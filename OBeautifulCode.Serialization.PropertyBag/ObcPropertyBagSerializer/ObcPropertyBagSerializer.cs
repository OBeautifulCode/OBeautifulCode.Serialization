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

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.String.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializer for writing-to and reading-from a property bag.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
    public partial class ObcPropertyBagSerializer : ObcSerializerBase
    {
        /// <summary>
        /// Reserved key for storing the type's versionless assembly qualified name.
        /// </summary>
        public const string ReservedKeyForTypeVersionlessAssemblyQualifiedName = "_Type";

        /// <summary>
        /// Reserved key for storing <see cref="object.ToString" />.
        /// </summary>
        public const string ReservedKeyForToString = "_" + nameof(object.ToString);

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

            this.SerializerRepresentation = new SerializerRepresentation(SerializationKind.PropertyBag, propertyBagSerializationConfigurationType?.ConcreteSerializationConfigurationDerivativeType.ToRepresentation());
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.PropertyBag;

        /// <inheritdoc />
        public override SerializerRepresentation SerializerRepresentation { get; }

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

            var serializedObject =  this.SerializeToNamedPropertyBagWithStringValues(objectToSerialize);

            var result = this.dictionaryStringSerializer.SerializeDictionaryToString(serializedObject);

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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var stringRepresentation = ConvertByteArrayToString(serializedBytes);

            var result = this.Deserialize(stringRepresentation, type);

            return result;
        }

        private static byte[] ConvertStringToByteArray(
            string stringRepresentation)
        {
            var result = SerializationEncoding.GetBytes(stringRepresentation);

            return result;
        }

        private static string ConvertByteArrayToString(
            byte[] stringRepresentationAsBytes)
        {
            var result = SerializationEncoding.GetString(stringRepresentationAsBytes);

            return result;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private object ConstructAndFillProperties(
            Type objectType,
            IReadOnlyDictionary<string, string> properties)
        {
            var specificType = objectType;

            if (properties.ContainsKey(ReservedKeyForTypeVersionlessAssemblyQualifiedName))
            {
                var specifiedTypeVersionlessAssemblyQualifiedName = properties[ReservedKeyForTypeVersionlessAssemblyQualifiedName];

                specificType = specifiedTypeVersionlessAssemblyQualifiedName.ResolveFromLoadedTypes();
            }

            var propertyNameToObjectMap = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                if (property.Key == ReservedKeyForTypeVersionlessAssemblyQualifiedName || property.Key == ReservedKeyForToString)
                {
                    // reserved and not assigned to properties
                    continue;
                }

                var propertyInfo = specificType.GetPropertyFiltered(property.Key, MemberRelationships.DeclaredOrInherited, MemberOwners.Instance, MemberAccessModifiers.Public);

                var propertyType = propertyInfo.PropertyType;

                var targetValue = property.Value == null ? null : this.MakeObjectFromString(property.Value, propertyType);

                propertyNameToObjectMap.Add(property.Key, targetValue);
            }

            var propertyNamesUpper = propertyNameToObjectMap.Keys.Select(_ => _.ToUpperInvariant()).ToList();

            var discoveredConstructorToUse = specificType
                .GetConstructors()
                .Select(c => new { Parameters = c.GetParameters(), Constructor = c })
                .Where(t => t.Parameters.Select(p => p.Name.ToUpperInvariant()).Intersect(propertyNamesUpper).Count() == t.Parameters.Length)
                .OrderByDescending(t => t.Parameters.Length)
                .FirstOrDefault()?.Constructor;

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
                var propertyInfo = specificType.GetPropertyFiltered(nameToPropertyInfoAndObject.Key, MemberRelationships.DeclaredOrInherited, MemberOwners.Instance, MemberAccessModifiers.All, throwIfNotFound: false);

                if ((propertyInfo != null) && propertyInfo.CanWrite)
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
            if (serializedString == null)
            {
                throw new ArgumentNullException(nameof(serializedString));
            }

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
                var typeToSearchForParse = type;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeToSearchForParse = type.GenericTypeArguments.Single();
                }

                var parseMethod = typeToSearchForParse
                    .GetMethodsFiltered(MemberRelationships.DeclaredOrInherited, MemberOwners.All, MemberAccessModifiers.Public)
                    .SingleOrDefault(_ =>
                    {
                        var parameters = _.GetParameters();

                        return (_.Name == "Parse") && (parameters.Length == 1) && (parameters.Single().ParameterType == typeof(string));
                    });

                var result = parseMethod == null
                    ? serializedString
                    : parseMethod.Invoke(null, new object[] { serializedString });

                return result;
            }
        }

        private string MakeStringFromPropertyValue(
            object propertyValue)
        {
            if (propertyValue == null)
            {
                throw new ArgumentNullException(nameof(propertyValue));
            }

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
