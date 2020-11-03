// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.NamedObject.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    public partial class ObcPropertyBagSerializer : INamedPropertyBagObjectValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<string, object> SerializeToNamedPropertyBagWithObjectValues(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalPropertyBagThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            if (objectToSerialize == null)
            {
                return null;
            }

            var typeCaseInsensitivePropertyNameToPropertyMap = GetPropertyOfConcernNameToPropertyMap(objectType);

            var result = typeCaseInsensitivePropertyNameToPropertyMap.ToDictionary(
                _ => _.Key,
                _ =>
                {
                    var propertyValue = _.Value.GetValue(objectToSerialize);

                    return propertyValue;
                });

            if (this.propertyBagConfiguration.IncludeVersionlessAssemblyQualifiedNameAsProperty)
            {
                var versionlessAssemblyQualifiedName = objectType.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();

                result.Add(ReservedKeyForTypeVersionlessAssemblyQualifiedName, versionlessAssemblyQualifiedName);
            }

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<string, object> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.InternalPropertyBagThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            if (serializedPropertyBag == null)
            {
                return null;
            }

            // Do NOT change the order verifying/treating the serialized property bag and the type.
            // We have to treat the serialized property bag first to ensure the keys well-formed.
            // Verifying the type involves looking for the ReservedKeyForTypeVersionlessAssemblyQualifiedName key.
            serializedPropertyBag = GetSerializedPropertyBagToUseOrThrow(serializedPropertyBag);

            type = this.GetTypeToDeserializeIntoOrThrow(type, serializedPropertyBag);

            var result = DeserializeInternal(serializedPropertyBag, type);

            return result;
        }

        private static IReadOnlyDictionary<string, PropertyInfo> GetPropertyOfConcernNameToPropertyMap(
            Type type)
        {
            var properties = type.GetPropertiesFiltered(
                MemberRelationships.DeclaredOrInherited,
                MemberOwners.Instance,
                MemberAccessModifiers.Public,
                MemberMutability.Writable);

            if (properties.Select(_ => _.Name).Distinct(StringComparer.OrdinalIgnoreCase).Count() != properties.Count)
            {
                throw new SerializationException("One or more of properties (public, inherited or declared, writable, instance) on the object to serialize or the type to deserialize into have the same case-insensitive name.");
            }

            if (properties.Any(_ => _.Name.Equals(ReservedKeyForTypeVersionlessAssemblyQualifiedName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new SerializationException(Invariant($"There is at least one property on the object to serialize or the type to deserialize into who's case-insensitive name, '{ReservedKeyForTypeVersionlessAssemblyQualifiedName}', is reserved for the versionless assembly qualified name of the type."));
            }

            var result = properties.ToDictionary(
                _ => _.Name,
                _ => _,
                StringComparer.OrdinalIgnoreCase);

            return result;
        }

        private static object DeserializeInternal(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type)
        {
            var propertyOfConcernNameToPropertyMap = GetPropertyOfConcernNameToPropertyMap(type);

            var constructor = GetBestMatchConstructorOrThrow(type, propertyOfConcernNameToPropertyMap.Values.ToList());

            var result = constructor.IsDefaultConstructor()
                ? DeserializeUsingDefaultConstructor(serializedPropertyBag, type, propertyOfConcernNameToPropertyMap)
                : DeserializeUsingParameterizedConstructor(serializedPropertyBag, type, propertyOfConcernNameToPropertyMap, constructor);

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
                throw new SerializationException(Invariant($"Could not deserialize into '{type.ToStringReadable()}'; none of its public constructors have parameters where all parameters have a matching property (public, inherited or declared, writable, instance) by name and type."));
            }

            var maxParameterCount = candidateConstructors.Max(_ => _.GetParameters().Length);

            candidateConstructors = candidateConstructors.Where(_ => _.GetParameters().Length == maxParameterCount).ToList();

            if (candidateConstructors.Count > 1)
            {
                throw new SerializationException(Invariant($"Could not deserialize into '{type.ToStringReadable()}'; there are {candidateConstructors.Count} public constructors having the most number of parameters ({maxParameterCount}) where all parameters have a matching property (public, inherited or declared, writable, instance) by name and type, whereas only 1 was expected."));
            }

            var result = candidateConstructors.Single();

            if (result.IsDefaultConstructor())
            {
                if (!propertiesOfConcern.All(_ => _.SetMethod.IsPublic))
                {
                    throw new SerializationException(Invariant($"Could not deserialize into '{type.ToStringReadable()}'; the public constructor that best matches its properties (public, inherited or declared, writable, instance) is the default (parameterless) constructor but one or more of these properties has a non-public setter."));
                }
            }
            else
            {
                if (propertiesOfConcern.Any(_ => _.SetMethod.IsPublic))
                {
                    throw new SerializationException(Invariant($"Could not deserialize into '{type.ToStringReadable()}'; the public constructor that best matches its properties (public, inherited or declared, writable, instance) is parameterized but one or more of these properties has a public setter."));
                }

                // all of the declared properties have to appear in the parameter list
                var constructorParameterNames = new HashSet<string>(result.GetParameters().Select(_ => _.Name), StringComparer.OrdinalIgnoreCase);

                if (declaredOnlyPropertiesOfConcern.Any(_ => !constructorParameterNames.Contains(_.Name)))
                {
                    throw new SerializationException(Invariant($"Could not deserialize into '{type.ToStringReadable()}'; the public constructor that best matches its properties (public, inherited or declared, writable, instance) is parameterized but one or more of the properties declared on the type (not inherited) does not match a parameter in that constructor."));
                }
            }

            return result;
        }

        private static object DeserializeUsingDefaultConstructor(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type,
            IReadOnlyDictionary<string, PropertyInfo> propertyOfConcernNameToPropertyMap)
        {
            var result = type.Construct();

            foreach (var propertyName in propertyOfConcernNameToPropertyMap.Keys)
            {
                var propertyValue = GetPropertyValueOrThrow(serializedPropertyBag, type, propertyName, propertyOfConcernNameToPropertyMap);

                propertyOfConcernNameToPropertyMap[propertyName].SetValue(result, propertyValue);
            }

            return result;
        }

        private static object DeserializeUsingParameterizedConstructor(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type,
            IReadOnlyDictionary<string, PropertyInfo> propertyOfConcernNameToPropertyMap,
            ConstructorInfo constructor)
        {
            var constructorParameters = constructor.GetParameters();

            var constructorParameterValues = new object[constructorParameters.Length];

            for (var x = 0; x < constructorParameters.Length; x++)
            {
                var constructorParameterName = constructorParameters[x].Name;

                constructorParameterValues[x] = GetPropertyValueOrThrow(serializedPropertyBag, type, constructorParameterName, propertyOfConcernNameToPropertyMap);
            }

            var result = constructor.Invoke(constructorParameterValues);

            return result;
        }

        private static object GetPropertyValueOrThrow(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type,
            string propertyName,
            IReadOnlyDictionary<string, PropertyInfo> propertyOfConcernNameToPropertyMap)
        {
            if (!serializedPropertyBag.ContainsKey(propertyName))
            {
                throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} is missing a property.  Expected to find '{propertyName}' (case-insensitive search), which is a property (public, inherited or declared, writable, instance) on the return type '{type.ToStringReadable()}'."));
            }

            var result = serializedPropertyBag[propertyName];

            var propertyType = propertyOfConcernNameToPropertyMap[propertyName].PropertyType;

            if (result == null)
            {
                if (!propertyType.IsTypeAssignableToNull())
                {
                    throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} has a null value for the '{propertyName}' property, but that corresponds to a property on the return type '{type.ToStringReadable()}' that cannot be assigned to null."));
                }
            }
            else
            {
                var propertyValueType = result.GetType();

                if (!propertyType.IsAssignableFrom(propertyValueType))
                {
                    throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} contains the '{propertyName}' property who's value is of type '{propertyValueType.ToStringReadable()}', but that type cannot be assigned to the property type of '{propertyType.ToStringReadable()}' on the return type '{type.ToStringReadable()}'."));
                }
            }

            return result;
        }
    }
}
