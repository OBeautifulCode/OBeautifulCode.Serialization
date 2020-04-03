// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.Override.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected sealed override void InternalConfigure()
        {
            var dependentConfigTypes = new List<Type>(this.DependentConfigurationTypes.Reverse());
            while (dependentConfigTypes.Any())
            {
                var type = dependentConfigTypes.Last();
                dependentConfigTypes.RemoveAt(dependentConfigTypes.Count - 1);

                var dependentConfig = (JsonSerializationConfigurationBase)this.DependentConfigurationTypeToInstanceMap[type];
                dependentConfigTypes.AddRange(dependentConfig.DependentConfigurationTypes);

                this.ProcessConverter(dependentConfig.RegisteredConverters, false);
                this.ProcessInheritedTypeConverterTypes(dependentConfig.RegisteredTypeToDetailsMap.Keys.ToList());
            }

            var converters = (this.ConvertersToRegister ?? new RegisteredJsonConverter[0]).ToList();
            var handledTypes = this.ProcessConverter(converters);
            var registrationDetails = new RegistrationDetails(this.GetType());

            foreach (var handledType in handledTypes)
            {
                this.MutableRegisteredTypeToDetailsMap.Add(handledType, registrationDetails);
            }
        }

        /// <inheritdoc />
        protected sealed override void RegisterTypes(IReadOnlyCollection<Type> types)
        {
            new { types }.AsArg().Must().NotBeNull();

            var discoveredRegistrationDetails = new RegistrationDetails(this.GetType());
            this.MutableRegisteredTypeToDetailsMap.AddRange(types.ToDictionary(k => k, v => discoveredRegistrationDetails));

            this.ProcessInheritedTypeConverterTypes(types);
        }

        private IReadOnlyCollection<Type> ProcessConverter(IList<RegisteredJsonConverter> registeredConverters, bool checkForAlreadyRegistered = true)
        {
            var handledTypes = registeredConverters.SelectMany(_ => _.HandledTypes).ToList();

            if (checkForAlreadyRegistered && this.RegisteredTypeToDetailsMap.Keys.Intersect(handledTypes).Any())
            {
                throw new DuplicateRegistrationException(
                    Invariant($"Trying to register one or more types via {nameof(this.ConvertersToRegister)} processing, but one is already registered."),
                    handledTypes);
            }

            this.RegisteredConverters.AddRange(registeredConverters);
            this.TypesWithConverters.AddRange(handledTypes);
            this.TypesWithStringConverters.AddRange(
                registeredConverters
                    .Where(_ => _.OutputKind == RegisteredJsonConverterOutputKind.String)
                    .SelectMany(_ => _.HandledTypes).Distinct());

            return handledTypes;
        }

        private void ProcessInheritedTypeConverterTypes(IReadOnlyCollection<Type> types)
        {
            var inheritedTypeConverterTypes = types.Where(t =>
                !InheritedTypeConverterBlackList.Contains(t) &&
                (t.IsAbstract || t.IsInterface || types.Any(a => a != t && (t.IsAssignableTo(a) || a.IsAssignableTo(t))))).Distinct().ToList();

            // TODO: what info do we want to capture here? should we give registration details?
            this.InheritedTypesToHandle.AddRange(inheritedTypeConverterTypes.Except(this.TypesWithConverters));
        }
    }
}