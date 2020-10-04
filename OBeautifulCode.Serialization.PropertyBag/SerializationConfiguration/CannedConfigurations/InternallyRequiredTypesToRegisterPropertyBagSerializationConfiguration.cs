// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRequiredTypesToRegisterPropertyBagSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A default property bag serialization configuration that adds the internally required types to <see cref="TypesToRegisterForPropertyBag"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <remarks>
    /// This is a default serialization configuration for <see cref="PropertyBagSerializationConfigurationBase"/>.
    /// </remarks>
    public sealed class InternallyRequiredTypesToRegisterPropertyBagSerializationConfiguration : PropertyBagSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new PropertyBagSerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new[] { new TypeToRegisterForPropertyBag(typeof(DateTime), MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new ObcDateTimeStringSerializer()) };
    }
}