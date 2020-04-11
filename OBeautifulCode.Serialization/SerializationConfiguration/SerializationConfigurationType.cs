// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete serialization configuration type (derives from <see cref="SerializationConfigurationBase"/>).
    /// </summary>
    public class SerializationConfigurationType : IEquatableViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationConfigurationType"/> class.
        /// </summary>
        /// <param name="concreteSerializationConfigurationDerivativeType">The type of the concrete <see cref="SerializationConfigurationBase"/> derivative.</param>
        public SerializationConfigurationType(
            Type concreteSerializationConfigurationDerivativeType)
        {
            new { concreteSerializationConfigurationDerivativeType }.AsArg().Must().NotBeNull();
            new { concreteSerializationConfigurationDerivativeType.IsAbstract }.AsArg().Must().BeFalse();
            concreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(SerializationConfigurationBase)).AsArg(Invariant($"{nameof(concreteSerializationConfigurationDerivativeType)} is assignable to {nameof(SerializationConfigurationBase)}")).Must().BeTrue();
            concreteSerializationConfigurationDerivativeType.HasParameterlessConstructor().AsArg(Invariant($"{nameof(concreteSerializationConfigurationDerivativeType)}.{nameof(DomainExtensions.HasParameterlessConstructor)}()")).Must().BeTrue();

            this.ConcreteSerializationConfigurationDerivativeType = concreteSerializationConfigurationDerivativeType;
        }

        /// <summary>
        /// Gets the type of the concrete <see cref="SerializationConfigurationBase"/> derivative.
        /// </summary>
        public Type ConcreteSerializationConfigurationDerivativeType { get; private set; }
    }
}