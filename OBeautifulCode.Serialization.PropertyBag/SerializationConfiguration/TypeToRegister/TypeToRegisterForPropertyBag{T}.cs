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
        /// <param name="memberTypesToInclude">Specifies which member types of <typeparamref name="T"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <typeparamref name="T"/> that should also be registered.</param>
        /// <param name="stringSerializerBuilderFunc">A func that builds the <see cref="IStringSerializeAndDeserialize"/>.</param>
        public TypeToRegisterForPropertyBag(
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            Func<IStringSerializeAndDeserialize> stringSerializerBuilderFunc)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, stringSerializerBuilderFunc)
        {
        }
    }
}
