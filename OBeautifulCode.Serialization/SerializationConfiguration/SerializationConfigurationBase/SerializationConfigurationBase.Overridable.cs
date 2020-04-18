// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBase.Overridable.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System.Collections.Generic;

    public abstract partial class SerializationConfigurationBase
    {
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
    }
}