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
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultMemberTypesToInclude"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultRelatedTypesToInclude"/>.</param>
        /// <param name="bsonSerializerBuilder">Optional object that builds an <see cref="IBsonSerializer"/>.  DEFAULT is null (no serializer).</param>
        /// <param name="propertyNameWhitelist">Optional names of the properties to constrain the registration to.  DEFAULT is null (no whitelist).</param>
        public TypeToRegisterForBson(
            MemberTypesToInclude memberTypesToInclude = TypeToRegisterConstants.DefaultMemberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude = TypeToRegisterConstants.DefaultRelatedTypesToInclude,
            BsonSerializerBuilder bsonSerializerBuilder = null,
            IReadOnlyCollection<string> propertyNameWhitelist = null)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, bsonSerializerBuilder, propertyNameWhitelist)
        {
        }
    }
}
