// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnUnregisteredTypePropertyBagSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    /// <summary>
    /// A Property Bag serialization configuration that populates <see cref="DependentPropertyBagSerializationConfigurationTypes"/> with typeof(T),
    /// and sets <see cref="SerializationConfigurationBase.UnregisteredTypeEncounteredStrategy"/> to <see cref="UnregisteredTypeEncounteredStrategy.Throw"/>.
    /// </summary>
    /// <typeparam name="T">The dependent Property Bag serialization configuration type.</typeparam>
    public sealed class ThrowOnUnregisteredTypePropertyBagSerializationConfiguration<T> : PropertyBagSerializationConfigurationBase
        where T : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

        /// <inheritdoc />
        protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new[] { typeof(T).ToPropertyBagSerializationConfigurationType() };
    }
}