// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWrapRootObject.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    /// <summary>
    /// Wraps a root-level object.
    /// </summary>
    public interface IWrapRootObject
    {
        /// <summary>
        /// Gets the untyped root object being wrapped.
        /// </summary>
        object UntypedRootObject { get; }
    }
}
