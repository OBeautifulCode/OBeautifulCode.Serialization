// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.Newtonsoft.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    using OBeautifulCode.Assertion.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase
    {
        /// <summary>
        /// Map of <see cref="JsonFormattingKind" /> to a <see cref="Func{T1,T2,T3,T4,T5,T6,T7,T8,T9,TResult}" /> that will take a <see cref="SerializationDirection" /> and return the correct <see cref="JsonSerializerSettings" />.
        /// </summary>
        internal static readonly Dictionary<JsonFormattingKind, Func<SerializationDirection, JsonSerializerSettingsBuilder>>
            SerializationKindToSettingsSelectorByDirection =
                new Dictionary<JsonFormattingKind, Func<SerializationDirection, JsonSerializerSettingsBuilder>>
                {
                    {
                        JsonFormattingKind.Default, direction =>
                        {
                            switch (direction)
                            {
                                case SerializationDirection.Serialize:
                                    return DefaultSerializingSettingsBuilder;
                                case SerializationDirection.Deserialize:
                                    return DefaultDeserializingSettingsBuilder;
                                default:
                                    throw new NotSupportedException(Invariant($"Value of {nameof(direction)} - {direction} is not currently supported."));
                            }
                        }
                    },
                    {
                        JsonFormattingKind.Compact, direction =>
                        {
                            switch (direction)
                            {
                                case SerializationDirection.Serialize:
                                    return CompactSerializingSettingsBuilder;
                                case SerializationDirection.Deserialize:
                                    return CompactDeserializingSettingsBuilder;
                                default:
                                    throw new NotSupportedException(Invariant($"Value of {nameof(direction)} - {direction} is not currently supported."));
                            }
                        }
                    },
                    {
                        JsonFormattingKind.Minimal, direction =>
                        {
                            switch (direction)
                            {
                                case SerializationDirection.Serialize:
                                    return MinimalSerializingSettingsBuilder;
                                case SerializationDirection.Deserialize:
                                    return MinimalDeserializingSettingsBuilder;
                                default:
                                    throw new NotSupportedException(Invariant($"Value of {nameof(direction)} - {direction} is not currently supported."));
                            }
                        }
                    },
                };

        private static readonly JsonSerializerSettingsBuilder DefaultDeserializingSettingsBuilder = registeredTypes =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                DateParseHandling = DateParseHandling.None,
                FloatParseHandling = FloatParseHandling.Decimal,
            };

        private static readonly JsonSerializerSettingsBuilder DefaultSerializingSettingsBuilder = registeredTypes =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                DateParseHandling = DateParseHandling.None,
                FloatParseHandling = FloatParseHandling.Decimal,
            };

        private static readonly JsonSerializerSettingsBuilder CompactDeserializingSettingsBuilder = registeredTypes =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                DateParseHandling = DateParseHandling.None,
                FloatParseHandling = FloatParseHandling.Decimal,
            };

        private static readonly JsonSerializerSettingsBuilder CompactSerializingSettingsBuilder = registeredTypes =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                DateParseHandling = DateParseHandling.None,
                FloatParseHandling = FloatParseHandling.Decimal,
            };

        private static readonly JsonSerializerSettingsBuilder MinimalDeserializingSettingsBuilder = registeredTypes =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                DateParseHandling = DateParseHandling.None,
                FloatParseHandling = FloatParseHandling.Decimal,
            };

        private static readonly JsonSerializerSettingsBuilder MinimalSerializingSettingsBuilder = registeredTypes =>
            new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                DateParseHandling = DateParseHandling.None,
                FloatParseHandling = FloatParseHandling.Decimal,
            };

        /// <summary>
        /// Build <see cref="JsonSerializerSettings" /> to use for serialization using Newtonsoft.
        /// </summary>
        /// <param name="serializationDirection">Direction of serialization.</param>
        /// <param name="formattingKind">Kind of formatting to use.</param>
        /// <returns>Prepared settings to use with Newtonsoft.</returns>
        public JsonSerializerSettings BuildJsonSerializerSettings(
            SerializationDirection serializationDirection,
            JsonFormattingKind formattingKind = JsonFormattingKind.Default)
        {
            (serializationDirection == SerializationDirection.Serialize || serializationDirection == SerializationDirection.Deserialize)
                .AsArg(Invariant($"{nameof(serializationDirection)}-must-be-{nameof(SerializationDirection.Serialize)}-or{nameof(SerializationDirection.Serialize)}"))
                .Must().BeTrue();

            var resultBuilder = SerializationKindToSettingsSelectorByDirection[formattingKind](SerializationDirection.Serialize);

            var result = resultBuilder(this.RegisteredTypeToSerializationConfigurationTypeMap.Keys.ToList());

            var specifiedConverters = this.RegisteredConverters.Select(_ =>
                serializationDirection == SerializationDirection.Serialize
                    ? _.SerializingConverterBuilderFunc()
                    : _.DeserializingConverterBuilderFunc()).ToList();

            var defaultConverters = this.GetDefaultConverters(serializationDirection, formattingKind);

            var converters = new JsonConverter[0]
                .Concat(specifiedConverters)
                .Concat(defaultConverters)
                .ToList();

            // TODO: We may need this sorted differently; as in does it need to reverse?
            result.Converters = converters;

            if (this.OverrideContractResolver != null && this.OverrideContractResolver.ContainsKey(serializationDirection))
            {
                var overrideResolver = this.OverrideContractResolver[serializationDirection];
                new { overrideResolver }.AsArg().Must().NotBeNull();
                result.ContractResolver = overrideResolver.ContractResolverBuilder(this.RegisteredTypeToSerializationConfigurationTypeMap.Keys.ToList());
            }

            return result;
        }

        /// <summary>
        /// Build <see cref="JsonSerializerSettings" /> to use for serialization of anonymous types using Newtonsoft.
        /// </summary>
        /// <param name="serializationDirection">Direction of serialization.</param>
        /// <param name="formattingKind">Kind of formatting to use.</param>
        /// <returns>Prepared settings to use with Newtonsoft.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Keeping like other to offer option in the future of access to this.")]
        public JsonSerializerSettings BuildAnonymousJsonSerializerSettings(
            SerializationDirection serializationDirection,
            JsonFormattingKind formattingKind = JsonFormattingKind.Default)
        {
            // this is a hack to not mess with casing since the case must match for dynamic deserialization...
            var resultBuilder = SerializationKindToSettingsSelectorByDirection[formattingKind](serializationDirection);

            var result = resultBuilder(this.RegisteredTypeToSerializationConfigurationTypeMap.Keys.ToList());

            result.ContractResolver = new DefaultContractResolver();

            result.Converters = this.GetDefaultConverters(serializationDirection, formattingKind);

            return result;
        }

        private IList<JsonConverter> GetDefaultConverters(SerializationDirection serializationDirection, JsonFormattingKind formattingKind)
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

        private IList<JsonConverter> GetDefaultDeserializingConverters()
        {
            return new JsonConverter[0].Concat(
                new JsonConverter[]
                {
                    new DateTimeJsonConverter(),
                    new StringEnumConverter { CamelCaseText = true },
                    new SecureStringJsonConverter(),
                    new InheritedTypeReaderJsonConverter(this.HierarchyParticipatingTypes),
                    new DictionaryJsonConverter(this.TypesWithStringConverters),
                    new KeyValueArrayDictionaryJsonConverter(this.TypesWithStringConverters),
                }).ToList();
        }

        private IList<JsonConverter> GetDefaultSerializingConverters(JsonFormattingKind formattingKind)
        {
            var result = new JsonConverter[0].Concat(
                    new JsonConverter[]
                    {
                        new DateTimeJsonConverter(),
                        new StringEnumConverter { CamelCaseText = true },
                        new SecureStringJsonConverter(),
                    }).Concat(formattingKind == JsonFormattingKind.Minimal
                    ? new JsonConverter[0]
                    : new[] { new InheritedTypeWriterJsonConverter(this.HierarchyParticipatingTypes) })
                .Concat(
                    new JsonConverter[]
                    {
                        // new DictionaryJsonConverter(this.TypesWithStringConverters) - this converter cannot write (CanWrite => false)
                        new KeyValueArrayDictionaryJsonConverter(this.TypesWithStringConverters),
                    }).ToList();

            return result;
        }
    }
}