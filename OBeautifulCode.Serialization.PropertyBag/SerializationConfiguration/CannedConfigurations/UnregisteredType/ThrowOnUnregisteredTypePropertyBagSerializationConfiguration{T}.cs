// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowOnUnregisteredTypePropertyBagSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A Property Bag serialization configuration that sets <see cref="DependentPropertyBagSerializationConfigurationTypes"/> to typeof(T),
    /// sets <see cref="SerializationConfigurationBase.UnregisteredTypeEncounteredStrategy"/> to <see cref="UnregisteredTypeEncounteredStrategy.Throw"/>,
    /// and sets the remaining public/overrideable properties to the corresponding properties on the dependent serialization configuration.
    /// </summary>
    /// <typeparam name="T">The dependent Property Bag serialization configuration type.</typeparam>
    public sealed class ThrowOnUnregisteredTypePropertyBagSerializationConfiguration<T> : PropertyBagSerializationConfigurationBase
        where T : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

        /// <inheritdoc />
        public override string StringSerializationKeyValueDelimiter => ((PropertyBagSerializationConfigurationBase)this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentPropertyBagSerializationConfigurationTypes.Single()]).StringSerializationKeyValueDelimiter;

        /// <inheritdoc />
        public override string StringSerializationLineDelimiter => ((PropertyBagSerializationConfigurationBase)this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentPropertyBagSerializationConfigurationTypes.Single()]).StringSerializationLineDelimiter;

        /// <inheritdoc />
        public override string StringSerializationNullValueEncoding => ((PropertyBagSerializationConfigurationBase)this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentPropertyBagSerializationConfigurationTypes.Single()]).StringSerializationNullValueEncoding;

        /// <inheritdoc />
        protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new[] { typeof(T).ToPropertyBagSerializationConfigurationType() };
    }
}