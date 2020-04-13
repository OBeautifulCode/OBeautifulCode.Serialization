// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationType{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type.Recipes;

    /// <inheritdoc cref="PropertyBagSerializationConfigurationType" />
    /// <typeparam name="T">The type of concrete <see cref="PropertyBagSerializationConfigurationBase"/> derivative.</typeparam>
    public class PropertyBagSerializationConfigurationType<T> : PropertyBagSerializationConfigurationType, IEquatable<PropertyBagSerializationConfigurationType<T>>
        where T : PropertyBagSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBagSerializationConfigurationType{T}"/> class.
        /// </summary>
        public PropertyBagSerializationConfigurationType()
            : base(typeof(T))
        {
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="PropertyBagSerializationConfigurationType{T}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(PropertyBagSerializationConfigurationType<T> left, PropertyBagSerializationConfigurationType<T> right)
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
        /// Determines whether two objects of type <see cref="PropertyBagSerializationConfigurationType{T}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(PropertyBagSerializationConfigurationType<T> left, PropertyBagSerializationConfigurationType<T> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(PropertyBagSerializationConfigurationType<T> other)
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