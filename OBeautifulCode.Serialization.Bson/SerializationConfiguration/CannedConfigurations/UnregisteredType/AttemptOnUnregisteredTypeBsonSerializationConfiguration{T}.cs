// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttemptOnUnregisteredTypeBsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    using OBeautifulCode.Serialization;

    /// <summary>
    /// A BSON serialization configuration that sets <see cref="DependentBsonSerializationConfigurationTypes"/> to typeof(T),
    /// sets <see cref="SerializationConfigurationBase.UnregisteredTypeEncounteredStrategy"/> to <see cref="UnregisteredTypeEncounteredStrategy.Attempt"/>,
    /// and sets the remaining public/overrideable properties to the corresponding properties on the dependent serialization configuration.
    /// </summary>
    /// <typeparam name="T">The dependent BSON serialization configuration type.</typeparam>
    public sealed class AttemptOnUnregisteredTypeBsonSerializationConfiguration<T> : BsonSerializationConfigurationBase
        where T : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Attempt;

        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(T).ToBsonSerializationConfigurationType() };
    }
}