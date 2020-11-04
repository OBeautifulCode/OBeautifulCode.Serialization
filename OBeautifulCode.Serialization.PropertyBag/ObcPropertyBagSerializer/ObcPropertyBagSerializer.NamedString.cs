// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.NamedString.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ObcPropertyBagSerializer : INamedPropertyBagStringValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> SerializeToNamedPropertyBagWithStringValues(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalPropertyBagThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            if (objectToSerialize == null)
            {
                return null;
            }

            var propertyNameToObjectMap = this.SerializeToNamedPropertyBagWithObjectValues(objectToSerialize);

            var result = propertyNameToObjectMap.ToDictionary(
                _ => _.Key,
                _ => this.MakeStringFromObject(_.Value));

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

            var result = this.DeserializeInternal(serializedPropertyBag, type);

            return result;
        }

        private object DeserializeInternal(
            IReadOnlyDictionary<string, string> serializedPropertyBag,
            Type type)
        {
            var propertiesOfConcern = GetPropertiesOfConcern(type, ordered: false);

            var propertyOfConcernNameToPropertyMap = GetPropertyOfConcernNameToPropertyMap(propertiesOfConcern);

            var propertyNameToObjectMap = new Dictionary<string, object>();

            foreach (var serializedPropertyName in serializedPropertyBag.Keys)
            {
                if (serializedPropertyName.Equals(ReservedKeyForTypeVersionlessAssemblyQualifiedNameInNamedPropertyBag, StringComparison.OrdinalIgnoreCase))
                {
                    // Reserved and not assigned to properties.
                    continue;
                }

                // Is this a property of concern?  If not, just ignore (maybe a property was removed after serializing an object).
                if (propertyOfConcernNameToPropertyMap.ContainsKey(serializedPropertyName))
                {
                    var property = propertyOfConcernNameToPropertyMap[serializedPropertyName];

                    var propertyValue = serializedPropertyBag[serializedPropertyName];

                    // The PropertyType might not be assignable to null,
                    // but we'll let the Deserialize call below throw in that case.
                    var targetValue = propertyValue == null
                        ? null
                        : this.MakeObjectFromString(propertyValue, property.PropertyType);

                    propertyNameToObjectMap.Add(serializedPropertyName, targetValue);
                }
            }

            var result = this.Deserialize(propertyNameToObjectMap, type);

            return result;
        }
    }
}