// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToIncludeOrigin.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Identifies the source of a type that is included when registering some other type.
    /// The included type is registered as well.
    /// </summary>
    public enum TypeToIncludeOrigin
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// The type to include was introduced when processing <see cref="TypeToRegister.MemberTypesToInclude"/>.
        /// (<see cref="MemberTypesToInclude"/>).
        /// </summary>
        GettingMemberTypes,

        /// <summary>
        /// The type to include was introduced when processing <see cref="TypeToRegister.RelatedTypesToInclude"/>.
        /// </summary>
        GettingRelatedTypes,
    }
}
