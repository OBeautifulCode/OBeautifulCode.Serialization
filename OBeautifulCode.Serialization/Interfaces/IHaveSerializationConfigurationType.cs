// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHaveSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Interface to expose the serialization configuration type.
    /// </summary>
    public interface IHaveSerializationConfigurationType
    {
        /// <summary>
        /// Gets serialization configuration type.
        /// </summary>
        SerializationConfigurationType SerializationConfigurationType { get; }
    }
}
