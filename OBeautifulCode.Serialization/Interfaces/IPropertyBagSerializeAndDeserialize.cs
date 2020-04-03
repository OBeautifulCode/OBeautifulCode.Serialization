// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyBagSerializeAndDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Serialization.Internal;

    /// <summary>
    /// Interface to serialize and deserialize to and from a string.
    /// </summary>
    public interface IPropertyBagSerializeAndDeserialize : IPropertyBagSerialize, IPropertyBagDeserialize
    {
    }

    /// <summary>
    /// Interface to serialize to a string.
    /// </summary>
    public interface IPropertyBagSerialize : IHaveConfigurationType
    {
        /// <summary>
        /// Serializes an object into a string.
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>Serialized object into a string.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        IReadOnlyDictionary<string, string> SerializeToPropertyBag(
            object objectToSerialize);
    }

    /// <summary>
    /// Interface to deserialize from a string.
    /// </summary>
    public interface IPropertyBagDeserialize : IHaveConfigurationType
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