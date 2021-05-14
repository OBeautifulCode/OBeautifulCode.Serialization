// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectWrapper{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Type;

    /// <summary>
    /// Wraps an object declared as type <see cref="object"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to wrap.</typeparam>
    public partial class ObjectWrapper<T> : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWrapper{T}"/> class.
        /// </summary>
        /// <param name="v">The object to wrap.</param>
        public ObjectWrapper(
            T v)
        {
            if (v == null)
            {
                throw new ArgumentNullException(nameof(v));
            }

            this.V = v;
        }

        /// <summary>
        /// Gets the wrapped object.
        /// </summary>
        public T V { get; private set; }
    }
}