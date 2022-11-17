// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlyJsonSerializationConfiguration{T1,T2,T3,T4}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// A JSON serialization configuration that populates <see cref="DependentJsonSerializationConfigurationTypes"/> with typeof(<typeparamref name="T1"/>), typeof(<typeparamref name="T2"/>), typeof(<typeparamref name="T3"/>), and typeof(<typeparamref name="T4"/>).
    /// </summary>
    /// <typeparam name="T1">The first dependent JSON serialization configuration type.</typeparam>
    /// <typeparam name="T2">The second dependent JSON serialization configuration type.</typeparam>
    /// <typeparam name="T3">The third dependent JSON serialization configuration type.</typeparam>
    /// <typeparam name="T4">The fourth dependent JSON serialization configuration type.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = ObcSuppressBecause.CA1005_AvoidExcessiveParametersOnGenericTypes_SpecifiedParametersRequiredForNeededFunctionality)]
    public sealed class DependencyOnlyJsonSerializationConfiguration<T1, T2, T3, T4> : JsonSerializationConfigurationBase
        where T1 : JsonSerializationConfigurationBase
        where T2 : JsonSerializationConfigurationBase
        where T3 : JsonSerializationConfigurationBase
        where T4 : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new[]
        {
            typeof(T1).ToJsonSerializationConfigurationType(),
            typeof(T2).ToJsonSerializationConfigurationType(),
            typeof(T3).ToJsonSerializationConfigurationType(),
            typeof(T4).ToJsonSerializationConfigurationType(),
        };
    }
}