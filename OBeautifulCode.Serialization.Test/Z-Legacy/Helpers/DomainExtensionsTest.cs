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
            exception.Message.Should().Contain("serializerRepresentation");
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
    }
}
