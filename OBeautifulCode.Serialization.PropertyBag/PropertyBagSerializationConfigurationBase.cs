// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;

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

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<TypeToRegister> TypesToRegister => this.TypesToRegisterForPropertyBag;

        /// <summary>
        /// Gets the types to register for property bag serialization.
        /// </summary>
        protected virtual IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag { get; } = new TypeToRegisterForPropertyBag[0];

        /// <inheritdoc />
        protected sealed override void ProcessRegistrationDetailsPriorToRegistration(RegistrationDetails registrationDetails)
        {
            if (registrationDetails.TypeToRegister is TypeToRegisterForPropertyBag typeToRegisterForPropertyBag)
            {
                var type = typeToRegisterForPropertyBag.Type;

                var stringSerializerBuilderFunc = typeToRegisterForPropertyBag.StringSerializerBuilderFunc;

                if (stringSerializerBuilderFunc != null)
                {
                    this.TypeToSerializerMap.Add(type, stringSerializerBuilderFunc());
                }
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForPropertyBag)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }

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