// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSerializerBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// Serializer that utilizes a fully configured <see cref="SerializationConfigurationBase"/>.
    /// </summary>
    public abstract class ObcSerializerBase : ISerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSerializerBase"/> class.
        /// </summary>
        /// <param name="serializationConfigurationType">The serialization configuration type to use.</param>
        protected ObcSerializerBase(
            SerializationConfigurationType serializationConfigurationType)
        {
            if (serializationConfigurationType == null)
            {
                throw new ArgumentNullException(nameof(serializationConfigurationType));
            }

            this.SerializationConfigurationType = serializationConfigurationType;
            this.SerializationConfiguration = SerializationConfigurationManager.GetOrAddSerializationConfiguration(serializationConfigurationType);
        }

        /// <summary>
        /// Gets the serialization configuration.
        /// </summary>
        public SerializationConfigurationBase SerializationConfiguration { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType { get; }

        /// <inheritdoc />
        public abstract SerializationKind SerializationKind { get; }

        /// <inheritdoc />
        public abstract SerializerRepresentation SerializerRepresentation { get; }

        /// <inheritdoc />
        public abstract byte[] SerializeToBytes(
            object objectToSerialize);

        /// <inheritdoc />
        public abstract string SerializeToString(
            object objectToSerialize);

        /// <inheritdoc />
        public abstract T Deserialize<T>(
            string serializedString);

        /// <inheritdoc />
        public abstract object Deserialize(
            string serializedString,
            Type type);

        /// <inheritdoc />
        public abstract T Deserialize<T>(
            byte[] serializedBytes);

        /// <inheritdoc />
        public abstract object Deserialize(
            byte[] serializedBytes,
            Type type);
    }
}
