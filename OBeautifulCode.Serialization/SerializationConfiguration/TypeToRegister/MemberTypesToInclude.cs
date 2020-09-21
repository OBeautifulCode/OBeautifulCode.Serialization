// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberTypesToInclude.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// Specifies which member types to include when registering a type.
    /// The included member types will be registered as well.
    /// </summary>
    [Flags]
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames", Justification = ObcSuppressBecause.CA1714_FlagsEnumsShouldHavePluralNames_TheNameIsPlural)]
    public enum MemberTypesToInclude
    {
        /// <summary>
        /// Do not include any of the member types.
        /// </summary>
        None = 0,

        /// <summary>
        /// Include the types of all declared properties (public and non-public).
        /// </summary>
        DeclaredProperties = 1,

        /// <summary>
        /// Include the types of all declared fields (public and non-public).
        /// </summary>
        DeclaredFields = 2,

        /// <summary>
        /// Include the types of all generic arguments.
        /// </summary>
        GenericArguments = 4,

        /// <summary>
        /// Include the array element type.
        /// </summary>
        ArrayElement = 8,

        /// <summary>
        /// Include all possible member types.
        /// </summary>
        All = DeclaredProperties | DeclaredFields | GenericArguments | ArrayElement,
    }
}
