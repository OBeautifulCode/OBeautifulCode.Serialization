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

        private static readonly IStringSerializeAndDeserialize NullableDateTimeStringSerializer = new ObcNullableDateTimeStringSerializer();

        private static readonly JsonConverterBuilder SecureStringJsonConverterBuilder = new JsonConverterBuilder(
            "secure-string-converter",
            () => new StringSerializerBackedJsonConverter<SecureString>(SecureStringStringSerializer),
            () => new StringSerializerBackedJsonConverter<SecureString>(SecureStringStringSerializer));

        private static readonly JsonConverterBuilder DateTimeJsonConverterBuilder = new JsonConverterBuilder(
            "date-time-converter",
            () => new StringSerializerBackedJsonConverter<DateTime>(DateTimeStringSerializer),
            () => new StringSerializerBackedJsonConverter<DateTime>(DateTimeStringSerializer));

        private static readonly JsonConverterBuilder NullableDateTimeJsonConverterBuilder = new JsonConverterBuilder(
            "nullable-date-time-converter",
            () => new StringSerializerBackedJsonConverter<DateTime?>(NullableDateTimeStringSerializer),
            () => new StringSerializerBackedJsonConverter<DateTime?>(NullableDateTimeStringSerializer));

        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new JsonSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => InternallyRequiredTypes
            .Select(_ => _.ToTypeToRegisterForJson())
            .Concat(new[]
            {
                typeof(SecureString).ToTypeToRegisterForJson(MemberTypesToInclude.None, RelatedTypesToInclude.None, SecureStringJsonConverterBuilder, SecureStringStringSerializer),
                typeof(DateTime).ToTypeToRegisterForJson(MemberTypesToInclude.None, RelatedTypesToInclude.None, DateTimeJsonConverterBuilder, DateTimeStringSerializer),
                typeof(DateTime?).ToTypeToRegisterForJson(MemberTypesToInclude.None, RelatedTypesToInclude.None, NullableDateTimeJsonConverterBuilder, NullableDateTimeStringSerializer),
            })
            .ToList();

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => this.TypesToRegisterForJson.Select(_ => _.Type.Namespace).Distinct().ToList();
    }
}