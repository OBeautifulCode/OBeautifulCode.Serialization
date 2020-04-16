// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOnlyWithDiscoveryPropertyBagSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    /// <summary>
    /// A Property Bag serialization configuration that will only register, with discovery, typeof(T).
    /// </summary>
    /// <typeparam name="T">The type to auto-register with discovery.</typeparam>
    public sealed class RegisterOnlyWithDiscoveryPropertyBagSerializationConfiguration<T> : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new[] { typeof(T).ToTypeToRegisterForPropertyBag() };
    }
}