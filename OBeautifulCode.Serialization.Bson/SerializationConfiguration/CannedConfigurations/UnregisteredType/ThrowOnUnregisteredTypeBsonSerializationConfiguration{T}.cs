// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnUnregisteredTypeBsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    /// <summary>
    /// A BSON serialization configuration that populates <see cref="DependentBsonSerializationConfigurationTypes"/> with typeof(T),
    /// and sets <see cref="SerializationConfigurationBase.UnregisteredTypeEncounteredStrategy"/> to <see cref="UnregisteredTypeEncounteredStrategy.Throw"/>.
    /// </summary>
    /// <typeparam name="T">The dependent BSON serialization configuration type.</typeparam>
    public sealed class ThrowOnUnregisteredTypeBsonSerializationConfiguration<T> : BsonSerializationConfigurationBase
        where T : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(T).ToBsonSerializationConfigurationType() };
    }
}