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

        private static readonly Dictionary<Type, SerializationConfigurationBase> Instances = new Dictionary<Type, SerializationConfigurationBase>();

        /// <summary>
        /// Registers the class maps for the specified <see cref="SerializationConfigurationBase"/> type.
        /// </summary>
        /// <typeparam name="T">Type of derivative of <see cref="SerializationConfigurationBase"/> to use.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static void Configure<T>()
            where T : SerializationConfigurationBase, new()
        {
            // TODO: is there a race condition here? should we lock while calling configure...
            FetchOrCreateConfigurationInstance(typeof(T));
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
            var result = ConfigureWithReturn<T>(typeof(T));
            return result;
        }

        /// <summary>
        /// Registers the class maps for the specified <see cref="SerializationConfigurationBase"/> type.
        /// </summary>
        /// <param name="type">Type of derivative of <see cref="SerializationConfigurationBase"/> to use.</param>
        /// <typeparam name="TReturn">Type of derivative of <see cref="SerializationConfigurationBase"/> to return as.</typeparam>
        /// <returns>Configured instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static TReturn ConfigureWithReturn<TReturn>(Type type)
            where TReturn : SerializationConfigurationBase
        {
            new { type }.AsArg().Must().NotBeNull();
            var returnType = typeof(TReturn);
            type.IsAssignableTo(returnType).AsArg(Invariant($"typeMustBeSubclassOf{returnType}")).Must().BeTrue();
            type.HasParameterlessConstructor().AsArg("typeHasParameterLessConstructor").Must().BeTrue();

            var result = FetchOrCreateConfigurationInstance(type);

            return (TReturn)result;
        }

        /// <summary>
        /// Registers the class maps for the specified <see cref="SerializationConfigurationBase"/> type.
        /// </summary>
        /// <param name="type">Type of derivative of <see cref="SerializationConfigurationBase"/> to use.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static void Configure(Type type)
        {
            new { type }.AsArg().Must().NotBeNull();
            type.IsSubclassOf(typeof(SerializationConfigurationBase)).AsArg(Invariant($"typeMustBeSubclassOf{nameof(SerializationConfigurationBase)}")).Must().BeTrue();
            type.HasParameterlessConstructor().AsArg("typeHasParameterLessConstructor").Must().BeTrue();

            FetchOrCreateConfigurationInstance(type);
         }

        private static Type GetInheritorOfSerializationBase(this Type configurationType)
        {
            var type = configurationType.BaseType;
            while (type != null && type.BaseType != null && type.BaseType != typeof(SerializationConfigurationBase))
            {
                type = type.BaseType;
            }

            return
                   type != null
                   && type.BaseType != null
                   && type.BaseType == typeof(SerializationConfigurationBase)
                ? type
                : null;
        }

        private static SerializationConfigurationBase FetchOrCreateConfigurationInstance(Type configurationType)
        {
            lock (SyncInstances)
            {
                if (!Instances.ContainsKey(configurationType))
                {
                    var instance = (SerializationConfigurationBase)configurationType.Construct();

                    var allDependentConfigTypes = instance.GetDependentSerializationConfigurationTypesWithInternalIfApplicable().ToList();

                    allDependentConfigTypes = allDependentConfigTypes.Distinct().ToList();

                    var configInheritor = configurationType.GetInheritorOfSerializationBase();

                    // TODO: test this throw.
                    // This protects against a JsonSerializationConfiguration listing dependent types that are BsonSerializationConfiguration derivatives, and vice-versa.
                    var rogueDependents = allDependentConfigTypes.Where(_ => _.GetInheritorOfSerializationBase() != configInheritor).ToList();
                    if (rogueDependents.Any())
                    {
                        throw new InvalidOperationException(Invariant($"Configuration {configurationType} has {nameof(instance.GetDependentSerializationConfigurationTypesWithInternalIfApplicable)} ({string.Join(",", rogueDependents)}) that do not share the same first layer of inheritance {configInheritor}."));
                    }

                    var dependentConfigTypeToConfigMap = new Dictionary<Type, SerializationConfigurationBase>();

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

                    Instances.Add(configurationType, instance);
                }

                var result = Instances[configurationType];

                return result;
            }
        }
    }
}