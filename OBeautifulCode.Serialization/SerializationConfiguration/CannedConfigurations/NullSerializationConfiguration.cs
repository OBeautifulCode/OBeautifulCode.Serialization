﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;

    using OBeautifulCode.Type;

    /// <summary>
    /// A serialization configuration that with no dependent serialization configurations.
    /// This configuration will result in no types registered.
    /// </summary>
    public sealed class NullSerializationConfiguration : SerializationConfigurationBase, IImplementNullObjectPattern
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new SerializationConfigurationType[0];
    }
}