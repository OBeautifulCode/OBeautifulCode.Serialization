// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializer for writing-to and reading-from a property bag.
    /// </summary>
    public partial class ObcPropertyBagSerializer : ObcSerializerBase
    {
        /// <summary>
        /// Reserved key for storing the type's versionless assembly qualified name.
        /// </summary>
        public const string ReservedKeyForTypeVersionlessAssemblyQualifiedName = "_Type";

        /// <summary>
        /// Encoding to use for conversion in and out of bytes.
        /// </summary>
        public static readonly Encoding SerializationEncoding = Encoding.UTF8;

        private readonly ObcDictionaryStringStringSerializer dictionaryStringSerializer;

        private readonly IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> configuredTypeToSerializerMap;

        private readonly PropertyBagSerializationConfigurationBase propertyBagConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcPropertyBagSerializer"/> class.
        /// </summary>
        /// <param name="propertyBagSerializationConfigurationType">Type of configuration to use.</param>
        public ObcPropertyBagSerializer(
            PropertyBagSerializationConfigurationType propertyBagSerializationConfigurationType = null)
            : base(propertyBagSerializationConfigurationType ?? typeof(NullPropertyBagSerializationConfiguration).ToPropertyBagSerializationConfigurationType())
        {
            this.propertyBagConfiguration = (PropertyBagSerializationConfigurationBase)this.SerializationConfiguration;

            this.dictionaryStringSerializer = new ObcDictionaryStringStringSerializer(
                this.propertyBagConfiguration.StringSerializationKeyValueDelimiter,
                this.propertyBagConfiguration.StringSerializationLineDelimiter,
                this.propertyBagConfiguration.StringSerializationNullValueEncoding);

            this.configuredTypeToSerializerMap = this.propertyBagConfiguration.BuildConfiguredTypeToSerializerMap();

            this.SerializerRepresentation = new SerializerRepresentation(SerializationKind.PropertyBag, propertyBagSerializationConfigurationType?.ConcreteSerializationConfigurationDerivativeType.ToRepresentation());
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.PropertyBag;

        /// <inheritdoc />
        public override SerializerRepresentation SerializerRepresentation { get; }

        /// <inheritdoc />
        public override string SerializeToString(
            object objectToSerialize)
        {
            var namedPropertyBagWithStringValues =  this.SerializeToNamedPropertyBagWithStringValues(objectToSerialize);

            var result = this.dictionaryStringSerializer.SerializeDictionaryToString(namedPropertyBagWithStringValues);

            return result;
        }

        /// <inheritdoc />
        public override byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var stringRepresentation = this.SerializeToString(objectToSerialize);

            var result = stringRepresentation == null
                ? null
                : SerializationEncoding.GetBytes(stringRepresentation);

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            string serializedString)
        {
            var objectType = typeof(T);

            var result = (T)this.Deserialize(serializedString, objectType);

            return result;
        }

        /// <inheritdoc cref="IStringSerializeAndDeserialize"/>
        public override object Deserialize(
            string serializedString,
            Type type)
        {
            var dictionary = this.dictionaryStringSerializer.DeserializeToDictionary(serializedString);

            var result = this.Deserialize(dictionary, type);

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            byte[] serializedBytes)
        {
            var result = (T)this.Deserialize(serializedBytes, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            var stringRepresentation = serializedBytes == null
                ? null
                : SerializationEncoding.GetString(serializedBytes);

            var result = this.Deserialize(stringRepresentation, type);

            return result;
        }

        private static IReadOnlyDictionary<string, TValue> GetSerializedPropertyBagToUseOrThrow<TValue>(
            IReadOnlyDictionary<string, TValue> serializedPropertyBag)
        {
            if (serializedPropertyBag.Count != serializedPropertyBag.Keys.Distinct(StringComparer.OrdinalIgnoreCase).Count())
            {
                throw new SerializationException(Invariant($"{nameof(serializedPropertyBag)} contains two or more properties with the same case-insensitive name."));
            }

            var result = serializedPropertyBag.ToDictionary(_ => _.Key, _ => _.Value, StringComparer.OrdinalIgnoreCase);

            return result;
        }

        private void InternalPropertyBagThrowOnUnregisteredTypeIfAppropriate(
            Type objectType,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(objectType, serializationDirection, objectToSerialize);
        }

        private Type GetTypeToDeserializeIntoOrThrow<TValue>(
            Type type,
            IReadOnlyDictionary<string, TValue> serializedPropertyBag)
        {
            var result = type;

            // serializePropertyBag will have case-insensitive keys because of ValidateSerializedPropertyBagAndMakeCaseInsensitiveKeys
            if (serializedPropertyBag.ContainsKey(ReservedKeyForTypeVersionlessAssemblyQualifiedName))
            {
                var assemblyQualifiedNameObject = serializedPropertyBag[ReservedKeyForTypeVersionlessAssemblyQualifiedName];

                if (!(assemblyQualifiedNameObject is string assemblyQualifiedName))
                {
                    throw new SerializationException(Invariant($"The property bag specifies the type to deserialize into via the '{ReservedKeyForTypeVersionlessAssemblyQualifiedName}' property (version-less assembly qualified name), but the value of that property is of type '{assemblyQualifiedNameObject.GetType().ToStringReadable()}' instead of '{typeof(string).ToStringReadable()}'."));
                }

                result = assemblyQualifiedName.ToTypeRepresentationFromAssemblyQualifiedName().ResolveFromLoadedTypes();

                this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(result, SerializationDirection.Deserialize, null);

                if (!type.IsAssignableFrom(result))
                {
                    throw new SerializationException(Invariant($"The property bag specifies the type to deserialize into via the '{ReservedKeyForTypeVersionlessAssemblyQualifiedName}' property (version-less assembly qualified name) as '{result.ToStringReadable()}', but that type is not assignable to '{type.ToStringReadable()}', which is the type specified in the deserialize call."));
                }
            }

            // Unlike serialization, where we use the runtime type (object.GetType()), in deserialization we are told
            // what type to deserialize into and so we have to validate that that's a type we can construct.
            var isValidType = result.IsClosedNonAnonymousClassType() && (!result.IsAbstract);

            if (!isValidType)
            {
                var exceptionMessageSuffix = result == type
                    ? Invariant($"The type specified in the deserialize call is '{result.ToStringReadable()}', which is not a closed non-anonymous concrete class.")
                    : Invariant($"The property bag specifies the type to deserialize into via the '{ReservedKeyForTypeVersionlessAssemblyQualifiedName}' property (versionless assembly qualified name) as '{result.ToStringReadable()}', which is not a closed non-anonymous concrete class.");

                throw new SerializationException(Invariant($"Can only deserialize into a closed non-anonymous concrete class.  {exceptionMessageSuffix}"));
            }

            return result;
        }
    }
}
