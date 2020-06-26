// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcNullableDateTimeStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// String serializer for <see cref="Nullable{DateTime}"/> />.
    /// </summary>
    public class ObcNullableDateTimeStringSerializer : IStringSerializeAndDeserialize
    {
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
                new { objectToSerialize }.AsArg().Must().BeOfType<DateTime>();

                result = ObcDateTimeStringSerializer.SerializeToString((DateTime)objectToSerialize);
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
            new { type }.AsArg().Must().NotBeNull().And().BeEqualTo(typeof(DateTime?));

            object result;

            if (serializedString == null)
            {
                result = null;
            }
            else
            {
                result = ObcDateTimeStringSerializer.DeserializeToDateTime(serializedString);
            }

            return result;
        }
    }
}