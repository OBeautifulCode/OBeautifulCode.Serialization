// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    using static System.FormattableString;

    /// <summary>
    /// Default implementation of <see cref="ISerializerFactory" />.
    /// </summary>
    public sealed class PropertyBagSerializerFactory : ISerializerFactory
    {
        private static readonly PropertyBagSerializerFactory InternalInstance = new PropertyBagSerializerFactory();

        private readonly object sync = new object();

        private PropertyBagSerializerFactory()
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
                    case SerializationKind.PropertyBag: return new ObcPropertyBagSerializer(configurationType, unregisteredTypeEncounteredStrategy);
                    default: throw new NotSupportedException(Invariant($"{nameof(serializationDescription)} from enumeration {nameof(SerializationKind)} of {serializationDescription.SerializationKind} is not supported."));
                }
            }
        }
    }
}
