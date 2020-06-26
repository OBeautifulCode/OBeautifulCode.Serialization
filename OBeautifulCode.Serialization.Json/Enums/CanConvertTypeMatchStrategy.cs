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
        /// The registered type is assignable to the incoming type-to-consider.
        /// </summary>
        RegisteredTypeIsAssignableToTypeToConsider,
    }
}