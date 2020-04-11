// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete BSON serialization configuration type (derives from <see cref="BsonSerializationConfigurationBase"/>).
    /// </summary>
    public class BsonSerializationConfigurationType : SerializationConfigurationType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializationConfigurationType"/> class.
        /// </summary>
        /// <param name="concreteSerializationConfigurationDerivativeType">The type of the concrete <see cref="BsonSerializationConfigurationBase"/> derivative.</param>
        public BsonSerializationConfigurationType(
            Type concreteSerializationConfigurationDerivativeType)
            : base(concreteSerializationConfigurationDerivativeType)
        {
            concreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(BsonSerializationConfigurationBase)).AsArg(Invariant($"{nameof(concreteSerializationConfigurationDerivativeType)} is assignable to {nameof(BsonSerializationConfigurationBase)}")).Must().BeTrue();
        }
    }
}