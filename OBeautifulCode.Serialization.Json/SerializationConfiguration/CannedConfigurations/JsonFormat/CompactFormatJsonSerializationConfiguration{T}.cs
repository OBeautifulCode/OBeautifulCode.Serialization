// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompactFormatJsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A JSON serialization configuration that sets <see cref="DependentJsonSerializationConfigurationTypes"/> to typeof(T),
    /// sets <see cref="JsonSerializationConfigurationBase.JsonFormattingKind"/> to <see cref="JsonFormattingKind.Compact"/>,
    /// and sets the remaining public/overrideable properties to the corresponding properties on the dependent serialization configuration.
    /// </summary>
    /// <typeparam name="T">The dependent JSON serialization configuration type.</typeparam>
    public sealed class CompactFormatJsonSerializationConfiguration<T> : JsonSerializationConfigurationBase
        where T : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentJsonSerializationConfigurationTypes.Single()].UnregisteredTypeEncounteredStrategy;

        /// <inheritdoc />
        public override JsonFormattingKind JsonFormattingKind => JsonFormattingKind.Compact;

        /// <inheritdoc />
        public override IReadOnlyDictionary<SerializationDirection, RegisteredContractResolver> OverrideContractResolver => ((JsonSerializationConfigurationBase)this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentJsonSerializationConfigurationTypes.Single()]).OverrideContractResolver;

        /// <inheritdoc />
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new[] { typeof(T).ToJsonSerializationConfigurationType() };
    }
}