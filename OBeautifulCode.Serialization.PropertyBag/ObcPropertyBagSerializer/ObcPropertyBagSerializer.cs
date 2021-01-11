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
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializer for writing-to and reading-from a property bag.
    /// </summary>
    public partial class ObcPropertyBagSerializer : ObcSerializerBase
    {
        /// <summary>
        /// Reserved key for storing the type's versionless assembly qualified name in a named property bag.
        /// </summary>
        public const string ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag = "_Type";

        /// <summary>
        /// Reserved key for storing the type's versionless assembly qualified name in an ordinal property bag.
        /// </summary>
        public const int ReservedKeyForTypeVersionlessAssemblyQualifiedNameInOrdinalPropertyBag = -1;

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
            var namedPropertyBagWithStringValues =  this.SerializeToNamedPropertyBagWithStringValues(objectToSerialize);

            var result = this.dictionaryStringSerializer.SerializeDictionaryToString(namedPropertyBagWithStringValues);

            return result;
        }

        /// <inheritdoc />
        public override byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var stringRepresentation = this.SerializeToString(objectToSerialize);

            var result = stringRepresentation == null
                ? null
                : SerializationEncoding.GetBytes(stringRepresentation);

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            string serializedString)
        {
            var objectType = typeof(T);

            var result = (T)this.Deserialize(serializedString, objectType);

            return result;
        }

        /// <inheritdoc cref="IStringSerializeAndDeserialize"/>
        public override object Deserialize(
            string serializedString,
            Type type)
        {
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
            var stringRepresentation = serializedBytes == null
                ? null
                : SerializationEncoding.GetString(serializedBytes);

            var result = this.Deserialize(stringRepresentation, type);

            return result;
        }

        private static IReadOnlyDictionary<string, TValue> GetSerializedPropertyBagToUseOrThrow<TValue>(
            IReadOnlyDictionary<string, TValue> serializedPropertyBag)
        {
            if (serializedPropertyBag.Count != serializedPropertyBag.Keys.Distinct(StringComparer.OrdinalIgnoreCase).Count())
            {
                throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} contains two or more properties with the same case-insensitive name."));
            }

            var result = serializedPropertyBag.ToDictionary(_ => _.Key, _ => _.Value, StringComparer.OrdinalIgnoreCase);

            return result;
        }

        private static IReadOnlyDictionary<int, TValue> GetSerializedPropertyBagToUseOrThrow<TValue>(
            IReadOnlyDictionary<int, TValue> serializedPropertyBag)
        {
            var indicesInOrder = serializedPropertyBag.Keys
                .Where(_ => _ != ReservedKeyForTypeVersionlessAssemblyQualifiedNameInOrdinalPropertyBag)
                .OrderBy(_ => _)
                .ToArray();

            if (indicesInOrder.Any())
            {
                if (indicesInOrder.First() != 0)
                {
                    throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} is malformed.  The keys are not consecutive from 0 to the number of properties ({indicesInOrder.Length})."));
                }

                if (indicesInOrder.Last() != (indicesInOrder.Length - 1))
                {
                    throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} is malformed.  The keys are not consecutive from 0 to the number of properties ({indicesInOrder.Length})."));
                }
            }

            var result = serializedPropertyBag;

            return result;
        }

        private static IReadOnlyDictionary<string, PropertyInfo> GetPropertyOfConcernNameToPropertyMap(
            IReadOnlyCollection<PropertyInfo> propertiesOfConcern)
        {
            var result = propertiesOfConcern.ToDictionary(
                _ => _.Name,
                _ => _,
                StringComparer.OrdinalIgnoreCase);

            return result;
        }

        private static IReadOnlyList<PropertyInfo> GetPropertiesOfConcern(
            Type type,
            bool ordered)
        {
            var result = type.GetPropertiesFiltered(
                MemberRelationships.DeclaredInTypeOrAncestorTypes,
                MemberOwners.Instance,
                MemberAccessModifiers.Public,
                MemberMutability.Writable,
                orderMembersBy: OrderMembersBy.DeclaringTypeDerivationPathThenByMemberName);

            if (result.Select(_ => _.Name).Distinct(StringComparer.OrdinalIgnoreCase).Count() != result.Count)
            {
                throw new SerializationException("One or more of properties (public, inherited or declared, writable, instance) on the object to serialize or the type to deserialize into have the same case-insensitive name.");
            }

            if (result.Any(_ => _.Name.Equals(ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag, StringComparison.OrdinalIgnoreCase)))
            {
                throw new SerializationException(Invariant($"There is at least one property on the object to serialize or the type to deserialize into who's case-insensitive name, '{ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag}', is reserved for the versionless assembly qualified name of the type."));
            }

            if (ordered)
            {
                var constructor = GetBestMatchConstructorOrThrow(type, result);

                if (!constructor.IsDefaultConstructor())
                {
                    var propertyOfConcernNameToPropertyMap = GetPropertyOfConcernNameToPropertyMap(result);

                    var constructorPropertiesInOrder = constructor
                        .GetParameters()
                        .Select(_ => propertyOfConcernNameToPropertyMap[_.Name])
                        .ToList();

                    var propertiesMissingFromConstructor = result.Where(_ => !constructorPropertiesInOrder.Contains(_)).ToList();

                    result = new PropertyInfo[0]
                        .Concat(constructorPropertiesInOrder)
                        .Concat(propertiesMissingFromConstructor)
                        .ToList();
                }
            }

            return result;
        }

        private static ConstructorInfo GetBestMatchConstructorOrThrow(
            Type type,
            IReadOnlyCollection<PropertyInfo> propertiesOfConcern)
        {
            var declaredOnlyPropertiesOfConcern = propertiesOfConcern.Where(_ => _.DeclaringType == type).ToList();

            // ALL of the below code was copy/pasted from CodeGen
            var candidateConstructors = type.GetConstructorsMatchedToProperties(propertiesOfConcern, ConstructorsMatchedToPropertiesStrategy.AllConstructorParametersHaveMatchingProperty, MemberAccessModifiers.Public, MemberRelationships.DeclaredInType);

            if (candidateConstructors.Count == 0)
            {
                throw new SerializationException(Invariant($"Could not serialize/deserialize a '{type.ToStringReadable()}'; none of its public constructors have parameters where all parameters have a matching property (public, inherited or declared, writable, instance) by name and type."));
            }

            var maxParameterCount = candidateConstructors.Max(_ => _.GetParameters().Length);

            candidateConstructors = candidateConstructors.Where(_ => _.GetParameters().Length == maxParameterCount).ToList();

            if (candidateConstructors.Count > 1)
            {
                throw new SerializationException(Invariant($"Could not serialize/deserialize a '{type.ToStringReadable()}'; there are {candidateConstructors.Count} public constructors having the most number of parameters ({maxParameterCount}) where all parameters have a matching property (public, inherited or declared, writable, instance) by name and type, whereas only 1 was expected."));
            }

            var result = candidateConstructors.Single();

            if (result.IsDefaultConstructor())
            {
                if (!propertiesOfConcern.All(_ => _.SetMethod.IsPublic))
                {
                    throw new SerializationException(Invariant($"Could not serialize/deserialize a '{type.ToStringReadable()}'; the public constructor that best matches its properties (public, inherited or declared, writable, instance) is the default (parameterless) constructor but one or more of these properties has a non-public setter."));
                }
            }
            else
            {
                if (propertiesOfConcern.Any(_ => _.SetMethod.IsPublic))
                {
                    throw new SerializationException(Invariant($"Could not serialize/deserialize a '{type.ToStringReadable()}'; the public constructor that best matches its properties (public, inherited or declared, writable, instance) is parameterized but one or more of these properties has a public setter."));
                }

                // all of the declared properties have to appear in the parameter list
                var constructorParameterNames = new HashSet<string>(result.GetParameters().Select(_ => _.Name), StringComparer.OrdinalIgnoreCase);

                if (declaredOnlyPropertiesOfConcern.Any(_ => !constructorParameterNames.Contains(_.Name)))
                {
                    throw new SerializationException(Invariant($"Could not serialize/deserialize a '{type.ToStringReadable()}'; the public constructor that best matches its properties (public, inherited or declared, writable, instance) is parameterized but one or more of the properties declared on the type (not inherited) does not match a parameter in that constructor."));
                }
            }

            return result;
        }

        private void InternalPropertyBagThrowOnUnregisteredTypeIfAppropriate(
            Type objectType,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(objectType, serializationDirection, objectToSerialize);
        }

        private string MakeStringFromObject(
            object value)
        {
            if (value == null)
            {
                return null;
            }

            var propertyType = value.GetType();

            if (this.configuredTypeToSerializerMap.ContainsKey(propertyType))
            {
                var serializer = this.configuredTypeToSerializerMap[propertyType];

                var result = serializer.SerializeToString(value);

                return result;
            }
            else if (value is DateTime propertyValueAsDateTime)
            {
                var result = ObcDateTimeStringSerializer.SerializeToString(propertyValueAsDateTime);

                return result;
            }
            else
            {
                string result;

                if ((propertyType != typeof(string)) && (value is IEnumerable propertyValueAsEnumerable))
                {
                    var values = new List<string>();

                    foreach (var element in propertyValueAsEnumerable)
                    {
                        var serializedItem = this.MakeStringFromObject(element);

                        values.Add(serializedItem);
                    }

                    result = values.ToCsv(this.dictionaryStringSerializer.NullValueEncoding);
                }
                else if (value is ISerializeToString propertyValueAsSerializeToString)
                {
                    result = propertyValueAsSerializeToString.SerializeToString();
                }
                else
                {
                    result = value.ToString();
                }

                return result;
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        private object MakeObjectFromString(
            string value,
            Type type)
        {
            if (value == null)
            {
                return null;
            }

            if (this.configuredTypeToSerializerMap.ContainsKey(type))
            {
                var serializer = this.configuredTypeToSerializerMap[type];

                var result = serializer.Deserialize(value, type);

                return result;
            }
            else if (type.IsEnum)
            {
                var result = Enum.Parse(type, value);

                return result;
            }
            else if (type.IsArray)
            {
                var arrayItemType = type.GetElementType() ?? throw new SerializationException(Invariant($"Found array type that cannot extract element type: {type}"));

                var asList = (IList)this.MakeObjectFromString(value, typeof(List<>).MakeGenericType(arrayItemType));

                var asArrayList = new ArrayList(asList);

                var result = asArrayList.ToArray(arrayItemType);

                return result;
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                var result = ObcDateTimeStringSerializer.DeserializeToDateTime(value);

                return result;
            }
            else if ((type != typeof(string)) && typeof(IEnumerable).IsAssignableFrom(type))
            {
                var itemType = type.GenericTypeArguments.SingleOrDefault() ?? throw new SerializationException(Invariant($"Found {typeof(IEnumerable)} type that cannot extract element type: {type}"));

                var stringValues = value.FromCsv(this.dictionaryStringSerializer.NullValueEncoding);

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
                    ? value
                    : parseMethod.Invoke(null, new object[] { value });

                return result;
            }
        }

        private Type GetTypeToDeserializeIntoOrThrow<TKey, TValue>(
            Type type,
            IReadOnlyDictionary<TKey, TValue> serializedPropertyBag,
            TKey reservedKeyForTypeVersionlessAssemblyQualifiedName)
        {
            var result = type;

            // serializePropertyBag will have case-insensitive keys because of ValidateSerializedPropertyBagAndMakeCaseInsensitiveKeys
            if (serializedPropertyBag.ContainsKey(reservedKeyForTypeVersionlessAssemblyQualifiedName))
            {
                var assemblyQualifiedNameObject = serializedPropertyBag[reservedKeyForTypeVersionlessAssemblyQualifiedName];

                if (!(assemblyQualifiedNameObject is string assemblyQualifiedName))
                {
                    throw new SerializationException(Invariant($"The property bag specifies the type to deserialize into via the '{ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag}' property (version-less assembly qualified name), but the value of that property is of type '{assemblyQualifiedNameObject.GetType().ToStringReadable()}' instead of '{typeof(string).ToStringReadable()}'."));
                }

                result = assemblyQualifiedName.ToTypeRepresentationFromAssemblyQualifiedName().ResolveFromLoadedTypes();

                this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(result, SerializationDirection.Deserialize, null);

                if (!type.IsAssignableFrom(result))
                {
                    throw new SerializationException(Invariant($"The property bag specifies the type to deserialize into via the '{ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag}' property (version-less assembly qualified name) as '{result.ToStringReadable()}', but that type is not assignable to '{type.ToStringReadable()}', which is the type specified in the deserialize call."));
                }
            }

            // Unlike serialization, where we use the runtime type (object.GetType()), in deserialization we are told
            // what type to deserialize into and so we have to validate that that's a type we can construct.
            var isValidType = result.IsClosedNonAnonymousClassType() && (!result.IsAbstract);

            if (!isValidType)
            {
                var exceptionMessageSuffix = result == type
                    ? Invariant($"The type specified in the deserialize call is '{result.ToStringReadable()}', which is not a closed non-anonymous concrete class.")
                    : Invariant($"The property bag specifies the type to deserialize into via the '{ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag}' property (versionless assembly qualified name) as '{result.ToStringReadable()}', which is not a closed non-anonymous concrete class.");

                throw new SerializationException(Invariant($"Can only deserialize into a closed non-anonymous concrete class.  {exceptionMessageSuffix}"));
            }

            return result;
        }
    }
}
