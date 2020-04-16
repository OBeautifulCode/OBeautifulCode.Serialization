// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Collection.Recipes;

    using static System.FormattableString;

#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable SA1202 // Public members should come before protected members
#pragma warning disable SA1203 // Constant fields should appear before non-constant fields
#pragma warning disable SA1204 // Static members should appear before non-static members

    /// <summary>
    /// Base class to use for creating a <see cref="ObcPropertyBagSerializer" /> configuration.
    /// </summary>
    public abstract class PropertyBagSerializationConfigurationBase : SerializationConfigurationBase
    {
        /// <summary>
        /// Gets the map of type to specific serializer.
        /// </summary>
        private Dictionary<Type, IStringSerializeAndDeserialize> TypeToSerializerMap { get; } = new Dictionary<Type, IStringSerializeAndDeserialize>();

        /// <inheritdoc />
        protected override void RegisterTypes(IReadOnlyCollection<Type> types)
        {
            foreach (var type in types ?? new Type[0])
            {
                this.MutableRegisteredTypeToSerializationConfigurationTypeMap.Add(type, this.GetType().ToPropertyBagSerializationConfigurationType());
            }
        }

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[]
        {
            typeof(InternallyRequiredTypesWithDiscoveryPropertyBagSerializationConfiguration).ToPropertyBagSerializationConfigurationType(),
        };

        /// <summary>
        /// Gets the <see cref="PropertyBagSerializationConfigurationBase"/>s that are needed for the current implementation of <see cref="PropertyBagSerializationConfigurationBase"/>.  Optionally overrideable, DEFAULT is empty collection.
        /// </summary>
        protected virtual IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new PropertyBagSerializationConfigurationType[0];

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => this.DependentPropertyBagSerializationConfigurationTypes;

        /// <summary>
        /// Gets the registered serializer set to use.
        /// </summary>
        protected virtual IList<StringSerializerForTypes> TypesToRegisterWithStringSerializers { get; } = new List<StringSerializerForTypes>();

        /// <summary>
        /// Gets the key value delimiter to use for string serialization of the property bag.
        /// </summary>
        public virtual string StringSerializationKeyValueDelimiter { get; } = ObcDictionaryStringStringSerializer.DefaultKeyValueDelimiter;

        /// <summary>
        /// Gets the line delimiter to use for string serialization of the property bag.
        /// </summary>
        public virtual string StringSerializationLineDelimiter { get; } = ObcDictionaryStringStringSerializer.DefaultLineDelimiter;

        /// <summary>
        /// Gets the null value encoding to use for string serialization of the property bag.
        /// </summary>
        public virtual string StringSerializationNullValueEncoding { get; } = ObcDictionaryStringStringSerializer.DefaultNullValueEncoding;

        public override TypeToRegister BuildSpecificTypeToRegister(Type type)
        {
            return type.ToTypeToRegisterForPropertyBag();
        }

        /// <inheritdoc />
        protected sealed override void InternalConfigure()
        {
            var dependentConfigTypes = new List<SerializationConfigurationType>(this.DependentSerializationConfigurationTypesWithDefaultsIfApplicable.Reverse());

            while (dependentConfigTypes.Any())
            {
                var type = dependentConfigTypes.Last();
                dependentConfigTypes.RemoveAt(dependentConfigTypes.Count - 1);

                var dependentConfig = (PropertyBagSerializationConfigurationBase)this.DependentSerializationConfigurationTypeToInstanceMap[type];
                dependentConfigTypes.AddRange(dependentConfig.DependentSerializationConfigurationTypesWithDefaultsIfApplicable);

                this.ProcessSerializer(dependentConfig.TypesToRegisterWithStringSerializers, false);
            }

            var serializers = (this.SerializersToRegister ?? new StringSerializerForTypes[0]).ToList();
            var handledTypes = this.ProcessSerializer(serializers);

            foreach (var handledType in handledTypes)
            {
                this.MutableRegisteredTypeToSerializationConfigurationTypeMap.Add(handledType, this.GetType().ToPropertyBagSerializationConfigurationType());
            }
        }

        private IReadOnlyCollection<Type> ProcessSerializer(IList<StringSerializerForTypes> registeredSerializers, bool checkForAlreadyRegistered = true)
        {
            var handledTypes = registeredSerializers.SelectMany(_ => _.HandledTypes).ToList();

            if (checkForAlreadyRegistered && this.RegisteredTypeToSerializationConfigurationTypeMap.Keys.Intersect(handledTypes).Any())
            {
                throw new DuplicateRegistrationException(
                    Invariant($"Trying to register one or more types via {nameof(this.SerializersToRegister)} processing, but one is already registered."),
                    handledTypes);
            }

            this.TypesToRegisterWithStringSerializers.AddRange(registeredSerializers);

            foreach (var registeredSerializer in registeredSerializers)
            {
                foreach (var handledType in registeredSerializer.HandledTypes)
                {
                    if (this.TypeToSerializerMap.ContainsKey(handledType))
                    {
                        if (checkForAlreadyRegistered)
                        {
                            throw new DuplicateRegistrationException(
                                Invariant($"Type {handledType} is already registered."),
                                new[] { handledType });
                        }
                    }
                    else
                    {
                        this.TypeToSerializerMap.Add(handledType, registeredSerializer.SerializerBuilderFunc());
                    }
                }
            }

            return handledTypes;
        }

        /// <summary>
        /// Gets the optional serializers to add.
        /// </summary>
        protected virtual IReadOnlyCollection<StringSerializerForTypes> SerializersToRegister => new List<StringSerializerForTypes>();

        /// <summary>
        /// Builds a map of type to serializer.
        /// </summary>
        /// <returns>Map of type to specific serializer.</returns>
        public IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> BuildConfiguredTypeToSerializerMap()
        {
            return this.TypeToSerializerMap;
        }
    }

    /// <summary>
    /// Strategy on dealing with collisions in the <see cref="PropertyBagSerializationConfigurationBase" /> logic.
    /// </summary>
    public enum TypeSerializationRegistrationCollisionStrategy
    {
        /// <summary>
        /// Throw an exception.
        /// </summary>
        Throw,

        /// <summary>
        /// First one registered is the one used; internal custom followed by in order run through dependents.
        /// </summary>
        FirstWins,

        /// <summary>
        /// Last one registered is the one used; internal custom followed by in order run through dependents.
        /// </summary>
        LastWins,
    }

#pragma warning restore SA1201 // Elements should appear in the correct order
#pragma warning restore SA1202 // Public members should come before protected members
#pragma warning restore SA1203 // Constant fields should appear before non-constant fields
#pragma warning restore SA1204 // Static members should appear before non-static members
}