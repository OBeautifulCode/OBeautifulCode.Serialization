// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterJsonSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    /// <summary>
    /// A JSON serialization configuration that adds <typeparamref name="T1"/> and <typeparamref name="T2"/> to <see cref="TypesToRegisterForJson"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <typeparam name="T1">The first type register.</typeparam>
    /// <typeparam name="T2">The second type to register.</typeparam>
    public sealed class TypesToRegisterJsonSerializationConfiguration<T1, T2> : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[] { typeof(T1).ToTypeToRegisterForJson(), typeof(T2).ToTypeToRegisterForJson() };

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => new[] { typeof(T1).Namespace, typeof(T2).Namespace };
    }
}