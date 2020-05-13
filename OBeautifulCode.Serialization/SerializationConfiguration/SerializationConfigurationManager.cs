// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationManager.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Manager to create, initialize, and cache implementations of <see cref="SerializationConfigurationBase"/>.
    /// </summary>
    public static class SerializationConfigurationManager
    {
        private static readonly object SyncInstances = new object();

        private static readonly Dictionary<SerializationConfigurationType, SerializationConfigurationBase> Instances =
            new Dictionary<SerializationConfigurationType, SerializationConfigurationBase>();

        /// <summary>
        /// Gets an existing, fully initialized serialization configuration or creates and fully initializes a new one if
        /// one does not already exist for the specified serialization configuration type.
        /// </summary>
        /// <param name="serializationConfigurationType">The serialization configuration type.</param>
        /// <returns>
        /// An initialized instance of the specified serialization configuration type.
        /// </returns>
        public static SerializationConfigurationBase GetOrAddSerializationConfiguration(
            SerializationConfigurationType serializationConfigurationType)
        {
            new { serializationConfigurationType }.AsArg().Must().NotBeNull();

            var result = GetOrAddInstance(serializationConfigurationType);

            return result;
        }

        private static SerializationConfigurationBase GetOrAddInstance(
            SerializationConfigurationType serializationConfigurationType)
        {
            lock (SyncInstances)
            {
                if (!Instances.ContainsKey(serializationConfigurationType))
                {
                    var instance = serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.Construct<SerializationConfigurationBase>();

                    var children = instance.DependentSerializationConfigurationTypesWithDefaultsIfApplicable.Distinct().ToList();

                    var descendentTypeToInstanceMap = new Dictionary<SerializationConfigurationType, SerializationConfigurationBase>();

                    foreach (var child in children)
                    {
                        var childInstance = GetOrAddInstance(child);

                        var childDescendantsTypeToInstanceMap = childInstance.DescendantSerializationConfigurationTypeToInstanceMap;

                        // add the dependent's dependents to the dictionary
                        foreach (var childDescendant in childDescendantsTypeToInstanceMap.Keys)
                        {
                            if (!descendentTypeToInstanceMap.ContainsKey(childDescendant))
                            {
                                var childDescendantInstance = childDescendantsTypeToInstanceMap[childDescendant];

                                descendentTypeToInstanceMap.Add(childDescendant, childDescendantInstance);
                            }
                        }

                        // add the dependent to the dictionary
                        if (!descendentTypeToInstanceMap.ContainsKey(child))
                        {
                            descendentTypeToInstanceMap.Add(child, childInstance);
                        }
                    }

                    // initialize with fully resolve dependency tree
                    instance.Initialize(descendentTypeToInstanceMap);

                    Instances.Add(serializationConfigurationType, instance);
                }

                var result = Instances[serializationConfigurationType];

                return result;
            }
        }
    }
}