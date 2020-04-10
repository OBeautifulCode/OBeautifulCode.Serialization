// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationDetails.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Exception for issues in <see cref="OBeautifulCode.Serialization"/>.
    /// </summary>
    public class RegistrationDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationDetails"/> class.
        /// </summary>
        /// <param name="registeringSerializationConfigurationType">The type of the <see cref="SerializationConfigurationBase"/> that performed the registration.</param>
        public RegistrationDetails(
            Type registeringSerializationConfigurationType)
        {
            this.RegisteringSerializationConfigurationType = registeringSerializationConfigurationType;
        }

        /// <summary>
        /// Gets the type that performed the registration.
        /// </summary>
        public Type RegisteringSerializationConfigurationType { get; }
    }
}