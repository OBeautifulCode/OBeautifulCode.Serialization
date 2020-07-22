// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete BSON serialization configuration type (derives from <see cref="BsonSerializationConfigurationBase"/>).
    /// </summary>
    public class BsonSerializationConfigurationType : SerializationConfigurationType, IEquatable<BsonSerializationConfigurationType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializationConfigurationType"/> class.
        /// </summary>
        /// <param name="concreteSerializationConfigurationDerivativeType">The type of the concrete <see cref="BsonSerializationConfigurationBase"/> derivative.</param>
        public BsonSerializationConfigurationType(
            Type concreteSerializationConfigurationDerivativeType)
            : base(concreteSerializationConfigurationDerivativeType)
        {
            if (!concreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(BsonSerializationConfigurationBase)))
            {
                throw new ArgumentException(Invariant($"'{nameof(concreteSerializationConfigurationDerivativeType)} is assignable to {nameof(BsonSerializationConfigurationBase)}' is false"));
            }
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="BsonSerializationConfigurationType"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(BsonSerializationConfigurationType left, BsonSerializationConfigurationType right)
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
        /// Determines whether two objects of type <see cref="BsonSerializationConfigurationType"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(BsonSerializationConfigurationType left, BsonSerializationConfigurationType right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(BsonSerializationConfigurationType other)
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
        public override bool Equals(object obj) => this == (obj as BsonSerializationConfigurationType);

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