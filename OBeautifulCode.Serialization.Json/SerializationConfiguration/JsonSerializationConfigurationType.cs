// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete JSON serialization configuration type (derives from <see cref="JsonSerializationConfigurationBase"/>).
    /// </summary>
    public class JsonSerializationConfigurationType : SerializationConfigurationType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationConfigurationType"/> class.
        /// </summary>
        /// <param name="concreteSerializationConfigurationDerivativeType">The type of the concrete <see cref="JsonSerializationConfigurationBase"/> derivative.</param>
        public JsonSerializationConfigurationType(
            Type concreteSerializationConfigurationDerivativeType)
            : base(concreteSerializationConfigurationDerivativeType)
        {
            concreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(JsonSerializationConfigurationBase)).AsArg(Invariant($"{nameof(concreteSerializationConfigurationDerivativeType)} is assignable to {nameof(JsonSerializationConfigurationBase)}")).Must().BeTrue();
        }
    }
}