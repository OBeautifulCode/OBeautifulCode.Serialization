// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete Property Bag serialization configuration type (derives from <see cref="PropertyBagSerializationConfigurationBase"/>).
    /// </summary>
    public class PropertyBagSerializationConfigurationType : SerializationConfigurationType, IEquatable<PropertyBagSerializationConfigurationType>
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

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.PropertyBag;

        /// <summary>
        /// Determines whether two objects of type <see cref="PropertyBagSerializationConfigurationType"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(PropertyBagSerializationConfigurationType left, PropertyBagSerializationConfigurationType right)
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
        /// Determines whether two objects of type <see cref="PropertyBagSerializationConfigurationType"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(PropertyBagSerializationConfigurationType left, PropertyBagSerializationConfigurationType right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(PropertyBagSerializationConfigurationType other)
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
        public override bool Equals(object obj) => this == (obj as PropertyBagSerializationConfigurationType);

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