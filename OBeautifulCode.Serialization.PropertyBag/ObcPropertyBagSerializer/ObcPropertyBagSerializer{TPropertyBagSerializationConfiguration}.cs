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
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        public ObcPropertyBagSerializer(
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
            : base(typeof(TPropertyBagSerializationConfiguration).ToPropertyBagSerializationConfigurationType(), unregisteredTypeEncounteredStrategy)
        {
        }
    }
}
