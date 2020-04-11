// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationTypeExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    /// <summary>
    /// Extension methods related to <see cref="JsonSerializationConfigurationType"/>.
    /// </summary>
    public static class JsonSerializationConfigurationTypeExtensions
    {
        /// <summary>
        /// Gets the <see cref="JsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </summary>
        /// <param name="jsonSerializationConfigurationType">The type of the JSON serialization configuration.</param>
        /// <returns>
        /// The <see cref="JsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </returns>
        public static JsonSerializationConfigurationType ToJsonSerializationConfigurationType(
            this Type jsonSerializationConfigurationType)
        {
            var result = new JsonSerializationConfigurationType(jsonSerializationConfigurationType);

            return result;
        }
    }
}