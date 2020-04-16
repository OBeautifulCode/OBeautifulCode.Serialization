// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationDetails.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Details about the registration of a type.
    /// </summary>
    public class RegistrationDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationDetails"/> class.
        /// </summary>
        /// <param name="typeToRegister">The type to register.</param>
        /// <param name="serializationConfigurationType">The type of the registering serialization configuration.</param>
        public RegistrationDetails(
            TypeToRegister typeToRegister,
            SerializationConfigurationType serializationConfigurationType)
        {
            new { typeToRegister }.AsArg().Must().NotBeNull();
            new { serializationConfigurationType }.AsArg().Must().NotBeNull();

            this.TypeToRegister = typeToRegister;
            this.SerializationConfigurationType = serializationConfigurationType;
        }

        /// <summary>
        /// Gets the type to register.
        /// </summary>
        public TypeToRegister TypeToRegister { get; }

        /// <summary>
        /// Gets the type of the registering serialization configuration.
        /// </summary>
        public SerializationConfigurationType SerializationConfigurationType { get; }
    }
}
