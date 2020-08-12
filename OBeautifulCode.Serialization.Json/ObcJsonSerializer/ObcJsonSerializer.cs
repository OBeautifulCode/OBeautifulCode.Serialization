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

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Json.Internal;

    using static System.FormattableString;

    /// <summary>
    /// JSON serializer.
    /// </summary>
    /// <remarks>
    /// Here is the source of Newtonsoft.Json 9.0.1: <a href="https://github.com/JamesNK/Newtonsoft.Json/tree/e5ac9a8473dfdefb8fe2cddae433a9aaa94a5b37"/>.
    /// </remarks>
    public class ObcJsonSerializer : ObcSerializerBase
    {
        private static readonly Encoding SerializationEncoding = Encoding.UTF8;

        private readonly JsonSerializationConfigurationBase jsonSerializationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcJsonSerializer"/> class.
        /// </summary>
        /// <param name="jsonSerializationConfigurationType">Optional type of configuration to use; DEFAULT is none.</param>
        public ObcJsonSerializer(
            JsonSerializationConfigurationType jsonSerializationConfigurationType = null)
            : base(jsonSerializationConfigurationType ?? typeof(NullJsonSerializationConfiguration).ToJsonSerializationConfigurationType())
        {
            this.jsonSerializationConfiguration = (JsonSerializationConfigurationBase)this.SerializationConfiguration;

            if (this.jsonSerializationConfiguration.JsonFormattingKind == JsonFormattingKind.Invalid)
            {
                throw new ArgumentOutOfRangeException(Invariant($"'{nameof(this.jsonSerializationConfiguration.JsonFormattingKind)}' == '{JsonFormattingKind.Invalid}'"), (Exception)null);
            }

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
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

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

            var jsonSerializerSettings = this.jsonSerializationConfiguration.BuildJsonSerializerSettings(SerializationDirection.Serialize, this.jsonSerializationConfiguration);

            var result = JsonConvert.SerializeObject(objectToSerialize, jsonSerializerSettings);

            if (this.jsonSerializationConfiguration.JsonFormattingKind == JsonFormattingKind.Compact)
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

            var jsonSerializerSettings = this.jsonSerializationConfiguration.BuildJsonSerializerSettings(SerializationDirection.Deserialize, this.jsonSerializationConfiguration);

            var result = JsonConvert.DeserializeObject<T>(serializedString, jsonSerializerSettings);

            return result;
        }

        /// <inheritdoc />
        public override object Deserialize(
            string serializedString,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.InternalJsonThrowOnUnregisteredTypeIfAppropriate(type, SerializationDirection.Deserialize, null);

            object result;

            if (type == typeof(DynamicTypePlaceholder))
            {
                dynamic dyn = JObject.Parse(serializedString);

                result = dyn;
            }
            else
            {
                var jsonSerializerSettings = this.jsonSerializationConfiguration.BuildJsonSerializerSettings(SerializationDirection.Deserialize, this.jsonSerializationConfiguration);

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
