// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcVersionStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// String serializer for <see cref="Version"/>.
    /// </summary>
    public class ObcVersionStringSerializer : IStringSerializeAndDeserialize
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

            if (objectToSerialize is Version objectToSerializeAsVersion)
            {
                result = objectToSerializeAsVersion.ToString();
            }
            else
            {
                throw new ArgumentException(Invariant($"{nameof(objectToSerialize)} is not a {nameof(Version)}; it is a {objectToSerialize.GetType().ToStringReadable()}."));
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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type != typeof(Version))
            {
                throw new ArgumentException(Invariant($"{nameof(type)} != typeof({nameof(Version)}); '{nameof(type)}' is of type '{type.ToStringReadable()}'"));
            }

            if (serializedString == null)
            {
                return null;
            }

            if (!Version.TryParse(serializedString, out Version result))
            {
                throw new InvalidOperationException(Invariant($"The serialized {nameof(Version)} is malformed: {serializedString}"));
            }

            return result;
        }
    }
}