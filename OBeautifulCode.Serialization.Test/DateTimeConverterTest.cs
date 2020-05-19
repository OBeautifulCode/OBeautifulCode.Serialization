// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeConverterTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FluentAssertions;

    using Xunit;

    public static class DateTimeConverterTest
    {
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_utc___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow;

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallback(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_unspecified___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow.ToUnspecified();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallback(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_zero_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallback(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_positive_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time"));

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallback(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_negative_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, DateTime deserialized)
            {
                deserialized.Kind.Should().Be(expected.Kind);
                deserialized.Should().Be(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallback(ThrowIfObjectsDiffer);
        }
    }
}