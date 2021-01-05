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
        /// The default formatting which creates pretty-looking JSON and includes all the available information.
        /// </summary>
        Default,

        /// <summary>
        /// A compact format with no newlines.
        /// </summary>
        Compact,

        /// <summary>
        /// A concise format with no newlines and no type discriminators written into the payload.
        /// </summary>
        Concise,

        /// <summary>
        /// A minimal format with no newlines, no type discriminators, and where null properties are omitted.
        /// </summary>
        Minimal,

        /// <summary>
        /// Invalid option.
        /// </summary>
        Invalid,
    }
}