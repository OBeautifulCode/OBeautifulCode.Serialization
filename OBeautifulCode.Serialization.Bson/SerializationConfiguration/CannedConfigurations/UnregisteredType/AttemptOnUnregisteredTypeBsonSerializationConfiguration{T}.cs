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
    /// A BSON serialization configuration that populates <see cref="DependentBsonSerializationConfigurationTypes"/> with typeof(T),
    /// and sets <see cref="SerializationConfigurationBase.UnregisteredTypeEncounteredStrategy"/> to <see cref="UnregisteredTypeEncounteredStrategy.Attempt"/>.
    /// </summary>
    /// <typeparam name="T">The dependent BSON serialization configuration type.</typeparam>
    public sealed class AttemptOnUnregisteredTypeBsonSerializationConfiguration<T> : BsonSerializationConfigurationBase
        where T : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Attempt;

        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(T).ToBsonSerializationConfigurationType() };
    }
}