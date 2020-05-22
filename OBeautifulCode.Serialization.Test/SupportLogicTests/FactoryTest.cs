// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Compression;
    using OBeautifulCode.Compression.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class FactoryTest
    {
        [Fact]
        public static void BuildSerializer___Null_description___Throws()
        {
            // Arrange
            Action action = () => SerializerFactory.Instance.BuildSerializer(null);
            Action jsonAction = () => new JsonSerializerFactory(CompressorFactory.Instance).BuildSerializer(null);

            // Act
            var exception = Record.Exception(action);
            var jsonException = Record.Exception(jsonAction);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'serializerRepresentation') is null.");
            jsonException.Should().NotBeNull();
            jsonException.Should().BeOfType<ArgumentNullException>();
            jsonException.Message.Should().Be("Provided value (name: 'serializerRepresentation') is null.");
        }

        [Fact]
        public static void BuildSerializer___Json___Gets_Json_serializer()
        {
            // Arrange
            var serializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                null,
                CompressionKind.None);

            // Act
            var serializer = SerializerFactory.Instance.BuildSerializer(serializerRepresentation);
            var jsonSerializer = new JsonSerializerFactory(CompressorFactory.Instance).BuildSerializer(serializerRepresentation);

            // Assert
            serializer.Should().NotBeNull();
            serializer.Should().BeOfType<ObcJsonSerializer>();
            jsonSerializer.Should().NotBeNull();
            jsonSerializer.Should().BeOfType<ObcJsonSerializer>();
        }

        [Fact]
        public static void BuildSerializer___Bson___Null_description___Throws()
        {
            // Arrange
            Action action = () => SerializerFactory.Instance.BuildSerializer(null);
            Action bsonAction = () => new BsonSerializerFactory(CompressorFactory.Instance).BuildSerializer(null);

            // Act
            var exception = Record.Exception(action);
            var bsonException = Record.Exception(bsonAction);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'serializerRepresentation') is null.");
            bsonException.Should().NotBeNull();
            bsonException.Should().BeOfType<ArgumentNullException>();
            bsonException.Message.Should().Be("Provided value (name: 'serializerRepresentation') is null.");
        }

        [Fact]
        public static void BuildSerializer___Bson___Gets_Bson_serializer()
        {
            // Arrange
            var expectedConfigType = typeof(NullBsonSerializationConfiguration);

            var serializerRepresentation = new SerializerRepresentation(
                SerializationKind.Bson,
                expectedConfigType.ToRepresentation());

            // Act
            var serializer = SerializerFactory.Instance.BuildSerializer(serializerRepresentation);
            var bsonSerializer = new BsonSerializerFactory(CompressorFactory.Instance).BuildSerializer(serializerRepresentation);

            // Assert
            serializer.Should().NotBeNull();
            serializer.Should().BeOfType<ObcBsonSerializer>();
            serializer.SerializationConfigurationType.Should().NotBeNull();
            serializer.SerializationConfigurationType.Should().Be(expectedConfigType.ToBsonSerializationConfigurationType());

            bsonSerializer.Should().NotBeNull();
            bsonSerializer.Should().BeOfType<ObcBsonSerializer>();
            bsonSerializer.SerializationConfigurationType.Should().NotBeNull();
            bsonSerializer.SerializationConfigurationType.Should().Be(expectedConfigType.ToBsonSerializationConfigurationType());
        }

        [Fact]
        public static void ToDescribedSerialization___Valid___Just_passes_to_specific_factory_version_with_default_factory()
        {
            // Arrange
            var objectToPackageIntoDescribedSerialization = A.Dummy<string>();
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);

            // Act
            var describedSerialization = objectToPackageIntoDescribedSerialization.ToDescribedSerialization(serializerRepresentation, SerializationFormat.String);

            // Assert
            describedSerialization.Should().NotBeNull();
            describedSerialization.PayloadTypeRepresentation.Should().Be(objectToPackageIntoDescribedSerialization.GetType().ToRepresentation());
            describedSerialization.SerializedPayload.Should().Be("\"" + objectToPackageIntoDescribedSerialization + "\"");
            describedSerialization.SerializerRepresentation.Should().Be(serializerRepresentation);
        }

        [Fact]
        public static void FromDescribedSerialization___Valid___Just_passes_to_specific_factory_version_with_default_factory()
        {
            // Arrange
            var expected = A.Dummy<string>();
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);
            var payload = "\"" + expected + "\"";
            var describedSerialization = new DescribedSerialization(
                expected.GetType().ToRepresentation(),
                payload,
                serializerRepresentation,
                SerializationFormat.String);

            // Act
            var actual = describedSerialization.DeserializePayload();

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }

        [Fact]
        public static void FromDescribedSerializationGeneric___Valid___Just_passes_to_specific_factory_version_with_default_factory()
        {
            // Arrange
            var expected = A.Dummy<string>();
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);
            var payload = "\"" + expected + "\"";
            var describedSerialization = new DescribedSerialization(
                expected.GetType().ToRepresentation(),
                payload,
                serializerRepresentation,
                SerializationFormat.String);

            // Act
            var actual = describedSerialization.DeserializePayload<string>();

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }

        [Fact]
        public static void AnonymousObject___Can_be_round_tripped_back_into_a_dynamic()
        {
            // Arrange
            var input = new { Item = "item", Items = new[] { "item1", "item2" } };
            var serializerRepresentationJson = new SerializerRepresentation(SerializationKind.Json);
            var serializerRepresentationBson = new SerializerRepresentation(SerializationKind.Bson);

            // Act
            var serializedJson = input.ToDescribedSerialization(serializerRepresentationJson, SerializationFormat.String);
            dynamic deserializedJson = serializedJson.DeserializePayload();

            var serializedBson = input.ToDescribedSerialization(serializerRepresentationBson, SerializationFormat.String);
            dynamic deserializedBson = serializedBson.DeserializePayload();

            // Assert
            ((string)deserializedJson.Item).Should().Be(input.Item);
            ((string)deserializedJson.Items[0]).Should().Be(input.Items[0]);
            ((string)deserializedJson.Items[1]).Should().Be(input.Items[1]);

            ((string)deserializedBson.Item).Should().Be(input.Item);
            ((string)deserializedBson.Items[0]).Should().Be(input.Items[0]);
            ((string)deserializedBson.Items[1]).Should().Be(input.Items[1]);
        }
    }
}
