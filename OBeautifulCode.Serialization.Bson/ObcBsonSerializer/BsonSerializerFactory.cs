// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    using static System.FormattableString;

    /// <summary>
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public sealed class BsonSerializerFactory : ISerializerFactory
    {
        private static readonly BsonSerializerFactory InternalInstance = new BsonSerializerFactory();

        private readonly object sync = new object();

        private BsonSerializerFactory()
        {
            /* no-op to make sure this can only be accessed via instance property */
        }

        /// <summary>
        /// Gets the singleton entry point to the code.
        /// </summary>
        public static ISerializerFactory Instance => InternalInstance;

        /// <inheritdoc />
        public ISerializeAndDeserialize BuildSerializer(
            SerializerDescription serializerDescription,
            AssemblyMatchStrategy assemblyMatchStrategy = AssemblyMatchStrategy.AnySingleVersion,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            new { serializerDescription }.AsArg().Must().NotBeNull();

            lock (this.sync)
            {
                var configurationType = serializerDescription.ConfigurationTypeRepresentation?.ResolveFromLoadedTypes(assemblyMatchStrategy, throwIfCannotResolve: true);

                switch (serializerDescription.SerializationKind)
                {
                    case SerializationKind.Bson:
                        return new ObcBsonSerializer(configurationType?.ToBsonSerializationConfigurationType(), unregisteredTypeEncounteredStrategy);
                    default:
                        throw new NotSupportedException(Invariant($"{nameof(serializerDescription)} from enumeration {nameof(SerializationKind)} of {serializerDescription.SerializationKind} is not supported."));
                }
            }
        }
    }
}
