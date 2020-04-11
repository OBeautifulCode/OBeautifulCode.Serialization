// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesWithDiscoveryBsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A default serialization configuration that will only register, with discovery, the internally required types.
    /// </summary>
    /// <remarks>
    /// This is a default serialization configuration for <see cref="BsonSerializationConfigurationBase"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesWithDiscoveryBsonSerializationConfiguration : BsonSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new BsonSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => InternallyRequiredTypes;
    }
}