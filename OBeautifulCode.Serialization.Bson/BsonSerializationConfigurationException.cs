// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationException.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for issues configuring BSON.
    /// </summary>
    [Serializable]
    public class BsonSerializationConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializationConfigurationException"/> class.
        /// </summary>
        public BsonSerializationConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializationConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        public BsonSerializationConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializationConfigurationException"/> class.
        /// </summary>
        /// <param name="message">Message for exception.</param>
        /// <param name="innerException">Inner exception.</param>
        public BsonSerializationConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializationConfigurationException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Reading context.</param>
        protected BsonSerializationConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
