// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.OrdinalString.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ObcPropertyBagSerializer : IOrdinalPropertyBagStringValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<int, string> SerializeToOrdinalPropertyBagWithStringValues(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalPropertyBagThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            if (objectToSerialize == null)
            {
                return null;
            }

            var ordinalPropertyToObjectMap = this.SerializeToOrdinalPropertyBagWithObjectValues(objectToSerialize);

            var result = ordinalPropertyToObjectMap.ToDictionary(
                _ => _.Key,
                _ => this.MakeStringFromObject(_.Value));

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<int, string> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<int, string> serializedPropertyBag,
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

            type = this.GetTypeToDeserializeIntoOrThrow(type, serializedPropertyBag, ReservedKeyForTypeVersionlessAssemblyQualifiedNameInOrdinalPropertyBag);

            var result = this.DeserializeInternal(serializedPropertyBag, type);

            return result;
        }

        private object DeserializeInternal(
            IReadOnlyDictionary<int, string> serializedPropertyBag,
            Type type)
        {
            var propertiesOfConcernInOrder = GetPropertiesOfConcern(type, ordered: true);

            var propertyIndexToObjectMap = new Dictionary<int, object>();

            foreach (var index in serializedPropertyBag.Keys)
            {
                if (index == ReservedKeyForTypeVersionlessAssemblyQualifiedNameInOrdinalPropertyBag)
                {
                    // Reserved and not assigned to properties.
                    continue;
                }

                // Is this a property of concern?  If not, just ignore (maybe a property was removed after serializing an object).
                if (index < propertiesOfConcernInOrder.Count)
                {
                    var property = propertiesOfConcernInOrder[index];

                    var propertyValue = serializedPropertyBag[index];

                    // The PropertyType might not be assignable to null,
                    // but we'll let the Deserialize call below throw in that case.
                    var targetValue = propertyValue == null
                        ? null
                        : this.MakeObjectFromString(propertyValue, property.PropertyType);

                    propertyIndexToObjectMap.Add(index, targetValue);
                }
            }

            var result = this.Deserialize(propertyIndexToObjectMap, type);

            return result;
        }
    }
}