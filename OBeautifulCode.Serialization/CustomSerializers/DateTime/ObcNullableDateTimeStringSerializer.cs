﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcNullableDateTimeStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

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
                if (objectToSerialize.GetType() != typeof(DateTime))
                {
                    throw new ArgumentException(Invariant($"{nameof(objectToSerialize)}.GetType() != typeof({nameof(DateTime)}); '{nameof(objectToSerialize)}' is of type '{objectToSerialize.GetType().ToStringReadable()}'"));
                }

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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type != typeof(DateTime?))
            {
                throw new ArgumentException(Invariant($"{nameof(type)} != typeof({nameof(DateTime)}?); '{nameof(type)}' is of type '{type.ToStringReadable()}'"));
            }

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