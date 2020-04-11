// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationTypeExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    /// <summary>
    /// Extension methods related to <see cref="PropertyBagSerializationConfigurationType"/>.
    /// </summary>
    public static class PropertyBagSerializationConfigurationTypeExtensions
    {
        /// <summary>
        /// Gets the <see cref="PropertyBagSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </summary>
        /// <param name="propertyBagSerializationConfigurationType">The type of the Property Bag serialization configuration.</param>
        /// <returns>
        /// The <see cref="PropertyBagSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </returns>
        public static PropertyBagSerializationConfigurationType ToPropertyBagSerializationConfigurationType(
            this Type propertyBagSerializationConfigurationType)
        {
            var result = new PropertyBagSerializationConfigurationType(propertyBagSerializationConfigurationType);

            return result;
        }
    }
}