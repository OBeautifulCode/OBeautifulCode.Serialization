// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrdinalPropertyBagStringValuesDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface to deserialize an object from a property bag,
    /// keyed on the properties' ordinal positions with the property values represented in strings.
    /// </summary>
    public interface IOrdinalPropertyBagStringValuesDeserialize
    {
        /// <summary>
        /// Deserializes the property bag into the specified <typeparamref name="T"/>.
        /// </summary>
        /// <param name="serializedPropertyBag">The property bag to deserialize.</param>
        /// <typeparam name="T">Type to deserialize into.</typeparam>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        T Deserialize<T>(
            IReadOnlyDictionary<int, string> serializedPropertyBag);

        /// <summary>
        /// Deserializes the property bag into an object.
        /// </summary>
        /// <param name="serializedPropertyBag">The property bag to deserialize.</param>
        /// <param name="type">Type to deserialize into.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        object Deserialize(
            IReadOnlyDictionary<int, string> serializedPropertyBag,
            Type type);
    }
}
