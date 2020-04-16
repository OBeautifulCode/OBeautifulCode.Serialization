// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    /// <summary>
    /// Extension methods related to JSON serialization configuration.
    /// </summary>
    public static class JsonSerializationConfigurationExtensions
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

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForJson"/> from a type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="MemberTypesToInclude.All"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="RelatedTypesToInclude.Descendants"/>.</param>
        /// <param name="jsonConverterBuilder">Optional <see cref="JsonConverterBuilder"/>.  DEFAULT is null.</param>
        /// <returns>
        /// The type to register for JSON serialization.
        /// </returns>
        public static TypeToRegisterForJson ToTypeToRegisterForJson(
            this Type type,
            MemberTypesToInclude memberTypesToInclude = MemberTypesToInclude.All,
            RelatedTypesToInclude relatedTypesToInclude = RelatedTypesToInclude.Descendants,
            JsonConverterBuilder jsonConverterBuilder = null)
        {
            var result = new TypeToRegisterForJson(type, memberTypesToInclude, relatedTypesToInclude, jsonConverterBuilder);

            return result;
        }
    }
}