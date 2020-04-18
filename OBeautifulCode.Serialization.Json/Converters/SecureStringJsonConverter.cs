﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecureStringJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    using Newtonsoft.Json;

    /// <summary>
    /// Class that enables the Json serializer to construct SecureString instances.
    /// </summary>
    internal class SecureStringJsonConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// true if this instance can convert the specified object type; otherwise, false.
        /// </returns>
        public override bool CanConvert(
            Type objectType)
        {
            return objectType == typeof(SecureString);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
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

            var result = reader.Value?.ToString().ToSecureString();

            return result;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var stringValue = value as SecureString;

            if (stringValue == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(stringValue.ToInsecureString());
            }
        }
    }

#pragma warning disable SA1204 // Static members should appear before non-static members

    /// <summary>
    /// Extension methods on <see cref="SecureString"/>.
    /// </summary>
    internal static class SecureStringExtensions
    {
        /// <summary>
        /// Converts the source string into a secure string. Caller should dispose of the secure string appropriately.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>A secure version of the source string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller is expected to dispose of object.")]
        public static SecureString ToSecureString(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var result = new SecureString();

            foreach (var c in source.ToCharArray())
            {
                result.AppendChar(c);
            }

            result.MakeReadOnly();

            return result;
        }

        /// <summary>
        /// Converts the source secure string into a standard insecure string.
        /// </summary>
        /// <param name="source">The source secure string.</param>
        /// <returns>
        /// The standard insecure string.
        /// </returns>
        public static string ToInsecureString(
            this SecureString source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(source);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }

#pragma warning restore SA1204 // Static members should appear before non-static members
}
