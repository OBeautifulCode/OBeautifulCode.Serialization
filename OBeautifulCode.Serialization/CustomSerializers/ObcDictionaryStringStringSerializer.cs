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

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// String serializer for <see cref="IReadOnlyDictionary{String, String}"/>.
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
            // ReSharper disable once JoinNullCheckWithUsage
            if (keyValueDelimiter == null)
            {
                throw new ArgumentNullException(nameof(keyValueDelimiter));
            }

            // ReSharper disable once JoinNullCheckWithUsage
            if (lineDelimiter == null)
            {
                throw new ArgumentNullException(nameof(lineDelimiter));
            }

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
        public string SerializeToString(
            object objectToSerialize)
        {
            string result;

            if (objectToSerialize == null)
            {
                result = null;
            }
            else
            {
                var dictionary = objectToSerialize as IReadOnlyDictionary<string, string>;

                if (dictionary == null)
                {
                    throw new ArgumentNullException(Invariant($"typeMustBeConvertibleTo-{nameof(IReadOnlyDictionary<string, string>)}-found-{objectToSerialize.GetType()}"));
                }

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
                 result = null;
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

                    // When serializing a dictionary (all key/value pairs) to a single string,
                    // we have the problem of differentiate values that are empty from values that are none
                    // (e.g. if pipe is a delimiter in "some text||", is the token after "some text" null or empty?)
                    // The only way to deal with this is to write null as a special value that can be parsed to null.
                    var value = keyValuePair.Value ?? this.NullValueEncoding;

                    if (key.Contains(this.KeyValueDelimiter))
                    {
                        throw new ArgumentException(Invariant($"Key-cannot-contain-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}--found-on-key--{key}"));
                    }

                    if ((value ?? string.Empty).Contains(this.KeyValueDelimiter))
                    {
                        throw new ArgumentException(Invariant($"Key-cannot-contain-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}--found-on-value--{value}"));
                    }

                    if (key.Contains(this.LineDelimiter))
                    {
                        throw new ArgumentException(Invariant($"Key-cannot-contain-{nameof(this.LineDelimiter)}--{(Environment.NewLine == this.LineDelimiter ? "NEWLINE" : this.LineDelimiter)}--found-on-key--{key}"));
                    }

                    if ((value ?? string.Empty).Contains(this.LineDelimiter))
                    {
                        throw new ArgumentException(Invariant($"Key-cannot-contain-{nameof(this.LineDelimiter)}--{(Environment.NewLine == this.LineDelimiter ? "NEWLINE" : this.LineDelimiter)}--found-on-value--{value}"));
                    }

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
            var type = typeof(T);

            var result = (T)this.Deserialize(serializedString, type);

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var dictionary = type.Construct() as IReadOnlyDictionary<string, string>;

            if (dictionary == null)
            {
                throw new ArgumentNullException(Invariant($"typeMustBeConvertibleTo-{nameof(IReadOnlyDictionary<string, string>)}-found-{type}"));
            }

            var result = this.DeserializeToDictionary(serializedString);

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

                        if ((items.Length != 1) && (items.Length != 2))
                        {
                            throw new ArgumentOutOfRangeException(Invariant($"Line-must-split-on-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}-to-1-or-2-items-this-did-not--{line}"), (Exception)null);
                        }

                        var key = items[0];

                        var value = items.Length == 2 ? items[1] : string.Empty;

                        value = value == this.NullValueEncoding ? null : value;

                        result.Add(key, value);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(Invariant($"Failed to deserialize '{serializedString}'"), ex);
                }
            }

            return result;
        }
    }
}