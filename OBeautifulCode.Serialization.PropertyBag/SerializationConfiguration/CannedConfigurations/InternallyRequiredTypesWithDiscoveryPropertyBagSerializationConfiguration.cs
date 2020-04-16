﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesWithDiscoveryPropertyBagSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A default serialization configuration that will only register, with discovery, the internally required types.
    /// </summary>
    /// <remarks>
    /// This is a default serialization configuration for <see cref="PropertyBagSerializationConfigurationBase"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesWithDiscoveryPropertyBagSerializationConfiguration : PropertyBagSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new PropertyBagSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => InternallyRequiredTypes;
    }
}