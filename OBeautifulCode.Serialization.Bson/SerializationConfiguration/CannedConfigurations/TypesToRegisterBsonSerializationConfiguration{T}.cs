// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterBsonSerializationConfiguration{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    /// <summary>
    /// A BSON serialization configuration that adds <typeparamref name="T"/> to <see cref="TypesToRegisterForBson"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <typeparam name="T">The type register.</typeparam>
    public sealed class TypesToRegisterBsonSerializationConfiguration<T> : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[] { typeof(T).ToTypeToRegisterForBson() };
    }
}