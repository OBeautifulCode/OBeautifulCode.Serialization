// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcDictionaryStringStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Serialization.Internal;

    using static System.FormattableString;

    /// <summary>
    /// Interface to work with compression.
    /// </summary>
    public class ObcDictionaryStringStringSerializer : IStringSerializeAndDeserialize
    {
        /// <summary>
        /// Default delimiter for specifying a key and value in a string.
        /// </summary>
        public const string DefaultKeyValueDelimiter = "=";

        /// <summary>
        /// Default delimiter for specifying multiple entries in a single string.
        /// </summary>
        public const string DefaultLineDelimiter = "\r\n";

        /// <summary>
        /// Default encoding of NULLs.
        /// </summary>
        public const string DefaultNullValueEncoding = "<null>";

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcDictionaryStringStringSerializer"/> class.
        /// </summary>
        /// <param name="keyValueDelimiter">Delimiter for the key and value.</param>
        /// <param name="lineDelimiter">Delimiter for the lines.</param>
        /// <param name="nullValueEncoding">Encoding for NULLs.</param>
        public ObcDictionaryStringStringSerializer(
            string keyValueDelimiter = DefaultKeyValueDelimiter,
            string lineDelimiter = DefaultLineDelimiter,
            string nullValueEncoding = DefaultNullValueEncoding)
        {
            new { keyValueDelimiter }.AsArg().Must().NotBeNull();
            new { lineDelimiter }.AsArg().Must().NotBeNull();

            this.KeyValueDelimiter = keyValueDelimiter;
            this.LineDelimiter = lineDelimiter;
            this.NullValueEncoding = nullValueEncoding;
        }

        /// <summary>
        /// Gets the key value delimiter.
        /// </summary>
        public string KeyValueDelimiter { get; }

        /// <summary>
        /// Gets the line delimiter.
        /// </summary>
        public string LineDelimiter { get; }

        /// <summary>
        /// Gets the null encoding.
        /// </summary>
        public string NullValueEncoding { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => null;

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            string result;

            if (objectToSerialize == null)
            {
                result = SerializationConfigurationBase.NullSerializedStringValue;
            }
            else
            {
                var dictionary = objectToSerialize as IReadOnlyDictionary<string, string>;

                dictionary.AsArg(Invariant($"typeMustBeConvertibleTo-{nameof(IReadOnlyDictionary<string, string>)}-found-{objectToSerialize.GetType()}")).Must().NotBeNull();

                result = this.SerializeDictionaryToString(dictionary);
            }

            return result;
        }

        /// <summary>
        /// Serializes a dictionary to a string.
        /// </summary>
        /// <param name="dictionary">Dictionary to serialize.</param>
        /// <returns>
        /// String serialized dictionary.
        /// </returns>
        public string SerializeDictionaryToString(
            IReadOnlyDictionary<string, string> dictionary)
        {
            string result;

            if (dictionary == null)
            {
                 result = SerializationConfigurationBase.NullSerializedStringValue;
            }
            else if (!dictionary.Any())
            {
                result = string.Empty;
            }
            else
            {
                var stringBuilder = new StringBuilder();

                foreach (var keyValuePair in dictionary)
                {
                    var key = keyValuePair.Key;

                    var value = keyValuePair.Value ?? this.NullValueEncoding;

                    key.Contains(this.KeyValueDelimiter).AsArg(Invariant($"Key-cannot-contain-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}--found-on-key--{key}")).Must().BeFalse();

                    (value ?? string.Empty).Contains(this.KeyValueDelimiter).AsArg(Invariant($"Key-cannot-contain-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}--found-on-value--{value}")).Must().BeFalse();

                    key.Contains(this.LineDelimiter).AsArg(Invariant($"Key-cannot-contain-{nameof(this.LineDelimiter)}--{(Environment.NewLine == this.LineDelimiter ? "NEWLINE" : this.LineDelimiter)}--found-on-key--{key}")).Must().BeFalse();

                    (value ?? string.Empty).Contains(this.LineDelimiter).AsArg(Invariant($"Key-cannot-contain-{nameof(this.LineDelimiter)}--{(Environment.NewLine == this.LineDelimiter ? "NEWLINE" : this.LineDelimiter)}--found-on-value--{value}")).Must().BeFalse();

                    stringBuilder.Append(Invariant($"{key}{this.KeyValueDelimiter}{value ?? string.Empty}"));

                    stringBuilder.Append(this.LineDelimiter);
                }

                result = stringBuilder.ToString();
            }

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            T result;

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                result = default;
            }
            else
            {
                result = (T)this.Deserialize(serializedString, typeof(T));
            }

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            object result;

            if (serializedString == SerializationConfigurationBase.NullSerializedStringValue)
            {
                result = null;
            }
            else
            {
                new { type }.AsArg().Must().NotBeNull();

                var dictionary = type.Construct() as IReadOnlyDictionary<string, string>;

                dictionary.AsArg(Invariant($"typeMustBeConvertibleTo-{nameof(IReadOnlyDictionary<string, string>)}-found-{type}")).Must().NotBeNull();

                result = this.DeserializeToDictionary(serializedString);
            }

            return result;
        }

        /// <summary>
        /// Deserialize the string to a dictionary of string, string.
        /// </summary>
        /// <param name="serializedString">String to deserialize.</param>
        /// <returns>
        /// Deserialized dictionary.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public Dictionary<string, string> DeserializeToDictionary(
            string serializedString)
        {
            Dictionary<string, string> result;

            if (serializedString == null)
            {
                result = null;
            }
            else if (string.IsNullOrEmpty(serializedString))
            {
                result = new Dictionary<string, string>();
            }
            else
            {
                try
                {
                    result = new Dictionary<string, string>();

                    var lines = serializedString.Split(new[] { this.LineDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var items = line.Split(new[] { this.KeyValueDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                        items.Length.AsArg(Invariant($"Line-must-split-on-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}-to-1-or-2-items-this-did-not--{line}")).Must().BeInRange(1, 2);

                        var key = items[0];

                        var value = items.Length == 2 ? items[1] : string.Empty;

                        value = value == this.NullValueEncoding ? null : value;

                        result.Add(key, value);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new ObcSerializationException(Invariant($"Failed to deserialize '{serializedString}'"), ex);
                }
            }

            return result;
        }
    }
}