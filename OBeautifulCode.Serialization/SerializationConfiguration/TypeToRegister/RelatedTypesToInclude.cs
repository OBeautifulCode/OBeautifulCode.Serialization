// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelatedTypesToInclude.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Specifies which related types to include when registering a type.
    /// The included related types will be registered as well.
    /// </summary>
    public enum RelatedTypesToInclude
    {
        /// <summary>
        /// Do not include any other types.
        /// </summary>
        None,

        /// <summary>
        /// For interfaces and abstract classes, include all types that are assignable to the type being registered, otherwise do not include any other types.
        /// </summary>
        Default,

        /// <summary>
        /// Include all types that the type being registered is assignable to.
        /// </summary>
        Ancestors,

        /// <summary>
        /// Include all types that are assignable to the type being registered.
        /// </summary>
        Descendants,

        /// <summary>
        /// Include all types that the type being registered is assignable to and all types that are assignable to the type being registered.
        /// </summary>
        AncestorsAndDescendants,
    }
}
