// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Recipes
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.PropertyBag;

    using static System.FormattableString;

    /// <summary>
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public sealed class SerializerFactory : ISerializerFactory
    {
        private static readonly SerializerFactory InternalInstance = new SerializerFactory();

        private readonly object sync = new object();

        private SerializerFactory()
        {
            /* no-op to make sure this can only be accessed via instance property */
        }

        /// <summary>
        /// Gets the singleton entry point to the code.
        /// </summary>
        public static ISerializerFactory Instance => InternalInstance;

        /// <inheritdoc />
        public ISerializeAndDeserialize BuildSerializer(SerializationDescription serializationDescription, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple, UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            new { serializationDescription }.AsArg().Must().NotBeNull();

            lock (this.sync)
            {
                var configurationType = serializationDescription.ConfigurationTypeRepresentation?.ResolveFromLoadedTypes(typeMatchStrategy, multipleMatchStrategy);

                switch (serializationDescription.SerializationKind)
                {
                    case SerializationKind.Bson: return new ObcBsonSerializer(configurationType, unregisteredTypeEncounteredStrategy);
                    case SerializationKind.Json: return new ObcJsonSerializer(configurationType, unregisteredTypeEncounteredStrategy);
                    case SerializationKind.PropertyBag: return new ObcPropertyBagSerializer(configurationType, unregisteredTypeEncounteredStrategy);
                    default: throw new NotSupportedException(Invariant($"{nameof(serializationDescription)} from enumeration {nameof(SerializationKind)} of {serializationDescription.SerializationKind} is not supported."));
                }
            }
        }
    }
}
