// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootObjectThatSerializesToStringWrapper.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    /// <summary>
    /// Wraps a root-level object that serializes to string.
    /// </summary>
    /// <typeparam name="T">The type of the object to wrap.</typeparam>
    public class RootObjectThatSerializesToStringWrapper<T> : IWrapRootObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootObjectThatSerializesToStringWrapper{T}"/> class.
        /// </summary>
        /// <param name="rootObject">The root object.</param>
        public RootObjectThatSerializesToStringWrapper(
            T rootObject)
        {
            if (rootObject == null)
            {
                throw new ArgumentNullException(nameof(rootObject));
            }

            this.RootObject = rootObject;
        }

        /// <summary>
        /// Gets the root object.
        /// </summary>
        public T RootObject { get; private set; }

        /// <inheritdoc />
        public object UntypedRootObject => this.RootObject;
    }
}
