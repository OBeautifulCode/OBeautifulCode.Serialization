﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationBase.Override.cs" company="OBeautifulCode">
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

    public abstract partial class PropertyBagSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[]
        {
            typeof(InternallyRequiredTypesToRegisterPropertyBagSerializationConfiguration).ToPropertyBagSerializationConfigurationType(),
        };

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => this.DependentPropertyBagSerializationConfigurationTypes;

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<TypeToRegister> TypesToRegister => this.TypesToRegisterForPropertyBag;

        /// <inheritdoc />
        protected sealed override IReadOnlyDictionary<Type, object> TypesPermittedToHaveUnregisteredMembers => this.typesWithCustomSerializers;

        /// <inheritdoc />
        protected sealed override SerializationConfigurationType BuildSerializationConfigurationType()
        {
            var result = this.GetType().ToPropertyBagSerializationConfigurationType();

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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (recursiveOriginType == null)
            {
                throw new ArgumentNullException(nameof(recursiveOriginType));
            }

            if (directOriginType == null)
            {
                throw new ArgumentNullException(nameof(directOriginType));
            }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            var genericTypeDefinitionTypeToRegister = (TypeToRegisterForPropertyBag)this.RegisteredTypeToRegistrationDetailsMap[genericTypeDefinition].TypeToRegister;

            var result = new TypeToRegisterForPropertyBag(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude, genericTypeDefinitionTypeToRegister.StringSerializerBuilderFunc);

            return result;
        }

        /// <inheritdoc />
        protected sealed override void ProcessRegistrationDetailsPriorToRegistration(
            RegistrationDetails registrationDetails,
            RegistrationTime registrationTime)
        {
            if (registrationDetails == null)
            {
                throw new ArgumentNullException(nameof(registrationDetails));
            }

            if (registrationDetails.TypeToRegister is TypeToRegisterForPropertyBag typeToRegisterForPropertyBag)
            {
                // TypeToRegisterForPropertyBag throws NotSupportedException on generic type definitions
                // if (registrationDetails.TypeToRegister.Type.IsGenericTypeDefinition)
                // {
                //     throw new NotSupportedException(Invariant($"Registering generic type definitions is not supported in {nameof(PropertyBagSerializationConfigurationBase)}."));
                // }
                this.ProcessTypeToRegisterForPropertyBag(typeToRegisterForPropertyBag);
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForPropertyBag)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }
    }
}