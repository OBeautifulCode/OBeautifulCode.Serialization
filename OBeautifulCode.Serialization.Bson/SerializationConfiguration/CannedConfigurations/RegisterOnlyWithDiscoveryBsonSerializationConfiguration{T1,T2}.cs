// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOnlyWithDiscoveryBsonSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
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
    /// <typeparam name="T1">The first type to auto-register with discovery.</typeparam>
    /// <typeparam name="T2">The second type to auto-register with discovery.</typeparam>
    public sealed class RegisterOnlyWithDiscoveryBsonSerializationConfiguration<T1, T2> : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => new[] { typeof(T1), typeof(T2) };
    }
}