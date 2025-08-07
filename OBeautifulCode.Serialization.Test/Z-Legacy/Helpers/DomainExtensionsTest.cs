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
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class DomainExtensionsTest
    {
        [Fact]
        public static void ToDescribedSerializationWithSpecificFactory___Should_throw_ArgumentNullException___When_parameter_serializerRepresentation_is_null()
        {
            // Arrange
            Action action = () => A.Dummy<string>().ToDescribedSerializationUsingSpecificFactory(
                null,
                A.Dummy<SerializerRepresentationSelectionStrategy>(),
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
        public static void ToDescribedSerializationWithSpecificFactory___Should_throw_ArgumentOutOfRangeException___When_parameter_serializerRepresentationSelectionStrategy_is_Unknown()
        {
            // Arrange
            Action action = () => A.Dummy<string>().ToDescribedSerializationUsingSpecificFactory(
                A.Dummy<SerializerRepresentation>(),
                SerializerRepresentationSelectionStrategy.Unknown,
                SerializerFactories.Standard,
                A.Dummy<SerializationFormat>());

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentOutOfRangeException>();
            exception.Message.Should().Contain("serializerRepresentationSelectionStrategy");
        }

        [Fact]
        public static void ToDescribedSerializationUsingSpecificFactory___Null_serializer_factory___Throws()
        {
            // Arrange
            Action action = () => A.Dummy<string>().ToDescribedSerializationUsingSpecificFactory(
                A.Dummy<SerializerRepresentation>(),
                A.Dummy<SerializerRepresentationSelectionStrategy>(),
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
                A.Dummy<SerializerRepresentationSelectionStrategy>(),
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
                A.Dummy<SerializerRepresentationSelectionStrategy>(),
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
        public static void ToDescribedSerializationWithSpecificFactory___Should_return_DescribedSerializationBase_with_specified_SerializerRepresentation___When_parameter_serializerRepresentationSelectionStrategy_is_UseSpecifiedRepresentation()
        {
            // Arrange
            var objectToPackageIntoDescribedSerialization = A.Dummy<string>();

            var serializerRepresentation = new SerializerRepresentation(
                SerializationKind.Bson,
                compressionKind: CompressionKind.DotNetZip);

            // Act
            var describedSerialization = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificFactory(
                serializerRepresentation,
                SerializerRepresentationSelectionStrategy.UseSpecifiedRepresentation,
                SerializerFactories.Standard,
                SerializationFormat.String);

            // Assert
            describedSerialization.SerializerRepresentation.Should().Be(serializerRepresentation);
        }

        [Fact]
        public static void ToDescribedSerializationWithSpecificFactory___Should_return_DescribedSerializationBase_with_SerializerRepresentation_of_the_serializer_built_by_the_factory___When_parameter_serializerRepresentationSelectionStrategy_is_UseRepresentationOfSerializerBuiltByFactory()
        {
            // Arrange
            var objectToPackageIntoDescribedSerialization = A.Dummy<string>();

            var serializerRepresentation = new SerializerRepresentation(
                SerializationKind.Bson,
                compressionKind: CompressionKind.DotNetZip);

            var expectedSerializerRepresentation = new SerializerRepresentation(
                SerializationKind.Bson,
                typeof(NullBsonSerializationConfiguration).ToRepresentation(),
                compressionKind: CompressionKind.DotNetZip);

            // Act
            var describedSerialization = objectToPackageIntoDescribedSerialization.ToDescribedSerializationUsingSpecificFactory(
                serializerRepresentation,
                SerializerRepresentationSelectionStrategy.UseRepresentationOfSerializerBuiltByFactory,
                SerializerFactories.Standard,
                SerializationFormat.String);

            // Assert
            describedSerialization.SerializerRepresentation.AsTest().Must().BeEqualTo(expectedSerializerRepresentation);
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
                A.Dummy<SerializerRepresentationSelectionStrategy>(),
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
