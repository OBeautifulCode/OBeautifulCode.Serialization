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

        /// <summary>
        /// Determines if the specified type is in the <see cref="System"/> namespace.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// true if the specified type is in the <see cref="System"/> namespace, otherwise false.
        /// </returns>
        public static bool IsSystemType(
            this Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var result = type.Namespace?.StartsWith(nameof(System), StringComparison.Ordinal) ?? false;

            return result;
        }

        /// <summary>
        /// Determines if the specified type is a closed generic type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// true if the specified type is a closed generic type; otherwise false.
        /// </returns>
        public static bool IsClosedGenericType(
            this Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var result = type.IsGenericType && (!type.ContainsGenericParameters);

            return result;
        }

        /// <summary>
        /// Determines if the specified member is compiler-generated.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        /// <returns>
        /// True if the member is compiler-generated, otherwise false.
        /// </returns>
        public static bool IsCompilerGenerated(
            this MemberInfo memberInfo)
        {
            var result = memberInfo.CustomAttributes.Select(s => s.AttributeType).Contains(typeof(CompilerGeneratedAttribute));

            return result;
        }
    }
}