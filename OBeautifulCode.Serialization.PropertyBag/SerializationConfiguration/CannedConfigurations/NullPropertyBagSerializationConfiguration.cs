// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullPropertyBagSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using OBeautifulCode.Type;

    /// <summary>
    /// A Property Bag serialization configuration that with no dependent serialization configurations.
    /// This configuration will result in no types registered.
    /// </summary>
    public sealed class NullPropertyBagSerializationConfiguration : PropertyBagSerializationConfigurationBase, IImplementNullObjectPattern
    {
    }
}