// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesToRegisterBsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A default BSON serialization configuration that adds the internally required types to <see cref="TypesToRegisterForBson"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <remarks>
    /// This is a default serialization configuration for <see cref="BsonSerializationConfigurationBase"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesToRegisterBsonSerializationConfiguration : BsonSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        private static readonly IReadOnlyCollection<TypeToRegisterForBson> AdditionalTypesToRegister =
            new[]
            {
                typeof(RootObjectThatSerializesToStringWrapper<>).ToTypeToRegisterForBson(),
            };

        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new BsonSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => InternallyRequiredNamespacePrefixFilters
            .Concat(AdditionalTypesToRegister.Select(_ => _.Type.Namespace).Distinct())
            .ToList();

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson =>
            InternallyRequiredTypes
                .Select(_ => _.ToTypeToRegisterForBson())
                .Concat(AdditionalTypesToRegister)
                .ToList();
    }
}