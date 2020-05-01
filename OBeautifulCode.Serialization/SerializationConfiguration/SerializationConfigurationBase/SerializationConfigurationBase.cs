// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Common configuration base across all kinds of serialization.
    /// </summary>
    public abstract partial class SerializationConfigurationBase
    {
        private readonly object syncConfigure = new object();

        private readonly Dictionary<Type, RegistrationDetails> registeredTypeToRegistrationDetailsMap = new Dictionary<Type, RegistrationDetails>();

        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationConfigurationBase"/> class.
        /// </summary>
        protected SerializationConfigurationBase()
        {
            this.SerializationConfigurationType = this.GetType().ToSerializationConfigurationType();
        }

        /// <summary>
        /// Gets a map of the dependent (all ancestors) serialization configuration types to their initialized instance.
        /// </summary>
        public IReadOnlyDictionary<SerializationConfigurationType, SerializationConfigurationBase> DependentSerializationConfigurationTypeToInstanceMap { get; private set; }

        /// <summary>
        /// Gets all specified dependent configuration types, including all internal configuration types
        /// unless this this configuration type is <see cref="IIgnoreDefaultDependencies"/>.
        /// </summary>
        public IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypesWithDefaultsIfApplicable => this.GetDependentSerializationConfigurationTypesWithDefaultsIfApplicable();

        /// <summary>
        /// Gets a map of registered type to the type of serialization configuration that registered that type.
        /// </summary>
        protected IReadOnlyDictionary<Type, RegistrationDetails> RegisteredTypeToRegistrationDetailsMap => this.registeredTypeToRegistrationDetailsMap;

        /// <summary>
        /// Gets the serialization configuration type of the current instance.
        /// </summary>
        protected SerializationConfigurationType SerializationConfigurationType { get; }

        /// <summary>
        /// Run configuration logic.
        /// </summary>
        /// <param name="dependentSerializationConfigurationTypeToInstanceMap">Map of dependent configuration type to configured instance.</param>
        public void Initialize(
            IReadOnlyDictionary<SerializationConfigurationType, SerializationConfigurationBase> dependentSerializationConfigurationTypeToInstanceMap)
        {
            new { dependentSerializationConfigurationTypeToInstanceMap }.AsArg().Must().NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();

            if (!this.initialized)
            {
                lock (this.syncConfigure)
                {
                    if (!this.initialized)
                    {
                        this.DependentSerializationConfigurationTypeToInstanceMap = dependentSerializationConfigurationTypeToInstanceMap;

                        this.RegisterTypesFromDependentSerializationConfigurations();

                        var typesToRegister = new Queue<TypeToRegister>(this.TypesToRegister ?? new TypeToRegister[0]);

                        this.ProcessTypesToRegister(typesToRegister);

                        this.FinalizeInitialization();

                        this.initialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the specified type has been registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// true if the type has been registered, otherwise false.
        /// </returns>
        public bool IsRegisteredType(
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();
            new { type.ContainsGenericParameters }.AsArg().Must().BeFalse();

            bool result;

            if (this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
            {
                result = true;
            }
            else if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                result = this.registeredTypeToRegistrationDetailsMap.ContainsKey(genericTypeDefinition) && type.GenericTypeArguments.All(this.IsRegisteredType);
            }
            else
            {
                result = false;
            }

            return result;
        }

        private IReadOnlyCollection<SerializationConfigurationType> GetDependentSerializationConfigurationTypesWithDefaultsIfApplicable()
        {
            var result = this.DependentSerializationConfigurationTypes.ToList();

            if (!(this is IIgnoreDefaultDependencies))
            {
                result.AddRange(this.DefaultDependentSerializationConfigurationTypes);
            }

            return result;
        }

        private void RegisterTypesFromDependentSerializationConfigurations()
        {
            foreach (var dependentSerializationConfigurationType in this.DependentSerializationConfigurationTypeToInstanceMap.Keys)
            {
                var dependentSerializationConfiguration = this.DependentSerializationConfigurationTypeToInstanceMap[dependentSerializationConfigurationType];

                var registrationDetailsForDirectlyRegisteredTypes = dependentSerializationConfiguration
                    .registeredTypeToRegistrationDetailsMap
                    .Where(_ => _.Value.SerializationConfigurationType == dependentSerializationConfigurationType)
                    .Select(_ => _.Value)
                    .ToList();

                foreach (var registrationDetails in registrationDetailsForDirectlyRegisteredTypes)
                {
                    this.RegisterType(registrationDetails);
                }
            }
        }

        private void ProcessTypesToRegister(
            Queue<TypeToRegister> typesToRegister)
        {
            new { typesToRegister }.AsOp().Must().NotContainAnyNullElements();

            typesToRegister.All(_ => _.IsOriginatingType).AsOp(Invariant($"All {nameof(TypeToRegister)} objects in {nameof(this.TypesToRegister)} are originating {nameof(TypeToRegister)} objects.")).Must().BeTrue();

            // this could have been done in the while loop below but we think the algorithm
            // is easier to understand by registering these types upfront
            foreach (var typeToRegister in typesToRegister)
            {
                // this.RegisterType will throw if the user specifies the same type twice or if type
                // has already been registered by a dependent serialization configuration
                this.RegisterType(typeToRegister);
            }

            var visitedTypesToRegisterIds = new HashSet<string>();

            while (typesToRegister.Count > 0)
            {
                var typeToRegister = typesToRegister.Dequeue();

                // we already registered all originating types above so they should
                // not be registered here.
                // additionalTypesToInclude (variable below) is guaranteed to exclude
                // typeToRegister.Type (dequeued value), and so it's not possible
                // to create any more originating types.
                if (!typeToRegister.IsOriginatingType)
                {
                    // if we've gotten here and the type is already registered, it means that the
                    // type was introduced by GetRelatedTypesToInclude() or GetMemberTypesToInclude()
                    // We do not want to throw because recursive calls to those methods could result
                    // in traversing the same type multiple times, so just skip the registration.
                    if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(typeToRegister.Type))
                    {
                        // these types need to be considered for spawning additional types to include
                        // and thus need to be processed in the queue, but they themselves cannot be registered.
                        // for example, if the type is List<List<MyModelType>>, we don't want to register it,
                        // but we want to run member inclusion to get at MyModelType.
                        if (IsTypeThatCanBeRegistered(typeToRegister))
                        {
                            this.RegisterType(typeToRegister);
                        }
                    }
                }

                var typeToRegisterId = BuildIdIgnoringOrigin(typeToRegister);

                if (!visitedTypesToRegisterIds.Contains(typeToRegisterId))
                {
                    visitedTypesToRegisterIds.Add(typeToRegisterId);

                    var relatedTypes = GetRelatedTypesToInclude(typeToRegister.Type, typeToRegister.RelatedTypesToInclude);

                    var memberTypes = GetMemberTypesToInclude(typeToRegister.Type, typeToRegister.MemberTypesToInclude);

                    var additionalTypesToInclude = new Type[0]
                        .Concat(relatedTypes)
                        .Concat(memberTypes)
                        .Distinct()
                        .Where(_ => !VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance.Equals(_, typeToRegister.Type))
                        .ToList();

                    foreach (var additionalTypeToInclude in additionalTypesToInclude)
                    {
                        var additionalTypeToRegister = typeToRegister.CreateSpawnedTypeToRegister(additionalTypeToInclude);

                        var additionalTypeToRegisterId = BuildIdIgnoringOrigin(additionalTypeToRegister);

                        if (!visitedTypesToRegisterIds.Contains(additionalTypeToRegisterId))
                        {
                            typesToRegister.Enqueue(additionalTypeToRegister);
                        }
                    }
                }
            }
        }

        private void RegisterType(
            TypeToRegister typeToRegister)
        {
            new { typeToRegister }.AsArg().Must().NotBeNull();

            var registrationDetails = new RegistrationDetails(typeToRegister, this.SerializationConfigurationType);

            this.RegisterType(registrationDetails);
        }

        private void RegisterType(
            RegistrationDetails registrationDetails)
        {
            var typeToRegister = registrationDetails.TypeToRegister;

            var type = typeToRegister.Type;

            if (!IsTypeThatCanBeRegistered(typeToRegister))
            {
                throw new InvalidOperationException(Invariant($"Serialization configuration {this.SerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()} is attempting to register the following type which cannot be registered: {type.ToStringReadable()}."));
            }

            if (this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
            {
                var existingSerializationConfigurationType = this.registeredTypeToRegistrationDetailsMap[type].SerializationConfigurationType;

                throw new InvalidOperationException(Invariant($"Serialization configuration type {registrationDetails.SerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()} is attempting to register type '{type.ToStringReadable()}' but it was already registered by {existingSerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()}."));
            }

            this.ProcessRegistrationDetailsPriorToRegistration(registrationDetails);

            this.registeredTypeToRegistrationDetailsMap.Add(type, registrationDetails);
        }
    }
}