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

                // During initialization, we wait until FinalizeInitialization() so that we have ALL
                // of the registered types on hand, to determine which ones participate in a hierarchy.
                // This is required because ParticipatesInHierarchy() requires the whole set of registered
                // types (and specifically to look for a non-abstract class that has inheritors, which
                // is rare/a design smell).  Post-initialization, we are looking at a closed generic type
                // and should determine if it participates in a hierarchy.  There is potentially the case
                // where a non-abstract closed generic class shows up first, we determine it does not
                // participate in a hierarchy because it has no inheritors, and subsequently another
                // closed generic class shows up that inherits from the first type.  Should be rare;
                // cross that bridge when we get there.
                if (registrationTime == RegistrationTime.PostInitialization)
                {
                    this.AddHierarchyParticipatingTypes(new[] { registrationDetails.TypeToRegister.Type });
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

            this.AddHierarchyParticipatingTypes(registeredTypes);
        }

        private void AddHierarchyParticipatingTypes(
            IReadOnlyCollection<Type> typesToInspect)
        {
            var registeredTypes = this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList();

            var hierarchyTypes =
                typesToInspect
                    .Except(this.typesWithConverters.Keys)
                    .Where(_ => ParticipatesInHierarchy(_, registeredTypes))
                    .ToList();

            foreach (var hierarchyType in hierarchyTypes)
            {
                this.hierarchyParticipatingTypes.TryAdd(hierarchyType, null);
            }
        }
    }
}