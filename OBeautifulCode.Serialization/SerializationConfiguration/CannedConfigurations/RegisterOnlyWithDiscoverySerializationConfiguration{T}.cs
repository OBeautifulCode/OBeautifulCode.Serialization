﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOnlyWithDiscoverySerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A serialization configuration that will only register, with discovery, typeof(T).
    /// </summary>
    /// <remarks>
    /// This is useful to have types registered so that you can set <see cref="UnregisteredTypeEncounteredStrategy.Throw"/> when using
    /// a serializer that can accomodate a <see cref="SerializationConfigurationBase"/> (doesn't require a proprietary derivative of that base class).
    /// </remarks>
    /// <typeparam name="T">The type to auto-register with discovery.</typeparam>
    public sealed class RegisterOnlyWithDiscoverySerializationConfiguration<T> : SerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[] { typeof(InternallyRequiredTypesRegisterOnlyWithDiscoverySerializationConfiguration).ToSerializationConfigurationType() };

        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => new[] { typeof(T) };
    }
}