// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationManager.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Serialization.Internal;

    using static System.FormattableString;

    /// <summary>
    /// Manager to create, initialize, and cache implementations of <see cref="SerializationConfigurationBase"/>.
    /// </summary>
    public static class SerializationConfigurationManager
    {
        private static readonly object SyncInstances = new object();

        private static readonly Dictionary<SerializationConfigurationType, SerializationConfigurationBase> Instances =
            new Dictionary<SerializationConfigurationType, SerializationConfigurationBase>();

        private static readonly object SyncBsonSerializationConfigurationTypeInUse = new object();

        private static SerializationConfigurationType bsonSerializationConfigurationTypeInUse;

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
            var result = (T)GetOrAddSerializationConfiguration(typeof(T).ToSerializationConfigurationType());

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
            new { serializationConfigurationType.SerializationKind }.AsArg().Must().NotBeEqualTo(SerializationKind.Invalid);

            if (serializationConfigurationType.SerializationKind == SerializationKind.Bson)
            {
                // see exception message below for why this branch logic exists just for BSON
                if (bsonSerializationConfigurationTypeInUse == null)
                {
                    lock (SyncBsonSerializationConfigurationTypeInUse)
                    {
                        if (bsonSerializationConfigurationTypeInUse == null)
                        {
                            bsonSerializationConfigurationTypeInUse = serializationConfigurationType;
                        }
                    }
                }

                if (serializationConfigurationType != bsonSerializationConfigurationTypeInUse)
                {
                    throw new NotSupportedException(Invariant($"Attempting to instantiate a BSON serializer with {serializationConfigurationType}, but a serializer using {bsonSerializationConfigurationTypeInUse}, a different configuration, has already been instantiated.  This is not supported in BSON; once a configuration has been established by a serializer that's the only configuration that can be used for all operations in the AppDomain.  This is a uniquely BSON constraint; see comments in ObcBsonDiscriminatorConvention for details.  You can use recipes in OBeautifulCode.Reflection.Recipes.AppDomainHelper to perform operations with a different configuration in a new AppDomain.  See AppDomainHelper.ExecuteInNewAppDomain(...)."));
                }
            }

            var result = GetOrAddInstance(serializationConfigurationType);

            result.SetupForSerializationOperations();

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