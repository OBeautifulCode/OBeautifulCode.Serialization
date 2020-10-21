// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INamedPropertyBagStringValuesDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface to deserialize an object from a property bag,
    /// keyed on property name with the property values represented in strings.
    /// </summary>
    public interface INamedPropertyBagStringValuesDeserialize
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
            IReadOnlyDictionary<string, string> serializedPropertyBag);

        /// <summary>
        /// Deserializes the property bag into an object.
        /// </summary>
        /// <param name="serializedPropertyBag">The property bag to deserialize.</param>
        /// <param name="type">Type to deserialize into.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        object Deserialize(
            IReadOnlyDictionary<string, string> serializedPropertyBag,
            Type type);
    }
}
