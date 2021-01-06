// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForJson{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using NewtonsoftFork.Json;

    /// <inheritdoc />
    /// <typeparam name="T">The type to register.</typeparam>
    public sealed class TypeToRegisterForJson<T> : TypeToRegisterForJson
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForJson{T}"/> class.
        /// </summary>
        /// <param name="memberTypesToInclude">Specifies which member types of <typeparamref name="T"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <typeparamref name="T"/> that should also be registered.</param>
        /// <param name="jsonConverterBuilder">Builds a serializing and deserializing <see cref="JsonConverter"/>.</param>
        /// <param name="keyInDictionaryStringSerializer">The serializer to use when dictionaries are keyed on <typeparamref name="T"/> and the keys should be written-to/read-from a string.</param>
        public TypeToRegisterForJson(
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            JsonConverterBuilder jsonConverterBuilder,
            IStringSerializeAndDeserialize keyInDictionaryStringSerializer)
        : base(typeof(T), memberTypesToInclude, relatedTypesToInclude, jsonConverterBuilder, keyInDictionaryStringSerializer)
        {
        }
    }
}
