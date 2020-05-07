// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterConstants.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Constants related to <see cref="TypeToRegister"/>.
    /// </summary>
    public static class TypeToRegisterConstants
    {
        /// <summary>
        /// The default (when not specified), <see cref="MemberTypesToInclude"/>.  Include all member types.
        /// </summary>
        public const MemberTypesToInclude DefaultMemberTypesToInclude = MemberTypesToInclude.All;

        /// <summary>
        /// The default (when not specified), <see cref="RelatedTypesToInclude"/>.  Include all ancestors and descendants.
        /// </summary>
        public const RelatedTypesToInclude DefaultRelatedTypesToInclude = RelatedTypesToInclude.AncestorsAndDescendants;
    }
}
