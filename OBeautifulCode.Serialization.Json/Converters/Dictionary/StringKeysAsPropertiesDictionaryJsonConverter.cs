// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringKeysAsPropertiesDictionaryJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Serialization.Json.Internal;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializes a dictionary to an object whose properties are the Key/Value pairs.
    /// The property names are the Keys serialized as strings.  The property values are
    /// the serialized Values.
    /// This converter is used when the keys serialize as strings.
    /// </summary>
    internal class StringKeysAsPropertiesDictionaryJsonConverter : DictionaryJsonConverterBase
    {
        private readonly IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> typeToKeyInDictionaryStringSerializerMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringKeysAsPropertiesDictionaryJsonConverter"/> class.
        /// </summary>
        /// <param name="typeToKeyInDictionaryStringSerializerMap">A map of type to serializer to use when dictionaries are keyed on that type and should be written-to/read-from a string.</param>
        public StringKeysAsPropertiesDictionaryJsonConverter(
            IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> typeToKeyInDictionaryStringSerializerMap)
            : base(typeToKeyInDictionaryStringSerializerMap?.Keys.ToList())
        {
            this.typeToKeyInDictionaryStringSerializerMap = typeToKeyInDictionaryStringSerializerMap;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = ObcSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            new { value }.AsArg().Must().NotBeNull();
            new { serializer }.AsArg().Must().NotBeNull();

            var valueAsDictionary = (IDictionary)value;

            var keyType = value.GetType().GetClosedDictionaryKeyType();

            var objectToWrite = new JObject();

            foreach (DictionaryEntry keyValuePair in valueAsDictionary)
            {
                string keyProperty;

                using (var keyWriter = new StringWriter(CultureInfo.InvariantCulture))
                {
                    var key = keyValuePair.Key;

                    if (this.typeToKeyInDictionaryStringSerializerMap.ContainsKey(keyType))
                    {
                        // this covers types with registered serializers for keys
                        keyProperty = this.typeToKeyInDictionaryStringSerializerMap[keyType].SerializeToString(key);
                    }
                    else
                    {
                        // this covers typeof(string) and value types

                        // Send the Key back through the front-door to get picked-up
                        // by a registered converter or Newtonsoft if there is no registered
                        // converter for the Key Type.
                        serializer.Serialize(keyWriter, key, keyType);

                        // The resulting string is in JSON, so it's surrounded by double quotes
                        // (e.g. ""myKey"").  We deserialize that payload into a string here to
                        // get the string to use for the Key.
                        using (var keyReader = new StringReader(keyWriter.ToString()))
                        {
                            keyProperty = (string)serializer.Deserialize(keyReader, typeof(string));
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(keyProperty))
                {
                    throw new InvalidOperationException(Invariant($"Key in dictionary serializes to a null or white space string.  Key type is {keyType.ToStringReadable()}."));
                }

                var jsonProperty = new JProperty(keyProperty, keyValuePair.Value == null ? JValue.CreateNull() : JToken.FromObject(keyValuePair.Value, serializer));

                objectToWrite.Add(jsonProperty);
            }

            objectToWrite.WriteTo(writer);
        }

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            new { reader }.AsArg().Must().NotBeNull();
            new { objectType }.AsArg().Must().NotBeNull();

            var keyType = objectType.GetClosedSystemDictionaryKeyType();

            var valueType = objectType.GetClosedSystemDictionaryValueType();

            var wrappedDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

            var wrappedDictionary = wrappedDictionaryType.Construct();

            var wrappedDictionaryAddMethod = wrappedDictionaryType.GetMethod(nameof(Dictionary<object, object>.Add));

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();

                while (reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType != JsonToken.PropertyName)
                    {
                        throw new JsonSerializationException("Unexpected token!");
                    }

                    var key = this.WrappedDeserialize(reader, keyType, serializer, isKey: true);

                    reader.Read();

                    var value = this.WrappedDeserialize(reader, valueType, serializer, isKey: false);

                    reader.Read();

                    // ReSharper disable once PossibleNullReferenceException
                    wrappedDictionaryAddMethod.Invoke(wrappedDictionary, new[] { key, value });
                }
            }
            else
            {
                throw new JsonSerializationException("Unexpected token!");
            }

            var result = ConvertResultAsNecessary(objectType, wrappedDictionary, new[] { keyType, valueType });

            return result;
        }

        /// <inheritdoc />
        protected override bool ShouldHandleKeyType(
            Type keyType)
        {
            var result = (keyType == typeof(string)) ||
                          keyType.IsValueType ||
                          this.TypesThatSerializeToString.Contains(keyType);

            return result;
        }

        private object WrappedDeserialize(
            JsonReader jsonReader,
            Type objectType,
            JsonSerializer serializer,
            bool isKey)
        {
            const string jsonReaderUnderlyingTokenFieldName = "_tokenType";

            if (jsonReader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else
            {
                var undoPropertyNameTokenFieldHack = false;

                if (jsonReader.TokenType == JsonToken.PropertyName)
                {
                    undoPropertyNameTokenFieldHack = true;

                    jsonReader.SetFieldValue(jsonReaderUnderlyingTokenFieldName, JsonToken.String);
                }

                object result;

                if (isKey && this.typeToKeyInDictionaryStringSerializerMap.ContainsKey(objectType))
                {
                    result = this.typeToKeyInDictionaryStringSerializerMap[objectType].Deserialize(jsonReader.Value?.ToString(), objectType);
                }
                else
                {
                    result = serializer.Deserialize(jsonReader, objectType);
                }

                if (undoPropertyNameTokenFieldHack)
                {
                    jsonReader.SetFieldValue(jsonReaderUnderlyingTokenFieldName, JsonToken.PropertyName);
                }

                return result;
            }
        }
    }
}
