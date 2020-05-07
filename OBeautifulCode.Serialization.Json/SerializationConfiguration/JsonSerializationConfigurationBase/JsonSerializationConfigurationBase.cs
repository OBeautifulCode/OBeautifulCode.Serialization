// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase : SerializationConfigurationBase
    {
        private HashSet<Type> TypesWithConverters { get; } = new HashSet<Type>();

        private HashSet<Type> TypesWithStringConverters { get; } = new HashSet<Type>();

        private IList<JsonConverterBuilder> JsonConverterBuilders { get; } = new List<JsonConverterBuilder>();

        private HashSet<Type> HierarchyParticipatingTypes { get; } = new HashSet<Type>();

        /// <summary>
        /// Build <see cref="JsonSerializerSettings" /> to use for serialization using Newtonsoft.
        /// </summary>
        /// <param name="serializationDirection">Direction of serialization.</param>
        /// <param name="formattingKind">Kind of formatting to use.</param>
        /// <returns>
        /// Prepared settings to use with Newtonsoft.
        /// </returns>
        public JsonSerializerSettings BuildJsonSerializerSettings(
            SerializationDirection serializationDirection,
            JsonFormattingKind formattingKind = JsonFormattingKind.Default)
        {
            (serializationDirection == SerializationDirection.Serialize || serializationDirection == SerializationDirection.Deserialize)
                .AsArg(Invariant($"{nameof(serializationDirection)}-must-be-{nameof(SerializationDirection.Serialize)}-or{nameof(SerializationDirection.Serialize)}"))
                .Must().BeTrue();

            var resultBuilder = SerializationKindToSettingsSelectorByDirection[formattingKind](SerializationDirection.Serialize);

            var result = resultBuilder(this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList());

            var specifiedConverters = this.JsonConverterBuilders.Select(_ => _.GetJsonConverterBuilderFuncBySerializationDirection(serializationDirection)()).ToList();

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

                result.ContractResolver = overrideResolver.ContractResolverBuilder(this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList());
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
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Keeping like other to offer option in the future of access to this.")]
        public JsonSerializerSettings BuildAnonymousJsonSerializerSettings(
            SerializationDirection serializationDirection,
            JsonFormattingKind formattingKind = JsonFormattingKind.Default)
        {
            // this is a hack to not mess with casing since the case must match for dynamic deserialization...
            var resultBuilder = SerializationKindToSettingsSelectorByDirection[formattingKind](serializationDirection);

            var result = resultBuilder(this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList());

            result.ContractResolver = new DefaultContractResolver();

            result.Converters = this.GetDefaultConverters(serializationDirection, formattingKind);

            return result;
        }

        private void ProcessTypeToRegisterForJson(
            TypeToRegisterForJson typeToRegisterForJson)
        {
            var type = typeToRegisterForJson.Type;

            var jsonConverterBuilder = typeToRegisterForJson.JsonConverterBuilder;

            if (jsonConverterBuilder != null)
            {
                this.TypesWithConverters.Add(type);

                if (jsonConverterBuilder.OutputKind == JsonConverterOutputKind.String)
                {
                    this.TypesWithStringConverters.Add(type);
                }

                if (this.JsonConverterBuilders.All(_ => _.Id != jsonConverterBuilder.Id))
                {
                    this.JsonConverterBuilders.Add(jsonConverterBuilder);
                }
            }
        }

        private void AddHierarchyParticipatingTypes(
            IReadOnlyCollection<Type> types)
        {
            var inheritedTypeConverterTypes = types.Where(t =>
                (!InheritedTypeConverterBlackList.Contains(t)) &&
                (t.IsAbstract || t.IsInterface || types.Any(a => (a != t) && (t.IsAssignableTo(a) || a.IsAssignableTo(t))))).Distinct().ToList();

            // TODO: what info do we want to capture here? should we give registration details?
            this.HierarchyParticipatingTypes.AddRange(inheritedTypeConverterTypes.Except(this.TypesWithConverters));
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

        private IList<JsonConverter> GetDefaultDeserializingConverters()
        {
            var result = new JsonConverter[0].Concat(
                new JsonConverter[]
                {
                    new DateTimeJsonConverter(),
                    new StringEnumConverter { CamelCaseText = true },
                    new SecureStringJsonConverter(),
                    new InheritedTypeReaderJsonConverter(this.HierarchyParticipatingTypes),
                    new DictionaryJsonConverter(this.TypesWithStringConverters),
                    new KeyValueArrayDictionaryJsonConverter(this.TypesWithStringConverters),
                }).ToList();

            return result;
        }
    }
}