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
            exception.Message.Should().Be("Provided value (name: 'serializerDescription') is null.");
            jsonException.Should().NotBeNull();
            jsonException.Should().BeOfType<ArgumentNullException>();
            jsonException.Message.Should().Be("Provided value (name: 'serializerDescription') is null.");
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
            exception.Message.Should().Be("Provided value (name: 'serializerDescription') is null.");
            bsonException.Should().NotBeNull();
            bsonException.Should().BeOfType<ArgumentNullException>();
            bsonException.Message.Should().Be("Provided value (name: 'serializerDescription') is null.");
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
    }
}
