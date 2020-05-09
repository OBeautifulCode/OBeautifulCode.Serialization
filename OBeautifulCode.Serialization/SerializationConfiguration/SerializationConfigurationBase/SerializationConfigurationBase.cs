// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Common configuration base across all kinds of serialization.
    /// </summary>
    public abstract partial class SerializationConfigurationBase
    {
        private readonly object syncConfigure = new object();

        private readonly List<SerializationConfigurationBase> ancestorSerializationConfigurationInstances = new List<SerializationConfigurationBase>();

        private readonly ConcurrentDictionary<Type, RegistrationDetails> registeredTypeToRegistrationDetailsMap = new ConcurrentDictionary<Type, RegistrationDetails>();

        private readonly HashSet<string> visitedTypesToRegisterIds = new HashSet<string>();

        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationConfigurationBase"/> class.
        /// </summary>
        protected SerializationConfigurationBase()
        {
            this.SerializationConfigurationType = this.GetType().ToSerializationConfigurationType();
        }

        /// <summary>
        /// Gets a map of the descendant (children, grandchildren, etc.) serialization configuration types to their initialized instance.
        /// </summary>
        public IReadOnlyDictionary<SerializationConfigurationType, SerializationConfigurationBase> DescendantSerializationConfigurationTypeToInstanceMap { get; private set; }

        /// <summary>
        /// Gets all specified dependent (child) configuration types, including all internal configuration types
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
        /// <param name="descendantSerializationConfigurationTypeToInstanceMap">Map of descendant (children, grandchildren, etc.) configuration type to configured instance.</param>
        public void Initialize(
            IReadOnlyDictionary<SerializationConfigurationType, SerializationConfigurationBase> descendantSerializationConfigurationTypeToInstanceMap)
        {
            new { descendantSerializationConfigurationTypeToInstanceMap }.AsArg().Must().NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();

            if (!this.initialized)
            {
                lock (this.syncConfigure)
                {
                    if (!this.initialized)
                    {
                        this.DescendantSerializationConfigurationTypeToInstanceMap = descendantSerializationConfigurationTypeToInstanceMap;

                        this.RegisterTypesFromDescendantSerializationConfigurations();

                        this.RegisterThisInstanceAsAncestorOfDescendantSerializationConfigurations();

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

            var result = this.registeredTypeToRegistrationDetailsMap.ContainsKey(type);

            return result;
        }

        /// <summary>
        /// Gets the serialization configuration type that registered the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The serialization configuration type that registered the specified type.
        /// </returns>
        public SerializationConfigurationType GetRegisteringSerializationConfigurationType(
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var result = this.registeredTypeToRegistrationDetailsMap[type].SerializationConfigurationType;

            return result;
        }

        /// <summary>
        /// Registers the specified closed generic type, post-initialization.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <remarks>
        /// These types are runtime types; they cannot be "discovered" during initialization and yet they still
        /// need to be registered so that derivative serialization configurations can perform the property setup
        /// to serialize the type.
        /// </remarks>
        public void RegisterClosedGenericTypePostInitialization(
            Type type)
        {
            type.IsClosedGenericType().AsArg(Invariant($"{nameof(SerializationConfigurationExtensions.IsClosedGenericType)}({type?.ToStringReadable() ?? "<null>"})")).Must().BeTrue();

            if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                this.registeredTypeToRegistrationDetailsMap.ContainsKey(genericTypeDefinition).AsArg(Invariant($"{genericTypeDefinition.ToStringReadable()} is registered")).Must().BeTrue();

                lock (this.syncConfigure)
                {
                    if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
                    {
                        // it's not clear what the direct and recursive origin would be, even given the tracked RegistrationDetails
                        // It's not as simple as this.registeredTypeToRegistrationDetailsMap[genericTypeDefinition]
                        // Consider type IGeneric<GenericClass<string>>>.  The registered typeof(IGeneric<>) might have been encountered
                        // is a completely different way than GenericClass (different origin, different Member/RelatedTypesToInclude).
                        // Choosing type as it's own direct and recursive origin is actually not bad - at least it lets us connect the
                        // specified type to the additional types encountered in ProcessTypesToRegister().
                        // Related, it's also not clear how to set Member/RelatedTypesToInclude.  Here we are choosing the most inclusive
                        // settings, but there are certainly scenarios where the resulting registered types, reasoned against the originally
                        // registered types, would not have been registered.  In practice we don't believe consumers will be so surgical
                        // about the choice of Member/RelatedTypesToInclude and in fact there's even a bit of a smell in that (e.g. only wanting
                        // descendants of a type but not wanting to register it's ancestors)
                        var typeToRegister = this.BuildTypeToRegisterForPostInitializationRegistration(type, type, type, TypeToRegisterConstants.DefaultMemberTypesToInclude, TypeToRegisterConstants.DefaultRelatedTypesToInclude);

                        var queue = new Queue<TypeToRegister>();

                        queue.Enqueue(typeToRegister);

                        var processTypesToRegisterResult = this.ProcessTypesToRegister(queue);

                        foreach (var ancestorSerializationConfiguration in this.ancestorSerializationConfigurationInstances)
                        {
                            ancestorSerializationConfiguration.RegisterTypesFromDescendantPostInitialization(processTypesToRegisterResult);
                        }
                    }
                }
            }
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

        private void RegisterTypesFromDescendantSerializationConfigurations()
        {
            foreach (var descendantSerializationConfigurationType in this.DescendantSerializationConfigurationTypeToInstanceMap.Keys)
            {
                var descendantSerializationConfiguration = this.DescendantSerializationConfigurationTypeToInstanceMap[descendantSerializationConfigurationType];

                var registrationDetailsForDirectlyRegisteredTypes = descendantSerializationConfiguration
                    .registeredTypeToRegistrationDetailsMap
                    .Where(_ => _.Value.SerializationConfigurationType == descendantSerializationConfigurationType)
                    .Select(_ => _.Value)
                    .ToList();

                foreach (var registrationDetails in registrationDetailsForDirectlyRegisteredTypes)
                {
                    this.RegisterType(registrationDetails);
                }

                this.visitedTypesToRegisterIds.AddRange(descendantSerializationConfiguration.visitedTypesToRegisterIds);
            }
        }

        private void RegisterThisInstanceAsAncestorOfDescendantSerializationConfigurations()
        {
            foreach (var descendantSerializationConfiguration in this.DescendantSerializationConfigurationTypeToInstanceMap.Values)
            {
                descendantSerializationConfiguration.ancestorSerializationConfigurationInstances.Add(this);
            }
        }

        private void RegisterTypesFromDescendantPostInitialization(
            ProcessTypesToRegisterResult processTypesToRegisterResult)
        {
            this.visitedTypesToRegisterIds.AddRange(processTypesToRegisterResult.VisitedTypesToRegisterIds);

            foreach (var registrationDetails in processTypesToRegisterResult.RegistrationDetails)
            {
                var type = registrationDetails.TypeToRegister.Type;

                // We should NOT throw if the type is already registered, consider:
                // a descendant config has MyGeneric<> registered and
                // descendant.RegisterClosedGenericTypePostInitialization(MyGeneric<SomeType>) is called.
                // the descendant didn't have SomeType registered, which it discovers and then registers.
                // This config, an ancestor, will be notified and will register MyGeneric<SomeType> but it already has
                // SomeType registered.  That's a legitimate scenario and this config should just skip SomeType and
                // move on.  This config doesn't need to explore MyGeneric<SomeType> for types to register.
                // If SomeType was already registered by the descendant, then it would have been inherited by
                // this config during initialization.  Truly, only new the new types discovered by the descendant
                // need to be considered for registration here.
                if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
                {
                    this.RegisterType(registrationDetails);
                }
            }
        }

        private ProcessTypesToRegisterResult ProcessTypesToRegister(
            Queue<TypeToRegister> typesToRegister)
        {
            new { typesToRegister }.AsOp().Must().NotContainAnyNullElements();

            typesToRegister.All(_ => _.IsOriginatingType).AsOp(Invariant($"All {nameof(TypeToRegister)} objects in {nameof(this.TypesToRegister)} are originating {nameof(TypeToRegister)} objects.")).Must().BeTrue();

            var result = new ProcessTypesToRegisterResult();

            // this could have been done in the while loop below but we think the algorithm
            // is easier to understand by registering these types upfront
            foreach (var typeToRegister in typesToRegister)
            {
                // this.RegisterType will throw if the user specifies the same type twice or if type
                // has already been registered by a dependent serialization configuration
                var registrationDetails = this.RegisterType(typeToRegister);

                result.RegistrationDetails.Add(registrationDetails);
            }

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
                            var registrationDetails = this.RegisterType(typeToRegister);

                            result.RegistrationDetails.Add(registrationDetails);
                        }
                    }
                }

                var typeToRegisterId = BuildIdIgnoringOrigin(typeToRegister);

                if (!this.visitedTypesToRegisterIds.Contains(typeToRegisterId))
                {
                    this.visitedTypesToRegisterIds.Add(typeToRegisterId);

                    result.VisitedTypesToRegisterIds.Add(typeToRegisterId);

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

                        if (!this.visitedTypesToRegisterIds.Contains(additionalTypeToRegisterId))
                        {
                            typesToRegister.Enqueue(additionalTypeToRegister);
                        }
                    }
                }
            }

            return result;
        }

        private RegistrationDetails RegisterType(
            TypeToRegister typeToRegister)
        {
            new { typeToRegister }.AsArg().Must().NotBeNull();

            var result = new RegistrationDetails(typeToRegister, this.SerializationConfigurationType);

            this.RegisterType(result);

            return result;
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

            this.registeredTypeToRegistrationDetailsMap.TryAdd(type, registrationDetails);
        }

        private class ProcessTypesToRegisterResult
        {
            public List<RegistrationDetails> RegistrationDetails { get; } = new List<RegistrationDetails>();

            public List<string> VisitedTypesToRegisterIds { get; } = new List<string>();
        }
    }
}