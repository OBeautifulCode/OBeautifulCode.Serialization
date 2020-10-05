// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesToRegisterJsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;

    /// <summary>
    /// A default JSON serialization configuration that adds the internally required types to <see cref="TypesToRegisterForJson"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <remarks>
    /// This is a default serialization configuration for <see cref="JsonSerializationConfigurationBase"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesToRegisterJsonSerializationConfiguration : JsonSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        private static readonly IStringSerializeAndDeserialize SecureStringStringSerializer = new ObcSecureStringStringSerializer();

        private static readonly IStringSerializeAndDeserialize DateTimeStringSerializer = new ObcDateTimeStringSerializer();

        private static readonly IStringSerializeAndDeserialize VersionStringSerializer = new ObcVersionStringSerializer();

        private static readonly IStringSerializeAndDeserialize NullableDateTimeStringSerializer = new ObcNullableDateTimeStringSerializer();

        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new JsonSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => InternallyRequiredNamespacePrefixFilters;

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => InternallyRequiredTypes
            .Select(_ => _.ToTypeToRegisterForJson())
            .Concat(new[]
            {
                typeof(SecureString).ToTypeToRegisterForJsonUsingStringSerializer(SecureStringStringSerializer),
                typeof(DateTime).ToTypeToRegisterForJsonUsingStringSerializer(DateTimeStringSerializer),
                typeof(DateTime?).ToTypeToRegisterForJsonUsingStringSerializer(NullableDateTimeStringSerializer),
                typeof(Version).ToTypeToRegisterForJsonUsingStringSerializer(VersionStringSerializer),
            })
            .ToList();
    }
}