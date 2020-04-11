// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesRegisterOnlyWithDiscoverySerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A default serialization configuration that will only register, with discovery, the internally required types.
    /// </summary>
    /// <remarks>
    /// This is the default serialization configuration for <see cref="RegisterOnlyWithDiscoverySerializationConfiguration{T}"/> and <see cref="RegisterOnlyWithDiscoverySerializationConfiguration{T1, T2}"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesRegisterOnlyWithDiscoverySerializationConfiguration : SerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => InternallyRequiredTypes;
    }
}