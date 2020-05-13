// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegister{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <inheritdoc />
    /// <typeparam name="T">The type to register.</typeparam>
    public abstract class TypeToRegister<T> : TypeToRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegister{T}"/> class.
        /// </summary>
        /// <param name="memberTypesToInclude">Specifies which member types of <typeparamref name="T"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <typeparamref name="T"/> that should also be registered.</param>
        protected TypeToRegister(
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude)
        {
        }
    }
}
