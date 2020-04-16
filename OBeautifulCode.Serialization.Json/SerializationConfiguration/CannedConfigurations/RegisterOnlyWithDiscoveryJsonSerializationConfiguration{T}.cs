// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOnlyWithDiscoveryJsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    /// <summary>
    /// A JSON serialization configuration that will only register, with discovery, typeof(T).
    /// </summary>
    /// <typeparam name="T">The type to auto-register with discovery.</typeparam>
    public sealed class RegisterOnlyWithDiscoveryJsonSerializationConfiguration<T> : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[] { typeof(T).ToTypeToRegisterForJson() };
    }
}