// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBinarySerializeAndDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Serialization.Internal;

    /// <summary>
    /// Interface to serialize and deserialize to and from a byte array.
    /// </summary>
    public interface IBinarySerializeAndDeserialize : IBinarySerialize, IBinaryDeserialize
    {
    }

    /// <summary>
    /// Interface to serialize to a byte array.
    /// </summary>
    public interface IBinarySerialize : IHaveSerializationConfigurationType
    {
        /// <summary>
        /// Serializes an object into a byte array.
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>
        /// Serialized object into a byte array.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        byte[] SerializeToBytes(
            object objectToSerialize);
    }

    /// <summary>
    /// Interface to deserialize from a byte array.
    /// </summary>
    public interface IBinaryDeserialize : IHaveSerializationConfigurationType
    {
        /// <summary>
        /// Deserializes the byte array into an object.
        /// </summary>
        /// <param name="serializedBytes">Byte array to deserialize.</param>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <returns>
        /// Deserialized bytes into object of specified type.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        T Deserialize<T>(
            byte[] serializedBytes);

        /// <summary>
        /// Deserializes the byte array into an object.
        /// </summary>
        /// <param name="serializedBytes">Byte array to deserialize.</param>
        /// <param name="type">Type to deserialize into.</param>
        /// <returns>
        /// Deserialized bytes into object of specified type.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        object Deserialize(
            byte[] serializedBytes,
            Type type);
    }
}