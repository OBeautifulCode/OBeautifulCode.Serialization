// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationDirection.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Direction of serialization.
    /// </summary>
    public enum SerializationDirection
    {
        /// <summary>
        /// Unknown direction.
        /// </summary>
        Unknown,

        /// <summary>
        /// Serializing object.
        /// </summary>
        Serialize,

        /// <summary>
        /// Deserializing object.
        /// </summary>
        Deserialize,
    }
}
