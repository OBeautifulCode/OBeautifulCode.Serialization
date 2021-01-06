// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;

    using NewtonsoftFork.Json;
    using NewtonsoftFork.Json.Serialization;

    using Type = System.Type;

    /// <summary>
    /// Builds a contract resolver.
    /// </summary>
    /// <param name="getRegisteredTypesToRegistrationDetailsMapFunc">
    /// A func that gets the registered types mapped to the registration details.
    /// Note that this is a func so that we can always get the latest registered types.
    /// That set can get mutated with post-initialization registrations.
    /// </param>
    /// <returns>
    /// The contract resolver.
    /// </returns>
    public delegate IContractResolver ContractResolverBuilder(
        Func<IReadOnlyDictionary<Type, RegistrationDetails>> getRegisteredTypesToRegistrationDetailsMapFunc);

    /// <summary>
    /// Builds a JSON serializer settings.
    /// </summary>
    /// <param name="getRegisteredTypesToRegistrationDetailsMapFunc">
    /// A func that gets the registered types mapped to the registration details.
    /// Note that this is a func so that we can always get the latest registered types.
    /// That set can get mutated with post-initialization registrations.
    /// </param>
    /// <returns>
    /// The JSON serializer settings.
    /// </returns>
    public delegate JsonSerializerSettings JsonSerializerSettingsBuilder(
        Func<IReadOnlyDictionary<Type, RegistrationDetails>> getRegisteredTypesToRegistrationDetailsMapFunc);
}
