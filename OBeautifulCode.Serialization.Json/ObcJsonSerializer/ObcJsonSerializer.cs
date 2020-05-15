// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcJsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Json.Internal;
    using OBeautifulCode.Type.Recipes;

    /// <summary>
    /// JSON serializer.
    /// </summary>
    public class ObcJsonSerializer : ObcSerializerBase
    {
        private static readonly Encoding SerializationEncoding = Encoding.UTF8;

        private readonly JsonSerializerSettings anonymousWriteSerializationSettings;

        private readonly JsonSerializationConfigurationBase jsonConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcJsonSerializer"/> class.
        /// </summary>
        /// <param name="jsonSerializationConfigurationType">Optional type of configuration to use; DEFAULT is none.</param>
        public ObcJsonSerializer(
            JsonSerializationConfigurationType jsonSerializationConfigurationType = null)
            : base(jsonSerializationConfigurationType ?? typeof(NullJsonSerializationConfiguration).ToJsonSerializationConfigurationType())
        {
            this.jsonConfiguration = (JsonSerializationConfigurationBase)this.SerializationConfiguration;

            new { this.jsonConfiguration.JsonFormattingKind }.AsArg().Must().NotBeEqualTo(JsonFormattingKind.Invalid);

            this.anonymousWriteSerializationSettings = this.jsonConfiguration.BuildAnonymousJsonSerializerSettings(SerializationDirection.Serialize, this.jsonConfiguration.JsonFormattingKind);

            this.SerializerRepresentation = new SerializerRepresentation(SerializationKind.Json, jsonSerializationConfigurationType?.ConcreteSerializationConfigurationDerivativeType.ToRepresentation());
        }

        /// <inheritdoc />
        public override SerializationKind SerializationKind => SerializationKind.Json;

        /// <inheritdoc />
        public override SerializerRepresentation SerializerRepresentation { get; }

        /// <summary>
        /// Converts JSON string into a byte array.
        /// </summary>
        /// <param name="json">JSON string.</param>
        /// <returns>
        /// Byte array.
        /// </returns>
        public static byte[] ConvertJsonToByteArray(
            string json)
        {
            var result = SerializationEncoding.GetBytes(json);

            return result;
        }

        /// <summary>
        /// Converts JSON byte array into a string.
        /// </summary>
        /// <param name="jsonAsBytes">JSON string as bytes.</param>
        /// <returns>
        /// JSON string.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static string ConvertByteArrayToJson(
            byte[] jsonAsBytes)
        {
            var result = SerializationEncoding.GetString(jsonAsBytes);

            return result;
        }

        /// <inheritdoc />
        public override byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var jsonString = this.SerializeToString(objectToSerialize);

            var result = ConvertJsonToByteArray(jsonString);

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            byte[] serializedBytes)
        {
            var result = this.Deserialize(serializedBytes, typeof(T));

            return (T)result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var jsonString = ConvertByteArrayToJson(serializedBytes);

            var result = this.Deserialize(jsonString, type);

            return result;
        }

        /// <inheritdoc />
        public override string SerializeToString(
            object objectToSerialize)
        {
            var objectType = objectToSerialize?.GetType();

            this.InternalJsonThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Serialize, objectToSerialize);

            var jsonSerializerSettings = (objectToSerialize != null) && objectType.IsClosedAnonymousType()
                ? this.anonymousWriteSerializationSettings
                : this.jsonConfiguration.BuildJsonSerializerSettings(SerializationDirection.Serialize, this.jsonConfiguration);

            var result = JsonConvert.SerializeObject(objectToSerialize, jsonSerializerSettings);

            if (this.jsonConfiguration.JsonFormattingKind == JsonFormattingKind.Compact)
            {
                result = result.Replace(Environment.NewLine, string.Empty);
            }

            return result;
        }

        /// <inheritdoc />
        public override T Deserialize<T>(
            string serializedString)
        {
            var objectType = typeof(T);

            this.InternalJsonThrowOnUnregisteredTypeIfAppropriate(objectType, SerializationDirection.Deserialize, null);

            var jsonSerializerSettings = this.jsonConfiguration.BuildJsonSerializerSettings(SerializationDirection.Deserialize, this.jsonConfiguration);

            var result = JsonConvert.DeserializeObject<T>(serializedString, jsonSerializerSettings);

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            string serializedString,
            Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            this.InternalJsonThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            object result;

            if (type == typeof(DynamicTypePlaceholder))
            {
                dynamic dyn = JObject.Parse(serializedString);

                result = dyn;
            }
            else
            {
                var jsonSerializerSettings = this.jsonConfiguration.BuildJsonSerializerSettings(SerializationDirection.Deserialize, this.jsonConfiguration);

                result = JsonConvert.DeserializeObject(serializedString, type, jsonSerializerSettings);
            }

            return result;
        }

        private void InternalJsonThrowOnUnregisteredTypeIfAppropriate(
            Type objectType,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            // this type needs no registration and has value in consistent escaping/encoding...
            if (objectType != typeof(string))
            {
                this.SerializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(objectType, serializationDirection, objectToSerialize);
            }
        }
    }
}
