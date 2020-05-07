// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlySerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A serialization configuration that populates <see cref="DependentSerializationConfigurationTypes"/> with typeof(T).
    /// </summary>
    /// <typeparam name="T">The dependent serialization configuration type.</typeparam>
    public sealed class DependencyOnlySerializationConfiguration<T> : SerializationConfigurationBase
        where T : SerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new[] { typeof(T).ToSerializationConfigurationType() };

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override TypeToRegister BuildTypeToRegisterForPostInitializationRegistration(
            Type type,
            Type recursiveOriginType,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude)
        {
            var result = new TypeToRegister(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude);

            return result;
        }
    }
}