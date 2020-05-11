// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnregisteredTypeEncounteredStrategy.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Enumeration of options on how to deal with an attempted operation on an object where the type has not been registered.
    /// </summary>
    public enum UnregisteredTypeEncounteredStrategy
    {
        /// <summary>
        /// Attempt an operation on an object without prior registration.
        /// </summary>
        Attempt,

        /// <summary>
        /// Throw if an operation is attempted on an object without prior registration.
        /// </summary>
        Throw,
    }
}