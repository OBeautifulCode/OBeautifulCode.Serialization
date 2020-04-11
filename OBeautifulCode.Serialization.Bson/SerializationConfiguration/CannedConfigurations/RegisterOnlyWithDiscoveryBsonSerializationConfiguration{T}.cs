// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOnlyWithDiscoveryBsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A BSON serialization configuration that will only register, with discovery, typeof(T).
    /// </summary>
    /// <typeparam name="T">The type to auto-register with discovery.</typeparam>
    public sealed class RegisterOnlyWithDiscoveryBsonSerializationConfiguration<T> : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => new[] { typeof(T) };
    }
}