// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase : SerializationConfigurationBase
    {
        private static readonly IReadOnlyCollection<Type> InheritedTypeConverterBlackList =
            new[]
            {
                typeof(string),
                typeof(object),
            };

        /// <inheritdoc />
        public sealed override IReadOnlyCollection<Type> InternalDependentSerializationConfigurationTypes => new[] { typeof(InternalJsonSerializationConfiguration) };

        /// <summary>
        /// Gets the types that have been registered with a converter.
        /// </summary>
        protected HashSet<Type> TypesWithConverters { get; } = new HashSet<Type>();

        /// <summary>
        /// Gets the registered converter set to use.
        /// </summary>
        protected IList<RegisteredJsonConverter> RegisteredConverters { get; } = new List<RegisteredJsonConverter>();

        /// <summary>
        /// Gets the inherited types to handle.
        /// </summary>
        protected HashSet<Type> InheritedTypesToHandle { get; } = new HashSet<Type>();

        /// <summary>
        /// Gets the types with registered converters that yield a string as the output (this allows for standard use as a key in a dictionary).
        /// </summary>
        protected HashSet<Type> TypesWithStringConverters { get; } = new HashSet<Type>();
    }

    /// <summary>
    /// Internal implementation of <see cref="JsonSerializationConfigurationBase" /> that will auto register necessary internal types.
    /// </summary>
    public sealed class InternalJsonSerializationConfiguration : JsonSerializationConfigurationBase, IDoNotNeedInternalDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => InternallyRequiredTypes;
    }

    /// <summary>
    /// Generic implementation of <see cref="JsonSerializationConfigurationBase" /> that will auto register with discovery using type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">Type to auto register with discovery.</typeparam>
    public sealed class GenericDiscoveryJsonSerializationConfiguration<T> : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => new[] { typeof(T) };
    }

    /// <summary>
    /// Generic implementation of <see cref="JsonSerializationConfigurationBase" /> that will auto register with discovery using type <typeparamref name="T1" />, <typeparamref name="T2" />.
    /// </summary>
    /// <typeparam name="T1">Type one to auto register with discovery.</typeparam>
    /// <typeparam name="T2">Type two to auto register with discovery.</typeparam>
    public sealed class GenericDiscoveryJsonSerializationConfiguration<T1, T2> : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => new[] { typeof(T1), typeof(T2) };
    }

    /// <summary>
    /// Null implementation of <see cref="JsonSerializationConfigurationBase"/>.
    /// </summary>
    public sealed class NullJsonSerializationConfiguration : JsonSerializationConfigurationBase, IImplementNullObjectPattern
    {
    }
}