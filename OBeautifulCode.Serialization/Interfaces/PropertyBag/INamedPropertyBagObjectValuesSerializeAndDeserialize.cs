// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INamedPropertyBagObjectValuesSerializeAndDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Interface to serialize and deserialize an object to a property bag,
    /// keyed on property name with the property values represented in objects.
    /// </summary>
    public interface INamedPropertyBagObjectValuesSerializeAndDeserialize : INamedPropertyBagObjectValuesSerialize, INamedPropertyBagObjectValuesDeserialize
    {
    }
}