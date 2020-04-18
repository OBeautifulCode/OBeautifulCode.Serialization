// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Type = System.Type;

    /// <summary>
    /// Builds a contract resolver.
    /// </summary>
    /// <param name="registeredTypes">The registered types.</param>
    /// <returns>
    /// The contract resolver.
    /// </returns>
    public delegate IContractResolver ContractResolverBuilder(
        IReadOnlyCollection<Type> registeredTypes);

    /// <summary>
    /// Builds a JSON serializer settings.
    /// </summary>
    /// <param name="registeredTypes">The registered types.</param>
    /// <returns>
    /// The JSON serializer settings.
    /// </returns>
    public delegate JsonSerializerSettings JsonSerializerSettingsBuilder(
        IReadOnlyCollection<Type> registeredTypes);
}
