// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.Static.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        private static readonly JsonSerializerSettingsBuilder DefaultDeserializingSettingsBuilder =
            getRegisteredTypesToRegistrationDetailsMapFunc =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(getRegisteredTypesToRegistrationDetailsMapFunc),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder DefaultSerializingSettingsBuilder =
            getRegisteredTypesToRegistrationDetailsMapFunc =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(getRegisteredTypesToRegistrationDetailsMapFunc),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder CompactDeserializingSettingsBuilder =
            getRegisteredTypesToRegistrationDetailsMapFunc =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(getRegisteredTypesToRegistrationDetailsMapFunc),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder CompactSerializingSettingsBuilder =
            getRegisteredTypesToRegistrationDetailsMapFunc =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Include,
                    ContractResolver = new CamelStrictConstructorContractResolver(getRegisteredTypesToRegistrationDetailsMapFunc),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder MinimalDeserializingSettingsBuilder =
            getRegisteredTypesToRegistrationDetailsMapFunc =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelStrictConstructorContractResolver(getRegisteredTypesToRegistrationDetailsMapFunc),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static readonly JsonSerializerSettingsBuilder MinimalSerializingSettingsBuilder =
            getRegisteredTypesToRegistrationDetailsMapFunc =>
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelStrictConstructorContractResolver(getRegisteredTypesToRegistrationDetailsMapFunc),
                    DateParseHandling = DateParseHandling.None,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };

        private static bool ParticipatesInHierarchy(
            Type type,
            IReadOnlyCollection<Type> registeredTypes)
        {
            if (type.IsAbstract)
            {
                return true;
            }

            if (type.IsInterface)
            {
                return true;
            }

            // has a base class
            var baseType = type.BaseType;
            if ((baseType != null) && (baseType != typeof(object)) && (!type.IsValueType))
            {
                return true;
            }

            // not abstract, but is the base class of some other class
            if (registeredTypes.Any(registeredType => (type != registeredType) && type.IsAssignableFrom(registeredType)))
            {
                return true;
            }

            return false;
        }
    }
}