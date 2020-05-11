// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBase.Override.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    public abstract partial class BsonSerializationConfigurationBase
    {
        private readonly object syncSetupForSerializationOperations = new object();

        // ReSharper disable once RedundantDefaultMemberInitializer
        private bool isSetupForSerializationOperations = false;

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
        public override void SetupForSerializationOperations()
        {
            if (!this.isSetupForSerializationOperations)
            {
                lock (this.syncSetupForSerializationOperations)
                {
                    if (!this.isSetupForSerializationOperations)
                    {
                        foreach (var registrationDetails in this.RegisteredTypeToRegistrationDetailsMap.Values)
                        {
                            // See notes in ObcBsonDiscriminatorConvention.  We cannot process the registered types
                            // until we are ready to serialize/de-serialize because, at that time, we know which
                            // serialization configuration we're going to be using.  For BSON, SerializationConfigurationManager
                            // guarantees that only one configuration type can used for operations (it'll throw if you
                            // try to new-up a serializer with one config type, and then another with a different config type).
                            this.ProcessTypeToRegisterForBson((TypeToRegisterForBson)registrationDetails.TypeToRegister);
                        }

                        this.isSetupForSerializationOperations = true;
                    }
                }
            }
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

            var genericTypeDefinitionTypeToRegister = (TypeToRegisterForBson)this.RegisteredTypeToRegistrationDetailsMap[genericTypeDefinition].TypeToRegister;

            var result = new TypeToRegisterForBson(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude, genericTypeDefinitionTypeToRegister.SerializerBuilderFunc, genericTypeDefinitionTypeToRegister.PropertyNameWhitelist);

            return result;
        }

        /// <inheritdoc />
        protected sealed override void ProcessRegistrationDetailsPriorToRegistration(
            RegistrationDetails registrationDetails,
            RegistrationTime registrationTime)
        {
            new { registrationDetails }.AsArg().Must().NotBeNull();
            new { registrationTime }.AsArg().Must().NotBeEqualTo(RegistrationTime.Unknown);

            // there's nothing to do if it's a generic type definition
            // the concrete types will be registered post-initialization: upon serializing
            // that's done by ObcSerializerBase recursing through the runtime types of the object
            // being serialized.  upon deserialization this is handled by ObcBsonDiscriminatorConvention.
            if (registrationDetails.TypeToRegister.Type.IsGenericTypeDefinition)
            {
                return;
            }

            if (registrationDetails.TypeToRegister is TypeToRegisterForBson typeToRegisterForBson)
            {
                // during initialization, we have to hold-up on processing the type because
                // we are not sure what the "top-level" config is yet.
                // post-initialization we should just go ahead and process the type because the
                // "top-level" config has already been established.
                // note that checking registrationDetails.SerializationConfigurationType is to avoid double processing
                // because this method will be called on the config that registered the type and all ancestors.
                // See notes in ObcBsonDiscriminatorConvention.
                // See notes in SerializationConfigurationManager.
                if ((registrationTime == RegistrationTime.PostInitialization) && (registrationDetails.SerializationConfigurationType == this.SerializationConfigurationType))
                {
                    this.ProcessTypeToRegisterForBson(typeToRegisterForBson);
                }
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForBson)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }
    }
}