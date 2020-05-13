// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete serialization configuration type (derives from <see cref="SerializationConfigurationBase"/>).
    /// </summary>
    public abstract class SerializationConfigurationType : IEquatable<SerializationConfigurationType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationConfigurationType"/> class.
        /// </summary>
        /// <param name="concreteSerializationConfigurationDerivativeType">The type of the concrete <see cref="SerializationConfigurationBase"/> derivative.</param>
        protected SerializationConfigurationType(
            Type concreteSerializationConfigurationDerivativeType)
        {
            new { concreteSerializationConfigurationDerivativeType }.AsArg().Must().NotBeNull();
            new { concreteSerializationConfigurationDerivativeType.IsAbstract }.AsArg().Must().BeFalse();
            concreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(SerializationConfigurationBase)).AsArg(Invariant($"{nameof(concreteSerializationConfigurationDerivativeType)} is assignable to {nameof(SerializationConfigurationBase)}")).Must().BeTrue();
            concreteSerializationConfigurationDerivativeType.HasDefaultConstructor().AsArg(Invariant($"{nameof(concreteSerializationConfigurationDerivativeType)}.{nameof(TypeExtensions.HasDefaultConstructor)}()")).Must().BeTrue();

            this.ConcreteSerializationConfigurationDerivativeType = concreteSerializationConfigurationDerivativeType;
        }

        /// <summary>
        /// Gets the type of the concrete <see cref="SerializationConfigurationBase"/> derivative.
        /// </summary>
        public Type ConcreteSerializationConfigurationDerivativeType { get; }

        /// <summary>
        /// Determines whether two objects of type <see cref="SerializationConfigurationType"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(SerializationConfigurationType left, SerializationConfigurationType right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = left.Equals(right);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="SerializationConfigurationType"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(SerializationConfigurationType left, SerializationConfigurationType right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(SerializationConfigurationType other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.ConcreteSerializationConfigurationDerivativeType == other.ConcreteSerializationConfigurationDerivativeType;

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as SerializationConfigurationType);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.ConcreteSerializationConfigurationDerivativeType).Value;

        /// <inheritdoc />
        public override string ToString()
        {
            var result = this.ConcreteSerializationConfigurationDerivativeType.ToStringReadable();

            return result;
        }
    }
}