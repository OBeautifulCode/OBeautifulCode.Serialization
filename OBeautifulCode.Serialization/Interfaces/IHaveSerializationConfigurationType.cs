// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHaveSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Interface to expose the <see cref="System.Type" /> of configuration.
    /// </summary>
    public interface IHaveSerializationConfigurationType
    {
        /// <summary>
        /// Gets the <see cref="System.Type" /> of configuration.
        /// </summary>
        Type SerializationConfigurationType { get; }
    }
}
