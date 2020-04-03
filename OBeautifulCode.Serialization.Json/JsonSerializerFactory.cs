// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    using static System.FormattableString;

    /// <summary>
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public sealed class JsonSerializerFactory : ISerializerFactory
    {
        private static readonly JsonSerializerFactory InternalInstance = new JsonSerializerFactory();

        private readonly object sync = new object();

        private JsonSerializerFactory()
        {
            /* no-op to make sure this can only be accessed via instance property */
        }

        /// <summary>
        /// Gets the singleton entry point to the code.
        /// </summary>
        public static ISerializerFactory Instance => InternalInstance;

        /// <inheritdoc />
        public ISerializeAndDeserialize BuildSerializer(SerializerDescription serializerDescription, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple, UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            new { serializerDescription }.AsArg().Must().NotBeNull();

            lock (this.sync)
            {
                var configurationType = serializerDescription.ConfigurationTypeRepresentation?.ResolveFromLoadedTypes(typeMatchStrategy, multipleMatchStrategy);

                switch (serializerDescription.SerializationKind)
                {
                    case SerializationKind.Json: return new ObcJsonSerializer(configurationType, unregisteredTypeEncounteredStrategy);
                    default: throw new NotSupportedException(Invariant($"{nameof(serializerDescription)} from enumeration {nameof(SerializationKind)} of {serializerDescription.SerializationKind} is not supported."));
                }
            }
        }
    }
}
