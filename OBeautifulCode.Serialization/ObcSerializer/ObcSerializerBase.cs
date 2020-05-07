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

            // only closed types are supported
            new { type.ContainsGenericParameters }.AsArg().Must().BeFalse();

            this.InternalThrowOnUnregisteredTypeIfAppropriate(type, type);
        }

        private void InternalThrowOnUnregisteredTypeIfAppropriate(
            Type originalType,
            Type typeToValidate)
        {
            if (typeToValidate.IsArray)
            {
                this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, typeToValidate.GetElementType());
            }
            else if (typeToValidate.IsGenericType)
            {
                // is the closed type registered?  if so, nothing to do
                if (!this.SerializationConfiguration.IsRegisteredType(typeToValidate))
                {
                    if (typeToValidate.IsSystemType())
                    {
                        // the closed system type is not registered, confirm that the generic type arguments are registered
                        // so this will inspect the element of type of lists, the key/value types of dictionaries, etc.
                        // we do NOT need to do this for non-System types; the code below will call
                        // RegisterClosedGenericTypePostInitialization which, using MemberTypesToExplore.All, will
                        // recurse through the types nested in the generic.
                        foreach (var genericArgumentType in typeToValidate.GenericTypeArguments)
                        {
                            this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, genericArgumentType);
                        }
                    }
                    else
                    {
                        // for non-System generic types that are not registered, the generic type definition should be registered
                        var genericTypeDefinition = typeToValidate.GetGenericTypeDefinition();

                        this.ThrowUnregisteredTypeEncounteredStrategyIfAppropriate(originalType, genericTypeDefinition);

                        this.SerializationConfiguration.RegisterClosedGenericTypePostInitialization(typeToValidate);
                    }
                }
            }
            else if (typeToValidate.IsSystemType())
            {
            }
            else
            {
                this.ThrowUnregisteredTypeEncounteredStrategyIfAppropriate(originalType, typeToValidate);
            }
        }

        private void ThrowUnregisteredTypeEncounteredStrategyIfAppropriate(
            Type originalType,
            Type typeToValidate)
        {
            if ((this.UnregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Throw) && (!this.SerializationConfiguration.IsRegisteredType(typeToValidate)))
            {
                if (originalType == typeToValidate)
                {
                    throw new UnregisteredTypeAttemptException(Invariant($"Attempted to perform operation on unregistered type '{originalType.ToStringReadable()}'."), originalType);
                }
                else
                {
                    throw new UnregisteredTypeAttemptException(Invariant($"Attempted to perform operation on type '{originalType.ToStringReadable()}', which contains the unregistered type '{typeToValidate.ToStringReadable()}'."), originalType);
                }
            }
        }
    }
}
