// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.Virtual.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    using OBeautifulCode.Serialization;

    public abstract partial class JsonSerializationConfigurationBase
    {
        /// <summary>
        /// Gets the kind of formatting to use.
        /// </summary>
        public virtual JsonFormattingKind JsonFormattingKind => JsonFormattingKind.Default;

        /// <summary>
        /// Gets the <see cref="JsonSerializationConfigurationBase"/>s that are needed for the current implementation of <see cref="JsonSerializationConfigurationBase"/>.  Optionally overrideable, DEFAULT is empty collection.
        /// </summary>
        protected virtual IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new JsonSerializationConfigurationType[0];

        /// <summary>
        /// Gets the types to register for JSON serialization.
        /// </summary>
        protected virtual IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson { get; } = new TypeToRegisterForJson[0];

        /// <summary>
        /// Gets the optional override to the contract resolver of the settings gotten from the provided kind for reading.
        /// </summary>
        protected virtual IReadOnlyDictionary<SerializationDirection, RegisteredContractResolver> OverrideContractResolver => null;
    }
}