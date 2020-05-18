// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullBsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using OBeautifulCode.Type;

    /// <summary>
    /// A BSON serialization configuration that with no dependent serialization configurations.
    /// This configuration will result in no types registered.
    /// </summary>
    public sealed class NullBsonSerializationConfiguration : BsonSerializationConfigurationBase, IImplementNullObjectPattern
    {
        /// <inheritdoc />
        public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Attempt;
    }
}