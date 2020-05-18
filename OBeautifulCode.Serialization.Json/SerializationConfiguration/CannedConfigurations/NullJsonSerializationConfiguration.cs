// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullJsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using OBeautifulCode.Type;

    /// <summary>
    /// A JSON serialization configuration that with no dependent serialization configurations.
    /// This configuration will result in no types registered.
    /// </summary>
    public sealed class NullJsonSerializationConfiguration : JsonSerializationConfigurationBase, IImplementNullObjectPattern
    {
        /// <inheritdoc />
        public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Attempt;
    }
}