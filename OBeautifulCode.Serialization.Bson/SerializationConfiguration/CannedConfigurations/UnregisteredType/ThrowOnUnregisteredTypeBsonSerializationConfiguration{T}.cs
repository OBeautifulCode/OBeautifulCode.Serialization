// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnUnregisteredTypeBsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    /// <summary>
    /// A BSON serialization configuration that sets <see cref="DependentBsonSerializationConfigurationTypes"/> to typeof(T),
    /// sets <see cref="SerializationConfigurationBase.UnregisteredTypeEncounteredStrategy"/> to <see cref="UnregisteredTypeEncounteredStrategy.Throw"/>,
    /// and sets the remaining public/overrideable properties to the corresponding properties on the dependent serialization configuration.
    /// </summary>
    /// <typeparam name="T">The dependent BSON serialization configuration type.</typeparam>
    public sealed class ThrowOnUnregisteredTypeBsonSerializationConfiguration<T> : BsonSerializationConfigurationBase
        where T : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(T).ToBsonSerializationConfigurationType() };
    }
}