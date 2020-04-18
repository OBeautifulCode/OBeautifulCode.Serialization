// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonFormattingKind.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    /// <summary>
    /// Kind of serialization to use.
    /// </summary>
    public enum JsonFormattingKind
    {
        /// <summary>
        /// Default option.
        /// </summary>
        Default,

        /// <summary>
        /// Compact option.
        /// </summary>
        Compact,

        /// <summary>
        /// Minimal option.
        /// </summary>
        Minimal,

        /// <summary>
        /// Invalid option.
        /// </summary>
        Invalid,
    }
}