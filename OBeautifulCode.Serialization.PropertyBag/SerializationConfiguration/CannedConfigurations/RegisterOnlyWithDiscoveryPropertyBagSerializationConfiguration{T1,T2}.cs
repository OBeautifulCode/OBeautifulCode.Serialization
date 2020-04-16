// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterOnlyWithDiscoveryPropertyBagSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    /// <summary>
    /// A Property Bag serialization configuration that will only register, with discovery, typeof(T).
    /// </summary>
    /// <typeparam name="T1">The first type to auto-register with discovery.</typeparam>
    /// <typeparam name="T2">The second type to auto-register with discovery.</typeparam>
    public sealed class RegisterOnlyWithDiscoveryPropertyBagSerializationConfiguration<T1, T2> : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new[] { typeof(T1).ToTypeToRegisterForPropertyBag(), typeof(T2).ToTypeToRegisterForPropertyBag() };
    }
}