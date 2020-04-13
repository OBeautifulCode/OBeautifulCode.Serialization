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
    using System.Diagnostics.CodeAnalysis;
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
        /// The string representation of null.
        /// </summary>
        public const string NullSerializedStringValue = "null";

        /// <summary>
        /// Binding flags used in <see cref="DiscoverAllContainedAssignableTypes"/> to reflect on a type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = ObcSuppressBecause.CA1726_UsePreferredTerms_NameOfTypeOfIdentifierUsesTheTermFlags)]
        public const BindingFlags DiscoveryBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Types that will be added by default to the <see cref="RegisterTypes" />.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = ObcSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly IReadOnlyCollection<Type> InternallyRequiredTypes = new[]
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

        private static readonly HashSet<Type> DiscoverAssignableTypesBlackList =
            new HashSet<Type>(
                new[]
                {
                    typeof(string),
                    typeof(object),
                    typeof(ValueType),
                    typeof(Enum),
                    typeof(Array),
                });

        private static readonly ConcurrentDictionary<Assembly, IReadOnlyCollection<Type>> AssemblyToTypesToConsiderForRegistration =
            new ConcurrentDictionary<Assembly, IReadOnlyCollection<Type>>();

        private readonly object syncConfigure = new object();

        private bool initialized;

        /// <summary>
        /// Gets a map of the the dependent configuration type (and any ancestors) to their configured instance.
        /// </summary>
        public IReadOnlyDictionary<SerializationConfigurationType, SerializationConfigurationBase> DependentSerializationConfigurationTypeToInstanceMap { get; private set; }

        /// <summary>
        /// Gets a map of registration details keyed on type registered.
        /// </summary>
        public IReadOnlyDictionary<Type, SerializationConfigurationType> RegisteredTypeToSerializationConfigurationTypeMap => this.MutableRegisteredTypeToSerializationConfigurationTypeMap;

        /// <summary>
        /// Gets the version of <see cref="RegisteredTypeToSerializationConfigurationTypeMap" />.
        /// </summary>
        protected Dictionary<Type, SerializationConfigurationType> MutableRegisteredTypeToSerializationConfigurationTypeMap { get; } = new Dictionary<Type, SerializationConfigurationType>();

        /// <summary>
        /// Gets a list of <see cref="SerializationConfigurationBase"/>'s that are needed for the current implementation of <see cref="SerializationConfigurationBase"/>.  Optionally overrideable, DEFAULT is empty collection.
        /// </summary>
        protected abstract IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes { get; }

        /// <summary>
        /// Gets the dependent configurations that are required to be in-effect for <see cref="SerializationKind"/>-associated abstract inheritors,
        /// (e.g. BsonSerializationConfiguration) so that, in turn, their concrete inheritors (e.g. MyDomainBsonSerializationConfiguration)
        /// do not need to specify these dependencies and so that any and all serialization that utilizes such concrete inheritors will work as expected.
        /// These will be ignored for any configuration that implements <see cref="IIgnoreDefaultDependencies"/>.
        /// </summary>
        protected abstract IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes { get; }

        /// <summary>
        /// Gets a list of <see cref="Type"/>s to auto-register.
        /// Auto-registration is a convenient way to register types; it accounts for interface implementations and class inheritance when performing the registration.
        /// For interface types, all implementations will be also be registered.  For classes, all inheritors will also be registered.  These additional types do not need to be specified.
        /// </summary>
        protected virtual IReadOnlyCollection<Type> TypesToAutoRegister => new Type[0];

        /// <summary>
        /// Gets a list of <see cref="Type"/>s to auto-register with discovery.
        /// Will take these specified types and recursively detect all model objects used then treat them all as if they were specified on <see cref="TypesToAutoRegister" /> directly.
        /// </summary>
        protected virtual IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery => new Type[0];

        /// <summary>
        /// Gets a list of class <see cref="Type"/>s to register.
        /// </summary>
        protected virtual IReadOnlyCollection<Type> ClassTypesToRegister => new Type[0];

        /// <summary>
        /// Gets a list of parent class <see cref="Type"/>s to register.  These classes and all of their inheritors will be registered.  The inheritors do not need to be specified.
        /// </summary>
        protected virtual IReadOnlyCollection<Type> ClassTypesToRegisterAlongWithInheritors => new Type[0];

        /// <summary>
        /// Gets a list of interface <see cref="Type"/>s whose implementations should be registered.
        /// </summary>
        protected virtual IReadOnlyCollection<Type> InterfaceTypesToRegisterImplementationOf => new Type[0];

        /// <summary>
        /// Register the specified type and class types that are assignable to those specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>Types assignable to the types provided.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        public static IReadOnlyCollection<Type> DiscoverAllAssignableTypes(IReadOnlyCollection<Type> types)
        {
            new { types }.AsArg().Must().NotBeNull();

            var allTypesToConsiderForRegistration = GetAllTypesToConsiderForRegistration();

            var result = allTypesToConsiderForRegistration
                .Where(_ => types.Any(typeToAutoRegister => IsAssignableToOrFrom(_, typeToAutoRegister)))
                .Concat(types.Where(_ => _.IsInterface)) // add interfaces back as they were explicitly provided.
                .ToList();

            return result;
        }

        /// <summary>
        /// Gets all specified dependent configuration types, including all internal configuration types
        /// unless this this configuration type is <see cref="IIgnoreDefaultDependencies"/>.
        /// </summary>
        /// <returns>
        /// All specified dependent configuration types, including all internal configuration types
        /// unless this this configuration type is <see cref="IIgnoreDefaultDependencies"/>.
        /// </returns>
        public IReadOnlyCollection<SerializationConfigurationType> GetDependentSerializationConfigurationTypesWithDefaultsIfApplicable()
        {
            var result = this.DependentSerializationConfigurationTypes.ToList();

            if (!(this is IIgnoreDefaultDependencies))
            {
                result.AddRange(this.DefaultDependentSerializationConfigurationTypes);
            }

            return result;
        }

        /// <summary>
        /// Run configuration logic.
        /// </summary>
        /// <param name="dependentSerializationConfigurationTypeToInstanceMap">Map of dependent configuration type to configured instance.</param>
        public virtual void Initialize(
            IReadOnlyDictionary<SerializationConfigurationType, SerializationConfigurationBase> dependentSerializationConfigurationTypeToInstanceMap)
        {
            new { dependentSerializationConfigurationTypeToInstanceMap }.AsArg().Must().NotBeNull();

            if (!this.initialized)
            {
                lock (this.syncConfigure)
                {
                    if (!this.initialized)
                    {
                        // Configure dependency map.
                        this.DependentSerializationConfigurationTypeToInstanceMap = dependentSerializationConfigurationTypeToInstanceMap;
                        foreach (var dependentSerializationConfigurationTypeToInstance in dependentSerializationConfigurationTypeToInstanceMap)
                        {
                            var dependentConfigType = dependentSerializationConfigurationTypeToInstance.Key;
                            var dependentConfigInstance = dependentSerializationConfigurationTypeToInstance.Value;

                            var dependentConfigRegisteredTypes = dependentConfigInstance
                                .RegisteredTypeToSerializationConfigurationTypeMap
                                .Where(_ => _.Value == dependentConfigType).ToList();

                            foreach (var dependentConfigRegisteredType in dependentConfigRegisteredTypes)
                            {
                                var dependentTypeToRegister = dependentConfigRegisteredType.Key;

                                if (this.MutableRegisteredTypeToSerializationConfigurationTypeMap.ContainsKey(dependentTypeToRegister))
                                {
                                    var dependentConfigTypeHavingAlreadyRegisteredSpecifiedType = dependentConfigRegisteredType.Value;

                                    throw new InvalidOperationException(Invariant($"Config {this.GetType().ToStringReadable()} is attempting to register type '{dependentTypeToRegister.ToStringReadable()}' from dependent config {dependentConfigType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()} but it was already registered by {dependentConfigTypeHavingAlreadyRegisteredSpecifiedType.ConcreteSerializationConfigurationDerivativeType.ToStringReadable()}."));
                                }

                                this.MutableRegisteredTypeToSerializationConfigurationTypeMap.Add(
                                    dependentTypeToRegister,
                                    dependentConfigType);
                            }
                        }

                        // Save locals to work with.
                        var localClassTypesToRegister = this.ClassTypesToRegister ?? new List<Type>();
                        var localInterfaceTypesToRegisterImplementationOf = this.InterfaceTypesToRegisterImplementationOf ?? new List<Type>();
                        var localTypesToAutoRegisterWithDiscovery = this.TypesToAutoRegisterWithDiscovery ?? new List<Type>();
                        var localTypesToAutoRegister = this.TypesToAutoRegister ?? new List<Type>();
                        var localClassTypesToRegisterAlongWithInheritors = this.ClassTypesToRegisterAlongWithInheritors ?? new List<Type>();

                        // Basic assertions.
                        this.DependentSerializationConfigurationTypeToInstanceMap.AsArg().Must().NotBeNull();
                        localInterfaceTypesToRegisterImplementationOf.AsArg().Must().NotContainAnyNullElements();
                        localTypesToAutoRegisterWithDiscovery.AsArg().Must().NotContainAnyNullElements();
                        localTypesToAutoRegister.AsArg().Must().NotContainAnyNullElements();
                        localClassTypesToRegisterAlongWithInheritors.AsArg().Must().NotContainAnyNullElements();

                        localClassTypesToRegisterAlongWithInheritors.Select(_ => _.IsClass).AsArg(Invariant($"{nameof(this.ClassTypesToRegisterAlongWithInheritors)}.Select(_ => _.{nameof(Type.IsClass)})")).Must().Each().BeTrue();
                        localInterfaceTypesToRegisterImplementationOf.Select(_ => _.IsInterface).AsArg(Invariant($"{nameof(this.InterfaceTypesToRegisterImplementationOf)}.Select(_ => _.{nameof(Type.IsInterface)})")).Must().Each().BeTrue();

                        // Run optional initial config for the last inheritor.
                        this.InitialConfiguration();

                        // Run inheritor specific setup logic (like custom third party serializers/converters/resolvers).
                        this.InternalConfigure();

                        // Accumulate all possible types from configuration.
                        var discoveredTypes = this.DiscoverAllContainedAssignableTypes(localTypesToAutoRegisterWithDiscovery);

                        var typesToAutoRegister = new Type[0]
                            .Concat(localClassTypesToRegister)
                            .Concat(localTypesToAutoRegister)
                            .Concat(localInterfaceTypesToRegisterImplementationOf)
                            .Concat(localClassTypesToRegisterAlongWithInheritors)
                            .Concat(localTypesToAutoRegisterWithDiscovery)
                            .Concat(discoveredTypes)
                            .ToList();

                        var typesToAutoRegisterAlreadyHandledByDependentConfiguration = typesToAutoRegister.Intersect(this.RegisteredTypeToSerializationConfigurationTypeMap.Keys).ToList();
                        if (typesToAutoRegisterAlreadyHandledByDependentConfiguration.Any())
                        {
                            // TODO: what info do we want to capture here?
                            throw new DuplicateRegistrationException("Attempted to register types that are already registered", typesToAutoRegisterAlreadyHandledByDependentConfiguration);
                        }

                        var allTypesToRegister =
                            DiscoverAllAssignableTypes(typesToAutoRegister)
                                .Concat(discoveredTypes)
                                .Distinct()
                                .ToList();

                        // Register all types with inheritors to do additional configuration.
                        this.RegisterTypes(allTypesToRegister);

                        // Run optional final config for the last inheritor.
                        this.FinalConfiguration();

                        this.initialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Optional template method to be used by a specific configuration implementation at the beginning of the configuration logic.
        /// </summary>
        protected virtual void InitialConfiguration()
        {
            /* no-op - just for additional custom logic */
        }

        /// <summary>
        /// Optional to each serializer, a template method for any specific logic for the serialization implementation.
        /// </summary>
        protected virtual void InternalConfigure()
        {
            /* no-op - just for additional custom logic */
        }

        /// <summary>
        /// Optional template method to be used by a specific configuration implementation at the end of the configuration logic.
        /// </summary>
        protected virtual void FinalConfiguration()
        {
            /* no-op - just for additional custom logic */
        }

        /// <summary>
        /// Register type using specific internal conventions.
        /// </summary>
        /// <param name="type">Type to register.</param>
        protected void RegisterType(Type type)
        {
            this.RegisterTypes(new[] { type });
        }

        /// <summary>
        /// Register type using specific internal conventions.
        /// </summary>
        /// <typeparam name="T">Type to register.</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Want to use this as a generic.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Like this structure.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterType<T>()
        {
            this.RegisterTypes(new[] { typeof(T) });
        }

        /// <summary>
        /// Register types using specific internal conventions.
        /// </summary>
        /// <param name="types">Types to register.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected virtual void RegisterTypes(IReadOnlyCollection<Type> types)
        {
            foreach (var type in types ?? new Type[0])
            {
                this.MutableRegisteredTypeToSerializationConfigurationTypeMap.Add(type, this.GetType().ToSerializationConfigurationType());
            }
        }

        /// <summary>
        /// Discover all types that should considered for registration when looking for derivative types.
        /// </summary>
        /// <returns>All types that should be considered for registration.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Want this to be a method since it's running logic.")]
        private static IReadOnlyCollection<Type> GetAllTypesToConsiderForRegistration()
        {
            var assemblies = AssemblyLoader.GetLoadedAssemblies();

            foreach (var assembly in assemblies)
            {
                if (!AssemblyToTypesToConsiderForRegistration.ContainsKey(assembly))
                {
                    var typesToConsiderForThisAssembly =
                        new[] { assembly }
                            .GetTypesFromAssemblies()
                            .Where(_ =>
                                _.IsClass &&
                                (!_.IsClosedAnonymousType()) &&
                                (!DiscoverAssignableTypesBlackList.Contains(_)) && // all types will be assignable to these types, so filter them out
                                (!_.IsGenericTypeDefinition)) // can't do an IsAssignableTo check on generic type definitions
                            .ToList();

                    AssemblyToTypesToConsiderForRegistration.TryAdd(assembly, typesToConsiderForThisAssembly);
                }
            }

            var result = AssemblyToTypesToConsiderForRegistration.Values.SelectMany(_ => _).ToList();

            return result;
        }

        private static bool IsAssignableToOrFrom(
            Type type1,
            Type type2)
        {
            // note: we are PURPOSELY not using OBeautifulCode.Reflection.Recipes.TypeHelper.IsAssignableTo
            // because of performance issues related to the OBeautifulCode.Validation calls in that method.
            var result = (type1 == type2) || type1.IsAssignableFrom(type2) || type2.IsAssignableFrom(type1);

            return result;
        }

        private IReadOnlyCollection<Type> DiscoverAllContainedAssignableTypes(IReadOnlyCollection<Type> types)
        {
            var typeHashSet = new HashSet<Type>();
            var typeSeen = new HashSet<Type>();
            var typesToInspect = new HashSet<Type>(types);

            bool FilterToUsableTypes(MemberInfo memberInfo) => !memberInfo.CustomAttributes.Select(s => s.AttributeType).Contains(typeof(CompilerGeneratedAttribute));

            while (typesToInspect.Any())
            {
                var type = typesToInspect.First();
                typesToInspect.Remove(type);

                if (typeSeen.Contains(type) || this.RegisteredTypeToSerializationConfigurationTypeMap.ContainsKey(type))
                {
                    continue;
                }

                typeSeen.Add(type);

                if (type.IsGenericType)
                {
                    typesToInspect.AddRange(type.GenericTypeArguments);
                }

                if (type.IsArray)
                {
                    typesToInspect.Add(type.GetElementType());
                    continue;
                }

                if (type.Namespace?.StartsWith(nameof(System), StringComparison.Ordinal) ?? true)
                {
                    continue;
                }

                typeHashSet.Add(type);

                var assignableTypesToAdd = DiscoverAllAssignableTypes(new[] { type });

                var memberTypesToAdd = type.GetMembers(DiscoveryBindingFlags).Where(FilterToUsableTypes).SelectMany(
                    _ =>
                    {
                        if (_ is PropertyInfo propertyInfo)
                        {
                            return new[] { propertyInfo.PropertyType };
                        }
                        else if (_ is FieldInfo fieldInfo)
                        {
                            return new[] { fieldInfo.FieldType };
                        }
                        else
                        {
                            return new Type[0];
                        }
                    }).Where(_ => !_.IsGenericParameter).ToList();

                var newTypes = assignableTypesToAdd.Concat(memberTypesToAdd).ToList();

                newTypes.Distinct().ToList().ForEach(_ => typesToInspect.Add(_));
            }

            var result = typeHashSet.Where(_ => _.IsClosedNonAnonymousClassType()).ToList();

            return result;
        }
    }
}