﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INamedPropertyBagObjectValuesSerialize.cs" company="OBeautifulCode">
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
    /// keyed on property name with the property values represented in objects.
    /// </summary>
    public interface INamedPropertyBagObjectValuesSerialize
    {
        /// <summary>
        /// Serializes an object into a property bag (<see cref="IReadOnlyDictionary{String, Object}"/>).
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>
        /// The property bag.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        IReadOnlyDictionary<string, object> SerializeToNamedPropertyBagWithObjectValues(
            object objectToSerialize);
    }
}
