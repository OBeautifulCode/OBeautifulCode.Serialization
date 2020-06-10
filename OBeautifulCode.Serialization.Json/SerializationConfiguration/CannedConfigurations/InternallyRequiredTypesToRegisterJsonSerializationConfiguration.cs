// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesToRegisterJsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A default JSON serialization configuration that adds the internally required types to <see cref="TypesToRegisterForJson"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <remarks>
    /// This is a default serialization configuration for <see cref="JsonSerializationConfigurationBase"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesToRegisterJsonSerializationConfiguration : JsonSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new JsonSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => InternallyRequiredTypes.Select(_ => _.ToTypeToRegisterForJson()).ToList();

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => InternallyRequiredTypes.Select(_ => _.Namespace).Distinct().ToList();
    }
}