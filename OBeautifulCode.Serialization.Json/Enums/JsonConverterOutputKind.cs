// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConverterOutputKind.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using Newtonsoft.Json;

    /// <summary>
    /// Specifies the kind of output of a <see cref="JsonConverter"/>.
    /// </summary>
    public enum JsonConverterOutputKind
    {
        /// <summary>
        /// Completely unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Outputs what JSON will consider a string.
        /// </summary>
        String,

        /// <summary>
        /// Outputs what JSON will consider an object (i.e. a start object is emitted.)
        /// </summary>
        Object,
    }
}