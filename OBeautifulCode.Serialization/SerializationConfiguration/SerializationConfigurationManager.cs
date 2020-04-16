// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationManager.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Serialization.Internal;

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
        /// <typeparam name="T">The type of a derivative of <see cref="SerializationConfigurationBase"/> to use.</typeparam>
        /// <returns>
        /// An initialized instance of the specified serialization configuration type.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = ObcSuppressBecause.CA1004_GenericMethodsShouldProvideTypeParameter_OnlyInputsToMethodAreTypesAndItsMoreConciseToCallMethodUseGenericTypeParameters)]
        public static T GetOrAddSerializationConfiguration<T>()
            where T : SerializationConfigurationBase, new()
        {
            var result = (T)GetOrAddInstance(typeof(T).ToSerializationConfigurationType());

            return result;
        }

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

                        var grandchildTypeToInstanceMap = childInstance.DependentSerializationConfigurationTypeToInstanceMap;

                        // add the dependent's dependents to the dictionary
                        foreach (var grandchild in grandchildTypeToInstanceMap.Keys)
                        {
                            if (!descendentTypeToInstanceMap.ContainsKey(grandchild))
                            {
                                var grandchildInstance = grandchildTypeToInstanceMap[grandchild];

                                descendentTypeToInstanceMap.Add(grandchild, grandchildInstance);
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