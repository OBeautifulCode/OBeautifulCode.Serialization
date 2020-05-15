// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.Static.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using static System.FormattableString;

    public abstract partial class JsonSerializationConfigurationBase
    {
        private static readonly Dictionary<JsonFormattingKind, Func<SerializationDirection, JsonSerializerSettingsBuilder>> JsonFormattingKindToSettingsSelectorByDirection =
            new Dictionary<JsonFormattingKind, Func<SerializationDirection, JsonSerializerSettingsBuilder>>
            {
                {
                    JsonFormattingKind.Default,
                    direction =>
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
                    JsonFormattingKind.Compact,
                    direction =>
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
                    JsonFormattingKind.Minimal,
                    direction =>
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

        private static readonly IReadOnlyCollection<Type> InheritedTypeConverterBlackList =
            new[]
            {
                typeof(string),
                typeof(object),
            };

        private static readonly JsonSerializerSettingsBuilder DefaultDeserializingSettingsBuilder =
            registeredTypes =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder DefaultSerializingSettingsBuilder =
            registeredTypes =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder CompactDeserializingSettingsBuilder =
            registeredTypes =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder CompactSerializingSettingsBuilder =
            registeredTypes =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder MinimalDeserializingSettingsBuilder =
            registeredTypes =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder MinimalSerializingSettingsBuilder =
            registeredTypes =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelStrictConstructorContractResolver(registeredTypes),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };
    }
}