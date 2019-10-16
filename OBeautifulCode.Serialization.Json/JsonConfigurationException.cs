// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConfigurationException.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for issues configuring BSON.
    /// </summary>
    [Serializable]
    public class JsonConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationException"/> class.
        /// </summary>
        public JsonConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        public JsonConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        /// <param name="innerException">Inner exception.</param>
        public JsonConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Reading context.</param>
        protected JsonConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
