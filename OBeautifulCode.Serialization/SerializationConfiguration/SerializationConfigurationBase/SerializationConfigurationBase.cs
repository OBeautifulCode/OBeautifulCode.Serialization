﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Common configuration base across all kinds of serialization.
    /// </summary>
    public abstract partial class SerializationConfigurationBase
    {
        private static readonly ConcurrentDictionary<Type, List<MemberInfo>> CachedTypeToAllFieldsAndPropertiesMemberInfoMap = new ConcurrentDictionary<Type, List<MemberInfo>>();

        private readonly ConcurrentDictionary<SerializationDirection, ConcurrentDictionary<Type, object>> validatedTypes = new ConcurrentDictionary<SerializationDirection, ConcurrentDictionary<Type, object>>();

        private readonly object syncConfigure = new object();

        private readonly List<SerializationConfigurationBase> ancestorSerializationConfigurationInstances = new List<SerializationConfigurationBase>();

        private readonly ConcurrentDictionary<Type, RegistrationDetails> registeredTypeToRegistrationDetailsMap = new ConcurrentDictionary<Type, RegistrationDetails>();

        private readonly ConcurrentDictionary<Type, RegistrationTime> registeredTypeToRegistrationTimeMap = new ConcurrentDictionary<Type, RegistrationTime>();

        private readonly HashSet<string> visitedTypesToRegisterIds = new HashSet<string>();

        // ReSharper disable once CollectionNeverQueried.Local
        private readonly Dictionary<Type, TypeToRegister> filteredOutTypeToTypeToRegisterMap = new Dictionary<Type, TypeToRegister>();

        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationConfigurationBase"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = ObcSuppressBecause.CA2214_DoNotCallOverridableMethodsInConstructors_OverriddenMethodDoesNotRelyOnInitializationNorOtherConfigurationInConstructor)]
        protected SerializationConfigurationBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.SerializationConfigurationType = this.BuildSerializationConfigurationType();
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
            if (descendantSerializationConfigurationTypeToInstanceMap == null)
            {
                throw new ArgumentNullException(nameof(descendantSerializationConfigurationTypeToInstanceMap));
            }

            if (descendantSerializationConfigurationTypeToInstanceMap.Any(_ => _.Value == null))
            {
                throw new ArgumentException(Invariant($"'{nameof(descendantSerializationConfigurationTypeToInstanceMap)}' contains a key/value pair with a null value"));
            }

            if (!this.initialized)
            {
                lock (this.syncConfigure)
                {
                    if (!this.initialized)
                    {
                        this.DescendantSerializationConfigurationTypeToInstanceMap = descendantSerializationConfigurationTypeToInstanceMap;

                        this.RegisterTypesFromDescendantSerializationConfigurations();

                        this.RegisterThisInstanceAsAncestorOfDescendantSerializationConfigurations();

                        var typesToRegisterQueue = new Queue<TypeToRegister>(this.TypesToRegister ?? new TypeToRegister[0]);

                        this.ProcessTypesToRegister(typesToRegisterQueue);

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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = this.registeredTypeToRegistrationDetailsMap.ContainsKey(type);

            return result;
        }

        /// <summary>
        /// Throw on an unregistered type, if appropriate given the <see cref="UnregisteredTypeEncounteredStrategy"/>.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="serializationDirection">The serialization direction.</param>
        /// <param name="objectToSerialize">OPTIONAL object to serialize if serializing.  DEFAULT is null, assumes deserialization.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public void ThrowOnUnregisteredTypeIfAppropriate(
            Type type,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            // For serialization we are dealing with validating both declared and runtime types:
            //     We are holding an object and it's object.GetType(), we need to validate that type ahead of serialization.
            //     This means recursing thru the declared types AND runtime types of the original type's properties and fields.
            //     Further, for arrays, collections and dictionaries we need to recurse
            //     into the runtime types of the elements/keys/values and then consider the declared and runtime types
            //     of the element/keys/values fields and properties.  In that process there is certainly some redundant
            //     checks, but the heuristic would be harder to write in a correct way and anyways validatedTypes
            //     short-circuits a lot of validation.  After this process, we'll be 100% certain that all of the types involved
            //     with the object being serialized have been registered.  Because we are checking runtime types, we may encounter
            //     unregistered closed generic types whose generic type definitions ARE registered.  In that case, we call
            //     RegisterClosedGenericTypePostInitialization().  It's fine if, sometime later, we register the same closed generic
            //     type in a descendant config type, because the descendant will notify it's ancestors and the ancestors will only
            //     register the type if it is not yet registered.
            // For deserialization we are dealing with validating declared types:
            //     We validate that the declared type is registered and we recurse thru the declared types of the fields
            //     and properties and validate that they are registered as well.  Given this, upon deserialization
            //     the only unregistered types that we can encounter in the payload are concrete classes that are being
            //     assigned to an interface or base class type.  In this case, at serialization time, we would have written
            //     the assembly qualified name of the concrete type into the payload to facilitate de-serialization.
            //     So the type we resolve from the assembly qualified name is potentially unregistered, and thus we need to call
            //     THIS method on these types.  Assuming that the user is using the same config type for serialization and deserialization,
            //     serialization (per above) recurses thru all runtime types and would have thrown for unregistered, non-generic
            //     classes, and also throws if a generic class's generic type definition is not registered.
            //     As mentioned above closed generic classes will be registered if needed.  So its unlikely that calling this
            //     method to validate concrete classes being assigned to interface or base types will throw.
            //     What about generic classes that are NOT being deserialized into an interface or base class?
            //     If the top-level type, the type we are deserializing into, is a closed generic type then we'll register that type
            //     HERE in ValidateTypeIsRegistered() using RegisterClosedGenericTypePostInitialization(), if needed.
            //     Otherwise, if there is a field or property whose declared type is a closed generic, we will check that it's registered
            //     HERE in ValidateMembersAreRegistered() and if not call RegisterClosedGenericTypePostInitialization() if needed.
            if (type == null)
            {
                // this must be supported for serializing null
                // if type == null then objectToSerialize == null
                return;
            }

            if (type.ContainsGenericParameters)
            {
                throw new InvalidOperationException(Invariant($"Cannot serialize or deserialize an open type: {type.ToStringReadable()}."));
            }

            if (this.UnregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Attempt)
            {
                // Short-circuit attempt mode.  If not, then when we encounter a closed generic we will try to register
                // it, regardless of whether the generic type definition has been registered.  It's not clear whether attempt
                // should be registering any types at all.
                return;
            }

            this.InternalThrowOnUnregisteredTypeIfAppropriate(type, type, serializationDirection, objectToSerialize);

            // if serializing and the type is a System type (e.g. List<MyClass>)
            // then we need to iterate through the runtime types of the enumerable elements, if applicable
            if ((serializationDirection == SerializationDirection.Serialize) && type.IsSystemType())
            {
                this.ValidateEnumerableElementsAreRegisteredIfApplicable(type, serializationDirection, type, objectToSerialize);
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
                    var type = registrationDetails.TypeToRegister.Type;

                    var registrationTime = descendantSerializationConfiguration.registeredTypeToRegistrationTimeMap[type];

                    // It is possible that two descendants have the same type registered.
                    // This can happen with a post-initialization registration of a closed generic type.
                    // We first discovered it with two dependent types having EventBase<string> registered.
                    // In that situation, a high level ancestor config was used to serialize a type that derived from EventBase<string>
                    // causing a post-initialization registration.  Then a low level descendant config was used to serialize a type that
                    // also derived from EventBase<string>.  That resulted in a post-initialization registration with the low level descendant
                    // config.  It further bubbled-up the type and registered it with all ancestors.  When it reached the high level ancestor
                    // config, the type was already registered so it just skipped the type.  Finally, we created a serializer with a wrapping
                    // config type (e.g. MinimalFormatJsonSerializationConfiguration<High Level Ancestor>).  Now we had the problem where
                    // EventBase<string> was registered with two of it's descendants, the low and high level configs and we were throwing.
                    // This was a pretty complicated scenario and a simpler one is easy to construct: two sibling configs are used to serialize
                    // types that derive from EventBase<string>.  Both now contain a post-initialization registration.
                    // Then another config is declared with the sibling configs as its descendants.  The code would throw when that config is initialized.
                    // While we think it's also possible with initialization-time registrations, the configs would have to
                    // cast a wide net (registering types out of namespace of the domain) which is a bad practice.
                    // We want to throw in that case so we can at least investigate where this is happening.
                    // NOTE we really should be checking RegistrationDetails.TypeToRegister is the same in both descendant configs,
                    // but that would be a cumbersome fix.  This ensures that one of the registrations doesn't have a custom serializer or
                    // some other customization that the other registration doesn't have.  Here we are just blindly saying that the first registration
                    // "wins".
                    if (this.registeredTypeToRegistrationDetailsMap.ContainsKey(type) && (registrationTime == RegistrationTime.PostInitialization))
                    {
                        continue;
                    }

                    this.RegisterType(registrationDetails, RegistrationTime.Initialization);
                }

                // Previously, we were copying the visitedTypesToRegisterIds from the descendant configs into this config.
                // However, consider the case where a descendant config visits a type, but does not register the type
                // because of the TypeToRegisterNamespacePrefixFilters.  When this config comes along and sees the type,
                // it cannot be registered because the code passes over the type because it finds it in visitedTypesToRegisterIds.
                // Unfortunately, we have to trade-off performance here (exploring lots of the sames types that have already been
                // explored by descendants).  We could not find a heuristic or optimization that allowed us to keep some
                // of the visited types and discard the potentially problematic.
                // this.visitedTypesToRegisterIds.AddRange(descendantSerializationConfiguration.visitedTypesToRegisterIds);
            }
        }

        private void RegisterThisInstanceAsAncestorOfDescendantSerializationConfigurations()
        {
            foreach (var descendantSerializationConfiguration in this.DescendantSerializationConfigurationTypeToInstanceMap.Values)
            {
                descendantSerializationConfiguration.ancestorSerializationConfigurationInstances.Add(this);
            }
        }

        private void ProcessTypesToRegister(
            Queue<TypeToRegister> typesToRegisterQueue)
        {
            if (typesToRegisterQueue.Any(_ => _ == null))
            {
                throw new InvalidOperationException(Invariant($"'{nameof(typesToRegisterQueue)}' contains an element that is null"));
            }

            if (!typesToRegisterQueue.All(_ => _.IsOriginatingType))
            {
                throw new InvalidOperationException(Invariant($"One or more {nameof(TypeToRegister)} objects in {nameof(this.TypesToRegister)} are NOT originating {nameof(TypeToRegister)} objects."));
            }

            if (this.TypeToRegisterNamespacePrefixFilters == null)
            {
                throw new InvalidOperationException(Invariant($"{nameof(this.TypeToRegisterNamespacePrefixFilters)} is null"));
            }

            if (this.TypeToRegisterNamespacePrefixFilters.Any(string.IsNullOrWhiteSpace))
            {
                throw new InvalidOperationException(Invariant($"{nameof(this.TypeToRegisterNamespacePrefixFilters)} contains an element that is null or white space"));
            }

            SeedAncestorsAndDescendants(this.SerializationConfigurationType);

            while (typesToRegisterQueue.Count > 0)
            {
                var typeToRegister = typesToRegisterQueue.Dequeue();

                // This type might be an originating type and already be registered by a dependent config.
                // This typically occurs when the consumer wants to discover descendants in a new namespace
                // (a different set of namespaces than dependent config is filtering on).
                // OR
                // The type was introduced by GetRelatedTypesToInclude() or GetMemberTypesToInclude().
                // Recursive calls to those methods could result in traversing the same type multiple times.
                // In both cases, just skip it.
                if ((!this.registeredTypeToRegistrationDetailsMap.ContainsKey(typeToRegister.Type)) && (!this.filteredOutTypeToTypeToRegisterMap.ContainsKey(typeToRegister.Type)))
                {
                    // these types need to be considered for spawning additional types to include
                    // and thus need to be processed in the queue, but they themselves cannot be registered.
                    // for example, if the type is List<List<MyModelType>>, we don't want to register it,
                    // but we want to run member inclusion to get at MyModelType.
                    if (IsTypeThatCanBeRegistered(typeToRegister))
                    {
                        var typeToRegisterNamespace = typeToRegister.Type.Namespace;

                        // apply the namespace filters
                        // ReSharper disable once PossibleNullReferenceException
                        if ((!this.TypeToRegisterNamespacePrefixFilters.Any()) || this.TypeToRegisterNamespacePrefixFilters.Any(_ => typeToRegisterNamespace.StartsWith(_, StringComparison.Ordinal)))
                        {
                            this.RegisterType(typeToRegister, RegistrationTime.Initialization);
                        }
                        else
                        {
                            this.filteredOutTypeToTypeToRegisterMap.Add(typeToRegister.Type, typeToRegister);
                        }
                    }
                    else
                    {
                        if (typeToRegister.IsOriginatingType)
                        {
                            throw new InvalidOperationException(Invariant($"Serialization configuration {this.SerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()} is attempting to register the following type which cannot be registered: {typeToRegister.Type.ToStringReadable()}."));
                        }
                    }
                }

                var typeToRegisterId = BuildIdIgnoringOrigin(typeToRegister);

                if (!this.visitedTypesToRegisterIds.Contains(typeToRegisterId))
                {
                    this.visitedTypesToRegisterIds.Add(typeToRegisterId);

                    var relatedTypes = GetRelatedTypesToInclude(typeToRegister.Type, typeToRegister.RelatedTypesToInclude);

                    this.EnqueueAdditionalTypesToInclude(typeToRegister, relatedTypes, TypeToIncludeOrigin.GettingRelatedTypes, typesToRegisterQueue);

                    var memberTypes = GetMemberTypesToInclude(typeToRegister.Type, typeToRegister.MemberTypesToInclude);

                    this.EnqueueAdditionalTypesToInclude(typeToRegister, memberTypes, TypeToIncludeOrigin.GettingMemberTypes, typesToRegisterQueue);
                }
            }
        }

        private void EnqueueAdditionalTypesToInclude(
            TypeToRegister typeToRegister,
            IReadOnlyCollection<Type> additionalTypesToInclude,
            TypeToIncludeOrigin typeToIncludeOrigin,
            Queue<TypeToRegister> typesToRegisterQueue)
        {
            // This excludes typeToRegister.Type (the dequeued value),
            // so it's not possible to create any more originating types.
            additionalTypesToInclude =
                additionalTypesToInclude
                .Distinct()
                .Where(_ => !VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance.Equals(_, typeToRegister.Type))
                .ToList();

            foreach (var additionalTypeToInclude in additionalTypesToInclude)
            {
                var additionalTypeToRegister = typeToRegister.CreateSpawnedTypeToRegister(additionalTypeToInclude, typeToIncludeOrigin);

                var additionalTypeToRegisterId = BuildIdIgnoringOrigin(additionalTypeToRegister);

                if (!this.visitedTypesToRegisterIds.Contains(additionalTypeToRegisterId))
                {
                    typesToRegisterQueue.Enqueue(additionalTypeToRegister);
                }
            }
        }

        private RegistrationDetails RegisterType(
            TypeToRegister typeToRegister,
            RegistrationTime registrationTime)
        {
            if (typeToRegister == null)
            {
                throw new ArgumentNullException(nameof(typeToRegister));
            }

            var result = new RegistrationDetails(typeToRegister, this.SerializationConfigurationType);

            this.RegisterType(result, registrationTime);

            return result;
        }

        private void RegisterType(
            RegistrationDetails registrationDetails,
            RegistrationTime registrationTime)
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

                throw new InvalidOperationException(Invariant($"Serialization configuration {this.SerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()} is attempting to register type '{type.ToStringReadable()}' via {nameof(RegistrationDetails)} having {nameof(RegistrationDetails.SerializationConfigurationType)} {registrationDetails.SerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()}, but it was already registered by {existingSerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()}."));
            }

            this.ProcessRegistrationDetailsPriorToRegistration(registrationDetails, registrationTime);

            this.registeredTypeToRegistrationTimeMap.TryAdd(type, registrationTime);

            this.registeredTypeToRegistrationDetailsMap.TryAdd(type, registrationDetails);
        }

        private void InternalThrowOnUnregisteredTypeIfAppropriate(
            Type originalType,
            Type typeToValidate,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            this.ValidateTypeAndInheritancePathIsRegistered(originalType, typeToValidate, serializationDirection);

            // We don't want to validate the members of restricted types like bool, List<SomeType>, etc.
            // ValidateTypeIsRegistered will pull out the type(s) of the array element, collection elements,
            // dictionary keys/values and validate them.  On serialization, objects having array/collection/dictionary
            // properties will have those enumerables iterated and their runtime types checked in ValidateMembersAreRegistered().
            // So if we get there with a restricted type, there's nothing to do.
            if (!IsRestrictedType(typeToValidate))
            {
                if (!this.TypesPermittedToHaveUnregisteredMembers.ContainsKey(typeToValidate))
                {
                    this.ValidateMembersAreRegistered(originalType, typeToValidate, serializationDirection, objectToSerialize);
                }
            }
        }

        private void ValidateTypeAndInheritancePathIsRegistered(
            Type originalType,
            Type typeToValidate,
            SerializationDirection serializationDirection)
        {
            // Prevents StackOverflowException when you have a type such as MyType : MyGeneric<MyType>
            if (this.validatedTypes.TryGetValue(serializationDirection, out var validatedTypeToNothingMap) && validatedTypeToNothingMap.ContainsKey(typeToValidate))
            {
                return;
            }

            // For non-restricted types we need to validate the type itself as well as all ancestors.
            // This protects against the scenario where a derived type is registered, but it's
            // ancestor isn't.  In BSON, for example, the class map only covers the members
            // declared on the type itself.  So if BSON hasn't "seen" the ancestors, the right class
            // map will not be created/mapped to those ancestor types.
            // This approach is also particularly important for post-initialization registrations,
            // where serializes have no opportunity, during initialization, to "see" the closed generic
            // ancestors.
            // Note that ValidateMembersAreRegistered() will look for declared and non-declared members,
            // which is why we don't need to call ValidateTypeIsRegistered() on the ancestor types.
            var typeToValidateIncludingAncestors = IsRestrictedType(typeToValidate)
                ? new[] { typeToValidate }
                : new[] { typeToValidate }.Concat(typeToValidate.GetInheritancePath()).Except(new[] { typeof(object) }).ToArray();

            foreach (var localTypeToValidate in typeToValidateIncludingAncestors)
            {
                this.ValidateTypeIsRegistered(originalType, localTypeToValidate, serializationDirection);
            }
        }

        private void ValidateTypeIsRegistered(
            Type originalType,
            Type typeToValidate,
            SerializationDirection serializationDirection)
        {
            if (this.validatedTypes.TryGetValue(serializationDirection, out var validatedTypeToNothingMap) && validatedTypeToNothingMap.ContainsKey(typeToValidate))
            {
                return;
            }

            if (typeToValidate.IsArray)
            {
                // We found a situation where we were not performing a post-initialization registration for a declared generic type.
                // In this particular case, the object had a property whose type was an IReadOnlyCollection<CustomBaseType>.
                // CustomBaseType's inheritance path included OBC.Type.EventBase<string>.
                // We were previously just checking CustomBaseType's registration and not walking the inheritance path.
                // This resulted in a BsonException.  BSON was taking the liberty to register a class map for EventBase<string>
                // because one didn't exist, but then it's call to ObcBsonDiscriminatorConvention, triggered a call to ThrowOnUnregisteredTypeIfAppropriate
                // which ultimately attempted a post-initialization registration that attempted to add the same class map.
                this.ValidateTypeAndInheritancePathIsRegistered(originalType, typeToValidate.GetElementType(), serializationDirection);
            }
            else if (typeToValidate.IsGenericType)
            {
                // is the closed type registered?  if so, nothing to do
                if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(typeToValidate))
                {
                    // the closed type is not registered, confirm that the generic type arguments are registered
                    // so this will inspect the element type of lists, the key/value types of dictionaries, etc.
                    // will also check the arguments of custom generic types
                    foreach (var genericArgumentType in typeToValidate.GenericTypeArguments)
                    {
                        // See note above in IsArray section
                        this.ValidateTypeAndInheritancePathIsRegistered(originalType, genericArgumentType, serializationDirection);
                    }

                    if (!IsRestrictedType(typeToValidate))
                    {
                        // For non-restricted generic types that are not registered, the generic type definition should be registered.
                        var genericTypeDefinition = typeToValidate.GetGenericTypeDefinition();

                        this.ThrowIfTypeIsNotRegistered(originalType, genericTypeDefinition);

                        // We are purposefully registering the closed generic with THIS configuration and not with
                        // the config that registered the generic type definition (which is potentially a descendant config).
                        // THIS config is guaranteed to know about all of the the types that define the generic
                        // (e.g. MyGeneric1<MyGeneric2<MyGeneric2>>>) because we verified all of them in the foreach loop
                        // above.  As such, this is the most appropriate config to register the type with.
                        this.RegisterClosedGenericTypePostInitialization(typeToValidate);

                        this.ThrowIfTypeIsNotRegistered(originalType, typeToValidate);
                    }
                }
            }
            else if (IsRestrictedType(typeToValidate))
            {
                // do nothing for non-generic restricted types (e.g. bool, int, DateTime, Guid)
            }
            else
            {
                this.ThrowIfTypeIsNotRegistered(originalType, typeToValidate);
            }

            // We need to validate based on SerializationDirection because the logic for exploring types differs
            // by SerializationDirection.  We have found cases where serializing marks a type as validated,
            // but then hinders the exploration of that type on deserializing and types are missed.
            if (!this.validatedTypes.ContainsKey(serializationDirection))
            {
                this.validatedTypes.TryAdd(serializationDirection, new ConcurrentDictionary<Type, object>());
            }

            this.validatedTypes[serializationDirection].TryAdd(typeToValidate, null);
        }

        private void ThrowIfTypeIsNotRegistered(
            Type originalType,
            Type typeToValidate)
        {
            if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(typeToValidate))
            {
                if (originalType == typeToValidate)
                {
                    throw new InvalidOperationException(Invariant($"Attempted to perform operation on unregistered type '{originalType.ToStringReadable()}'."));
                }
                else
                {
                    throw new InvalidOperationException(Invariant($"Attempted to perform operation on type '{originalType.ToStringReadable()}', which contains the unregistered type '{typeToValidate.ToStringReadable()}'."));
                }
            }
        }

        private void RegisterClosedGenericTypePostInitialization(
            Type type)
        {
            // Caller guarantees that this type is closed, generic, and its generic type definition is registered.
            // These types are runtime types; they cannot be "discovered" during initialization and yet they still
            // need to be registered so that derivative serialization configurations can perform the proper setup
            // to serialize the type.
            if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
            {
                // ensures we don't attempt to register the same type multiple times
                lock (this.syncConfigure)
                {
                    if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
                    {
                        // it's not clear what the direct and recursive origin would be, even given the tracked RegistrationDetails
                        // It's not as simple as this.registeredTypeToRegistrationDetailsMap[genericTypeDefinition]
                        // Consider type IGeneric<GenericClass<string>>>.  The registered typeof(IGeneric<>) might have been encountered
                        // is a completely different way than GenericClass (different origin, different Member/RelatedTypesToInclude).
                        // Choosing type as it's own direct and recursive origin is actually not bad.
                        var typeToRegister = this.BuildTypeToRegisterForPostInitializationRegistration(type, type, type, MemberTypesToInclude.None, RelatedTypesToInclude.None);

                        var registrationDetails = this.RegisterType(typeToRegister, RegistrationTime.PostInitialization);

                        foreach (var ancestorSerializationConfiguration in this.ancestorSerializationConfigurationInstances)
                        {
                            ancestorSerializationConfiguration.RegisterTypeFromDescendantPostInitialization(registrationDetails);
                        }
                    }
                }
            }
        }

        private void RegisterTypeFromDescendantPostInitialization(
            RegistrationDetails registrationDetails)
        {
            var type = registrationDetails.TypeToRegister.Type;

            // We should NOT throw if the type is already registered.
            // It's entirely possible that the type was registered with this config type
            // (either pre or post initialization) and then RegisterClosedGenericTypePostInitialization()
            // is called on a descendant config because it's being used by some serializer.
            if (!this.registeredTypeToRegistrationDetailsMap.ContainsKey(type))
            {
                this.RegisterType(registrationDetails, RegistrationTime.PostInitialization);
            }
        }

        private void ValidateMembersAreRegistered(
            Type originalType,
            Type typeToValidate,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            // get or add members from cache
            if (!CachedTypeToAllFieldsAndPropertiesMemberInfoMap.ContainsKey(typeToValidate))
            {
                // note that there is some overlap between this and GetMemberTypesToInclude()
                // both are fetching property and field members of a type.  GetMemberTypesToInclude
                // uses the DeclaredOnly flag so there would be extra work to filter out the non-declared
                // members in GetMemberTypesToInclude().  Trying to harmonize this into a single cache
                // might introduce thread contention issues (potential a deadlock?) with syncConfigure.
                // Not worth the optimization.
                var memberInfosToAdd = typeToValidate
                    .GetMembersFiltered(MemberRelationships.DeclaredInTypeOrAncestorTypes, MemberOwners.Instance, MemberAccessModifiers.All, MemberKinds.Field | MemberKinds.Property)
                    .ToList();

                CachedTypeToAllFieldsAndPropertiesMemberInfoMap.TryAdd(typeToValidate, memberInfosToAdd);
            }

            var memberInfos = CachedTypeToAllFieldsAndPropertiesMemberInfoMap[typeToValidate];

            foreach (var memberInfo in memberInfos)
            {
                var memberDeclaredType = memberInfo.GetUnderlyingType();

                if ((serializationDirection == SerializationDirection.Deserialize) || (serializationDirection == SerializationDirection.Unknown))
                {
                    this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, memberDeclaredType, serializationDirection, null);
                }
                else if (serializationDirection == SerializationDirection.Serialize)
                {
                    object memberObject;

                    switch (memberInfo)
                    {
                        case PropertyInfo propertyInfo:
                            memberObject = propertyInfo.GetValue(objectToSerialize);
                            break;
                        case FieldInfo fieldInfo:
                            memberObject = fieldInfo.GetValue(objectToSerialize);
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"This type of {nameof(MemberInfo)} is not supported: {memberInfo.GetType().ToStringReadable()}."));
                    }

                    if (memberObject == null)
                    {
                        // just recurse the declared types
                        this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, memberDeclaredType, SerializationDirection.Unknown, null);
                    }
                    else
                    {
                        var memberRuntimeType = memberObject.GetType();

                        this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, memberRuntimeType, serializationDirection, memberObject);

                        // in case they are equal, save a redundant call
                        if (memberDeclaredType != memberRuntimeType)
                        {
                            // recurse the declared type, which is not the runtime type
                            this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, memberDeclaredType, SerializationDirection.Unknown, null);
                        }

                        this.ValidateEnumerableElementsAreRegisteredIfApplicable(originalType, serializationDirection, memberRuntimeType, memberObject);
                    }
                }
                else
                {
                    throw new NotSupportedException("This serialization direction is not supported: " + serializationDirection);
                }
            }
        }

        private void ValidateEnumerableElementsAreRegisteredIfApplicable(
            Type originalType,
            SerializationDirection serializationDirection,
            Type memberRuntimeType,
            object memberObject)
        {
            // NOTE: It is NOT possible to cache enumerable element/dictionary key/dictionary value types
            // and skip on subsequent calls to InternalThrowOnUnregisteredTypeIfAppropriate when iterating below.
            // Each item needs to be evaluated because the type may contain properties that are interface or abstract types themselves.
            // For example, lets say all of the element types are MyType, but MyType contains MyProperty which is an IMyInterface<T>.
            // Two different elements may have different runtime types for that property.
            if (memberRuntimeType.IsArray || memberRuntimeType.IsClosedSystemCollectionType())
            {
                var enumerableObject = (IEnumerable)memberObject;

                foreach (var elementObject in enumerableObject)
                {
                    if (elementObject != null)
                    {
                        var elementObjectType = elementObject.GetType();

                        this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, elementObjectType, serializationDirection, elementObject);
                    }
                }
            }
            else if (memberRuntimeType.IsClosedSystemDictionaryType())
            {
                var dictionaryObject = (IDictionary)memberObject;

                foreach (var keyObject in dictionaryObject.Keys)
                {
                    if (keyObject != null)
                    {
                        var keyObjectType = keyObject.GetType();

                        this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, keyObjectType, serializationDirection, keyObject);
                    }
                }

                foreach (var valueObject in dictionaryObject.Values)
                {
                    if (valueObject != null)
                    {
                        var valueObjectType = valueObject.GetType();

                        this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, valueObjectType, serializationDirection, valueObject);
                    }
                }
            }
            else
            {
                // not applicable
            }
        }
    }
}