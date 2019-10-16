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
        /// <param name="registeringType">The type that performed the registration.</param>
        public RegistrationDetails(Type registeringType)
        {
            this.RegisteringType = registeringType;
        }

        /// <summary>
        /// Gets the type that performed the registration.
        /// </summary>
        public Type RegisteringType { get; private set; }
    }
}