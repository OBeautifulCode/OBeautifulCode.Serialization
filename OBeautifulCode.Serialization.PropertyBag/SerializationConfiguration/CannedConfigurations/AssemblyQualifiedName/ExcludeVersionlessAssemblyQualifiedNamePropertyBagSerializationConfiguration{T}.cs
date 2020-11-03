// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcludeVersionlessAssemblyQualifiedNamePropertyBagSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A Property Bag serialization configuration that sets <see cref="DependentPropertyBagSerializationConfigurationTypes"/> to typeof(T),
    /// sets <see cref="IncludeVersionlessAssemblyQualifiedNameAsProperty"/> to false,
    /// and sets the remaining public/overrideable properties to the corresponding properties on the dependent serialization configuration.
    /// </summary>
    /// <typeparam name="T">The dependent Property Bag serialization configuration type.</typeparam>
    public sealed class ExcludeVersionlessAssemblyQualifiedNamePropertyBagSerializationConfiguration<T> : PropertyBagSerializationConfigurationBase
        where T : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentPropertyBagSerializationConfigurationTypes.Single()].UnregisteredTypeEncounteredStrategy;

        /// <inheritdoc />
        public override string StringSerializationKeyValueDelimiter => ((PropertyBagSerializationConfigurationBase)this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentPropertyBagSerializationConfigurationTypes.Single()]).StringSerializationKeyValueDelimiter;

        /// <inheritdoc />
        public override string StringSerializationLineDelimiter => ((PropertyBagSerializationConfigurationBase)this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentPropertyBagSerializationConfigurationTypes.Single()]).StringSerializationLineDelimiter;

        /// <inheritdoc />
        public override string StringSerializationNullValueEncoding => ((PropertyBagSerializationConfigurationBase)this.DescendantSerializationConfigurationTypeToInstanceMap[this.DependentPropertyBagSerializationConfigurationTypes.Single()]).StringSerializationNullValueEncoding;

        /// <inheritdoc />
        public override bool IncludeVersionlessAssemblyQualifiedNameAsProperty => false;

        /// <inheritdoc />
        protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new[] { typeof(T).ToPropertyBagSerializationConfigurationType() };
    }
}