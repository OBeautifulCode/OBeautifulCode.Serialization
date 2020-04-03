﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStringSerializeAndDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Serialization.Internal;

    /// <summary>
    /// Interface to serialize and deserialize to and from a string.
    /// </summary>
    public interface IStringSerializeAndDeserialize : IStringSerialize, IStringDeserialize
    {
    }

    /// <summary>
    /// Interface to serialize to a string.
    /// </summary>
    public interface IStringSerialize : IHaveSerializationConfigurationType
    {
        /// <summary>
        /// Serializes an object into a string.
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>
        /// Serialized object into a string.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        string SerializeToString(
            object objectToSerialize);
    }

    /// <summary>
    /// Interface to deserialize from a string.
    /// </summary>
    public interface IStringDeserialize : IHaveSerializationConfigurationType
    {
        /// <summary>
        /// Deserializes the string into an object.
        /// </summary>
        /// <param name="serializedString">String to deserialize.</param>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <returns>
        /// Deserialized string into object of specified type.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        T Deserialize<T>(
            string serializedString);

        /// <summary>
        /// Deserializes the string into an object.
        /// </summary>
        /// <param name="serializedString">String to deserialize.</param>
        /// <param name="type">Type to deserialize into.</param>
        /// <returns>
        /// Deserialized string into object of specified type.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        object Deserialize(
            string serializedString,
            Type type);
    }
}