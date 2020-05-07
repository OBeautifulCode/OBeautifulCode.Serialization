// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesToRegisterSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A default serialization configuration that adds the internally required types to <see cref="TypesToRegister"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <remarks>
    /// This is the default serialization configuration for <see cref="TypesToRegisterSerializationConfiguration{T}"/> and <see cref="TypesToRegisterSerializationConfiguration{T1,T2}"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesToRegisterSerializationConfiguration : SerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegister> TypesToRegister => InternallyRequiredTypes.Select(_ => _.ToTypeToRegister()).ToList();

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