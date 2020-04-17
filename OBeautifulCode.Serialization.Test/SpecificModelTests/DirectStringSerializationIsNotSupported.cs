// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectStringSerializationIsNotSupported.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using FluentAssertions;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using Xunit;

    public static class DirectStringSerializationIsNotSupported
    {
        [Fact]
        public static void BsonSerializer()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new ObcBsonSerializer<RegisterOnlyWithDiscoveryBsonSerializationConfiguration<string>>());

            // Assert
            actual.Should().BeOfType<InvalidOperationException>();
            actual.Message.Should().Contain("attempting to register the following type which cannot be registered: string");
        }

        [Fact]
        public static void JsonSerializer()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new ObcJsonSerializer<RegisterOnlyWithDiscoveryJsonSerializationConfiguration<string>>());

            // Assert
            actual.Should().BeOfType<InvalidOperationException>();
            actual.Message.Should().Contain("attempting to register the following type which cannot be registered: string");
        }
    }
}
