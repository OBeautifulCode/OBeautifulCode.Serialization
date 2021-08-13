// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Options;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;

    /// <summary>
    /// Extension methods related to BSON serialization configuration.
    /// </summary>
    public static class BsonSerializationConfigurationExtensions
    {
        private static readonly IStringSerializeAndDeserialize DateTimeStringSerializer = new ObcDateTimeStringSerializer();

        /// <summary>
        /// Gets the <see cref="BsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </summary>
        /// <param name="bsonSerializationConfigurationType">The type of the BSON serialization configuration.</param>
        /// <returns>
        /// The <see cref="BsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </returns>
        public static BsonSerializationConfigurationType ToBsonSerializationConfigurationType(
            this Type bsonSerializationConfigurationType)
        {
            var result = new BsonSerializationConfigurationType(bsonSerializationConfigurationType);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForBson"/> from a type using the most sensible settings.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <returns>
        /// The type to register for BSON serialization.
        /// </returns>
        public static TypeToRegisterForBson ToTypeToRegisterForBson(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = new TypeToRegisterForBson(type, MemberTypesToInclude.All, RelatedTypesToInclude.Default, null, null);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForBson"/> for a type using the most sensible settings,
        /// with a specified <see cref="IStringSerializeAndDeserialize"/> to use everywhere the type appears.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="stringSerializer">The string serializer to use for <paramref name="type"/>.</param>
        /// <returns>
        /// The type to register for BSON serialization.
        /// </returns>
        public static TypeToRegisterForBson ToTypeToRegisterForBsonUsingStringSerializer(
            this Type type,
            IStringSerializeAndDeserialize stringSerializer)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (stringSerializer == null)
            {
                throw new ArgumentNullException(nameof(stringSerializer));
            }

            var serializer = StringSerializerBackedBsonSerializer.Build(type, stringSerializer);

            IBsonSerializer BsonSerializerBuilderFunc() => serializer;

            var bsonSerializerBuilder = new BsonSerializerBuilder(BsonSerializerBuilderFunc, BsonSerializerOutputKind.String);

            var result = new TypeToRegisterForBson(type, MemberTypesToInclude.None, RelatedTypesToInclude.Default, bsonSerializerBuilder, null);

            return result;
        }

        /// <summary>
        /// Gets the serializer to use for a given type.
        /// </summary>
        /// <param name="type">The type to serialize.</param>
        /// <returns>
        /// The serializer to use for the specified type.
        /// </returns>
        public static IBsonSerializer GetAppropriateSerializer(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            IBsonSerializer result;

            if (type == typeof(string))
            {
                result = new StringSerializer();
            }
            else if (type == typeof(Guid))
            {
                result = new GuidSerializer();
            }
            else if (type.IsEnum)
            {
                result = typeof(EnumStringBsonSerializer<>).MakeGenericType(type).Construct<IBsonSerializer>();
            }
            else if (type.IsClosedNullableType())
            {
                result = typeof(NullableBsonSerializer<>).MakeGenericType(Nullable.GetUnderlyingType(type)).Construct<IBsonSerializer>();
            }
            else if (type == typeof(DateTime))
            {
                result = new StringSerializerBackedBsonSerializer<DateTime>(DateTimeStringSerializer);
            }
            else if (type.IsClosedSystemDictionaryType())
            {
                var keyType = type.GetClosedSystemDictionaryKeyType();

                var valueType = type.GetClosedSystemDictionaryValueType();

                // If there is no key or value serializer BSON will find an existing
                // registered serializer or fallback to the registered class map for the type.
                var keySerializer = GetAppropriateSerializer(keyType);

                var valueSerializer = GetAppropriateSerializer(valueType);

                result = typeof(DictionaryBsonSerializer<,,>).MakeGenericType(type, keyType, valueType).Construct<IBsonSerializer>(DictionaryRepresentation.ArrayOfDocuments, keySerializer, valueSerializer);
            }
            else if (type.IsArray)
            {
                var elementType = type.GetElementType();

                // If there is no element serializer BSON will find an existing
                // registered serializer or fallback to the registered class map for the type.
                var elementSerializer = GetAppropriateSerializer(elementType);

                result = elementSerializer == null
                    ? typeof(ArraySerializer<>).MakeGenericType(elementType).Construct<IBsonSerializer>()
                    : typeof(ArraySerializer<>).MakeGenericType(elementType).Construct<IBsonSerializer>(elementSerializer);
            }
            else if (type.IsClosedSystemCollectionType())
            {
                var elementType = type.GetClosedSystemCollectionElementType();

                // If there is no element serializer BSON will find an existing
                // registered serializer or fallback to the registered class map for the type.
                var elementSerializer = GetAppropriateSerializer(elementType);

                result = typeof(CollectionBsonSerializer<,>).MakeGenericType(type, elementType).Construct<IBsonSerializer>(elementSerializer);
            }
            else
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Gets the <see cref="MemberInfo"/>'s to use for auto mapping.
        /// </summary>
        /// <param name="type">Type to interrogate.</param>
        /// <returns>
        /// Collection of members to map.
        /// </returns>
        public static IReadOnlyCollection<MemberInfo> GetMembersToAutomap(
            this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // We use DeclaredOnly here because BSON uses the class map that was registered for the base class.
            var result = type.GetMembersFiltered(
                MemberRelationships.DeclaredInType,
                MemberOwners.Instance,
                MemberAccessModifiers.All,
                MemberKinds.Field | MemberKinds.Property,
                MemberMutability.Writable);

            return result;
        }
    }
}