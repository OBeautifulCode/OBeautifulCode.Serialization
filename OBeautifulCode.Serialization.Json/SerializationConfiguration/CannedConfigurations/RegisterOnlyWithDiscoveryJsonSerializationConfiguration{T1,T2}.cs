// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOnlyWithDiscoveryJsonSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    /// <summary>
    /// A JSON serialization configuration that will only register, with discovery, typeof(T).
    /// </summary>
    /// <typeparam name="T1">The first type to auto-register with discovery.</typeparam>
    /// <typeparam name="T2">The second type to auto-register with discovery.</typeparam>
    public sealed class RegisterOnlyWithDiscoveryJsonSerializationConfiguration<T1, T2> : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[] { typeof(T1).ToTypeToRegisterForJson(), typeof(T2).ToTypeToRegisterForJson() };
    }
}