// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete Property Bag serialization configuration type (derives from <see cref="PropertyBagSerializationConfigurationBase"/>).
    /// </summary>
    public class PropertyBagSerializationConfigurationType : SerializationConfigurationType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBagSerializationConfigurationType"/> class.
        /// </summary>
        /// <param name="concreteSerializationConfigurationDerivativeType">The type of the concrete <see cref="PropertyBagSerializationConfigurationBase"/> derivative.</param>
        public PropertyBagSerializationConfigurationType(
            Type concreteSerializationConfigurationDerivativeType)
            : base(concreteSerializationConfigurationDerivativeType)
        {
            concreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(PropertyBagSerializationConfigurationBase)).AsArg(Invariant($"{nameof(concreteSerializationConfigurationDerivativeType)} is assignable to {nameof(PropertyBagSerializationConfigurationBase)}")).Must().BeTrue();
        }
    }
}