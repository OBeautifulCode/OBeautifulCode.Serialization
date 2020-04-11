// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlyBsonSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    /// <summary>
    /// A BSON serialization configuration that populates <see cref="DependentBsonSerializationConfigurationTypes"/> with typeof(T1) and typeof(T2).
    /// </summary>
    /// <typeparam name="T1">The first dependent BSON serialization configuration type.</typeparam>
    /// <typeparam name="T2">The second dependent BSON serialization configuration type.</typeparam>
    public sealed class DependencyOnlyBsonSerializationConfiguration<T1, T2> : BsonSerializationConfigurationBase
        where T1 : BsonSerializationConfigurationBase
        where T2 : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(T1).ToBsonSerializationConfigurationType(), typeof(T2).ToBsonSerializationConfigurationType() };
    }
}