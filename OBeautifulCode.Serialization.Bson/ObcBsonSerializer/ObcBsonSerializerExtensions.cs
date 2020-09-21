// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonSerializerExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;

    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// Helper class for using <see cref="BsonSerializer"/>.
    /// </summary>
    public static class ObcBsonSerializerExtensions
    {
        /// <summary>
        /// Serializes an object into a byte array.
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>
        /// Serialized object into a byte array.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = ObcSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public static byte[] SerializeToBytes(
            this object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentNullException(nameof(objectToSerialize));
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new BsonBinaryWriter(memoryStream))
                {
                    BsonSerializer.Serialize(writer, objectToSerialize.GetType(), objectToSerialize);

                    writer.Close();

                    memoryStream.Close();

                    var result = memoryStream.ToArray();

                    return result;
                }
            }
        }

        /// <summary>
        /// Serializes an object into a <see cref="BsonDocument"/>.
        /// </summary>
        /// <param name="objectToSerialize">Object to serialize.</param>
        /// <returns>
        /// Serialized object into a byte array.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = ObcSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public static BsonDocument SerializeToDocument(
            this object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentNullException(nameof(objectToSerialize));
            }

            var result = new BsonDocument();

            using (var writer = new BsonDocumentWriter(result))
            {
                BsonSerializer.Serialize(writer, objectToSerialize.GetType(), objectToSerialize);

                writer.Close();
            }

            return result;
        }

        /// <summary>
        /// Deserializes a <see cref="BsonDocument"/> to an object.
        /// </summary>
        /// <param name="bsonDocumentToDeserialize"><see cref="BsonDocument"/> to deserialize.</param>
        /// <param name="type">Type to deserialize into.</param>
        /// <returns>
        /// Serialized object into a byte array.
        /// </returns>
        public static object DeserializeFromDocument(
            this BsonDocument bsonDocumentToDeserialize,
            Type type)
        {
            if (bsonDocumentToDeserialize == null)
            {
                throw new ArgumentNullException(nameof(bsonDocumentToDeserialize));
            }

            var result = type == typeof(DynamicTypePlaceholder)
                ? BsonSerializer.Deserialize<dynamic>(bsonDocumentToDeserialize)
                : BsonSerializer.Deserialize(bsonDocumentToDeserialize, type);

            return result;
        }

        /// <summary>
        /// Deserializes a <see cref="BsonDocument"/> to an object.
        /// </summary>
        /// <param name="bsonDocumentToDeserialize"><see cref="BsonDocument"/> to deserialize.</param>
        /// <typeparam name="T">Type to deserialize into.</typeparam>
        /// <returns>
        /// Serialized object into a byte array.
        /// </returns>
        public static T DeserializeFromDocument<T>(
            this BsonDocument bsonDocumentToDeserialize)
        {
            if (bsonDocumentToDeserialize == null)
            {
                throw new ArgumentNullException(nameof(bsonDocumentToDeserialize));
            }

            var result = BsonSerializer.Deserialize<T>(bsonDocumentToDeserialize);

            return result;
        }

        /// <summary>
        /// Deserializes the byte array into an object.
        /// </summary>
        /// <param name="serializedBytes">Byte array to deserialize.</param>
        /// <typeparam name="T">Type of object to deserialize.</typeparam>
        /// <returns>
        /// Deserialized bytes into object of specified type.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static T Deserialize<T>(
            this byte[] serializedBytes)
        {
            var result = (T)Deserialize(serializedBytes, typeof(T));

            return result;
        }

        /// <summary>
        /// Deserializes the byte array into an object.
        /// </summary>
        /// <param name="serializedBytes">Byte array to deserialize.</param>
        /// <param name="type">Type to deserialize into.</param>
        /// <returns>
        /// Deserialized bytes into object of specified type.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:Identifiers should not contain type names", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = ObcSuppressBecause.CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes)]
        public static object Deserialize(
            this byte[] serializedBytes,
            Type type)
        {
            if (serializedBytes == null)
            {
                throw new ArgumentNullException(nameof(serializedBytes));
            }

            using (var memoryStream = new MemoryStream(serializedBytes))
            {
                using (var reader = new BsonBinaryReader(memoryStream))
                {
                    var result = BsonSerializer.Deserialize(reader, type);

                    reader.Close();

                    memoryStream.Close();

                    return result;
                }
            }
        }

        /// <summary>
        /// Converts a string of JSON into a <see cref="BsonDocument"/>.
        /// </summary>
        /// <param name="json">JSON to convert.</param>
        /// <returns>
        /// Converted JSON into <see cref="BsonDocument"/>.
        /// </returns>
        public static BsonDocument ToBsonDocument(
            this string json)
        {
            var result = BsonDocument.Parse(json);

            return result;
        }
    }
}