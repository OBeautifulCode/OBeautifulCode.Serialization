// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializeAndDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Interface to serialize and deserialize to and from a byte array or string.
    /// </summary>
    public interface ISerializeAndDeserialize : ISerialize, IDeserialize, IStringSerializeAndDeserialize, IBinarySerializeAndDeserialize
    {
    }

    /// <summary>
    /// Interface to serialize to and from a byte array or string.
    /// </summary>
    public interface ISerialize : IStringSerialize, IBinarySerialize, IHaveSerializationKind
    {
    }

    /// <summary>
    /// Interface to deserialize to and from a byte array or string.
    /// </summary>
    public interface IDeserialize : IStringDeserialize, IBinaryDeserialize, IHaveSerializationKind
    {
    }

    /// <summary>
    /// Interface to expose the <see cref="Type" /> of configuration.
    /// </summary>
    public interface IHaveConfigurationType
    {
        /// <summary>
        /// Gets the <see cref="Type" /> of configuration.
        /// </summary>
        Type ConfigurationType { get; }
    }

    /// <summary>
    /// Interface to expose the <see cref="SerializationKind" /> of serializer.
    /// </summary>
    public interface IHaveSerializationKind
    {
        /// <summary>
        /// Gets the <see cref="SerializationKind" /> of the serializer.
        /// </summary>
        SerializationKind SerializationKind { get; }
    }
}