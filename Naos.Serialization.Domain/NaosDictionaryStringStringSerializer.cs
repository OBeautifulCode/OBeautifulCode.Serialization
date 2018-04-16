﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NaosDictionaryStringStringSerializer.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Serialization.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using OBeautifulCode.Reflection.Recipes;

    using Spritely.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Interface to work with compression.
    /// </summary>
    public class NaosDictionaryStringStringSerializer : IStringSerializeAndDeserialize
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
        /// Initializes a new instance of the <see cref="NaosDictionaryStringStringSerializer"/> class.
        /// </summary>
        /// <param name="keyValueDelimiter">Delimiter for the key and value.</param>
        /// <param name="lineDelimiter">Delimiter for the lines.</param>
        /// <param name="nullValueEncoding">Encoding for NULLs.</param>
        public NaosDictionaryStringStringSerializer(string keyValueDelimiter = DefaultKeyValueDelimiter, string lineDelimiter = DefaultLineDelimiter, string nullValueEncoding = DefaultNullValueEncoding)
        {
            new { keyValueDelimiter }.Must().NotBeNull().OrThrowFirstFailure();
            new { lineDelimiter }.Must().NotBeNull().OrThrowFirstFailure();

            this.KeyValueDelimiter = keyValueDelimiter;
            this.LineDelimiter = lineDelimiter;
            this.NullValueEncoding = nullValueEncoding;
        }

        /// <summary>
        /// Gets the key value delimiter.
        /// </summary>
        public string KeyValueDelimiter { get; private set; }

        /// <summary>
        /// Gets the line delimiter.
        /// </summary>
        public string LineDelimiter { get; private set; }

        /// <summary>
        /// Gets the null encoding.
        /// </summary>
        public string NullValueEncoding { get; private set; }

        /// <inheritdoc cref="IHaveSerializationKind" />
        public SerializationKind SerializationKind => SerializationKind.Default;

        /// <inheritdoc cref="IHaveConfigurationType" />
        public Type ConfigurationType => null;

        /// <inheritdoc cref="IStringSerializeAndDeserialize" />
        public string SerializeToString(object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var dictionary = objectToSerialize as IReadOnlyDictionary<string, string>;
            dictionary.Named(Invariant($"typeMustBeConvertableTo-{nameof(IReadOnlyDictionary<string, string>)}-found-{objectToSerialize.GetType()}")).Must()
                .NotBeNull().OrThrowFirstFailure();

            return this.SerializeDictionaryToString(dictionary);
        }

        /// <summary>
        /// Serializes a dictionary to a string.
        /// </summary>
        /// <param name="dictionary">Dictionary to serialize.</param>
        /// <returns>String serialized dictionary.</returns>
        public string SerializeDictionaryToString(IReadOnlyDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }

            if (!dictionary.Any())
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            foreach (var keyValuePair in dictionary)
            {
                var key = keyValuePair.Key;
                var value = keyValuePair.Value ?? this.NullValueEncoding;

                key.Contains(this.KeyValueDelimiter).Named(Invariant($"Key-cannot-contain-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}--found-on-key--{key}")).Must()
                    .BeFalse().OrThrowFirstFailure();
                (value ?? string.Empty).Contains(this.KeyValueDelimiter).Named(Invariant($"Key-cannot-contain-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}--found-on-value--{value}"))
                    .Must().BeFalse().OrThrowFirstFailure();

                key.Contains(this.LineDelimiter)
                    .Named(
                        Invariant(
                            $"Key-cannot-contain-{nameof(this.LineDelimiter)}--{(Environment.NewLine == this.LineDelimiter ? "NEWLINE" : this.LineDelimiter)}--found-on-key--{key}"))
                    .Must().BeFalse().OrThrowFirstFailure();
                (value ?? string.Empty).Contains(this.LineDelimiter)
                    .Named(
                        Invariant(
                            $"Key-cannot-contain-{nameof(this.LineDelimiter)}--{(Environment.NewLine == this.LineDelimiter ? "NEWLINE" : this.LineDelimiter)}--found-on-value--{value}"))
                    .Must().BeFalse().OrThrowFirstFailure();

                stringBuilder.Append(Invariant($"{key}{this.KeyValueDelimiter}{value ?? string.Empty}"));
                stringBuilder.Append(this.LineDelimiter);
            }

            var ret = stringBuilder.ToString();
            return ret;
        }

        /// <inheritdoc cref="IStringSerializeAndDeserialize"/>
        public T Deserialize<T>(string serializedString)
        {
            if (serializedString == null)
            {
                return default(T);
            }

            return (T)this.Deserialize(serializedString, typeof(T));
        }

        /// <inheritdoc cref="IStringSerializeAndDeserialize"/>
        public object Deserialize(string serializedString, Type type)
        {
            if (serializedString == null)
            {
                return null;
            }

            new { type }.Must().NotBeNull().OrThrowFirstFailure();

            var dictionary = type.Construct() as IReadOnlyDictionary<string, string>;
            dictionary.Named(Invariant($"typeMustBeConvertableTo-{nameof(IReadOnlyDictionary<string, string>)}-found-{type}")).Must().NotBeNull().OrThrowFirstFailure();

            return this.DeserializeToDictionary(serializedString);
        }

        /// <summary>
        /// Deserialize the string to a dictionary of string, string.
        /// </summary>
        /// <param name="serializedString">String to deserialize.</param>
        /// <returns>Deserialized dictionary.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = "Name/spelling is correct.")]
        public Dictionary<string, string> DeserializeToDictionary(string serializedString)
        {
            if (serializedString == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(serializedString))
            {
                return new Dictionary<string, string>();
            }

            try
            {
                var ret = new Dictionary<string, string>();
                var lines = serializedString.Split(new[] { this.LineDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var items = line.Split(new[] { this.KeyValueDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                    items.Length.Named(Invariant($"Line-must-split-on-{nameof(this.KeyValueDelimiter)}--{this.KeyValueDelimiter}-to-1-or-2-items-this-did-not--{line}"))
                        .Must().BeInRange(1, 2).OrThrowFirstFailure();
                    var key = items[0];
                    var value = items.Length == 2 ? items[1] : string.Empty;
                    value = value == this.NullValueEncoding ? null : value;
                    ret.Add(key, value);
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new NaosSerializationException(Invariant($"Failed to deserialize '{serializedString}'"), ex);
            }
        }
    }
}