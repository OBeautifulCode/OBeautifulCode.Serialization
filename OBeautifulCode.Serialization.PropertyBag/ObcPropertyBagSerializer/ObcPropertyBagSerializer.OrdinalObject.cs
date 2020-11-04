// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.OrdinalObject.cs" company="OBeautifulCode">
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

    public partial class ObcPropertyBagSerializer : IOrdinalPropertyBagObjectValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<int, object> SerializeToOrdinalPropertyBagWithObjectValues(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalPropertyBagThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            if (objectToSerialize == null)
            {
                return null;
            }

            var propertiesOfConcernInOrder = GetPropertiesOfConcern(objectType, ordered: true);

            var result = new Dictionary<int, object>();

            for (var x = 0; x < propertiesOfConcernInOrder.Count; x++)
            {
                var property = propertiesOfConcernInOrder[x];

                result.Add(x, property.GetValue(objectToSerialize));
            }

            if (this.propertyBagConfiguration.IncludeVersionlessAssemblyQualifiedNameAsProperty)
            {
                var versionlessAssemblyQualifiedName = objectType.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();

                result.Add(ReservedKeyForTypeVersionlessAssemblyQualifiedNameInOrdinalPropertyBag, versionlessAssemblyQualifiedName);
            }

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<int, object> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<int, object> serializedPropertyBag,
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
            // Verifying the type involves looking for the ReservedKeyForTypeVersionlessAssemblyQualifiedNameInOrdinalPropertyBag key.
            serializedPropertyBag = GetSerializedPropertyBagToUseOrThrow(serializedPropertyBag);

            type = this.GetTypeToDeserializeIntoOrThrow(type, serializedPropertyBag, ReservedKeyForTypeVersionlessAssemblyQualifiedNameInOrdinalPropertyBag);

            var result = DeserializeInternal(serializedPropertyBag, type);

            return result;
        }

        private static object DeserializeInternal(
            IReadOnlyDictionary<int, object> serializedPropertyBag,
            Type type)
        {
            var propertiesOfConcernInOrder = GetPropertiesOfConcern(type, ordered: true);

            var constructor = GetBestMatchConstructorOrThrow(type, propertiesOfConcernInOrder);

            var result = constructor.IsDefaultConstructor()
                ? DeserializeUsingDefaultConstructor(serializedPropertyBag, type, propertiesOfConcernInOrder)
                : DeserializeUsingParameterizedConstructor(serializedPropertyBag, type, constructor);

            return result;
        }

        private static object DeserializeUsingDefaultConstructor(
            IReadOnlyDictionary<int, object> serializedPropertyBag,
            Type type,
            IReadOnlyList<PropertyInfo> propertiesOfConcernInOrder)
        {
            var result = type.Construct();

            for (var x = 0; x < propertiesOfConcernInOrder.Count; x++)
            {
                var property = propertiesOfConcernInOrder[x];

                var propertyValue = GetPropertyValueOrThrow(serializedPropertyBag, type, x, property.Name, property.PropertyType);

                property.SetValue(result, propertyValue);
            }

            return result;
        }

        private static object DeserializeUsingParameterizedConstructor(
            IReadOnlyDictionary<int, object> serializedPropertyBag,
            Type type,
            ConstructorInfo constructor)
        {
            var constructorParameters = constructor.GetParameters();

            var constructorParameterValues = new object[constructorParameters.Length];

            for (var x = 0; x < constructorParameters.Length; x++)
            {
                var constructorParameter = constructorParameters[x];

                constructorParameterValues[x] = GetPropertyValueOrThrow(serializedPropertyBag, type, x, constructorParameter.Name, constructorParameter.ParameterType);
            }

            var result = constructor.Invoke(constructorParameterValues);

            return result;
        }

        private static object GetPropertyValueOrThrow(
            IReadOnlyDictionary<int, object> serializedPropertyBag,
            Type type,
            int index,
            string propertyName,
            Type propertyType)
        {
            if (!serializedPropertyBag.ContainsKey(index))
            {
                throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} is missing the property at index {index}, which corresponds to the '{propertyName}' property on the return type '{type.ToStringReadable()}'."));
            }

            var result = serializedPropertyBag[index];

            if (result == null)
            {
                if (!propertyType.IsTypeAssignableToNull())
                {
                    throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} has a null value for the property at index {index}, which corresponds to the '{propertyName}' property on the return type '{type.ToStringReadable()}', but that property cannot be assigned to null."));
                }
            }
            else
            {
                var propertyValueType = result.GetType();

                if (!propertyType.IsAssignableFrom(propertyValueType))
                {
                    throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} contains has a value of type '{propertyValueType.ToStringReadable()}' for the property at index {index}, which corresponds to the '{propertyName}' property on the return type '{type.ToStringReadable()}', but that value cannot be assigned to the property type '{propertyType.ToStringReadable()}'."));
                }
            }

            return result;
        }
    }
}