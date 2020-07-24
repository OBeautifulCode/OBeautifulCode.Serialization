// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForBson.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson.Serialization;

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
        /// <param name="bsonSerializerBuilder">Builds an<see cref="IBsonSerializer"/>.</param>
        /// <param name="propertyNameWhitelist">The names of the properties to constrain the registration to.</param>
        public TypeToRegisterForBson(
            Type type,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            BsonSerializerBuilder bsonSerializerBuilder,
            IReadOnlyCollection<string> propertyNameWhitelist)
            : this(type, type, type, memberTypesToInclude, relatedTypesToInclude, bsonSerializerBuilder, propertyNameWhitelist)
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
        /// <param name="bsonSerializerBuilder">Builds an <see cref="IBsonSerializer"/>.</param>
        /// <param name="propertyNameWhitelist">The names of the properties to constrain the registration to.</param>
        public TypeToRegisterForBson(
            Type type,
            Type recursiveOriginType,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            BsonSerializerBuilder bsonSerializerBuilder,
            IReadOnlyCollection<string> propertyNameWhitelist)
            : base(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (recursiveOriginType == null)
            {
                throw new ArgumentNullException(nameof(recursiveOriginType));
            }

            if (directOriginType == null)
            {
                throw new ArgumentNullException(nameof(directOriginType));
            }

            if ((bsonSerializerBuilder != null) && (propertyNameWhitelist != null))
            {
                throw new ArgumentException(Invariant($"{nameof(bsonSerializerBuilder)} and {nameof(propertyNameWhitelist)} cannot both be specified."));
            }

            if (bsonSerializerBuilder != null)
            {
                if (memberTypesToInclude != MemberTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(bsonSerializerBuilder)} is specified, but {nameof(Serialization.MemberTypesToInclude)} is not {MemberTypesToInclude.None}."));
                }

                if (type.IsGenericTypeDefinition)
                {
                    throw new NotSupportedException(Invariant($"{nameof(bsonSerializerBuilder)} is specified, but underlying type to register is an open generic."));
                }
            }

            if (propertyNameWhitelist != null)
            {
                if (!propertyNameWhitelist.Any())
                {
                    throw new ArgumentException(Invariant($"'{nameof(propertyNameWhitelist)}' is an empty enumerable"));
                }

                if (!propertyNameWhitelist.Any(string.IsNullOrWhiteSpace))
                {
                    throw new ArgumentException(Invariant($"'{nameof(propertyNameWhitelist)}' contains an element that is null or white space"));
                }

                if (memberTypesToInclude != MemberTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(propertyNameWhitelist)} is specified, but {nameof(Serialization.MemberTypesToInclude)} is not {MemberTypesToInclude.None}."));
                }

                if (relatedTypesToInclude != RelatedTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(propertyNameWhitelist)} is specified, but {nameof(Serialization.RelatedTypesToInclude)} is not {RelatedTypesToInclude.None}."));
                }

                if (type.IsGenericTypeDefinition)
                {
                    throw new NotSupportedException(Invariant($"{nameof(propertyNameWhitelist)} is specified, but underlying type to register is an open generic."));
                }
            }

            this.BsonSerializerBuilder = bsonSerializerBuilder;
            this.PropertyNameWhitelist = propertyNameWhitelist;
        }

        /// <summary>
        /// Gets an object that builds an <see cref="IBsonSerializer"/>.
        /// </summary>
        public BsonSerializerBuilder BsonSerializerBuilder { get; }

        /// <summary>
        /// Gets the names of the properties to constrain the registration to.
        /// </summary>
        public IReadOnlyCollection<string> PropertyNameWhitelist { get; }

        /// <inheritdoc />
        public override TypeToRegister CreateSpawnedTypeToRegister(
            Type type,
            TypeToIncludeOrigin typeToIncludeOrigin)
        {
            var result = new TypeToRegisterForBson(type, this.RecursiveOriginType, this.Type, this.MemberTypesToInclude, this.RelatedTypesToInclude, this.BsonSerializerBuilder, this.PropertyNameWhitelist);

            return result;
        }
    }
}
