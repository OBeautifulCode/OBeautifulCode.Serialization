// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHaveSerializationKind.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
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
