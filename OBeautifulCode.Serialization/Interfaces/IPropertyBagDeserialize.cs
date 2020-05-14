// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyBagDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface to deserialize from a string.
    /// </summary>
    public interface IPropertyBagDeserialize
    {
        /// <summary>
        /// Deserializes the property bag into an object.
        /// </summary>
        /// <param name="serializedPropertyBag">property bag to deserialize.</param>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <returns>
        /// Deserialized property bag into object of specified type.
        /// </returns>
        T Deserialize<T>(
            IReadOnlyDictionary<string, string> serializedPropertyBag);

        /// <summary>
        /// Deserializes the property bag into an object.
        /// </summary>
        /// <param name="serializedPropertyBag">property bag to deserialize.</param>
        /// <param name="type">Type to deserialize into.</param>
        /// <returns>
        /// Deserialized property bag into object of specified type.
        /// </returns>
        object Deserialize(
            IReadOnlyDictionary<string, string> serializedPropertyBag, Type type);
    }
}
