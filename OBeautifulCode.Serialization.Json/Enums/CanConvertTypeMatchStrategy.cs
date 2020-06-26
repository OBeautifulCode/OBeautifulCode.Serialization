// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanConvertTypeMatchStrategy.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using Newtonsoft.Json;

    /// <summary>
    /// The strategy to use in <see cref="JsonConverter.CanConvert(System.Type)"/> to match
    /// an incoming type-to-consider with the type that the <see cref="JsonConverter"/> is registered for.
    /// </summary>
    public enum CanConvertTypeMatchStrategy
    {
        /// <summary>
        /// The incoming type-to-consider must be equal to the registered type.
        /// </summary>
        TypeToConsiderEqualsRegisteredType,

        /// <summary>
        /// The incoming type-to-consider must be assignable to the registered type.
        /// </summary>
        TypeToConsiderIsAssignableToRegisteredType,

        /// <summary>
        /// The incoming type-to-consider must be assignable from the registered type.
        /// </summary>
        TypeToConsiderIsAssignableFromRegisteredType,

        /// <summary>
        /// The incoming type-to-consider must be assignable to OR from the registered type.
        /// </summary>
        TypeToConsiderIsAssignableToOrFromRegisteredType,
    }
}