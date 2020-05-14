// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Interface to serialize and deserialize to and from a byte array or string, with the ability to create a <see cref="SerializerRepresentation"/>.
    /// </summary>
    public interface ISerializer : ISerializeAndDeserialize, IHaveSerializationConfigurationType, IHaveSerializationKind, IHaveSerializerRepresentation
    {
    }
}