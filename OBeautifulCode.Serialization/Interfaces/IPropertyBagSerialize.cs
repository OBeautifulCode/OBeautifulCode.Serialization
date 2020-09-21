// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyBagSerialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// Interface to serialize to a string.
    /// </summary>
    public interface IPropertyBagSerialize
    {
        /// <summary>
        /// Serializes an object into a string.
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>Serialized object into a string.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        IReadOnlyDictionary<string, string> SerializeToPropertyBag(
            object objectToSerialize);
    }
}
