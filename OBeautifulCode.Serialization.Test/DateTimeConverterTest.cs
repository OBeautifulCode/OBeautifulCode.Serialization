﻿// --------------------------------------------------------------------------------------------------------------------
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
    }
}