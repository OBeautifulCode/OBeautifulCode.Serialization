// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionlessOpenTypeConsolidatingTypeEqualityComparer.cs" company="OBeautifulCode">
//     Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Compares two objects of type <see cref="Type"/> for equality, ignoring assembly version
    /// and treating two open types that are not equal but effectively the same type as equal.
    /// </summary>
    /// <remarks>
    /// an example of the need for this comparer:
    /// class Parent{T} { }
    /// class Child{T} : Parent{T} { }
    /// typeof(Child{T}).BaseType != typeof(Parent{T})
    /// VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance.Equals(typeof(Child{T}).BaseType, typeof(Parent{T})) == true.
    /// </remarks>
    public class VersionlessOpenTypeConsolidatingTypeEqualityComparer : IEqualityComparer<Type>
    {
        /// <summary>
        /// An instance.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = ObcSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly VersionlessOpenTypeConsolidatingTypeEqualityComparer Instance = new VersionlessOpenTypeConsolidatingTypeEqualityComparer();

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotSupportedExceptionForUnreachableCodePath)]
        public bool Equals(
            Type x,
            Type y)
        {
            if (x == null)
            {
                throw new NotSupportedException("null types not support.  x is null");
            }

            if (y == null)
            {
                throw new NotSupportedException("null types not support.  y is null");
            }

            bool result;

            if (x.IsGenericParameter || y.IsGenericParameter)
            {
                result = x.IsGenericParameter && y.IsGenericParameter;
            }
            else
            {
                result =
                    (x.GetFullyNestedName() == y.GetFullyNestedName()) &&
                    (x.Namespace == y.Namespace) &&
                    (x.Assembly.GetName().Name == y.Assembly.GetName().Name) &&
                    x.GetGenericArguments().IsSequenceEqualTo(y.GetGenericArguments(), VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance);
            }

            return result;
        }

        /// <inheritdoc />
        public int GetHashCode(
            Type obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var result = HashCodeHelper
                .Initialize()
                .Hash(obj.GetFullyNestedName())
                .Hash(obj.Namespace)
                .Hash(obj.Assembly.GetName().Name)
                .Value;

            return result;
        }
    }
}