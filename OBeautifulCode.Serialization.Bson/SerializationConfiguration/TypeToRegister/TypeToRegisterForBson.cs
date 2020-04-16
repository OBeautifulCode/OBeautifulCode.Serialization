// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForBson.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson.Serialization;
    using OBeautifulCode.Assertion.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Specifies a type to register for BSON serialization.
    /// </summary>
    public class TypeToRegisterForBson : TypeToRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForBson"/> class with a type that is it's own origin.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        /// <param name="serializerBuilderFunc">A func that builds the <see cref="IBsonSerializer"/>.</param>
        /// <param name="propertyNameWhitelist">The names of the properties to constrain the registration to.</param>
        public TypeToRegisterForBson(
            Type type,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            Func<IBsonSerializer> serializerBuilderFunc,
            IReadOnlyCollection<string> propertyNameWhitelist)
            : this(type, type, type, memberTypesToInclude, relatedTypesToInclude, serializerBuilderFunc, propertyNameWhitelist)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForBson"/> class, specifying the origin types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="recursiveOriginType">The type whose recursive processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> resulted in the creation of this <see cref="TypeToRegisterForBson"/>.</param>
        /// <param name="directOriginType">The type whose processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> directly resulted in the creation of this <see cref="TypeToRegisterForBson"/>.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        /// <param name="serializerBuilderFunc">A func that builds the <see cref="IBsonSerializer"/>.</param>
        /// <param name="propertyNameWhitelist">The names of the properties to constrain the registration to.</param>
        public TypeToRegisterForBson(
            Type type,
            Type recursiveOriginType,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            Func<IBsonSerializer> serializerBuilderFunc,
            IReadOnlyCollection<string> propertyNameWhitelist)
            : base(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude)
        {
            if ((serializerBuilderFunc != null) && (propertyNameWhitelist != null))
            {
                throw new ArgumentException(Invariant($"{nameof(serializerBuilderFunc)} and {nameof(propertyNameWhitelist)} cannot both be specified."));
            }

            if (serializerBuilderFunc != null)
            {
                if (memberTypesToInclude != MemberTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(serializerBuilderFunc)} is specified, but {nameof(Serialization.MemberTypesToInclude)} is not {MemberTypesToInclude.None}."));
                }
            }

            if (propertyNameWhitelist != null)
            {
                new { propertyNameWhitelist }.AsArg().Must().NotBeEmptyEnumerable().And().Each().NotBeNullNorWhiteSpace();

                if (memberTypesToInclude != MemberTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(propertyNameWhitelist)} is specified, but {nameof(Serialization.MemberTypesToInclude)} is not {MemberTypesToInclude.None}."));
                }

                if (relatedTypesToInclude != RelatedTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(propertyNameWhitelist)} is specified, but {nameof(Serialization.RelatedTypesToInclude)} is not {RelatedTypesToInclude.None}."));
                }
            }

            this.SerializerBuilderFunc = serializerBuilderFunc;
            this.PropertyNameWhitelist = propertyNameWhitelist;
        }

        /// <summary>
        /// Gets a func that builds the <see cref="IBsonSerializer"/>.
        /// </summary>
        public Func<IBsonSerializer> SerializerBuilderFunc { get; }

        /// <summary>
        /// Gets the names of the properties to constrain the registration to.
        /// </summary>
        public IReadOnlyCollection<string> PropertyNameWhitelist { get; }

        /// <inheritdoc />
        public override TypeToRegister CreateSpawnedTypeToRegister(
            Type type)
        {
            var result = new TypeToRegisterForBson(type, this.RecursiveOriginType, this.Type, this.MemberTypesToInclude, this.RelatedTypesToInclude, this.SerializerBuilderFunc, this.PropertyNameWhitelist);

            return result;
        }
    }
}
