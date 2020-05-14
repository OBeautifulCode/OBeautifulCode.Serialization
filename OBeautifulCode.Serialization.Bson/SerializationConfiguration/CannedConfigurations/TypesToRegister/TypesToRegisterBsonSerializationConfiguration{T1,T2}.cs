// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterBsonSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    /// <summary>
    /// A BSON serialization configuration that adds <typeparamref name="T1"/> and <typeparamref name="T2"/> to <see cref="TypesToRegisterForBson"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <typeparam name="T1">The first type to register.</typeparam>
    /// <typeparam name="T2">The second type to register.</typeparam>
    public sealed class TypesToRegisterBsonSerializationConfiguration<T1, T2> : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[] { typeof(T1).ToTypeToRegisterForBson(), typeof(T2).ToTypeToRegisterForBson() };
    }
}