// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.NamedString.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.String.Recipes;

    using static System.FormattableString;

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
            // Verifying the type involves looking for the ReservedKeyForTypeVersionlessAssemblyQualifiedName key.
            serializedPropertyBag = GetSerializedPropertyBagToUseOrThrow(serializedPropertyBag);

            type = this.GetTypeToDeserializeIntoOrThrow(type, serializedPropertyBag);

            var result = this.DeserializeInternal(serializedPropertyBag, type);

            return result;
        }

        private object DeserializeInternal(
            IReadOnlyDictionary<string, string> serializedPropertyBag,
            Type type)
        {
            var propertyOfConcernNameToPropertyMap = GetPropertyOfConcernNameToPropertyMap(type);

            var propertyNameToObjectMap = new Dictionary<string, object>();

            foreach (var serializedPropertyName in serializedPropertyBag.Keys)
            {
                if (serializedPropertyName == ReservedKeyForTypeVersionlessAssemblyQualifiedName)
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
    }
}