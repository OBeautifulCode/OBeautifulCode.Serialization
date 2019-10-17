﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FluentAssertions;

    using OBeautifulCode.Compression;
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
            Action jsonAction = () => JsonSerializerFactory.Instance.BuildSerializer(null);

            // Act
            var exception = Record.Exception(action);
            var jsonException = Record.Exception(jsonAction);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'serializationDescription') is null.");
            jsonException.Should().NotBeNull();
            jsonException.Should().BeOfType<ArgumentNullException>();
            jsonException.Message.Should().Be("Provided value (name: 'serializationDescription') is null.");
        }

        [Fact]
        public static void BuildSerializer___Json___Gets_Json_serializer()
        {
            // Arrange
            var serializerDescription = new SerializationDescription(
                SerializationKind.Json,
                SerializationFormat.String,
                null,
                CompressionKind.None);

            // Act
            var serializer = SerializerFactory.Instance.BuildSerializer(serializerDescription);
            var jsonSerializer = JsonSerializerFactory.Instance.BuildSerializer(serializerDescription);

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
            Action bsonAction = () => BsonSerializerFactory.Instance.BuildSerializer(null);

            // Act
            var exception = Record.Exception(action);
            var bsonException = Record.Exception(bsonAction);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'serializationDescription') is null.");
            bsonException.Should().NotBeNull();
            bsonException.Should().BeOfType<ArgumentNullException>();
            bsonException.Message.Should().Be("Provided value (name: 'serializationDescription') is null.");
        }

        [Fact]
        public static void BuildSerializer___Bson___Gets_Bson_serializer()
        {
            // Arrange
            var expectedConfigType = typeof(NullBsonConfiguration);
            var serializerDescription = new SerializationDescription(
                SerializationKind.Bson,
                SerializationFormat.String,
                expectedConfigType.ToRepresentation());

            // Act
            var serializer = SerializerFactory.Instance.BuildSerializer(serializerDescription);
            var bsonSerializer = BsonSerializerFactory.Instance.BuildSerializer(serializerDescription);

            // Assert
            serializer.Should().NotBeNull();
            serializer.Should().BeOfType<ObcBsonSerializer>();
            serializer.ConfigurationType.Should().NotBeNull();
            serializer.ConfigurationType.Should().Be(expectedConfigType);

            bsonSerializer.Should().NotBeNull();
            bsonSerializer.Should().BeOfType<ObcBsonSerializer>();
            bsonSerializer.ConfigurationType.Should().NotBeNull();
            bsonSerializer.ConfigurationType.Should().Be(expectedConfigType);
        }

        [Fact]
        public static void SerializationDescriptionToSerializerFactory_BuildSerializer___Works_for_matching_description()
        {
            // Arrange
            var configType = typeof(GenericDiscoveryJsonConfiguration<string>);
            var serializerDescription = new SerializationDescription(
                SerializationKind.Json,
                SerializationFormat.String,
                configType.ToRepresentation());

            var seededSerializer = new ObcJsonSerializer(configType);

            var factory = new SerializationDescriptionToSerializerFactory(serializerDescription, seededSerializer);

            // Act
            var actualSerializer = factory.BuildSerializer(serializerDescription);

            // Assert
            actualSerializer.Should().BeSameAs(seededSerializer);
        }

        [Fact]
        public static void SerializationDescriptionToSerializerFactory_BuildSerializer___Throws_for_nonmatching_description()
        {
            // Arrange
            var configType = typeof(GenericDiscoveryJsonConfiguration<string>);
            var serializerDescription = new SerializationDescription(
                SerializationKind.Json,
                SerializationFormat.String,
                configType.ToRepresentation());

            var seededSerializer = new ObcJsonSerializer(configType);

            var factory = new SerializationDescriptionToSerializerFactory(serializerDescription, seededSerializer);

            var invalidDescription = new SerializationDescription(SerializationKind.Bson, SerializationFormat.Binary);

            // Act
            var exception = Record.Exception(() => factory.BuildSerializer(invalidDescription));

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<NotSupportedException>();
            exception.Message.Should().StartWith("Supplied 'serializationDescription' (SerializationDescription: SerializationKind=Bson, SerializationFormat=Binary, CompressionKind=None, ConfigurationTypeRepresentation=, Metadata=,) does not match 'supportedSerializationDescription' (SerializationDescription: SerializationKind=Json, SerializationFormat=String, CompressionKind=None, ConfigurationTypeRepresentation=Representation.TypeRepresentation: Namespace = OBeautifulCode.Serialization.Json, Name = GenericDiscoveryJsonConfiguration`1, AssemblyQualifiedName = OBeautifulCode.Serialization.Json.GenericDiscoveryJsonConfiguration`1, OBeautifulCode.Serialization.Json");
        }
    }
}