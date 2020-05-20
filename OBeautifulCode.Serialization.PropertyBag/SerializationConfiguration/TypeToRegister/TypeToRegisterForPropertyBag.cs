// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForPropertyBag.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    using OBeautifulCode.Assertion.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Specifies a type to register for property bag serialization.
    /// </summary>
    public class TypeToRegisterForPropertyBag : TypeToRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForPropertyBag"/> class with a type that is it's own origin.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        /// <param name="stringSerializerBuilderFunc">A func that builds the <see cref="IStringSerializeAndDeserialize"/>.</param>
        public TypeToRegisterForPropertyBag(
            Type type,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            Func<IStringSerializeAndDeserialize> stringSerializerBuilderFunc)
            : this(type, type, type, memberTypesToInclude, relatedTypesToInclude, stringSerializerBuilderFunc)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForPropertyBag"/> class, specifying the origin types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="recursiveOriginType">The type whose recursive processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> resulted in the creation of this <see cref="TypeToRegisterForPropertyBag"/>.</param>
        /// <param name="directOriginType">The type whose processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> directly resulted in the creation of this <see cref="TypeToRegisterForPropertyBag"/>.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        /// <param name="stringSerializerBuilderFunc">A func that builds the <see cref="IStringSerializeAndDeserialize"/>.</param>
        public TypeToRegisterForPropertyBag(
            Type type,
            Type recursiveOriginType,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            Func<IStringSerializeAndDeserialize> stringSerializerBuilderFunc)
            : base(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude)
        {
            new { type }.AsArg().Must().NotBeNull();
            new { recursiveOriginType }.AsArg().Must().NotBeNull();
            new { directOriginType }.AsArg().Must().NotBeNull();

            if (type.IsGenericTypeDefinition)
            {
                throw new NotSupportedException(Invariant($"Open generics are not supported."));
            }

            if (stringSerializerBuilderFunc != null)
            {
                if (memberTypesToInclude != MemberTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(stringSerializerBuilderFunc)} is specified, but {nameof(Serialization.MemberTypesToInclude)} is not {MemberTypesToInclude.None}."));
                }
            }

            this.StringSerializerBuilderFunc = stringSerializerBuilderFunc;
        }

        /// <summary>
        /// Gets a func that builds the <see cref="IStringSerializeAndDeserialize"/>.
        /// </summary>
        public Func<IStringSerializeAndDeserialize> StringSerializerBuilderFunc { get; }

        /// <inheritdoc />
        public override TypeToRegister CreateSpawnedTypeToRegister(
            Type type)
        {
            var result = new TypeToRegisterForPropertyBag(type, this.RecursiveOriginType, this.Type, this.MemberTypesToInclude, this.RelatedTypesToInclude, this.StringSerializerBuilderFunc);

            return result;
        }
    }
}
