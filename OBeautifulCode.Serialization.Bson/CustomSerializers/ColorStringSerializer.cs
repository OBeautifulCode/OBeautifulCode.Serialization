// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Drawing;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// String serializer for <see cref="Color"/>.
    /// </summary>
    public class ColorStringSerializer : IStringSerializeAndDeserialize
    {
        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            new { objectToSerialize }.AsArg().Must().NotBeNull().And().BeOfType<Color>();

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
            new { serializedString }.AsArg().Must().NotBeNull();

            var result = ColorTranslator.FromHtml(serializedString);

            return result;
        }
    }
}
