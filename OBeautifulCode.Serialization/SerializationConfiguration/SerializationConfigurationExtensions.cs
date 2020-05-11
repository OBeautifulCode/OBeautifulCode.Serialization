// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods related to serialization configuration.
    /// </summary>
    public static class SerializationConfigurationExtensions
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

        /// <summary>
        /// Builds a <see cref="TypeToRegister"/> from a type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultMemberTypesToInclude"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultRelatedTypesToInclude"/>.</param>
        /// <returns>
        /// The type to register for serialization.
        /// </returns>
        public static TypeToRegister ToTypeToRegister(
            this Type type,
            MemberTypesToInclude memberTypesToInclude = TypeToRegisterConstants.DefaultMemberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude = TypeToRegisterConstants.DefaultRelatedTypesToInclude)
        {
            var result = new TypeToRegister(type, memberTypesToInclude, relatedTypesToInclude);

            return result;
        }
    }
}