// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlySerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;

    /// <summary>
    /// A serialization configuration that populates <see cref="DependentSerializationConfigurationTypes"/> with typeof(T).
    /// </summary>
    /// <typeparam name="T">The dependent serialization configuration type.</typeparam>
    public sealed class DependencyOnlySerializationConfiguration<T> : SerializationConfigurationBase
        where T : SerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new[] { typeof(T).ToSerializationConfigurationType() };

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new SerializationConfigurationType[0];
    }
}