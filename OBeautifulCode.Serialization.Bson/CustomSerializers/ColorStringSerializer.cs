// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Drawing;

    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// String serializer for <see cref="Color"/>.
    /// </summary>
    public class ColorStringSerializer : IStringSerializeAndDeserialize
    {
        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentNullException(nameof(objectToSerialize));
            }

            if (objectToSerialize.GetType() != typeof(Color))
            {
                throw new ArgumentException(Invariant($"{nameof(objectToSerialize)}.GetType() != typeof({nameof(Color)}); '{nameof(objectToSerialize)}' is of type '{objectToSerialize.GetType().ToStringReadable()}'"));
            }

            var result = ColorTranslator.ToHtml((Color)objectToSerialize);

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
            if (serializedString == null)
            {
                throw new ArgumentNullException(nameof(serializedString));
            }

            var result = ColorTranslator.FromHtml(serializedString);

            return result;
        }
    }
}
