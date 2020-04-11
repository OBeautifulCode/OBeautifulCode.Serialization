// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationManager.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Manager class to govern configuration of <see cref="SerializationConfigurationBase"/> implementations.
    /// </summary>
    public static class SerializationConfigurationManager
    {
        private static readonly object SyncInstances = new object();

        private static readonly Dictionary<SerializationConfigurationType, SerializationConfigurationBase> Instances = new Dictionary<SerializationConfigurationType, SerializationConfigurationBase>();

        /// <summary>
        /// Registers the class maps for the specified <see cref="SerializationConfigurationBase"/> type.
        /// </summary>
        /// <typeparam name="T">Type of derivative of <see cref="SerializationConfigurationBase"/> to use.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static void Configure<T>()
            where T : SerializationConfigurationBase, new()
        {
            // TODO: is there a race condition here? should we lock while calling configure...
            FetchOrCreateConfigurationInstance(typeof(T).ToSerializationConfigurationType());
        }

        /// <summary>
        /// Registers the class maps for the specified <see cref="SerializationConfigurationBase"/> type.
        /// </summary>
        /// <typeparam name="T">Type of derivative of <see cref="SerializationConfigurationBase"/> to configured and return as.</typeparam>
        /// <returns>Configured instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static T ConfigureWithReturn<T>()
            where T : SerializationConfigurationBase
        {
            var result = ConfigureWithReturn<T>(typeof(T).ToSerializationConfigurationType());

            return result;
        }

        /// <summary>
        /// Registers the class maps for the specified <see cref="SerializationConfigurationBase"/> type.
        /// </summary>
        /// <param name="serializationConfigurationType">Type of derivative of <see cref="SerializationConfigurationBase"/> to use.</param>
        /// <typeparam name="TReturn">Type of derivative of <see cref="SerializationConfigurationBase"/> to return as.</typeparam>
        /// <returns>Configured instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static TReturn ConfigureWithReturn<TReturn>(SerializationConfigurationType serializationConfigurationType)
            where TReturn : SerializationConfigurationBase
        {
            new { serializationConfigurationType }.AsArg().Must().NotBeNull();

            var returnType = typeof(TReturn);

            serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.IsAssignableTo(returnType).AsArg(Invariant($"typeMustBeSubclassOf{returnType}")).Must().BeTrue();
            serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.HasParameterlessConstructor().AsArg("typeHasParameterLessConstructor").Must().BeTrue();

            var result = FetchOrCreateConfigurationInstance(serializationConfigurationType);

            return (TReturn)result;
        }

        /// <summary>
        /// Registers the class maps for the specified <see cref="SerializationConfigurationBase"/> type.
        /// </summary>
        /// <param name="serializationConfigurationType">Type of derivative of <see cref="SerializationConfigurationBase"/> to use.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static void Configure(
            SerializationConfigurationType serializationConfigurationType)
        {
            new { serializationConfigurationType }.AsArg().Must().NotBeNull();

            FetchOrCreateConfigurationInstance(serializationConfigurationType);
         }

        private static SerializationConfigurationType GetInheritorOfSerializationBase(
            this SerializationConfigurationType serializationConfigurationType)
        {
            var type = serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.BaseType;

            while ((type != null) && (type.BaseType != null) && (type.BaseType != typeof(SerializationConfigurationBase)))
            {
                type = type.BaseType;
            }

            var result = (type != null) && (type.BaseType != null) && (type.BaseType == typeof(SerializationConfigurationBase))
                ? type.ToSerializationConfigurationType()
                : null;

            return result;
        }

        private static SerializationConfigurationBase FetchOrCreateConfigurationInstance(
            SerializationConfigurationType serializationConfigurationType)
        {
            lock (SyncInstances)
            {
                if (!Instances.ContainsKey(serializationConfigurationType))
                {
                    var instance = (SerializationConfigurationBase)serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.Construct();

                    var allDependentConfigTypes = instance.GetDependentSerializationConfigurationTypesWithInternalIfApplicable().ToList();

                    allDependentConfigTypes = allDependentConfigTypes.Distinct().ToList();

                    var configInheritor = serializationConfigurationType.GetInheritorOfSerializationBase();

                    // TODO: test this throw.
                    // This protects against a JsonSerializationConfiguration listing dependent types that are BsonSerializationConfiguration derivatives, and vice-versa.
                    var rogueDependents = allDependentConfigTypes.Where(_ => _.GetInheritorOfSerializationBase() != configInheritor).ToList();
                    if (rogueDependents.Any())
                    {
                        throw new InvalidOperationException(Invariant($"Configuration {serializationConfigurationType} has {nameof(instance.GetDependentSerializationConfigurationTypesWithInternalIfApplicable)} ({string.Join(",", rogueDependents)}) that do not share the same first layer of inheritance {configInheritor}."));
                    }

                    var dependentConfigTypeToConfigMap = new Dictionary<SerializationConfigurationType, SerializationConfigurationBase>();

                    foreach (var dependentConfigType in allDependentConfigTypes)
                    {
                        var dependentConfigInstance = FetchOrCreateConfigurationInstance(dependentConfigType);

                        var dependentConfigDependentSerializationConfigurationTypeToInstanceMap = dependentConfigInstance.DependentSerializationConfigurationTypeToInstanceMap;

                        foreach (var dependentConfigDependentConfigType in dependentConfigDependentSerializationConfigurationTypeToInstanceMap.Keys)
                        {
                            if (!dependentConfigTypeToConfigMap.ContainsKey(dependentConfigDependentConfigType))
                            {
                                dependentConfigTypeToConfigMap.Add(dependentConfigDependentConfigType, dependentConfigDependentSerializationConfigurationTypeToInstanceMap[dependentConfigDependentConfigType]);
                            }
                        }

                        if (!dependentConfigTypeToConfigMap.ContainsKey(dependentConfigType))
                        {
                            dependentConfigTypeToConfigMap.Add(dependentConfigType, dependentConfigInstance);
                        }
                    }

                    instance.Configure(dependentConfigTypeToConfigMap);

                    Instances.Add(serializationConfigurationType, instance);
                }

                var result = Instances[serializationConfigurationType];

                return result;
            }
        }
    }
}