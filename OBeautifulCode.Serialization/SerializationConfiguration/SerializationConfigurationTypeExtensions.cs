﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationTypeExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Extension methods related to <see cref="SerializationConfigurationType"/>.
    /// </summary>
    public static class SerializationConfigurationTypeExtensions
    {
        /// <summary>
        /// Gets the <see cref="SerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </summary>
        /// <param name="serializationConfigurationType">The type of the serialization configuration.</param>
        /// <returns>
        /// The <see cref="SerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </returns>
        public static SerializationConfigurationType ToSerializationConfigurationType(
            this Type serializationConfigurationType)
        {
            var result = new SerializationConfigurationType(serializationConfigurationType);

            return result;
        }
    }
}