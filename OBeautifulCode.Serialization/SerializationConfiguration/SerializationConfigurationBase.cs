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
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Internal;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Common configuration base across all kinds of serialization.
    /// </summary>
    public abstract class SerializationConfigurationBase
    {
        /// <summary>
        /// All types are assignable to these types, so filter them out.
        /// </summary>
        private static readonly HashSet<Type> RelatedTypesBlacklist = new HashSet<Type>(
            new[]
            {
                typeof(string),
                typeof(object),
                typeof(ValueType),
                typeof(Enum),
                typeof(Array),
            });

        private static readonly Dictionary<Assembly, IReadOnlyCollection<Type>> AssemblyToTypesToConsiderForRegistrationMap = new Dictionary<Assembly, IReadOnlyCollection<Type>>();

        private readonly object syncConfigure = new object();

        private readonly Dictionary<Type, RegistrationDetails> registeredTypeToRegistrationDetailsMap = new Dictionary<Type, RegistrationDetails>();

        private readonly SerializationConfigurationType serializationConfigurationType;

        private bool initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationConfigurationBase"/> class.
        /// </summary>
        protected SerializationConfigurationBase()
        {
            this.serializationConfigurationType = this.GetType().ToSerializationConfigurationType();
        }

        /// <summary>
        /// Gets the string representation of null.
        /// </summary>
        public string NullSerializedStringValue => "null";

        /// <summary>
        /// Gets the types that need to be registered for any and all serialization.
        /// </summary>
        public IReadOnlyCollection<Type> InternallyRequiredTypes => new[]
        {
            // OBC.Type
            typeof(UtcDateTimeRangeInclusive),

            // OBC.Compression
            typeof(CompressionKind),

            // OBC.Representation
            typeof(AssemblyRepresentation),
            typeof(ElementInitRepresentation),
            typeof(MemberBindingRepresentationBase),
            typeof(ExpressionRepresentationBase),
            typeof(TypeRepresentation),
            typeof(ConstructorInfoRepresentation),
            typeof(MemberInfoRepresentation),
            typeof(MethodInfoRepresentation),

            // OBC.Serialization:
            typeof(SerializerDescription),
            typeof(DescribedSerialization),
            typeof(DynamicTypePlaceholder),
        };

        /// <summary>
        /// Gets a map of the dependent (all ancestors) serialization configuration types to their initialized instance.
        /// </summary>
        public IReadOnlyDictionary<SerializationConfigurationType, SerializationConfigurationBase> DependentSerializationConfigurationTypeToInstanceMap { get; private set; }

        /// <summary>
        /// Gets a map of registered type to the type of serialization configuration that registered that type.
        /// </summary>
        public IReadOnlyDictionary<Type, RegistrationDetails> RegisteredTypeToRegistrationDetailsMap => this.registeredTypeToRegistrationDetailsMap;

        /// <summary>
        /// Gets all specified dependent configuration types, including all internal configuration types
        /// unless this this configuration type is <see cref="IIgnoreDefaultDependencies"/>.
        /// </summary>
        public IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypesWithDefaultsIfApplicable => this.GetDependentSerializationConfigurationTypesWithDefaultsIfApplicable();

        /// <summary>
        /// Gets the serialization configuration types that are needed for this serialization configuration to work.
        /// </summary>
        protected abstract IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes { get; }

        /// <summary>
        /// Gets the serialization configuration types that are required to be in-effect for <see cref="SerializationKind"/>-associated abstract inheritors,
        /// (e.g. BsonSerializationConfiguration) so that, in turn, their concrete inheritors (e.g. MyDomainBsonSerializationConfiguration)
        /// do not need to specify these dependencies and so that any and all serialization that utilizes such concrete inheritors will work as expected.
        /// These will be ignored for any serialization configuration that implements <see cref="IIgnoreDefaultDependencies"/>.
        /// </summary>
        protected abstract IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes { get; }

        /// <summary>
        /// Gets the types to register along with additional information about other types that this type should "pull-in" to registration.
        /// </summary>
        protected virtual IReadOnlyCollection<TypeToRegister> TypesToRegister => new TypeToRegister[0];

        /// <summary>
        /// Run configuration logic.
        /// </summary>
        /// <param name="dependentSerializationConfigurationTypeToInstanceMap">Map of dependent configuration type to configured instance.</param>
        public virtual void Initialize(
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
        /// Processes a <see cref="RegistrationDetails"/> prior to registration.
        /// </summary>
        /// <param name="registrationDetails">Details related to the registration.</param>
        protected virtual void ProcessRegistrationDetailsPriorToRegistration(
            RegistrationDetails registrationDetails)
        {
            /* no-op - inheritors can use this to examine the registration details prior to registration and perform inheritor-specific setup/logic (e.g. using a custom serializer for a type) */
        }

        /// <summary>
        /// Perform any final setup/logic.
        /// </summary>
        protected virtual void FinalizeInitialization()
        {
            /* no-op - inheritors can use this to wrap-up any setup/logic (e.g. in JSON we need to identify ALL types that participate in a hierarchy for the inherited type converter) */
        }

        private static IReadOnlyCollection<Type> GetRelatedTypesToInclude(
            Type type,
            RelatedTypesToInclude relatedTypesToInclude)
        {
            if (relatedTypesToInclude == RelatedTypesToInclude.None)
            {
                return new Type[0];
            }

            var allTypesToConsiderForRegistration = GetAllTypesToConsiderForRegistration();

            IReadOnlyCollection<Type> result;

            // note: we are PURPOSELY not using OBeautifulCode.Reflection.Recipes.TypeHelper.IsAssignableTo
            // because of performance issues related to the OBeautifulCode.Validation calls in that method.
            switch (relatedTypesToInclude)
            {
                case RelatedTypesToInclude.Ancestors:
                    result = allTypesToConsiderForRegistration.Where(_ => _.IsAssignableFrom(type)).ToList();
                    break;
                case RelatedTypesToInclude.Descendants:
                    // ReSharper disable once ConvertClosureToMethodGroup
                    result = allTypesToConsiderForRegistration.Where(_ => type.IsAssignableFrom(_)).ToList();
                    break;
                case RelatedTypesToInclude.AncestorsAndDescendants:
                    result = allTypesToConsiderForRegistration.Where(_ => type.IsAssignableFrom(_) || _.IsAssignableFrom(type)).ToList();
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(RelatedTypesToInclude)} is not supported: {relatedTypesToInclude}"));
            }

            result = result.Where(_ => _ != type).ToList();

            return result;
        }

        private static IReadOnlyCollection<Type> GetAllTypesToConsiderForRegistration()
        {
            var assemblies = AssemblyLoader.GetLoadedAssemblies();

            foreach (var assembly in assemblies)
            {
                if (!AssemblyToTypesToConsiderForRegistrationMap.ContainsKey(assembly))
                {
                    var typesToConsiderForThisAssembly = new[] { assembly }
                        .GetTypesFromAssemblies()
                        .Where(IsTypeThatCanBeRegistered) // && _.IsClass  In an old version of Serialization we only included class types but twe think this is algorithmically incorrect because in certain situations it will exclude types that should be identified as "related" in GetRelatedTypesToInclude
                        .ToList();

                    AssemblyToTypesToConsiderForRegistrationMap.Add(assembly, typesToConsiderForThisAssembly);
                }
            }

            var result = AssemblyToTypesToConsiderForRegistrationMap.Values.SelectMany(_ => _).ToList();

            return result;
        }

        private static IReadOnlyCollection<Type> GetMemberTypesToInclude(
            Type type,
            MemberTypesToInclude memberTypesToInclude)
        {
            if (memberTypesToInclude == MemberTypesToInclude.None)
            {
                return new Type[0];
            }

            var result = new List<Type>();

            if (memberTypesToInclude.HasFlag(MemberTypesToInclude.GenericArguments))
            {
                if (type.IsGenericType)
                {
                    result.AddRange(type.GenericTypeArguments);
                }
            }

            if (memberTypesToInclude.HasFlag(MemberTypesToInclude.ArrayElement))
            {
                if (type.IsArray)
                {
                    result.Add(type.GetElementType());
                }
            }

            bool IsCompilerGenerated(MemberInfo memberInfo) => memberInfo.CustomAttributes.Select(s => s.AttributeType).Contains(typeof(CompilerGeneratedAttribute));

            result.AddRange(
                type
                    .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(_ => !IsCompilerGenerated(_))
                    .SelectMany(
                        _ =>
                        {
                            if ((_ is PropertyInfo propertyInfo) && memberTypesToInclude.HasFlag(MemberTypesToInclude.DeclaredProperties))
                            {
                                return new[] { propertyInfo.PropertyType };
                            }

                            if ((_ is FieldInfo fieldInfo) && memberTypesToInclude.HasFlag(MemberTypesToInclude.DeclaredFields))
                            {
                                return new[] { fieldInfo.FieldType };
                            }

                            return new Type[0];
                        }));

            // result = result.Where(_ => _.IsClosedNonAnonymousClassType()).ToList() // older versions of Serialization filtered to closed non-anonymous class types but we believe this is algorithmically incomplete
            result = result
                .Where(_ => _ != type)
                .ToList();

            return result;
        }

        private static string BuildIdIgnoringOrigin(
            TypeToRegister typeToRegister)
        {
            var result = typeToRegister.Type.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName() + "_" + typeToRegister.RelatedTypesToInclude + "_" + (int)typeToRegister.MemberTypesToInclude;

            return result;
        }

        private static bool IsSystemType(
            Type type)
        {
            var result = type.Namespace?.StartsWith(nameof(System), StringComparison.Ordinal) ?? false;

            return result;
        }

        private static bool IsTypeThatCanBeRegistered(
            Type type)
        {
            var result = (!RelatedTypesBlacklist.Contains(type))
                      && (!IsSystemType(type))
                      && (!type.IsClosedAnonymousType())
                      && (!type.ContainsGenericParameters);

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
                    .RegisteredTypeToRegistrationDetailsMap
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
                        // these types need to be considered for spawning additional types in to include
                        // and thus need to be processed in the queue, but they themselves cannot be registered.
                        // for example, if the type is List<List<MyModelType>>, we don't want to register it,
                        // but we want to run member inclusion to get at MyModelType.
                        if (IsTypeThatCanBeRegistered(typeToRegister.Type))
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
                        .Where(_ => _ != typeToRegister.Type)
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

            var registrationDetails = new RegistrationDetails(typeToRegister, this.serializationConfigurationType);

            this.RegisterType(registrationDetails);
        }

        private void RegisterType(
            RegistrationDetails registrationDetails)
        {
            var type = registrationDetails.TypeToRegister.Type;

            if (!IsTypeThatCanBeRegistered(type))
            {
                throw new InvalidOperationException(Invariant($"Serialization configuration {this.serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()} is attempting to register the following type which cannot be registered: {type.ToStringReadable()}."));
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