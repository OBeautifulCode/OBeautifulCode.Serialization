// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationType{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    /// <inheritdoc />
    /// <typeparam name="T">The type of concrete <see cref="PropertyBagSerializationConfigurationBase"/> derivative.</typeparam>
    public class PropertyBagSerializationConfigurationType<T> : PropertyBagSerializationConfigurationType
        where T : PropertyBagSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBagSerializationConfigurationType{T}"/> class.
        /// </summary>
        public PropertyBagSerializationConfigurationType()
            : base(typeof(T))
        {
        }
    }
}