// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization;

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
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[]
        {
            typeof(InternallyRequiredTypesWithDiscoveryJsonSerializationConfiguration).ToJsonSerializationConfigurationType(),
        };

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => this.DependentJsonSerializationConfigurationTypes;

        /// <summary>
        /// Gets the <see cref="JsonSerializationConfigurationBase"/>s that are needed for the current implementation of <see cref="JsonSerializationConfigurationBase"/>.  Optionally overrideable, DEFAULT is empty collection.
        /// </summary>
        protected virtual IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new JsonSerializationConfigurationType[0];

        /// <summary>
        /// Gets the types that have been registered with a converter.
        /// </summary>
        protected HashSet<Type> TypesWithConverters { get; } = new HashSet<Type>();

        /// <summary>
        /// Gets the registered converter set to use.
        /// </summary>
        protected IList<RegisteredJsonConverter> RegisteredConverters { get; } = new List<RegisteredJsonConverter>();

        /// <summary>
        /// Gets the types that participate in a hierarchy.
        /// </summary>
        protected HashSet<Type> HierarchyParticipatingTypes { get; } = new HashSet<Type>();

        /// <summary>
        /// Gets the types with registered converters that yield a string as the output (this allows for standard use as a key in a dictionary).
        /// </summary>
        protected HashSet<Type> TypesWithStringConverters { get; } = new HashSet<Type>();
    }
}