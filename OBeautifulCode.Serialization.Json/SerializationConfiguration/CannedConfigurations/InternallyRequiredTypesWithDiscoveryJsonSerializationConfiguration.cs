﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesWithDiscoveryJsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A default serialization configuration that will only register, with discovery, the internally required types.
    /// </summary>
    /// <remarks>
    /// This is a default serialization configuration for <see cref="JsonSerializationConfigurationBase"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesWithDiscoveryJsonSerializationConfiguration : JsonSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new JsonSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => InternallyRequiredTypes;
    }
}