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
        protected sealed override IReadOnlyDictionary<Type, object> TypesPermittedToHaveUnregisteredMembers => this.typesWithConverters;

        /// <inheritdoc />
        protected sealed override SerializationConfigurationType BuildSerializationConfigurationType()
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

            var genericTypeDefinitionTypeToRegister = (TypeToRegisterForJson)this.RegisteredTypeToRegistrationDetailsMap[genericTypeDefinition].TypeToRegister;

            var result = new TypeToRegisterForJson(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude, genericTypeDefinitionTypeToRegister.JsonConverterBuilder, genericTypeDefinitionTypeToRegister.KeyInDictionaryStringSerializer);

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

            if (registrationDetails.TypeToRegister is TypeToRegisterForJson typeToRegisterForJson)
            {
                this.ProcessTypeToRegisterForJson(typeToRegisterForJson);

                // During initialization, we wait until FinalizeInitialization() so that we have ALL of
                // the registered types on hand to determine which ones are non-abstract base classes
                // (which is rare/a design smell).  The only way to make this determination is to isolate the
                // non-abstract classes and test whether any other registered type is assignable to those classes.
                // Post-initialization, we are looking at a closed generic type and should make the same determination.
                // There is potentially the case where a non-abstract closed generic class shows up first,
                // we determine it is not a base class because it has no inheritors,
                // and subsequently another closed generic class shows up that inherits from the first type.
                // Should be rare; cross that bridge when we get there.
                if (registrationTime == RegistrationTime.PostInitialization)
                {
                    this.AddNonAbstractBaseClassTypes(new[] { registrationDetails.TypeToRegister.Type });
                }
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForJson)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }

        /// <inheritdoc />
        protected sealed override void FinalizeInitialization()
        {
            var registeredTypes = this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList();

            // See note in ProcessRegistrationDetailsPriorToRegistration()
            this.AddNonAbstractBaseClassTypes(registeredTypes);
        }

        private void AddNonAbstractBaseClassTypes(
            IReadOnlyCollection<Type> typesToInspect)
        {
            var registeredTypes = this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList();

            var identifiedNonAbstractBaseClassTypes =
                typesToInspect
                    .Where(_ => IsNonAbstractBaseClassType(_, registeredTypes))
                    .ToList();

            foreach (var identifiedNonAbstractBaseClassType in identifiedNonAbstractBaseClassTypes)
            {
                this.nonAbstractBaseClassTypes.TryAdd(identifiedNonAbstractBaseClassType, null);
            }
        }
    }
}