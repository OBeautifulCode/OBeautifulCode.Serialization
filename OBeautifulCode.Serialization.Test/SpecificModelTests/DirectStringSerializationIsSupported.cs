// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectStringSerializationIsSupported.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using FakeItEasy;
    using FluentAssertions;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using Xunit;

    public static class DirectStringSerializationIsSupported
    {
        [Fact]
        public static void BsonSerializeStringToString()
        {
            // Arrange
            var bsonSerializer = new ObcBsonSerializer<GenericDiscoveryBsonSerializationConfiguration<string>>();
            var input = A.Dummy<string>();

            // Act
            var bsonStringException = Record.Exception(() => bsonSerializer.SerializeToString(input));

            // Assert
            bsonStringException.Should().BeOfType<NotSupportedException>();
        }

        [Fact]
        public static void BsonSerializeStringToBytes()
        {
            // Arrange
            var bsonSerializer = new ObcBsonSerializer<GenericDiscoveryBsonSerializationConfiguration<string>>();
            var input = A.Dummy<string>();

            // Act
            var bsonStringException = Record.Exception(() => bsonSerializer.SerializeToBytes(input));

            // Assert
            bsonStringException.Should().BeOfType<NotSupportedException>();
        }

        [Fact]
        public static void JsonSerializeStringToString()
        {
            // Arrange
            var jsonSerializer = new ObcJsonSerializer<GenericDiscoveryJsonSerializationConfiguration<string>>();
            var input = A.Dummy<string>();

            // Act
            var actualJsonString = jsonSerializer.SerializeToString(input);
            var actualJsonFromString = jsonSerializer.Deserialize<string>(actualJsonString);

            // Assert
            actualJsonFromString.Should().Be(input);
        }

        [Fact]
        public static void JsonSerializeStringToBytes()
        {
            // Arrange
            var jsonSerializer = new ObcJsonSerializer<GenericDiscoveryJsonSerializationConfiguration<string>>();
            var input = A.Dummy<string>();

            // Act
            var actualJsonBytes = jsonSerializer.SerializeToBytes(input);
            var actualJsonFromBytes = jsonSerializer.Deserialize<string>(actualJsonBytes);

            // Assert
            actualJsonFromBytes.Should().Be(input);
        }
    }
}
