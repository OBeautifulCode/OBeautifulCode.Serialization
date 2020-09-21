// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSecureStringStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// String serializer for <see cref="SecureString"/>.
    /// </summary>
    public class ObcSecureStringStringSerializer : IStringSerializeAndDeserialize
    {
        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            string result;

            if (objectToSerialize is SecureString objectToSerializeAsSecureString)
            {
                result = objectToSerializeAsSecureString.ToInsecureString();
            }
            else
            {
                throw new ArgumentException(Invariant($"{nameof(objectToSerialize)} is not a {nameof(SecureString)}; it is a {objectToSerialize.GetType().ToStringReadable()}."));
            }

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            var result = (T)this.Deserialize(serializedString, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            var result = serializedString?.ToSecureString();

            return result;
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
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = ObcSuppressBecause.CA2000_DisposeObjectsBeforeLosingScope_DisposableObjectIsMethodReturnObject)]
        public static SecureString ToSecureString(
            this string source)
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

                var result = Marshal.PtrToStringUni(unmanagedString);

                return result;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
    #pragma warning restore SA1204 // Static members should appear before non-static members
}