// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlySerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;

    /// <summary>
    /// A serialization configuration that populates <see cref="DependentSerializationConfigurationTypes"/> with typeof(T1) and typeof(T2).
    /// </summary>
    /// <typeparam name="T1">The first dependent serialization configuration type.</typeparam>
    /// <typeparam name="T2">The second dependent serialization configuration type.</typeparam>
    public sealed class DependencyOnlySerializationConfiguration<T1, T2> : SerializationConfigurationBase
        where T1 : SerializationConfigurationBase
        where T2 : SerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new[] { typeof(T1).ToSerializationConfigurationType(), typeof(T2).ToSerializationConfigurationType() };

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new SerializationConfigurationType[0];
    }
}