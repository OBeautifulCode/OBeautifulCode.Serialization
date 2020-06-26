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
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultMemberTypesToInclude"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultRelatedTypesToInclude"/>.</param>
        /// <param name="jsonConverterBuilder">Optional <see cref="JsonConverterBuilder"/>.  DEFAULT is null.</param>
        /// <param name="keyInDictionaryStringSerializer">OPTIONAL serializer to use when dictionaries are keyed on <typeparamref name="T"/> and the keys should be written-to/read-from a string.</param>
        public TypeToRegisterForJson(
            MemberTypesToInclude memberTypesToInclude = TypeToRegisterConstants.DefaultMemberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude = TypeToRegisterConstants.DefaultRelatedTypesToInclude,
            JsonConverterBuilder jsonConverterBuilder = null,
            IStringSerializeAndDeserialize keyInDictionaryStringSerializer = null)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, jsonConverterBuilder, keyInDictionaryStringSerializer)
        {
        }
    }
}
