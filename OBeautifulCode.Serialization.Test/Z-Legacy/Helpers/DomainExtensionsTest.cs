// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class DomainExtensionsTest
    {
        [Fact]
        public static void ToDescribedSerializationWithSpecificFactory___Null_serializer_representation___Throws()
        {
            // Arrange
            Action action = () => A.Dummy<string>().ToDescribedSerializationUsingSpecificFactory(
                null,
                SerializerFactories.Standard,
                A.Dummy<SerializationFormat>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Contain("serializerRepresentation");
        }

        [Fact]
        public static void ToDescribedSerializationUsingSpecificFactory___Null_serializer_factory___Throws()
        {
            // Arrange
            Action action = () => A.Dummy<string>().ToDescribedSerializationUsingSpecificFactory(
                A.Dummy<SerializerRepresentation>(),
                null,
                A.Dummy<SerializationFormat>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Contain("serializerFactory");
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
                SerializerFactories.Standard,
                SerializationFormat.String);

            // Assert
            describedSerialization.Should().NotBeNull();
            describedSerialization.Should().BeOfType<StringDescribedSerialization>();
            describedSerialization.PayloadTypeRepresentation.Should().Be(typeof(string).ToRepresentation());
            ((StringDescribedSerialization)describedSerialization).SerializedPayload.Should().Be("null");
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
                SerializerFactories.Standard,
                SerializationFormat.String);

            // Assert
            describedSerialization.Should().NotBeNull();
            describedSerialization.Should().BeOfType<StringDescribedSerialization>();
            describedSerialization.PayloadTypeRepresentation.Should().Be(typeof(string).ToRepresentation());
            ((StringDescribedSerialization)describedSerialization).SerializedPayload.Should().Be("null");
            describedSerialization.SerializerRepresentation.Should().Be(serializerRepresentation);
        }

        [Fact]
        public static void ToDescribedSerializationWithSpecificFactory___All_valid___Works()
        {
            // Arrange
            var objectToPackageIntoDescribedSerialization = A.Dummy<string>();
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);

            // Act
            var describedSerialization = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificFactory(
                serializerRepresentation,
                SerializerFactories.Standard,
                SerializationFormat.String);

            // Assert
            describedSerialization.Should().NotBeNull();
            describedSerialization.Should().BeOfType<StringDescribedSerialization>();
            describedSerialization.PayloadTypeRepresentation.Should().Be(objectToPackageIntoDescribedSerialization.GetType().ToRepresentation());
            ((StringDescribedSerialization)describedSerialization).SerializedPayload.Should().Be("\"" + objectToPackageIntoDescribedSerialization + "\"");
            describedSerialization.SerializerRepresentation.Should().Be(serializerRepresentation);
        }

        [Fact]
        public static void FromDescribedSerializationWithSpecificFactory___Null_object___Works()
        {
            // Arrange
            string expected = null;
            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, typeof(NullJsonSerializationConfiguration).ToRepresentation(), CompressionKind.None);
            var payload = "null";
            var describedSerialization = new StringDescribedSerialization(
                typeof(string).ToRepresentation(),
                serializerRepresentation,
                payload);

            // Act
            var actual = describedSerialization.DeserializePayloadUsingSpecificFactory(SerializerFactories.Standard);

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
            var describedSerialization = new StringDescribedSerialization(
                expected.GetType().ToRepresentation(),
                serializerRepresentation,
                payload);

            // Act
            var actual = describedSerialization.DeserializePayloadUsingSpecificFactory(SerializerFactories.Standard);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }
    }
}
