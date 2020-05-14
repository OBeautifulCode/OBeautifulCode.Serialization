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
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    public abstract partial class JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<TypeToRegister> TypesToRegister => this.TypesToRegisterForJson;

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[]
        {
            typeof(InternallyRequiredTypesToRegisterJsonSerializationConfiguration).ToJsonSerializationConfigurationType(),
        };

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => this.DependentJsonSerializationConfigurationTypes;

        /// <inheritdoc />
        protected sealed override SerializationConfigurationType GetSerializationConfigurationType()
        {
            var result = this.GetType().ToJsonSerializationConfigurationType();

            return result;
        }

        /// <inheritdoc />
        protected sealed override TypeToRegister BuildTypeToRegisterForPostInitializationRegistration(
            Type type,
            Type recursiveOriginType,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();

            var genericTypeDefinitionTypeToRegister = (TypeToRegisterForJson)this.RegisteredTypeToRegistrationDetailsMap[genericTypeDefinition].TypeToRegister;

            var result = new TypeToRegisterForJson(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude, genericTypeDefinitionTypeToRegister.JsonConverterBuilder);

            return result;
        }

        /// <inheritdoc />
        protected sealed override void ProcessRegistrationDetailsPriorToRegistration(
            RegistrationDetails registrationDetails,
            RegistrationTime registrationTime)
        {
            new { registrationDetails }.AsArg().Must().NotBeNull();

            if (registrationDetails.TypeToRegister is TypeToRegisterForJson typeToRegisterForJson)
            {
                // There's nothing to do if it's a generic type definition.
                // The closed generic types will be registered post-initialization.
                // Upon serializing, the serializer will call this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate,
                // which recurses through the runtime types of the object being serialized
                // and registers any unregistered closed generic types it encounters.
                // Upon deserialization this is handled by ObcBsonDiscriminatorConvention.
                if (registrationDetails.TypeToRegister.Type.IsGenericTypeDefinition)
                {
                    return;
                }

                this.ProcessTypeToRegisterForJson(typeToRegisterForJson);
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForJson)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }

        /// <inheritdoc />
        protected sealed override void FinalizeInitialization()
        {
            this.AddHierarchyParticipatingTypes(this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList());
        }
    }
}