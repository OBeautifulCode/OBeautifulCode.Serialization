// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationException.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for issues configuring JSON.
    /// </summary>
    [Serializable]
    public class JsonSerializationConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationConfigurationException"/> class.
        /// </summary>
        public JsonSerializationConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        public JsonSerializationConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        /// <param name="innerException">Inner exception.</param>
        public JsonSerializationConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationConfigurationException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Reading context.</param>
        protected JsonSerializationConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
