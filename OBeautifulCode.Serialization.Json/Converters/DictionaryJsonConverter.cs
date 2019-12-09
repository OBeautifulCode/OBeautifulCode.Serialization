// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Custom dictionary converter to do the right thing.
    /// Supports:
    /// - <see cref="IDictionary{TKey, TValue}"/>
    /// - <see cref="Dictionary{TKey, TValue}"/>
    /// - <see cref="IReadOnlyDictionary{TKey, TValue}" />
    /// - <see cref="ReadOnlyDictionary{TKey, TValue}" />
    /// - <see cref="ConcurrentDictionary{TKey, TValue}" />.
    /// </summary>
    internal class DictionaryJsonConverter : DictionaryJsonConverterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryJsonConverter"/> class.
        /// </summary>
        /// <param name="typesThatSerializeToString">Types that convert to a string when serialized.</param>
        public DictionaryJsonConverter(IReadOnlyCollection<Type> typesThatSerializeToString)
            : base(typesThatSerializeToString)
        {
        }

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new NotSupportedException();
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

            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();
                if (reader.TokenType == JsonToken.EndArray)
                {
                    return new Dictionary<string, string>();
                }
                else
                {
                    throw new JsonSerializationException("Non-empty JSON array does not make a valid Dictionary!");
                }
            }
            else if (reader.TokenType == JsonToken.Null)
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
        protected override bool ShouldConsiderKeyType(Type keyType)
        {
            // We exclude DateTime because if not, then it gets picked-up as a Value type
            // and activates this converter.  However, this converter doesn't support writing
            // (see above CanWrite = false), so it falls thru the converter stack and Newtonsoft
            // picks it up and seeing that the key is DateTime, Newtonsoft handles the serialization
            // instead of sending the request back to the converter stack (which would then get
            // picked-up by our own DateTime converter).  This is problematic because this converter
            // CanWrite, and when it attempts to deserialize a DateTime, our DateTime converter
            // throws because the format of the serialized string is not recognized.
            var result = ((keyType != typeof(DateTime)) && (keyType != typeof(DateTime?)))
                         &&  ((keyType == typeof(string)) ||
                              keyType.IsValueType ||
                              this.typesThatSerializeToString.Contains(keyType));

            return result;
        }
    }
}
