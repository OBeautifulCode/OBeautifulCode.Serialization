﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBase.Overridable.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    public abstract partial class SerializationConfigurationBase
    {
        /// <summary>
        /// Gets the strategy to use with when attempting to serialize or deserialize using this configuration and encountering a type that has not been registered.
        /// </summary>
        public virtual UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

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
        /// Gets the serialization configuration type.
        /// </summary>
        /// <returns>
        /// The serialization configuration type.
        /// </returns>
        protected abstract SerializationConfigurationType GetSerializationConfigurationType();

        /// <summary>
        /// Builds a <see cref="TypeToRegister"/> to be used for post-initialization registration,
        /// which only occurs for closed generic types at serialization-time, because these
        /// runtime types are not discoverable during initialization.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="recursiveOriginType">The type whose recursive processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> resulted in the creation of this <see cref="TypeToRegister"/>.</param>
        /// <param name="directOriginType">The type whose processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> directly resulted in the creation of this <see cref="TypeToRegister"/>.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        /// <returns>
        /// The type to register wrapper.
        /// </returns>
        protected abstract TypeToRegister BuildTypeToRegisterForPostInitializationRegistration(
            Type type,
            Type recursiveOriginType,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude);

        /// <summary>
        /// Processes a <see cref="RegistrationDetails"/> prior to registration.
        /// </summary>
        /// <param name="registrationDetails">Details related to the registration.</param>
        /// <param name="registrationTime">The time of registration.</param>
        protected virtual void ProcessRegistrationDetailsPriorToRegistration(
            RegistrationDetails registrationDetails,
            RegistrationTime registrationTime)
        {
            /* no-op - inheritors can use this to examine the registration details prior to registration and perform inheritor-specific setup/logic (e.g. using a custom serializer for a type) */
        }

        /// <summary>
        /// Perform any final setup/logic in initialization.
        /// </summary>
        protected virtual void FinalizeInitialization()
        {
            /* no-op - inheritors can use this to wrap-up any setup/logic (e.g. in JSON we need to identify ALL types that participate in a hierarchy for the inherited type converter) */
        }
    }
}