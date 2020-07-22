// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A wrapper for a concrete JSON serialization configuration type (derives from <see cref="JsonSerializationConfigurationBase"/>).
    /// </summary>
    public class JsonSerializationConfigurationType : SerializationConfigurationType, IEquatable<JsonSerializationConfigurationType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationConfigurationType"/> class.
        /// </summary>
        /// <param name="concreteSerializationConfigurationDerivativeType">The type of the concrete <see cref="JsonSerializationConfigurationBase"/> derivative.</param>
        public JsonSerializationConfigurationType(
            Type concreteSerializationConfigurationDerivativeType)
            : base(concreteSerializationConfigurationDerivativeType)
        {
            if (!concreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(JsonSerializationConfigurationBase)))
            {
                throw new ArgumentException(Invariant($"'{nameof(concreteSerializationConfigurationDerivativeType)} is assignable to {nameof(JsonSerializationConfigurationBase)}' is false"));
            }
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="JsonSerializationConfigurationType"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(JsonSerializationConfigurationType left, JsonSerializationConfigurationType right)
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
        /// Determines whether two objects of type <see cref="JsonSerializationConfigurationType"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(JsonSerializationConfigurationType left, JsonSerializationConfigurationType right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(JsonSerializationConfigurationType other)
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
        public override bool Equals(object obj) => this == (obj as JsonSerializationConfigurationType);

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