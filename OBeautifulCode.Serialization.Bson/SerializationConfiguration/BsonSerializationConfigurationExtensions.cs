// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Options;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;

    /// <summary>
    /// Extension methods related to BSON serialization configuration.
    /// </summary>
    public static class BsonSerializationConfigurationExtensions
    {
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
        /// Builds a <see cref="TypeToRegisterForBson"/> from a type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultMemberTypesToInclude"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultRelatedTypesToInclude"/>.</param>
        /// <param name="serializerBuilderFunc">Optional func that builds the <see cref="IBsonSerializer"/>.  DEFAULT is null (no serializer).</param>
        /// <param name="propertyNameWhitelist">Optional names of the properties to constrain the registration to.  DEFAULT is null (no whitelist).</param>
        /// <returns>
        /// The type to register for BSON serialization.
        /// </returns>
        public static TypeToRegisterForBson ToTypeToRegisterForBson(
            this Type type,
            MemberTypesToInclude memberTypesToInclude = TypeToRegisterConstants.DefaultMemberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude = TypeToRegisterConstants.DefaultRelatedTypesToInclude,
            Func<IBsonSerializer> serializerBuilderFunc = null,
            IReadOnlyCollection<string> propertyNameWhitelist = null)
        {
            var result = new TypeToRegisterForBson(type, memberTypesToInclude, relatedTypesToInclude, serializerBuilderFunc, propertyNameWhitelist);

            return result;
        }

        /// <summary>
        /// Gets the serializer to use for a given type.
        /// </summary>
        /// <param name="type">The type to serialize.</param>
        /// <param name="defaultToObjectSerializer">Optional.  If true (DEFAULT), then returns <see cref="ObjectSerializer"/> when the serializer cannot be determined.  Otherwise, returns null.</param>
        /// <returns>
        /// The serializer to use for the specified type.
        /// </returns>
        public static IBsonSerializer GetAppropriateSerializer(
            this Type type,
            bool defaultToObjectSerializer = true)
        {
            new { type }.AsArg().Must().NotBeNull();

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
                result = typeof(ObcBsonEnumStringSerializer<>).MakeGenericType(type).Construct<IBsonSerializer>();
            }
            else if (type.IsClosedNullableType())
            {
                result = typeof(ObcBsonNullableSerializer<>).MakeGenericType(Nullable.GetUnderlyingType(type)).Construct<IBsonSerializer>();
            }
            else if (type == typeof(DateTime))
            {
                result = new ObcBsonDateTimeSerializer();
            }
            else if (type.IsClosedSystemDictionaryType())
            {
                var keyType = type.GetClosedSystemDictionaryKeyType();

                var valueType = type.GetClosedSystemDictionaryValueType();

                var keySerializer = GetAppropriateSerializer(keyType);

                var valueSerializer = GetAppropriateSerializer(valueType);

                result = typeof(ObcBsonDictionarySerializer<,,>).MakeGenericType(type, keyType, valueType).Construct<IBsonSerializer>(DictionaryRepresentation.ArrayOfDocuments, keySerializer, valueSerializer);
            }
            else if (type.IsArray)
            {
                var elementType = type.GetElementType();

                // Don't default to object serializer because if there is no element serializer we want to let the ArraySerializer decide what to do.
                var elementSerializer = GetAppropriateSerializer(elementType, defaultToObjectSerializer: false);

                result = elementSerializer == null
                    ? typeof(ArraySerializer<>).MakeGenericType(elementType).Construct<IBsonSerializer>()
                    : typeof(ArraySerializer<>).MakeGenericType(elementType).Construct<IBsonSerializer>(elementSerializer);
            }
            else if (type.IsClosedSystemCollectionType())
            {
                var elementType = type.GetClosedSystemCollectionElementType();

                // Don't default to object serializer because if there is no element serializer we want to let the ObcCollectionSerializer decide what to do.
                var elementSerializer = GetAppropriateSerializer(elementType, defaultToObjectSerializer: false);

                result = typeof(ObcBsonCollectionSerializer<,>).MakeGenericType(type, elementType).Construct<IBsonSerializer>(elementSerializer);
            }
            else
            {
                result = defaultToObjectSerializer ? new ObjectSerializer() : null;
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
            new { type }.AsArg().Must().NotBeNull();

            bool FilterCompilerGenerated(MemberInfo memberInfo) => !memberInfo.CustomAttributes.Select(s => s.AttributeType).Contains(typeof(CompilerGeneratedAttribute));

            // We use DeclaredOnly here because BSON uses the class map that was registered for the base class.
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            var allMembers = type.GetMembers(bindingFlags).Where(FilterCompilerGenerated).ToList();

            var fields = allMembers
                .Where(_ => _.MemberType == MemberTypes.Field)
                .Cast<FieldInfo>()
                .Where(_ => !_.IsInitOnly)
                .ToList();

            const bool returnIfSetMethodIsNotPublic = true;

            var properties = allMembers
                .Where(_ => _.MemberType == MemberTypes.Property)
                .Cast<PropertyInfo>()
                .Where(_ => _.CanWrite || _.GetSetMethod(returnIfSetMethodIsNotPublic) != null)
                .ToList();

            var result = new MemberInfo[0].Concat(fields).Concat(properties).ToList();

            return result;
        }
    }
}