// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.Virtual.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase
    {
        /// <summary>
        /// Gets the optional override to the contract resolver of the settings gotten from the provided kind for reading.
        /// </summary>
        protected virtual IReadOnlyDictionary<SerializationDirection, RegisteredContractResolver> OverrideContractResolver => null;

        /// <summary>
        /// Gets the optional converters to add.
        /// </summary>
        protected virtual IReadOnlyCollection<RegisteredJsonConverter> ConvertersToRegister => null;
    }
}