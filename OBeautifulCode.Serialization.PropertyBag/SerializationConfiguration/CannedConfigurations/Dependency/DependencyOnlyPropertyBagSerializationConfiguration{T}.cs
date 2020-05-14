// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlyPropertyBagSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    /// <summary>
    /// A Property Bag serialization configuration that populates <see cref="DependentPropertyBagSerializationConfigurationTypes"/> with typeof(T).
    /// </summary>
    /// <typeparam name="T">The dependent Property Bag serialization configuration type.</typeparam>
    public sealed class DependencyOnlyPropertyBagSerializationConfiguration<T> : PropertyBagSerializationConfigurationBase
        where T : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new[] { typeof(T).ToPropertyBagSerializationConfigurationType() };
    }
}