// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializeToString.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Interface to serialize itself to a string.
    /// </summary>
    public interface ISerializeToString
    {
        /// <summary>
        /// Serialize to a string.
        /// </summary>
        /// <returns>String version of object.</returns>
        string SerializeToString();
    }
}