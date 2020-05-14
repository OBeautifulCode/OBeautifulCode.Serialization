// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterPropertyBagSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    /// <summary>
    /// A property bag serialization configuration that adds <typeparamref name="T1"/> and <typeparamref name="T2"/> to <see cref="TypesToRegisterForPropertyBag"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <typeparam name="T1">The first type to register.</typeparam>
    /// <typeparam name="T2">The second type to register.</typeparam>
    public sealed class TypesToRegisterPropertyBagSerializationConfiguration<T1, T2> : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new[] { typeof(T1).ToTypeToRegisterForPropertyBag(), typeof(T2).ToTypeToRegisterForPropertyBag() };
    }
}