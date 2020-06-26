﻿// --------------------------------------------------------------------------------------------------------------------
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
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultMemberTypesToInclude"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultRelatedTypesToInclude"/>.</param>
        /// <param name="jsonConverterBuilder">Optional <see cref="JsonConverterBuilder"/>.  DEFAULT is null.</param>
        /// <param name="keyInDictionaryStringSerializer">Optional serializer to use when dictionaries are keyed on <paramref name="type"/> and the keys should be written-to/read-from a string.</param>
        /// <returns>
        /// The type to register for JSON serialization.
        /// </returns>
        public static TypeToRegisterForJson ToTypeToRegisterForJson(
            this Type type,
            MemberTypesToInclude memberTypesToInclude = TypeToRegisterConstants.DefaultMemberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude = TypeToRegisterConstants.DefaultRelatedTypesToInclude,
            JsonConverterBuilder jsonConverterBuilder = null,
            IStringSerializeAndDeserialize keyInDictionaryStringSerializer = null)
        {
            var result = new TypeToRegisterForJson(type, memberTypesToInclude, relatedTypesToInclude, jsonConverterBuilder, keyInDictionaryStringSerializer);

            return result;
        }
    }
}