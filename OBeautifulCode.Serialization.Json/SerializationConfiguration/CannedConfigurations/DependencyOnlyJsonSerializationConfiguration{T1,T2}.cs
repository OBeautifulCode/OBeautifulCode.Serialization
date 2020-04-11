// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlyJsonSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    /// <summary>
    /// A JSON serialization configuration that populates <see cref="DependentJsonSerializationConfigurationTypes"/> with typeof(T1) and typeof(T2).
    /// </summary>
    /// <typeparam name="T1">The first dependent JSON serialization configuration type.</typeparam>
    /// <typeparam name="T2">The second dependent JSON serialization configuration type.</typeparam>
    public sealed class DependencyOnlyJsonSerializationConfiguration<T1, T2> : JsonSerializationConfigurationBase
        where T1 : JsonSerializationConfigurationBase
        where T2 : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new[] { typeof(T1).ToJsonSerializationConfigurationType(), typeof(T2).ToJsonSerializationConfigurationType() };
    }
}