// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationTime.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Specifies the time when a registration occurs.
    /// </summary>
    public enum RegistrationTime
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Registration is happening during initialization.
        /// </summary>
        Initialization,

        /// <summary>
        /// Registration is happening post-initialization.
        /// </summary>
        PostInitialization,
    }
}
