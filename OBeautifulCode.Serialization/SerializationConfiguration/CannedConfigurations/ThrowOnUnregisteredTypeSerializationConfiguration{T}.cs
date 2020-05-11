// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnUnregisteredTypeSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A serialization configuration that populates <see cref="DependentSerializationConfigurationTypes"/> with typeof(T),
    /// and sets <see cref="SerializationConfigurationBase.UnregisteredTypeEncounteredStrategy"/> to <see cref="UnregisteredTypeEncounteredStrategy.Throw"/>.
    /// </summary>
    /// <typeparam name="T">The dependent serialization configuration type.</typeparam>
    public sealed class ThrowOnUnregisteredTypeSerializationConfiguration<T> : SerializationConfigurationBase
        where T : SerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

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