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
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="MemberTypesToInclude.All"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="RelatedTypesToInclude.Descendants"/>.</param>
        /// <param name="serializerBuilderFunc">Optional func that builds the <see cref="IBsonSerializer"/>.  DEFAULT is null (no serializer).</param>
        /// <param name="propertyNameWhitelist">Optional names of the properties to constrain the registration to.  DEFAULT is null (no whitelist).</param>
        public TypeToRegisterForBson(
            MemberTypesToInclude memberTypesToInclude = MemberTypesToInclude.All,
            RelatedTypesToInclude relatedTypesToInclude = RelatedTypesToInclude.Descendants,
            Func<IBsonSerializer> serializerBuilderFunc = null,
            IReadOnlyCollection<string> propertyNameWhitelist = null)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, serializerBuilderFunc, propertyNameWhitelist)
        {
        }
    }
}
