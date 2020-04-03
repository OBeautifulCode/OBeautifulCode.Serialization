// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Bson;

    using Xunit;

    using static System.FormattableString;

    public static class ObcBsonSerializerTest
    {
        [Fact]
        public static void ObcBsonSerializer___Invalid_configuration_type___Throws()
        {
            // Arrange
            Action action = () => new ObcBsonSerializer(typeof(string));

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Provided value (name: 'typeMustBeSubclassOfOBeautifulCode.Serialization.SerializationConfigurationBase') is not true.  Provided value is 'False'.");
        }

        [Fact]
        public static void Constructor___Type_without_default_constructor___Throws()
        {
            // Arrange
            Action action = () =>
            {
                new ObcBsonSerializer(typeof(CustomNoPublicConstructor));
            };

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Provided value (name: 'typeHasParameterLessConstructor') is not true.  Provided value is 'False'.");
        }
    }
}
