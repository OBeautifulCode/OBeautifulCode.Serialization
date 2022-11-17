// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlyBsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    /// <summary>
    /// A BSON serialization configuration that populates <see cref="DependentBsonSerializationConfigurationTypes"/> with typeof(<typeparamref name="T"/>).
    /// </summary>
    /// <typeparam name="T">The dependent BSON serialization configuration type.</typeparam>
    public sealed class DependencyOnlyBsonSerializationConfiguration<T> : BsonSerializationConfigurationBase
        where T : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[]
        {
            typeof(T).ToBsonSerializationConfigurationType(),
        };
    }
}