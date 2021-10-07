// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationFormat.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Format of serialization.
    /// </summary>
    public enum SerializationFormat
    {
        /// <summary>
        /// Invalid default option.
        /// </summary>
        Invalid,

        /// <summary>
        /// String format.
        /// </summary>
        String,

        /// <summary>
        /// Binary format.
        /// </summary>
        Binary,

        /// <summary>
        /// Null format.
        /// </summary>
        Null,
    }
}