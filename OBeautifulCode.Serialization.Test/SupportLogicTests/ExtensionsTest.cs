// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsTest.cs" company="OBeautifulCode">
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
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class ExtensionsTest
    {
        [Fact]
        public static void ToDescribedSerializationWithSpecificFactory___Null_serializer_representation___Throws()
        {
            // Arrange
            Action action = () => DomainExtensions.ToDescribedSerializationUsingSpecificFactory(
                A.Dummy<string>(),
                null,
                SerializerFactory.Instance,
                A.Dummy<SerializationFormat>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'serializerDescription') is null.");
        }

        [Fact]
        public static void ToDescribedSerializationUsingSpecificFactory___Null_serializer_factory___Throws()
        {
            // Arrange
            Action action = () => DomainExtensions.ToDescribedSerializationUsingSpecificFactory(
                A.Dummy<string>(),
                A.Dummy<SerializerRepresentation>(),
                null,
                A.Dummy<SerializationFormat>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'serializerFactory') is null.");
        }

        [Fact]
        public static void ToDescribedSerializationWithSpecificFactory___Null_object___Works()
        {
            // Arrange
            string objectToPackageIntoDescribedSerialization = null;
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);

            // Act
            var describedSerialization = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificFactory(
                serializerRepresentation,
                SerializerFactory.Instance,
                SerializationFormat.String);

            // Assert
            describedSerialization.Should().NotBeNull();
            describedSerialization.PayloadTypeRepresentation.Should().Be(typeof(string).ToRepresentation());
            describedSerialization.SerializedPayload.Should().Be("null");
            describedSerialization.SerializerRepresentation.Should().Be(serializerRepresentation);
        }

        [Fact]
        public static void ToDescribedSerializationWithSpecificSerializer___Valid_object_and_serializer___Works()
        {
            // Arrange
            string objectToPackageIntoDescribedSerialization = null;
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);

            // Act
            var describedSerialization = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificFactory(
                serializerRepresentation,
                SerializerFactory.Instance,
                SerializationFormat.String);

            // Assert
            describedSerialization.Should().NotBeNull();
            describedSerialization.PayloadTypeRepresentation.Should().Be(typeof(string).ToRepresentation());
            describedSerialization.SerializedPayload.Should().Be("null");
            describedSerialization.SerializerRepresentation.Should().Be(serializerRepresentation);
        }

        [Fact]
        public static void ToDescribedSerializationWithSpecificFactory___All_valid___Works()
        {
            // Arrange
            var objectToPackageIntoDescribedSerialization = A.Dummy<string>();
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);

            // Act
            var describedSerialization = DomainExtensions.ToDescribedSerializationUsingSpecificFactory(
                objectToPackageIntoDescribedSerialization,
                serializerRepresentation,
                SerializerFactory.Instance,
                SerializationFormat.String);

            // Assert
            describedSerialization.Should().NotBeNull();
            describedSerialization.PayloadTypeRepresentation.Should().Be(objectToPackageIntoDescribedSerialization.GetType().ToRepresentation());
            describedSerialization.SerializedPayload.Should().Be("\"" + objectToPackageIntoDescribedSerialization + "\"");
            describedSerialization.SerializerRepresentation.Should().Be(serializerRepresentation);
        }

        [Fact]
        public static void FromDescribedSerializationWithSpecificFactory___Null_object___Works()
        {
            // Arrange
            string expected = null;
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);
            var payload = "null";
            var describedSerialization = new DescribedSerialization(
                typeof(string).ToRepresentation(),
                payload,
                serializerRepresentation,
                SerializationFormat.String);

            // Act
            var actual = DomainExtensions.DeserializePayloadUsingSpecificFactory(
                describedSerialization,
                SerializerFactory.Instance);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public static void FromDescribedSerializationWithSpecificFactory___All_valid___Works()
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
            var actual = DomainExtensions.DeserializePayloadUsingSpecificFactory(
                describedSerialization,
                SerializerFactory.Instance);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
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
