// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase : SerializationConfigurationBase
    {
        private readonly Dictionary<Type, object> typesWithConverters = new Dictionary<Type, object>();

        private readonly HashSet<Type> typesWithStringConverters = new HashSet<Type>();

        private readonly IList<JsonConverterBuilder> jsonConverterBuilders = new List<JsonConverterBuilder>();

        private readonly ConcurrentDictionary<Type, object> hierarchyParticipatingTypes = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Build <see cref="JsonSerializerSettings" /> to use for serialization using Newtonsoft.
        /// </summary>
        /// <param name="serializationDirection">Direction of serialization.</param>
        /// <param name="jsonSerializationConfiguration">The serialization configuration in use.</param>
        /// <returns>
        /// Prepared settings to use with Newtonsoft.
        /// </returns>
        public JsonSerializerSettings BuildJsonSerializerSettings(
            SerializationDirection serializationDirection,
            JsonSerializationConfigurationBase jsonSerializationConfiguration)
        {
            new { serializationDirection }.AsArg().Must().NotBeEqualTo(SerializationDirection.Unknown);
            new { jsonSerializationConfiguration }.AsArg().Must().NotBeNull();

            var jsonFormattingKind = jsonSerializationConfiguration.JsonFormattingKind;

            var jsonSerializerSettingsBuilder = JsonFormattingKindToSettingsSelectorByDirection[jsonFormattingKind](SerializationDirection.Serialize);

            var result = jsonSerializerSettingsBuilder(() => this.RegisteredTypeToRegistrationDetailsMap);

            var specifiedConverters = this.jsonConverterBuilders.Select(_ => _.GetJsonConverterBuilderFuncBySerializationDirection(serializationDirection)()).ToList();

            var defaultConverters = this.GetDefaultConverters(serializationDirection, jsonFormattingKind);

            // Newtonsoft uses the converters in the order they are provided, so specifiedConverters will have priority because they come first
            var converters = new JsonConverter[0]
                .Concat(specifiedConverters)
                .Concat(defaultConverters)
                .ToList();

            result.Converters = converters;

            if ((this.OverrideContractResolver != null) && this.OverrideContractResolver.ContainsKey(serializationDirection))
            {
                var overrideResolver = this.OverrideContractResolver[serializationDirection];

                new { overrideResolver }.AsArg().Must().NotBeNull();

                result.ContractResolver = overrideResolver.ContractResolverBuilder(() => this.RegisteredTypeToRegistrationDetailsMap);
            }

            return result;
        }

        /// <summary>
        /// Build <see cref="JsonSerializerSettings" /> to use for serialization of anonymous types using Newtonsoft.
        /// </summary>
        /// <param name="serializationDirection">Direction of serialization.</param>
        /// <param name="formattingKind">Kind of formatting to use.</param>
        /// <returns>
        /// Prepared settings to use with Newtonsoft.
        /// </returns>
        public JsonSerializerSettings BuildAnonymousJsonSerializerSettings(
            SerializationDirection serializationDirection,
            JsonFormattingKind formattingKind)
        {
            // this is a hack to not mess with casing since the case must match for dynamic deserialization...
            var jsonSerializerSettingsBuilder = JsonFormattingKindToSettingsSelectorByDirection[formattingKind](serializationDirection);

            var result = jsonSerializerSettingsBuilder(() => this.RegisteredTypeToRegistrationDetailsMap);

            result.ContractResolver = new DefaultContractResolver();

            result.Converters = this.GetDefaultConverters(serializationDirection, formattingKind);

            return result;
        }

        private void ProcessTypeToRegisterForJson(
            TypeToRegisterForJson typeToRegisterForJson)
        {
            var type = typeToRegisterForJson.Type;

            // generic type definitions are not a problem here because
            // TypeToRegisterForJson doesn't allow generic type definitions to be associated with converters.
            var jsonConverterBuilder = typeToRegisterForJson.JsonConverterBuilder;

            if (jsonConverterBuilder != null)
            {
                this.typesWithConverters.Add(type, null);

                if (jsonConverterBuilder.OutputKind == JsonConverterOutputKind.String)
                {
                    this.typesWithStringConverters.Add(type);
                }

                if (this.jsonConverterBuilders.All(_ => _.Id != jsonConverterBuilder.Id))
                {
                    this.jsonConverterBuilders.Add(jsonConverterBuilder);
                }
            }
        }

        private IList<JsonConverter> GetDefaultConverters(
            SerializationDirection serializationDirection,
            JsonFormattingKind formattingKind)
        {
            switch (serializationDirection)
            {
                case SerializationDirection.Serialize:
                    return this.GetDefaultSerializingConverters(formattingKind);
                case SerializationDirection.Deserialize:
                    return this.GetDefaultDeserializingConverters();
                default:
                    throw new NotSupportedException(Invariant($"{nameof(SerializationDirection)} value {serializationDirection} is not supported."));
            }
        }

        private IList<JsonConverter> GetDefaultSerializingConverters(
            JsonFormattingKind formattingKind)
        {
            // Newtonsoft uses the converters in the order they are specified.
            // So, it will first try the DateTimeJsonConverter, then the StringEnumConverter, and so on.
            var result = new JsonConverter[0].Concat(
                    new JsonConverter[]
                    {
                        new DateTimeJsonConverter(),
                        new StringEnumConverter { CamelCaseText = true },
                        new SecureStringJsonConverter(),
                    }).Concat(formattingKind == JsonFormattingKind.Minimal
                    ? new JsonConverter[0]
                    : new[] { new InheritedTypeWriterJsonConverter(() => this.hierarchyParticipatingTypes) })
                .Concat(
                    new JsonConverter[]
                    {
                        // new DictionaryJsonConverter(this.TypesWithStringConverters) - this converter cannot write (CanWrite => false)
                        new KeyValueArrayDictionaryJsonConverter(this.typesWithStringConverters),
                    }).ToList();

            return result;
        }

        private IList<JsonConverter> GetDefaultDeserializingConverters()
        {
            // Newtonsoft uses the converters in the order they are specified.
            // So, it will first try the DateTimeJsonConverter, then the StringEnumConverter, and so on.
            var result = new JsonConverter[0].Concat(
                new JsonConverter[]
                {
                    new DateTimeJsonConverter(),
                    new StringEnumConverter { CamelCaseText = true },
                    new SecureStringJsonConverter(),
                    new InheritedTypeReaderJsonConverter(() => this.hierarchyParticipatingTypes, this),
                    new DictionaryJsonConverter(this.typesWithStringConverters),
                    new KeyValueArrayDictionaryJsonConverter(this.typesWithStringConverters),
                }).ToList();

            return result;
        }
    }
}