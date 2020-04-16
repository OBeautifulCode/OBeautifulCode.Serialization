// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForJson{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    /// <inheritdoc />
    /// <typeparam name="T">The type to register.</typeparam>
    public sealed class TypeToRegisterForJson<T> : TypeToRegisterForJson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForJson{T}"/> class.
        /// </summary>
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="MemberTypesToInclude.All"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="RelatedTypesToInclude.Descendants"/>.</param>
        /// <param name="jsonConverterBuilder">Optional <see cref="JsonConverterBuilder"/>.  DEFAULT is null.</param>
        public TypeToRegisterForJson(
            MemberTypesToInclude memberTypesToInclude = MemberTypesToInclude.All,
            RelatedTypesToInclude relatedTypesToInclude = RelatedTypesToInclude.Descendants,
            JsonConverterBuilder jsonConverterBuilder = null)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, jsonConverterBuilder)
        {
        }
    }
}
