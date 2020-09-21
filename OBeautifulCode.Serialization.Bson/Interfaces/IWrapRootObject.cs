// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWrapRootObject.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// Wraps a root-level object.
    /// </summary>
    public interface IWrapRootObject
    {
        /// <summary>
        /// Gets the untyped root object being wrapped.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Untyped", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        object UntypedRootObject { get; }
    }
}
