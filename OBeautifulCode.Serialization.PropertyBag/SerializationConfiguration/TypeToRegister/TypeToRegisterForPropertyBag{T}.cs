// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForPropertyBag{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    /// <inheritdoc />
    /// <typeparam name="T">The type to register.</typeparam>
    public sealed class TypeToRegisterForPropertyBag<T> : TypeToRegisterForPropertyBag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForPropertyBag{T}"/> class.
        /// </summary>
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultMemberTypesToInclude"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultRelatedTypesToInclude"/>.</param>
        /// <param name="stringSerializerBuilderFunc">Optional func that builds the <see cref="IStringSerializeAndDeserialize"/>.  DEFAULT is null (no serializer).</param>
        public TypeToRegisterForPropertyBag(
            MemberTypesToInclude memberTypesToInclude = TypeToRegisterConstants.DefaultMemberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude = TypeToRegisterConstants.DefaultRelatedTypesToInclude,
            Func<IStringSerializeAndDeserialize> stringSerializerBuilderFunc = null)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, stringSerializerBuilderFunc)
        {
        }
    }
}
