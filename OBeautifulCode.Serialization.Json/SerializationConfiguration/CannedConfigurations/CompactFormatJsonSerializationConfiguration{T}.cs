// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompactFormatJsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    /// <summary>
    /// A JSON serialization configuration that populates <see cref="DependentJsonSerializationConfigurationTypes"/> with typeof(T),
    /// and sets <see cref="JsonSerializationConfigurationBase.JsonFormattingKind"/> to <see cref="JsonFormattingKind.Compact"/>.
    /// </summary>
    /// <typeparam name="T">The dependent JSON serialization configuration type.</typeparam>
    public sealed class CompactFormatJsonSerializationConfiguration<T> : JsonSerializationConfigurationBase
        where T : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        public override JsonFormattingKind JsonFormattingKind => JsonFormattingKind.Compact;

        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new[] { typeof(T).ToJsonSerializationConfigurationType() };
    }
}