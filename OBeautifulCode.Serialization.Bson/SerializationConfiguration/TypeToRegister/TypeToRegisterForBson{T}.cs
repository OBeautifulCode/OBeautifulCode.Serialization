// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForBson{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson.Serialization;

    /// <inheritdoc />
    /// <typeparam name="T">The type to register.</typeparam>
    public sealed class TypeToRegisterForBson<T> : TypeToRegisterForBson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForBson{T}"/> class.
        /// </summary>
        /// <param name="memberTypesToInclude">Specifies which member types of <typeparamref name="T"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <typeparamref name="T"/> that should also be registered.</param>
        /// <param name="bsonSerializerBuilder">Builds an<see cref="IBsonSerializer"/>.</param>
        /// <param name="propertyNameWhitelist">The names of the properties to constrain the registration to.</param>
        public TypeToRegisterForBson(
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            BsonSerializerBuilder bsonSerializerBuilder,
            IReadOnlyCollection<string> propertyNameWhitelist)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, bsonSerializerBuilder, propertyNameWhitelist)
        {
        }
    }
}
