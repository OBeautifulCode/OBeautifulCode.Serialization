// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerDescriptionToSerializerFactory.cs" company="OBeautifulCode">
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
    /// Registered implementation of <see cref="ISerializerFactory" /> that maps a <see cref="SerializerDescription" /> to an implementation of <see cref="ISerializeAndDeserialize" />.
    /// </summary>
    public class SerializerDescriptionToSerializerFactory : ISerializerFactory
    {
        private readonly SerializerDescription supportedSerializerDescription;
        private readonly ISerializeAndDeserialize serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializerDescriptionToSerializerFactory"/> class.
        /// </summary>
        /// <param name="supportedSerializerDescription"><see cref="SerializerDescription" /> that is supported by the serializer.</param>
        /// <param name="serializer"><see cref="ISerializeAndDeserialize" /> implementation to use.</param>
        public SerializerDescriptionToSerializerFactory(SerializerDescription supportedSerializerDescription, ISerializeAndDeserialize serializer)
        {
            new { supportedSerializerDescription }.AsArg().Must().NotBeNull();
            new { serializer }.AsArg().Must().NotBeNull();

            this.supportedSerializerDescription = supportedSerializerDescription;
            this.serializer = serializer;
        }

        /// <inheritdoc />
        public ISerializeAndDeserialize BuildSerializer(SerializerDescription serializerDescription, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple, UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            new { serializerDescription }.AsArg().Must().NotBeNull();

            if (serializerDescription != this.supportedSerializerDescription)
            {
                throw new NotSupportedException(Invariant($"Supplied '{nameof(serializerDescription)}' ({serializerDescription}) does not match '{nameof(this.supportedSerializerDescription)}' ({this.supportedSerializerDescription})."));
            }

            return this.serializer;
        }
    }
}