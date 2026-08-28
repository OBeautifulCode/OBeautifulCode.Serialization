// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegister.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Specifies a type to register.
    /// </summary>
    public abstract class TypeToRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegister"/> class with a type that is it's own origin.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        protected TypeToRegister(
            Type type,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude)
            : this(type, new[] { type }, type, memberTypesToInclude, relatedTypesToInclude)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegister"/> class, specifying the origin types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="recursiveOriginTypes">The type whose recursive processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> resulted in the creation of this <see cref="TypeToRegister"/>.</param>
        /// <param name="directOriginType">The type whose processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> directly resulted in the creation of this <see cref="TypeToRegister"/>.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        protected TypeToRegister(
            Type type,
            IReadOnlyList<Type> recursiveOriginTypes,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (recursiveOriginTypes == null)
            {
                throw new ArgumentNullException(nameof(recursiveOriginTypes));
            }

            if (recursiveOriginTypes.Count == 0)
            {
                throw new ArgumentException(Invariant($"{nameof(recursiveOriginTypes)} is empty"));
            }

            if (recursiveOriginTypes.Any(_ => _ == null))
            {
                throw new ArgumentException(Invariant($"{nameof(recursiveOriginTypes)} contains a null element"));
            }

            if (directOriginType == null)
            {
                throw new ArgumentNullException(nameof(directOriginType));
            }

            this.Type = type;
            this.RecursiveOriginTypes = recursiveOriginTypes;
            this.DirectOriginType = directOriginType;
            this.MemberTypesToInclude = memberTypesToInclude;
            this.RelatedTypesToInclude = relatedTypesToInclude;
        }

        /// <summary>
        /// Gets the type to register.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Type is the best name for this property.")]
        public Type Type { get; }

        /// <summary>
        /// Gets the types whose recursive processing of <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/> resulted in the creation of this <see cref="TypeToRegister"/>.
        /// </summary>
        public IReadOnlyList<Type> RecursiveOriginTypes { get; }

        /// <summary>
        /// Gets the type whose processing of <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/> directly resulted in the creation of this <see cref="TypeToRegister"/>.
        /// </summary>
        public Type DirectOriginType { get; }

        /// <summary>
        /// Gets a value that specifies which member types of <see cref="Type"/> that should also be registered.
        /// </summary>
        public MemberTypesToInclude MemberTypesToInclude { get; }

        /// <summary>
        /// Gets a value that specifies which types related to <see cref="Type"/> that should also be registered.
        /// </summary>
        public RelatedTypesToInclude RelatedTypesToInclude { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TypeToRegister"/> is it's own origin (was not spawned from some other <see cref="TypeToRegister"/>.
        /// </summary>
        public bool IsOriginatingType => (this.Type == this.RecursiveOriginTypes.First()) && (this.Type == this.DirectOriginType);

        /// <summary>
        /// Creates a <see cref="TypeToRegister"/> that is spawned in processing the <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/> of this instance.
        /// </summary>
        /// <param name="type">The spawned type.</param>
        /// <param name="typeToIncludeOrigin">The <see cref="TypeToIncludeOrigin"/>.</param>
        /// <returns>
        /// The spawned <see cref="TypeToRegister"/>.
        /// </returns>
        public abstract TypeToRegister CreateSpawnedTypeToRegister(
            Type type,
            TypeToIncludeOrigin typeToIncludeOrigin);

        /// <inheritdoc />
        public override string ToString()
        {
            var result = this.Type.ToStringReadable();

            return result;
        }
    }
}
