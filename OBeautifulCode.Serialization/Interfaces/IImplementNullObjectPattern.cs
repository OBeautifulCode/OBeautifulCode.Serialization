// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImplementNullObjectPattern.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Serialization.Internal;

    /// <summary>
    /// Interface to express usage of the Null Object Pattern (like an empty implementation of the contract it honors).
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = ObcSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IImplementNullObjectPattern
    {
    }
}