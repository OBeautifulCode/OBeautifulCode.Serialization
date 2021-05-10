// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValueArrayDictionaryJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using NewtonsoftFork.Json;
    using NewtonsoftFork.Json.Linq;

    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Serializes a dictionary to an array of key/value pairs.
    /// This converter is used when the keys do not serialize as strings.
    /// </summary>
    internal class KeyValueArrayDictionaryJsonConverter : DictionaryJsonConverterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueArrayDictionaryJsonConverter"/> class.
        /// </summary>
        /// <param name="typesThatSerializeToString">Types that convert to a string when serialized.</param>
        public KeyValueArrayDictionaryJsonConverter(
            IReadOnlyCollection<Type> typesThatSerializeToString)
            : base(typesThatSerializeToString)
        {
        }

        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer,
            Type declaredType)
        {
            var valueAsEnumerable = (IEnumerable)value;

            var elementsToWrite = new List<object>();

            foreach (var element in valueAsEnumerable)
            {
                var jo = JObject.FromObject(element, serializer);

                elementsToWrite.Add(jo);
            }

            var output = new JArray(elementsToWrite.ToArray());

            output.WriteTo(writer);
        }

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            // create a List<KeyValuePair<K,V>> where K and V are the generic type parameters of the dictionary we are deserializing into
            // Get the .Add() method so that we can add elements to that list
            var genericArguments = objectType.GenericTypeArguments;
            var keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(genericArguments);
            var listOfKeyValuePairType = typeof(List<>).MakeGenericType(keyValuePairType);
            var listOfKeyValuePairs = listOfKeyValuePairType.Construct();
            var listOfKeyValuePairAddMethod = listOfKeyValuePairType.GetMethod(nameof(List<object>.Add));

            // the object was serialized as a JArray of KeyValuePair, so deserialize it as such
            var deserializedJArray = JToken.ReadFrom(reader);

            foreach (var element in deserializedJArray)
            {
                var keyValuePair = element.ToObject(keyValuePairType, serializer);

                // ReSharper disable once PossibleNullReferenceException
                listOfKeyValuePairAddMethod.Invoke(listOfKeyValuePairs, new[] { keyValuePair });
            }

            // add each KeyValuePair<T,K> into a new Dictionary<T,K>
            var wrappedDictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments);
            var wrappedDictionary = wrappedDictionaryType.Construct();
            var wrappedDictionaryAddMethod = wrappedDictionaryType.GetMethod(nameof(Dictionary<object, object>.Add));

            var listOfKeyValuePairsAsEnumerable = (IEnumerable)listOfKeyValuePairs;
            foreach (var element in listOfKeyValuePairsAsEnumerable)
            {
                var key = element.GetPropertyValue(nameof(KeyValuePair<object, object>.Key), MemberRelationships.DeclaredInType, MemberOwners.Instance, MemberAccessModifiers.Public);

                var value = element.GetPropertyValue(nameof(KeyValuePair<object, object>.Value), MemberRelationships.DeclaredInType, MemberOwners.Instance, MemberAccessModifiers.Public);

                // ReSharper disable once PossibleNullReferenceException
                wrappedDictionaryAddMethod.Invoke(wrappedDictionary, new[] { key, value });
            }

            var result = ConvertResultAsNecessary(objectType, wrappedDictionary, genericArguments);

            return result;
        }

        /// <inheritdoc />
        protected override bool ShouldHandleKeyType(
            Type keyType)
        {
            var result =
                (keyType != typeof(string)) &&
                (!keyType.IsValueType) &&
                (!this.TypesThatSerializeToString.Contains(keyType));

            return result;
        }
    }
}
