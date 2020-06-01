﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDomainScenarios.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Recipes
{
    using System;

    /// <summary>
    /// Specifies various scenarios of serializing and de-serializing in the current App Domain or a new App Domain.
    /// </summary>
    [Flags]
    public enum AppDomainScenarios
    {
        /// <summary>
        /// None (default).
        /// </summary>
        None = 0,

        /// <summary>
        /// Serialize and de-serialize in the current App Domain.
        /// </summary>
        RoundtripInCurrentAppDomain,

        /// <summary>
        /// Serialize and de-serialize in a new App Domain.
        /// </summary>
        RoundtripInNewAppDomain,

        /// <summary>
        /// Serialize in the current App Domain and de-serialize in a new App Domain.
        /// </summary>
        /// <remarks>
        /// This is specifically to test that an object can be de-serialized without first
        /// needing to be serialized - that there's no config or caching that serialization
        /// performs that de-serialization is dependent on.
        /// </remarks>
        SerializeInCurrentAppDomainAndDeserializeInNewAppDomain,

        /// <summary>
        /// Serialize in a new App Domain and de-serialize in a new, but different App Domain.
        /// </summary>
        /// <remarks>
        /// This is specifically to test that an object can be de-serialized without first
        /// needing to be serialized - that there's no config or caching that serialization
        /// performs that de-serialization is dependent on.
        /// </remarks>
        SerializeInNewAppDomainAndDeserializeInNewAppDomain,
    }
}
