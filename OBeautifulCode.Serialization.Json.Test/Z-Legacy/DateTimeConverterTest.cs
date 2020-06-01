// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeConverterTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Recipes;
    using OBeautifulCode.Serialization.Test;

    using Xunit;

    public static class DateTimeConverterTest
    {
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_utc___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow;

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_unspecified___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow.ToUnspecified();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_zero_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_positive_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time"));

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_negative_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
        }
    }
}