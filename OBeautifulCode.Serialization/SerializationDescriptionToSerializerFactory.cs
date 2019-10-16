// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationDescriptionToSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    using static System.FormattableString;

    /// <summary>
    /// Registered implementation of <see cref="ISerializerFactory" /> that maps a <see cref="SerializationDescription" /> to an implementation of <see cref="ISerializeAndDeserialize" />.
    /// </summary>
    public class SerializationDescriptionToSerializerFactory : ISerializerFactory
    {
        private readonly SerializationDescription supportedSerializationDescription;
        private readonly ISerializeAndDeserialize serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationDescriptionToSerializerFactory"/> class.
        /// </summary>
        /// <param name="supportedSerializationDescription"><see cref="SerializationDescription" /> that is supported by the serializer.</param>
        /// <param name="serializer"><see cref="ISerializeAndDeserialize" /> implementation to use.</param>
        public SerializationDescriptionToSerializerFactory(SerializationDescription supportedSerializationDescription, ISerializeAndDeserialize serializer)
        {
            new { supportedSerializationDescription }.AsArg().Must().NotBeNull();
            new { serializer }.AsArg().Must().NotBeNull();

            this.supportedSerializationDescription = supportedSerializationDescription;
            this.serializer = serializer;
        }

        /// <inheritdoc />
        public ISerializeAndDeserialize BuildSerializer(SerializationDescription serializationDescription, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple, UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            new { serializationDescription }.AsArg().Must().NotBeNull();

            if (serializationDescription != this.supportedSerializationDescription)
            {
                throw new NotSupportedException(Invariant($"Supplied '{nameof(serializationDescription)}' ({serializationDescription}) does not match '{nameof(this.supportedSerializationDescription)}' ({this.supportedSerializationDescription})."));
            }

            return this.serializer;
        }
    }
}