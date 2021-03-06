﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBase.Override.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    public abstract partial class BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[]
        {
            typeof(InternallyRequiredTypesToRegisterBsonSerializationConfiguration).ToBsonSerializationConfigurationType(),
            typeof(NetDrawingBsonSerializationConfiguration).ToBsonSerializationConfigurationType(),
        };

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => this.DependentBsonSerializationConfigurationTypes;

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<TypeToRegister> TypesToRegister => this.TypesToRegisterForBson;

        /// <inheritdoc />
        protected sealed override IReadOnlyDictionary<Type, object> TypesPermittedToHaveUnregisteredMembers => this.typesWithCustomSerializerOrPropertyNamesWhitelist;

        /// <inheritdoc />
        protected sealed override SerializationConfigurationType BuildSerializationConfigurationType()
        {
            var result = this.GetType().ToBsonSerializationConfigurationType();

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

            var genericTypeDefinitionTypeToRegister = (TypeToRegisterForBson)this.RegisteredTypeToRegistrationDetailsMap[genericTypeDefinition].TypeToRegister;

            var result = new TypeToRegisterForBson(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude, genericTypeDefinitionTypeToRegister.BsonSerializerBuilder, genericTypeDefinitionTypeToRegister.PropertyNameWhitelist);

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

            if (registrationTime == RegistrationTime.Unknown)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(registrationTime)}' == '{RegistrationTime.Unknown}'"), (Exception)null);
            }

            if (registrationDetails.TypeToRegister is TypeToRegisterForBson typeToRegisterForBson)
            {
                // There's nothing to do if it's a generic type definition.
                // The closed generic types will be registered post-initialization.
                // Upon serializing, the serializer will call this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate,
                // which recurses through the runtime types of the object being serialized
                // and registers any unregistered closed generic types it encounters.
                // Upon deserialization this is handled by ObcBsonDiscriminatorConvention.
                if (typeToRegisterForBson.Type.IsGenericTypeDefinition)
                {
                    return;
                }

                this.ProcessTypeToRegisterForBson(typeToRegisterForBson, registrationDetails.SerializationConfigurationType);
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForBson)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }
    }
}