﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationBase.Override.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
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
        protected sealed override void ProcessRegistrationDetailsPriorToRegistration(
            RegistrationDetails registrationDetails)
        {
            new { registrationDetails }.AsArg().Must().NotBeNull();

            if (registrationDetails.TypeToRegister is TypeToRegisterForPropertyBag typeToRegisterForPropertyBag)
            {
                this.ProcessTypeToRegisterForPropertyBag(typeToRegisterForPropertyBag);
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForPropertyBag)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }
    }
}