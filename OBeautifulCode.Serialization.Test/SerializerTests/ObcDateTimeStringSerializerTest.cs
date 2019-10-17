// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcDateTimeStringSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Globalization;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    public static class ObcDateTimeStringSerializerTest
    {
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_utc___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow;
            var serializer = new ObcDateTimeStringSerializer();

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
            var serializer = new ObcDateTimeStringSerializer();

            // Act
            var serialized = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<DateTime>(serialized);

            // Assert
            actual.Kind.Should().Be(expected.Kind);
            actual.Should().Be(expected);
        }

        [Fact]
        public static void Deserialize___Using_UTC_reduced_precision___Works()
        {
            // Arrange
            var serializedDateTimes = new[]
            {
                "2017-05-06T02:28:46.2704883Z",
                "2017-05-06T02:28:46.270484Z",
                "2017-05-06T02:28:46.27048Z",
                "2017-05-06T02:28:46.2704Z",
                "2017-05-06T02:28:46.271Z",
                "2017-05-06T02:28:46.27Z",
                "2017-05-06T02:28:46.2Z",
                "2017-05-06T02:28:46Z",
                "2017-05-06T02:28:00Z",
                "2017-05-06T02:00:00Z",
                "2017-05-06T00:00:00Z",
            };

            var expected = serializedDateTimes.Select(_ => DateTime.Parse(_, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal)).ToList();

            var serializer = new ObcDateTimeStringSerializer();

            // Act
            var actual = serializedDateTimes.Select(_ => serializer.Deserialize<DateTime>(_));

            // Assert
            actual.Should().Equal(expected);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_local_zero_offset___Works()
        {
            // Arrange
            var expected = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            var serializer = new ObcDateTimeStringSerializer();

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
            var serializer = new ObcDateTimeStringSerializer();

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
            var serializer = new ObcDateTimeStringSerializer();

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
            var serializer = new ObcDateTimeStringSerializer();
            Action action = () => serializer.SerializeToString("not a datetime");

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Provided value (name: 'typeMustBeDateTimeOrNullableDateTime-System.String') is not true.  Provided value is 'False'.");
        }

        [Fact]
        public static void Deserialize___Null_type___Throws()
        {
            // Arrange
            var serializer = new ObcDateTimeStringSerializer();
            Action action = () => serializer.Deserialize(string.Empty, null);

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'type') is null.");
        }
    }
}