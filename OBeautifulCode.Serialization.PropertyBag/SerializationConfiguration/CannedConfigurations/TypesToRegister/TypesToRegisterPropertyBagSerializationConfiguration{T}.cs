// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterPropertyBagSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    /// <summary>
    /// A property bag serialization configuration that adds <typeparamref name="T"/> to <see cref="TypesToRegisterForPropertyBag"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <typeparam name="T">The type to register.</typeparam>
    public sealed class TypesToRegisterPropertyBagSerializationConfiguration<T> : PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new[] { typeof(T).ToTypeToRegisterForPropertyBag() };

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => new[] { typeof(T).Namespace };
    }
}