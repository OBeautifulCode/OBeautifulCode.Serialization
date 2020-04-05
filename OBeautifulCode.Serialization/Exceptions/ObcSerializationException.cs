// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSerializationException.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for issues in <see cref="OBeautifulCode.Serialization"/>.
    /// </summary>
    [Serializable]
    public class ObcSerializationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSerializationException"/> class.
        /// </summary>
        public ObcSerializationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSerializationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        public ObcSerializationException(
            string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSerializationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        /// <param name="innerException">Inner exception.</param>
        public ObcSerializationException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSerializationException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Reading context.</param>
        protected ObcSerializationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}