// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterJsonSerializationConfiguration{T1,T2,T3}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// A JSON serialization configuration that adds <typeparamref name="T1"/>, <typeparamref name="T2"/>, and <typeparamref name="T3"/> to <see cref="TypesToRegisterForJson"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <typeparam name="T1">The first type register.</typeparam>
    /// <typeparam name="T2">The second type to register.</typeparam>
    /// <typeparam name="T3">The third type to register.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = ObcSuppressBecause.CA1005_AvoidExcessiveParametersOnGenericTypes_SpecifiedParametersRequiredForNeededFunctionality)]
    public sealed class TypesToRegisterJsonSerializationConfiguration<T1, T2, T3> : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
        {
            typeof(T1).ToTypeToRegisterForJson(),
            typeof(T2).ToTypeToRegisterForJson(),
            typeof(T3).ToTypeToRegisterForJson(),
        };

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => new[]
        {
            typeof(T1).Namespace,
            typeof(T2).Namespace,
            typeof(T3).Namespace,
        };
    }
}