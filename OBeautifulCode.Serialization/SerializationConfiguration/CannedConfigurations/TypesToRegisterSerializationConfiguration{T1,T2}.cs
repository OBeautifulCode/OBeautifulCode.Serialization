// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterSerializationConfiguration{T1,T2}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A serialization configuration that adds <typeparamref name="T1"/> and <typeparamref name="T2"/> to <see cref="TypesToRegister"/>, using default behavior for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
    /// </summary>
    /// <remarks>
    /// This is useful to have types registered so that you can set <see cref="UnregisteredTypeEncounteredStrategy.Throw"/> when using
    /// a serializer that can accomodate a <see cref="SerializationConfigurationBase"/> (doesn't require a proprietary derivative of that base class).
    /// </remarks>
    /// <typeparam name="T1">The first type to register.</typeparam>
    /// <typeparam name="T2">The second type to register.</typeparam>
    public sealed class TypesToRegisterSerializationConfiguration<T1, T2> : SerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => new SerializationConfigurationType[0];

        /// <inheritdoc />
        protected override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[] { typeof(InternallyRequiredTypesToRegisterSerializationConfiguration).ToSerializationConfigurationType() };

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegister> TypesToRegister => new[] { typeof(T1).ToTypeToRegister(), typeof(T2).ToTypeToRegister() };
    }
}