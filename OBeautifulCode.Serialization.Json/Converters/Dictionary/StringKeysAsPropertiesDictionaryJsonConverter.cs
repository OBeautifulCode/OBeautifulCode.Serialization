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
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="StringKeysAsPropertiesDictionaryJsonConverter"/> class.
        /// </summary>
        /// <param name="typesThatSerializeToString">Types that convert to a string when serialized.</param>
        public StringKeysAsPropertiesDictionaryJsonConverter(
            IReadOnlyCollection<Type> typesThatSerializeToString)
            : base(typesThatSerializeToString)
        {
        }

        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            var valueAsDictionary = (IDictionary)value;

            var keyType = value.GetType().GetClosedDictionaryKeyType();

            var objectToWrite = new JObject();

            foreach (DictionaryEntry keyValuePair in valueAsDictionary)
            {
                string keyProperty;

                using (var keyWriter = new StringWriter())
                {
                    var key = keyValuePair.Key;

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

            const JsonToken propertyNameToken = JsonToken.PropertyName;

            const string jsonReaderUnderlyingTokenFieldName = "_tokenType";

            var genericArguments = objectType.GenericTypeArguments;

            new { genericArguments.Length }.AsArg().Must().BeEqualTo(2, "More ore less than 2 generic arguments means this cannot be a dictionary type that should supported.");

            var keyType = genericArguments.First();

            var valueType = genericArguments.Last();

            var wrappedDictionaryType = typeof(Dictionary<,>).MakeGenericType(genericArguments);

            var wrappedDictionary = wrappedDictionaryType.Construct();

            var wrappedDictionaryAddMethod = wrappedDictionaryType.GetMethod(nameof(Dictionary<object, object>.Add));

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                object WrappedDeserialize(JsonReader localReader, Type targetType)
                {
                    if (localReader.TokenType == JsonToken.Null)
                    {
                        return null;
                    }
                    else
                    {
                        var undoPropertyNameTokenFieldHack = false;

                        if (localReader.TokenType == propertyNameToken)
                        {
                            undoPropertyNameTokenFieldHack = true;

                            localReader.SetFieldValue(jsonReaderUnderlyingTokenFieldName, JsonToken.String);
                        }

                        var localResult = serializer.Deserialize(localReader, targetType);

                        if (undoPropertyNameTokenFieldHack)
                        {
                            localReader.SetFieldValue(jsonReaderUnderlyingTokenFieldName, propertyNameToken);
                        }

                        return localResult;
                    }
                }

                reader.Read();

                while (reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType != propertyNameToken)
                    {
                        throw new JsonSerializationException("Unexpected token!");
                    }

                    var key = WrappedDeserialize(reader, keyType);

                    reader.Read();

                    var value = WrappedDeserialize(reader, valueType);

                    reader.Read();

                    // ReSharper disable once PossibleNullReferenceException
                    wrappedDictionaryAddMethod.Invoke(wrappedDictionary, new[] { key, value });
                }
            }
            else
            {
                throw new JsonSerializationException("Unexpected token!");
            }

            var result = ConvertResultAsNecessary(objectType, wrappedDictionary, genericArguments);

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
    }
}
