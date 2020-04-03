// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfirmSameInheritorsUsedForDependencies.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class ConfirmSameInheritorsUsedForDependencies
    {
        [Fact]
        public static void SerializationConfigurationManagerDoesNotAllow()
        {
            // Arrange
            var config = typeof(SameInheritorJsonConfig);
            Action action = () => SerializationConfigurationManager
                .Configure(config);

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidOperationException>();
            exception.Message.Should().Be("Configuration OBeautifulCode.Serialization.Test.SameInheritorJsonConfig has DependentSerializationConfigurationTypes (OBeautifulCode.Serialization.Test.SameInheritorBsonConfigA) that do not share the same first layer of inheritance OBeautifulCode.Serialization.Json.JsonSerializationConfigurationBase.");
        }
    }

    public class SameInheritorJsonConfig : JsonSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentSerializationConfigurationTypes =>
            new[] { typeof(SameInheritorJsonConfigA), typeof(SameInheritorBsonConfigA) };
    }

    public class SameInheritorBsonConfigA : BsonSerializationConfigurationBase
    {
    }

    public class SameInheritorJsonConfigA : JsonSerializationConfigurationBase
    {
    }
}