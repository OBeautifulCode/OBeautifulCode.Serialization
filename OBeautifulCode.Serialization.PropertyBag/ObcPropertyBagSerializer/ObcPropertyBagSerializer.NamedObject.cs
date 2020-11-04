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

            var propertiesOfConcern = GetPropertiesOfConcern(objectType, ordered: false);

            var result = propertiesOfConcern.ToDictionary(
                _ => _.Name,
                _ =>
                {
                    var propertyValue = _.GetValue(objectToSerialize);

                    return propertyValue;
                });

            if (this.propertyBagConfiguration.IncludeVersionlessAssemblyQualifiedNameAsProperty)
            {
                var versionlessAssemblyQualifiedName = objectType.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();

                result.Add(ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag, versionlessAssemblyQualifiedName);
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
            // Verifying the type involves looking for the ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag key.
            serializedPropertyBag = GetSerializedPropertyBagToUseOrThrow(serializedPropertyBag);

            type = this.GetTypeToDeserializeIntoOrThrow(type, serializedPropertyBag, ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag);

            var result = DeserializeInternal(serializedPropertyBag, type);

            return result;
        }

        private static object DeserializeInternal(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type)
        {
            var propertiesOfConcern = GetPropertiesOfConcern(type, ordered: false);

            var constructor = GetBestMatchConstructorOrThrow(type, propertiesOfConcern);

            var result = constructor.IsDefaultConstructor()
                ? DeserializeUsingDefaultConstructor(serializedPropertyBag, type, propertiesOfConcern)
                : DeserializeUsingParameterizedConstructor(serializedPropertyBag, type, constructor);

            return result;
        }

        private static object DeserializeUsingDefaultConstructor(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type,
            IReadOnlyCollection<PropertyInfo> propertiesOfConcern)
        {
            var result = type.Construct();

            foreach (var property in propertiesOfConcern)
            {
                var propertyName = property.Name;

                var propertyValue = GetPropertyValueOrThrow(serializedPropertyBag, type, propertyName, property.PropertyType);

                property.SetValue(result, propertyValue);
            }

            return result;
        }

        private static object DeserializeUsingParameterizedConstructor(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type,
            ConstructorInfo constructor)
        {
            var constructorParameters = constructor.GetParameters();

            var constructorParameterValues = new object[constructorParameters.Length];

            for (var x = 0; x < constructorParameters.Length; x++)
            {
                var constructorParameter = constructorParameters[x];

                constructorParameterValues[x] = GetPropertyValueOrThrow(serializedPropertyBag, type, constructorParameter.Name, constructorParameter.ParameterType);
            }

            var result = constructor.Invoke(constructorParameterValues);

            return result;
        }

        private static object GetPropertyValueOrThrow(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type,
            string propertyName,
            Type propertyType)
        {
            if (!serializedPropertyBag.ContainsKey(propertyName))
            {
                throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} is missing a property.  Expected to find '{propertyName}' (case-insensitive search), which is a property (public, inherited or declared, writable, instance) on the return type '{type.ToStringReadable()}'."));
            }

            var result = serializedPropertyBag[propertyName];

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
