// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
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

        /// <summary>
        /// Gets the types that have been registered with a converter.
        /// </summary>
        private HashSet<Type> TypesWithConverters { get; } = new HashSet<Type>();

        /// <summary>
        /// Gets the registered converter set to use.
        /// </summary>
        private IList<JsonConverterBuilder> JsonConverterBuilders { get; } = new List<JsonConverterBuilder>();

        /// <summary>
        /// Gets the types that participate in a hierarchy.
        /// </summary>
        private HashSet<Type> HierarchyParticipatingTypes { get; } = new HashSet<Type>();

        /// <summary>
        /// Gets the types with registered converters that yield a string as the output (this allows for standard use as a key in a dictionary).
        /// </summary>
        private HashSet<Type> TypesWithStringConverters { get; } = new HashSet<Type>();
    }
}