﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationKind.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Format of serialization.
    /// </summary>
    public enum SerializationKind
    {
        /// <summary>
        /// Invalid default option.
        /// </summary>
        Invalid,

        /// <summary>
        /// JSON format.
        /// </summary>
        Json,

        /// <summary>
        /// BSON format.
        /// </summary>
        Bson,

        /// <summary>
        /// Property bag (Dictionary{string, string} format.
        /// </summary>
        PropertyBag,

        /// <summary>
        /// Proprietary serialization, both sides must understand how it was serialized to work.
        /// </summary>
        Proprietary,

        /// <summary>
        /// Wrapper to honor protocol using provided <see cref="Func{TResult}" />'s.
        /// </summary>
        LambdaBacked,
    }
}