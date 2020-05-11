// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer{TPropertyBagSerializationConfiguration}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    /// <inheritdoc />
    public sealed class ObcPropertyBagSerializer<TPropertyBagSerializationConfiguration> : ObcPropertyBagSerializer
        where TPropertyBagSerializationConfiguration : PropertyBagSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcPropertyBagSerializer{TPropertyBagConfiguration}"/> class.
        /// </summary>
        public ObcPropertyBagSerializer()
            : base(typeof(TPropertyBagSerializationConfiguration).ToPropertyBagSerializationConfigurationType())
        {
        }
    }
}
