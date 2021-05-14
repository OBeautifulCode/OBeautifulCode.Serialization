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

    using NewtonsoftFork.Json;
    using NewtonsoftFork.Json.Converters;

    using OBeautifulCode.Serialization;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase : SerializationConfigurationBase
    {
        private readonly ConcurrentDictionary<Type, object> typesWithConverters = new ConcurrentDictionary<Type, object>();

        private readonly Dictionary<Type, IStringSerializeAndDeserialize> typeToKeyInDictionaryStringSerializerMap = new Dictionary<Type, IStringSerializeAndDeserialize>();

        private readonly IList<JsonConverterBuilder> jsonConverterBuilders = new List<JsonConverterBuilder>();

        private readonly ConcurrentDictionary<Type, object> nonAbstractBaseClassTypes = new ConcurrentDictionary<Type, object>();

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
            if (serializationDirection == SerializationDirection.Unknown)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(serializationDirection)}' == '{SerializationDirection.Unknown}'"), (Exception)null);
            }

            if (jsonSerializationConfiguration == null)
            {
                throw new ArgumentNullException(nameof(jsonSerializationConfiguration));
            }

            var jsonFormattingKind = jsonSerializationConfiguration.JsonFormattingKind;

            var jsonSerializerSettingsBuilder = JsonFormattingKindToSettingsSelectorByDirection[jsonFormattingKind](SerializationDirection.Serialize);

            var result = jsonSerializerSettingsBuilder(() => this.RegisteredTypeToRegistrationDetailsMap);

            var specifiedConverters = this.jsonConverterBuilders.Select(_ => _.GetJsonConverterBuilderFuncBySerializationDirection(serializationDirection)()).ToList();

            var defaultConverters = this.GetDefaultConverters(serializationDirection, jsonFormattingKind);

            // Newtonsoft uses the converters in the order they are provided, so specifiedConverters will have priority because they come first
            var converters = new JsonConverter[0]
                .Concat(defaultConverters)
                .Concat(specifiedConverters)
                .ToList();

            result.Converters = converters;

            if ((this.OverrideContractResolver != null) && this.OverrideContractResolver.ContainsKey(serializationDirection))
            {
                var overrideResolver = this.OverrideContractResolver[serializationDirection];

                if (overrideResolver == null)
                {
                    throw new ArgumentException(Invariant($"{nameof(overrideResolver)} is null"));
                }

                result.ContractResolver = overrideResolver.ContractResolverBuilder(() => this.RegisteredTypeToRegistrationDetailsMap);
            }

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
                this.typesWithConverters.TryAdd(type, null);

                if (this.jsonConverterBuilders.All(_ => _.Id != jsonConverterBuilder.Id))
                {
                    this.jsonConverterBuilders.Add(jsonConverterBuilder);
                }
            }

            var keyInDictionaryStringSerializer = typeToRegisterForJson.KeyInDictionaryStringSerializer;

            if (keyInDictionaryStringSerializer != null)
            {
                this.typeToKeyInDictionaryStringSerializerMap.Add(type, keyInDictionaryStringSerializer);
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
            // So, it will first try the StringEnumConverter, then the InheritedTypeWriterJsonConverter, and so on.
            var result = new JsonConverter[0]
                .Concat(
                    new JsonConverter[]
                    {
                        new StringEnumConverter { CamelCaseText = true },
                    })
                .Concat(((formattingKind == JsonFormattingKind.Minimal) || (formattingKind == JsonFormattingKind.Concise))
                    ? new JsonConverter[0]
                    : new[] { new InheritedTypeWriterJsonConverter(() => this.nonAbstractBaseClassTypes, () => this.typesWithConverters) })
                .Concat(
                    new JsonConverter[]
                    {
                        new StringKeysAsPropertiesDictionaryJsonConverter(this.typeToKeyInDictionaryStringSerializerMap),
                        new KeyValueArrayDictionaryJsonConverter(this.typeToKeyInDictionaryStringSerializerMap.Keys.ToList()),
                    }).ToList();

            return result;
        }

        private IList<JsonConverter> GetDefaultDeserializingConverters()
        {
            // Newtonsoft uses the converters in the order they are specified.
            // So, it will first try the StringEnumConverter, then the InheritedTypeReaderJsonConverter, and so on.
            var result = new JsonConverter[0]
                .Concat(
                    new JsonConverter[]
                    {
                        new StringEnumConverter { CamelCaseText = true },
                        new InheritedTypeReaderJsonConverter(() => this.nonAbstractBaseClassTypes, () => this.typesWithConverters, this),
                        new StringKeysAsPropertiesDictionaryJsonConverter(this.typeToKeyInDictionaryStringSerializerMap),
                        new KeyValueArrayDictionaryJsonConverter(this.typeToKeyInDictionaryStringSerializerMap.Keys.ToList()),
                    })
                .ToList();

            return result;
        }
    }
}