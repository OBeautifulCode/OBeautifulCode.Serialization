﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfiguredSerializerBase.cs" company="OBeautifulCode">
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
    /// Base serializer to handle activation of configuration type.
    /// </summary>
    public abstract class ConfiguredSerializerBase : ISerializeAndDeserialize
    {
        /// <summary>
        /// Strategy on how to deal with unregistered types.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Prefer field here.")]
        #pragma warning disable SA1401 // Fields should be private
        protected readonly UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy;
        #pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// The initialized configuration provided or appropriate null implementation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "It is not mutated.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Prefer field here.")]
        #pragma warning disable SA1401 // Fields should be private
        protected readonly SerializationConfigurationBase configuration;
        #pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredSerializerBase"/> class.
        /// </summary>
        /// <param name="serializationConfigurationType">Configuration type to use.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; if the type is a <see cref="IImplementNullObjectPattern" /> and value is default then <see cref="UnregisteredTypeEncounteredStrategy.Throw" /> is used.</param>
        protected ConfiguredSerializerBase(
            SerializationConfigurationType serializationConfigurationType,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy)
        {
            new { serializationConfigurationType }.AsArg().Must().NotBeNull();

            if (unregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Default)
            {
                unregisteredTypeEncounteredStrategy =
                    serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(IImplementNullObjectPattern))
                        ? UnregisteredTypeEncounteredStrategy.Throw
                        : UnregisteredTypeEncounteredStrategy.Attempt;
            }

            this.unregisteredTypeEncounteredStrategy = unregisteredTypeEncounteredStrategy;

            this.SerializationConfigurationType = serializationConfigurationType;
            this.configuration = SerializationConfigurationManager.ConfigureWithReturn<SerializationConfigurationBase>(this.SerializationConfigurationType);
        }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType { get; private set; }

        /// <inheritdoc />
        public abstract SerializationKind SerializationKind { get; }

        /// <inheritdoc />
        public abstract byte[] SerializeToBytes(object objectToSerialize);

        /// <inheritdoc />
        public abstract string SerializeToString(object objectToSerialize);

        /// <inheritdoc />
        public abstract T Deserialize<T>(string serializedString);

        /// <inheritdoc />
        public abstract object Deserialize(string serializedString, Type type);

        /// <inheritdoc />
        public abstract T Deserialize<T>(byte[] serializedBytes);

        /// <inheritdoc />
        public abstract object Deserialize(byte[] serializedBytes, Type type);

        /// <summary>
        /// Throw an <see cref="UnregisteredTypeAttemptException" /> if appropriate.
        /// </summary>
        /// <param name="type">Type to check.</param>
        protected void ThrowOnUnregisteredTypeIfAppropriate(Type type)
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
                // this is for lists, dictionaries, and such.
                foreach (var genericArgumentType in type.GenericTypeArguments)
                {
                    this.ThrowOnUnregisteredTypeIfAppropriate(genericArgumentType);
                }
            }
            else
            {
                if (this.unregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Throw)
                {
                    if (!this.configuration.RegisteredTypeToSerializationConfigurationTypeMap.ContainsKey(type))
                    {
                        throw new UnregisteredTypeAttemptException(Invariant($"Attempted to perform operation on unregistered type '{type.FullName}'"), type);
                    }
                }
            }
        }
    }
}
