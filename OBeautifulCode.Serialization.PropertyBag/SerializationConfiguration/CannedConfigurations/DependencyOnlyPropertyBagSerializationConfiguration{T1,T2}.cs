// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlyPropertyBagSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    /// <summary>
    /// A Property Bag serialization configuration that populates <see cref="DependentPropertyBagSerializationConfigurationTypes"/> with typeof(T1) and typeof(T2).
    /// </summary>
    /// <typeparam name="T1">The first dependent Property Bag serialization configuration type.</typeparam>
    /// <typeparam name="T2">The second dependent Property Bag serialization configuration type.</typeparam>
    public sealed class DependencyOnlyPropertyBagSerializationConfiguration<T1, T2> : PropertyBagSerializationConfigurationBase
        where T1 : PropertyBagSerializationConfigurationBase
        where T2 : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new[] { typeof(T1).ToPropertyBagSerializationConfigurationType(), typeof(T2).ToPropertyBagSerializationConfigurationType() };
    }
}