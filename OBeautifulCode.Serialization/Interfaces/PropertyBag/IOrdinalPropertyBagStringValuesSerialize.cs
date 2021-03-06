﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrdinalPropertyBagStringValuesSerialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// Interface to serialize an object to a property bag,
    /// keyed on the properties' ordinal positions with the property values represented in strings.
    /// </summary>
    public interface IOrdinalPropertyBagStringValuesSerialize
    {
        /// <summary>
        /// Serializes an object into a property bag (<see cref="IReadOnlyDictionary{Int32, String}"/>).
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>
        /// The property bag.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        IReadOnlyDictionary<int, string> SerializeToOrdinalPropertyBagWithStringValues(
            object objectToSerialize);
    }
}
