// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSerializerBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializer that utilizes a fully configured <see cref="SerializationConfigurationBase"/>.
    /// </summary>
    public abstract class ObcSerializerBase : ISerializeAndDeserialize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSerializerBase"/> class.
        /// </summary>
        /// <param name="serializationConfigurationType">The serialization configuration type to use.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">
        /// Strategy of how to handle a type that has never been registered.
        /// If <see cref="UnregisteredTypeEncounteredStrategy.Default"/>:
        ///     If type is an <see cref="IImplementNullObjectPattern" /> then the <see cref="UnregisteredTypeEncounteredStrategy.Default"/> strategy will be used.
        ///     Otherwise, the <see cref="UnregisteredTypeEncounteredStrategy.Throw" /> strategy is used.
        /// </param>
        protected ObcSerializerBase(
            SerializationConfigurationType serializationConfigurationType,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy)
        {
            new { serializationConfigurationType }.AsArg().Must().NotBeNull();

            if (unregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Default)
            {
                unregisteredTypeEncounteredStrategy =
                    serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(IImplementNullObjectPattern))
                        ? UnregisteredTypeEncounteredStrategy.Attempt
                        : UnregisteredTypeEncounteredStrategy.Throw;
            }

            this.SerializationConfigurationType = serializationConfigurationType;
            this.UnregisteredTypeEncounteredStrategy = unregisteredTypeEncounteredStrategy;
            this.SerializationConfiguration = SerializationConfigurationManager.GetOrAddSerializationConfiguration(serializationConfigurationType);
        }

        /// <summary>
        /// Gets the strategy of how to handle a type that has never been registered.
        /// </summary>
        public UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy { get; }

        /// <summary>
        /// Gets the serialization configuration.
        /// </summary>
        public SerializationConfigurationBase SerializationConfiguration { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType { get; }

        /// <inheritdoc />
        public abstract SerializationKind SerializationKind { get; }

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

        /// <summary>
        /// Throw an <see cref="UnregisteredTypeAttemptException" /> if appropriate.
        /// </summary>
        /// <param name="type">Type to check.</param>
        protected void ThrowOnUnregisteredTypeIfAppropriate(
            Type type)
        {
            if (type == null)
            {
                // this must be supported for serializing null.
                return;
            }
            else if (type.IsArray)
            {
                this.ThrowOnUnregisteredTypeIfAppropriate(type.GetElementType());
            }
            else if (type.IsGenericType && (type.Namespace?.StartsWith(nameof(System), StringComparison.Ordinal) ?? false))
            {
                // this is for lists, dictionaries, nullable, and such.
                foreach (var genericArgumentType in type.GenericTypeArguments)
                {
                    this.ThrowOnUnregisteredTypeIfAppropriate(genericArgumentType);
                }
            }
            else
            {
                if (this.UnregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Throw)
                {
                    if (!this.SerializationConfiguration.RegisteredTypeToRegistrationDetailsMap.ContainsKey(type))
                    {
                        throw new UnregisteredTypeAttemptException(Invariant($"Attempted to perform operation on unregistered type '{type.ToStringReadable()}'."), type);
                    }
                }
            }
        }
    }
}
