// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Extension methods related to JSON serialization configuration.
    /// </summary>
    public static class JsonSerializationConfigurationExtensions
    {
        /// <summary>
        /// Gets the <see cref="JsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </summary>
        /// <param name="jsonSerializationConfigurationType">The type of the JSON serialization configuration.</param>
        /// <returns>
        /// The <see cref="JsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </returns>
        public static JsonSerializationConfigurationType ToJsonSerializationConfigurationType(
            this Type jsonSerializationConfigurationType)
        {
            var result = new JsonSerializationConfigurationType(jsonSerializationConfigurationType);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForJson"/> for a type using the most sensible settings.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <returns>
        /// The type to register for JSON serialization.
        /// </returns>
        public static TypeToRegisterForJson ToTypeToRegisterForJson(
            this Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var result = new TypeToRegisterForJson(type, MemberTypesToInclude.All, RelatedTypesToInclude.Default, null, null);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForJson"/> for a type using the most sensible settings,
        /// with a specified <see cref="IStringSerializeAndDeserialize"/> to use when dictionaries are keyed on that type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="stringSerializer">The string serializer to use when dictionaries are keyed on <paramref name="type"/>.</param>
        /// <returns>
        /// The type to register for JSON serialization.
        /// </returns>
        public static TypeToRegisterForJson ToTypeToRegisterForJsonUsingKeyInDictionaryStringSerializer(
            this Type type,
            IStringSerializeAndDeserialize stringSerializer)
        {
            new { type }.AsArg().Must().NotBeNull();
            new { stringSerializer }.AsArg().Must().NotBeNull();

            var result = new TypeToRegisterForJson(type, MemberTypesToInclude.All, RelatedTypesToInclude.Default, null, stringSerializer);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForJson"/> for a type using the most sensible settings,
        /// with a specified <see cref="IStringSerializeAndDeserialize"/> to use everywhere the type appears.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="stringSerializer">The string serializer to use for <paramref name="type"/>.</param>
        /// <returns>
        /// The type to register for JSON serialization.
        /// </returns>
        public static TypeToRegisterForJson ToTypeToRegisterForJsonUsingStringSerializer(
            this Type type,
            IStringSerializeAndDeserialize stringSerializer)
        {
            new { type }.AsArg().Must().NotBeNull();
            new { stringSerializer }.AsArg().Must().NotBeNull();

            var canConvertTypeMatchStrategy = type.ResolveDefaultIntoActionableRelatedTypesToInclude().ToCanConvertTypeMatchStrategy();

            var jsonConverterBuilderId = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();

            JsonConverter ConverterBuilderFunc() => new StringSerializerBackedJsonConverter(type, stringSerializer, canConvertTypeMatchStrategy);

            var jsonConverterBuilder = new JsonConverterBuilder(jsonConverterBuilderId, ConverterBuilderFunc, ConverterBuilderFunc);

            var result = new TypeToRegisterForJson(type, MemberTypesToInclude.None, RelatedTypesToInclude.Default, jsonConverterBuilder, stringSerializer);

            return result;
        }

        private static CanConvertTypeMatchStrategy ToCanConvertTypeMatchStrategy(
            this RelatedTypesToInclude relatedTypesToInclude)
        {
            CanConvertTypeMatchStrategy result;

            switch (relatedTypesToInclude)
            {
                case RelatedTypesToInclude.None:
                    result = CanConvertTypeMatchStrategy.TypeToConsiderEqualsRegisteredType;
                    break;
                case RelatedTypesToInclude.Descendants:
                    result = CanConvertTypeMatchStrategy.TypeToConsiderIsAssignableToRegisteredType;
                    break;
                case RelatedTypesToInclude.Ancestors:
                    result = CanConvertTypeMatchStrategy.TypeToConsiderIsAssignableFromRegisteredType;
                    break;
                case RelatedTypesToInclude.AncestorsAndDescendants:
                    result = CanConvertTypeMatchStrategy.TypeToConsiderIsAssignableToOrFromRegisteredType;
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(RelatedTypesToInclude)} is not supported: {relatedTypesToInclude}."));
            }

            return result;
        }
    }
}