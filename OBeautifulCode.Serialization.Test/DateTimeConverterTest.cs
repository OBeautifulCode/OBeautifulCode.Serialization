// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeConverterTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class DateTimeConverterTest
    {
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_utc___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow;
            var serializer = new ObcJsonSerializer();

            // Act
            var serialized = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<DateTime>(serialized);

            // Assert
            actual.Kind.Should().Be(expected.Kind);
            actual.Should().Be(expected);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_unspecified___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow.ToUnspecified();
            var serializer = new ObcJsonSerializer();

            // Act
            var serialized = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<DateTime>(serialized);

            // Assert
            actual.Kind.Should().Be(expected.Kind);
            actual.Should().Be(expected);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_zero_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var serializer = new ObcJsonSerializer();

            // Act
            var serialized = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<DateTime>(serialized);

            // Assert
            actual.Kind.Should().Be(expected.Kind);
            actual.Should().Be(expected);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_positive_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time"));
            var serializer = new ObcJsonSerializer();

            // Act
            var serialized = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<DateTime>(serialized);

            // Assert
            actual.Kind.Should().Be(expected.Kind);
            actual.Should().Be(expected);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_negative_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            var serializer = new ObcJsonSerializer();

            // Act
            var serialized = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<DateTime>(serialized);

            // Assert
            actual.Kind.Should().Be(expected.Kind);
            actual.Should().Be(expected);
        }

        [Fact]
        public static void Serialize___Not_date_time___Throws()
        {
            // Arrange
            var serializer = new ObcJsonSerializer();
            Action action = () => serializer.SerializeToString("not a datetime");

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Parameter 'typeMustBeDateTimeOrNullableDateTime-System.String' is not true.  Parameter value is 'False'.");
        }

        [Fact]
        public static void Deserialize___Null_type___Throws()
        {
            // Arrange
            var serializer = new ObcJsonSerializer();
            Action action = () => serializer.Deserialize(string.Empty, null);

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Parameter 'type' is null.");
        }
    }
}