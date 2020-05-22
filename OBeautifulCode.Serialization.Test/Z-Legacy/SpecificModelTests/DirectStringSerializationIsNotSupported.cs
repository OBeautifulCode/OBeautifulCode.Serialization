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
        public static void BsonSerializationConfiguration()
        {
            // Arrange, Act
            var actual = Record.Exception(() => SerializationConfigurationManager.GetOrAddSerializationConfiguration(typeof(TypesToRegisterBsonSerializationConfiguration<string>).ToBsonSerializationConfigurationType()));

            // Assert
            actual.Should().BeOfType<InvalidOperationException>();
            actual.Message.Should().Contain("attempting to register the following type which cannot be registered: string");
        }

        [Fact]
        public static void JsonSerializationConfiguration()
        {
            // Arrange, Act
            var actual = Record.Exception(() => SerializationConfigurationManager.GetOrAddSerializationConfiguration(typeof(TypesToRegisterJsonSerializationConfiguration<string>).ToJsonSerializationConfigurationType()));

            // Assert
            actual.Should().BeOfType<InvalidOperationException>();
            actual.Message.Should().Contain("attempting to register the following type which cannot be registered: string");
        }
    }
}
