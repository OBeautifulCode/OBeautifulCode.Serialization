// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcJsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// JSON serializer.
    /// </summary>
    public class ObcJsonSerializer : ConfiguredSerializerBase
    {
        /// <summary>
        /// Encoding to use for conversion in and out of bytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Is not mutable.")]
        public static readonly Encoding SerializationEncoding = Encoding.UTF8;

        /// <summary>
        /// Gets the serialization settings to use for writing dynamic or anonymous objects.
        /// </summary>
        private readonly JsonSerializerSettings anonymousWriteSerializationSettings;

        private readonly JsonSerializationConfigurationBase jsonConfiguration;

        private readonly JsonFormattingKind formattingKind;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcJsonSerializer"/> class.
        /// </summary>
        /// <param name="configurationType">Optional type of configuration to use; DEFAULT is none.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <param name="formattingKind">Optional type of formatting to use; DEFAULT is <see cref="JsonFormattingKind.Default" />.</param>
        public ObcJsonSerializer(
            Type configurationType = null,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default,
            JsonFormattingKind formattingKind = JsonFormattingKind.Default)
            : base(configurationType ?? typeof(NullJsonSerializationConfiguration), unregisteredTypeEncounteredStrategy)
        {
            new { formattingKind }.AsArg().Must().NotBeEqualTo(JsonFormattingKind.Invalid);
            this.formattingKind = formattingKind;

            if (configurationType != null)
            {
                configurationType.IsSubclassOf(typeof(JsonSerializationConfigurationBase)).AsArg(
                    Invariant($"Configuration type - {configurationType.FullName} - must derive from {nameof(JsonSerializationConfigurationBase)}.")).Must().BeTrue();

                configurationType.HasParameterlessConstructor().AsArg(
                    Invariant($"{nameof(configurationType)} must contain a default constructor to use in {nameof(ObcJsonSerializer)}.")).Must().BeTrue();
            }

            this.jsonConfiguration = (JsonSerializationConfigurationBase)this.configuration;
            this.anonymousWriteSerializationSettings = this.jsonConfiguration.BuildAnonymousJsonSerializerSettings(SerializationDirection.Serialize, this.formattingKind);
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.Json;

        /// <summary>
        /// Converts JSON string into a byte array.
        /// </summary>
        /// <param name="json">JSON string.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ConvertJsonToByteArray(string json)
        {
            var ret = SerializationEncoding.GetBytes(json);
            return ret;
        }

        /// <summary>
        /// Converts JSON byte array into a string.
        /// </summary>
        /// <param name="jsonAsBytes">JSON string as bytes.</param>
        /// <returns>JSON string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = "I like this name...")]
        public static string ConvertByteArrayToJson(byte[] jsonAsBytes)
        {
            var ret = SerializationEncoding.GetString(jsonAsBytes);
            return ret;
        }

        /// <inheritdoc />
        public override byte[] SerializeToBytes(object objectToSerialize)
        {
            var jsonString = this.SerializeToString(objectToSerialize);
            var jsonBytes = ConvertJsonToByteArray(jsonString);
            return jsonBytes;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(byte[] serializedBytes)
        {
            var ret = this.Deserialize(serializedBytes, typeof(T));
            return (T)ret;
        }

        /// <inheritdoc />
        public override object Deserialize(byte[] serializedBytes, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var jsonString = ConvertByteArrayToJson(serializedBytes);
            return this.Deserialize(jsonString, type);
        }

        /// <inheritdoc />
        public override string SerializeToString(object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalJsonThrowOnUnregisteredTypeIfAppropriate(objectType);

            var jsonSerializerSettings = objectToSerialize != null && objectType.IsClosedAnonymousType()
                ? this.anonymousWriteSerializationSettings
                : this.jsonConfiguration.BuildJsonSerializerSettings(SerializationDirection.Serialize, this.formattingKind);

            var ret = JsonConvert.SerializeObject(objectToSerialize, jsonSerializerSettings);

            if (this.formattingKind == JsonFormattingKind.Compact)
            {
                ret = ret.Replace(Environment.NewLine, string.Empty);
            }

            return ret;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(string serializedString)
        {
            var objectType = typeof(T);

            this.InternalJsonThrowOnUnregisteredTypeIfAppropriate(objectType);

            var jsonSerializerSettings = this.jsonConfiguration.BuildJsonSerializerSettings(SerializationDirection.Deserialize, this.formattingKind);
            var ret = JsonConvert.DeserializeObject<T>(serializedString, jsonSerializerSettings);

            return ret;
        }

        /// <inheritdoc />
        public override object Deserialize(string serializedString, Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.InternalJsonThrowOnUnregisteredTypeIfAppropriate(type);

            object ret;
            if (type == typeof(DynamicTypePlaceholder))
            {
                dynamic dyn = JObject.Parse(serializedString);
                ret = dyn;
            }
            else
            {
                var jsonSerializerSettings = this.jsonConfiguration.BuildJsonSerializerSettings(SerializationDirection.Deserialize, this.formattingKind);
                ret = JsonConvert.DeserializeObject(serializedString, type, jsonSerializerSettings);
            }

            return ret;
        }

        private void InternalJsonThrowOnUnregisteredTypeIfAppropriate(Type objectType)
        {
            // this type needs no registration and has value in consistent escaping/encoding...
            if (objectType != typeof(string))
            {
                this.ThrowOnUnregisteredTypeIfAppropriate(objectType);
            }
        }
    }

    /// <inheritdoc />
    public sealed class ObcJsonSerializer<TJsonSerializationConfiguration> : ObcJsonSerializer
        where TJsonSerializationConfiguration : JsonSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcJsonSerializer{TJsonSerializationConfiguration}"/> class.
        /// </summary>
        public ObcJsonSerializer()
            : base(typeof(TJsonSerializationConfiguration))
        {
        }
    }
}
