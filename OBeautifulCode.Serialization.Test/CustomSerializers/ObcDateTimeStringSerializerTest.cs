// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcDateTimeStringSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;

    using Xunit;

    using static System.FormattableString;

    public static class ObcDateTimeStringSerializerTest
    {
        [Fact]
        public static void SerializeToString_object___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            object value = null;

            // Act
            var actual = Record.Exception(() => systemUnderTest.SerializeToString(value));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void SerializeToString_object___Should_throw_ArgumentException___When_parameter_value_is_not_DateTime()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            var values = new[] { new object(), A.Dummy<string>(), A.Dummy<int>(), A.Dummy<Guid>() };

            // Act
            var actual = values.Select(_ => Record.Exception(() => systemUnderTest.SerializeToString(_))).ToList();

            // Assert
            actual.Must().Each().BeOfType<ArgumentException>();
            actual.Select(_ => _.Message).Must().Each().ContainString("objectToSerialize.GetType() != typeof(DateTime);");
        }

        [Fact]
        public static void SerializeToString_object___Should_return_serialized_string_representation_of_value_for_different_DateTimeKind___When_called()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // note that this will return "+00:00" if machine is on UTC time, which is what we expect
            var offset = DateTime.Now.ToString("%K");

            var scenarios = new List<(DateTime Value, string Expected)>
            {
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc), "2019-01-05T12:14:58.1920000Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(1), "2019-01-05T12:14:58.1920001Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20), "2019-01-05T12:14:58.1920020Z"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified), "2019-01-05T12:14:58.1920000"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(1), "2019-01-05T12:14:58.1920001"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(20), "2019-01-05T12:14:58.1920020"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local), "2019-01-05T12:14:58.1920000" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(1), "2019-01-05T12:14:58.1920001" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(20), "2019-01-05T12:14:58.1920020" + offset),
            };

            // Act
            var actuals = scenarios.Select(_ => systemUnderTest.SerializeToString(_.Value)).ToList();

            // Assert
            actuals.Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void SerializeToString_DateTime___Should_return_serialized_string_representation_of_value_for_different_DateTimeKind___When_called()
        {
            // Arrange

            // note that this will return "+00:00" if machine is on UTC time, which is what we expect
            var offset = DateTime.Now.ToString("%K");

            var scenarios = new List<(DateTime Value, string Expected)>
            {
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc), "2019-01-05T12:14:58.1920000Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(1), "2019-01-05T12:14:58.1920001Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20), "2019-01-05T12:14:58.1920020Z"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified), "2019-01-05T12:14:58.1920000"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(1), "2019-01-05T12:14:58.1920001"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(20), "2019-01-05T12:14:58.1920020"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local), "2019-01-05T12:14:58.1920000" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(1), "2019-01-05T12:14:58.1920001" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(20), "2019-01-05T12:14:58.1920020" + offset),
            };

            // Act
            var actuals = scenarios.Select(_ => ObcDateTimeStringSerializer.SerializeToString(_.Value)).ToList();

            // Assert
            actuals.Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void Deserialize_T___Should_throw_ArgumentNullException___When_parameter_serializedString_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // Act
            var actual = Record.Exception(() => systemUnderTest.Deserialize<DateTime>(null));

            // Assert
            actual.Must().BeOfType<ArgumentNullException>();
            actual.Message.Must().ContainString("serializedString");
        }

        [Fact]
        public static void Deserialize_T___Should_throw_ArgumentException___When_parameter_serializedString_is_white_space()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // Act
            var actual = Record.Exception(() => systemUnderTest.Deserialize<DateTime>(Invariant($"  {Environment.NewLine} ")));

            // Assert
            actual.Must().BeOfType<ArgumentException>();
            actual.Message.Must().ContainString("serializedString");
            actual.Message.Must().ContainString("white space");
        }

        [Fact]
        public static void Deserialize_T___Should_throw_ArgumentException___When_type_parameter_is_not_DateTime()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // Act
            var actual1 = Record.Exception(() => systemUnderTest.Deserialize<object>("2019-01-05T12:14:58.1920000Z"));
            var actual2 = Record.Exception(() => systemUnderTest.Deserialize<DateTime?>("2019-01-05T12:14:58.1920000Z"));

            // Assert
            actual1.Must().BeOfType<ArgumentException>();
            actual1.Message.Must().BeEqualTo("type != typeof(DateTime); 'type' is of type 'object'");

            actual2.Must().BeOfType<ArgumentException>();
            actual2.Message.Must().BeEqualTo("type != typeof(DateTime); 'type' is of type 'DateTime?'");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_ArgumentNullException___When_parameter_serializedString_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // Act
            var actual = Record.Exception(() => systemUnderTest.Deserialize(null, typeof(DateTime)));

            // Assert
            actual.Must().BeOfType<ArgumentNullException>();
            actual.Message.Must().ContainString("serializedString");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_ArgumentException___When_parameter_serializedString_is_white_space()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // Act
            var actual = Record.Exception(() => systemUnderTest.Deserialize(Invariant($"  {Environment.NewLine} "), typeof(DateTime)));

            // Assert
            actual.Must().BeOfType<ArgumentException>();
            actual.Message.Must().ContainString("serializedString");
            actual.Message.Must().ContainString("white space");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // Act
            var actual = Record.Exception(() => systemUnderTest.Deserialize("2019-01-05T12:14:58.1920000Z", null));

            // Assert
            actual.Must().BeOfType<ArgumentNullException>();
            actual.Message.Must().ContainString("type");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_ArgumentException___When_parameter_type_is_not_DateTime()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // Act
            var actual1 = Record.Exception(() => systemUnderTest.Deserialize("2019-01-05T12:14:58.1920000Z", typeof(object)));
            var actual2 = Record.Exception(() => systemUnderTest.Deserialize("2019-01-05T12:14:58.1920000Z", typeof(DateTime?)));

            // Assert
            actual1.Must().BeOfType<ArgumentException>();
            actual1.Message.Must().BeEqualTo("type != typeof(DateTime); 'type' is of type 'object'");

            actual2.Must().BeOfType<ArgumentException>();
            actual2.Message.Must().BeEqualTo("type != typeof(DateTime); 'type' is of type 'DateTime?'");
        }

        [Fact]
        public static void DeserializeToDateTime___Should_throw_ArgumentNullException___When_parameter_serializedString_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ObcDateTimeStringSerializer.DeserializeToDateTime(null));

            // Assert
            actual.Must().BeOfType<ArgumentNullException>();
            actual.Message.Must().ContainString("serializedString");
        }

        [Fact]
        public static void DeserializeToDateTime___Should_throw_ArgumentException___When_parameter_serializedString_is_white_space()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ObcDateTimeStringSerializer.DeserializeToDateTime(Invariant($"  {Environment.NewLine} ")));

            // Assert
            actual.Must().BeOfType<ArgumentException>();
            actual.Message.Must().ContainString("serializedString");
            actual.Message.Must().ContainString("white space");
        }

        [Fact]
        public static void Deserialize_T___Should_throw_InvalidOperationException___When_serializedString_is_malformed()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            var serializedDateTimes = new[]
            {
                "2017-05-06T02:28:46", // no precision not supported for Unspecified
                "2017-05-06T02:28:46+00:00", // no precision not supported for Local
                "2017-05-06T02:28:46.270488", // less than 7 precision not supported for Unspecified
                "2017-05-06T02:28:46.270488+00:00", // less than 7 precision not supported for Local
                "2017-05-06T02:28:46.27048838", // too much precision
                "2017-05-06T02:28:46.27048838+00:00", // too much precision
                "2017-05-06T02:28:46.27048838Z", // too much precision
                "2017-05-06T02:28:46.2704883K", // shouldn't end in K
                "some-string", // random string
                "2017-05-06", // only date
                "2017-05-06+00:00", // only date
                "2017-05-06Z", // only date
                "02:28:46.1938283", // only time
                "02:28:46.1938283+00:00", // only time
                "02:28:46.1938283Z", // only time
            };

            // Act
            var actuals = serializedDateTimes.Select(_ => Record.Exception(() => systemUnderTest.Deserialize<DateTime>(_))).ToList();

            // Assert
            actuals.Must().Each().BeOfType<InvalidOperationException>();
            actuals.Select(_ => _.Message).Must().Each().ContainString("is malformed; it's not in a supported format and cannot be deserialized.");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_InvalidOperationException___When_serializedString_is_malformed()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            var serializedDateTimes = new[]
            {
                "2017-05-06T02:28:46", // no precision not supported for Unspecified
                "2017-05-06T02:28:46+00:00", // no precision not supported for Local
                "2017-05-06T02:28:46.270488", // less than 7 precision not supported for Unspecified
                "2017-05-06T02:28:46.270488+00:00", // less than 7 precision not supported for Local
                "2017-05-06T02:28:46.27048838", // too much precision
                "2017-05-06T02:28:46.27048838+00:00", // too much precision
                "2017-05-06T02:28:46.27048838Z", // too much precision
                "2017-05-06T02:28:46.2704883K", // shouldn't end in K
                "some-string", // random string
                "2017-05-06", // only date
                "2017-05-06+00:00", // only date
                "2017-05-06Z", // only date
                "02:28:46.1938283", // only time
                "02:28:46.1938283+00:00", // only time
                "02:28:46.1938283Z", // only time
            };

            // Act
            var actuals = serializedDateTimes.Select(_ => Record.Exception(() => systemUnderTest.Deserialize(_, typeof(DateTime)))).ToList();

            // Assert
            actuals.Must().Each().BeOfType<InvalidOperationException>();
            actuals.Select(_ => _.Message).Must().Each().ContainString("is malformed; it's not in a supported format and cannot be deserialized.");
        }

        [Fact]
        public static void DeserializeToDateTime___Should_throw_InvalidOperationException___When_serializedString_is_malformed()
        {
            // Arrange
            var serializedDateTimes = new[]
            {
                "2017-05-06T02:28:46", // no precision not supported for Unspecified
                "2017-05-06T02:28:46+00:00", // no precision not supported for Local
                "2017-05-06T02:28:46.270488", // less than 7 precision not supported for Unspecified
                "2017-05-06T02:28:46.270488+00:00", // less than 7 precision not supported for Local
                "2017-05-06T02:28:46.27048838", // too much precision
                "2017-05-06T02:28:46.27048838+00:00", // too much precision
                "2017-05-06T02:28:46.27048838Z", // too much precision
                "2017-05-06T02:28:46.2704883K", // shouldn't end in K
                "some-string", // random string
                "2017-05-06", // only date
                "2017-05-06+00:00", // only date
                "2017-05-06Z", // only date
                "02:28:46.1938283", // only time
                "02:28:46.1938283+00:00", // only time
                "02:28:46.1938283Z", // only time
            };

            // Act
            var actuals = serializedDateTimes.Select(_ => Record.Exception(() => ObcDateTimeStringSerializer.DeserializeToDateTime(_))).ToList();

            // Assert
            actuals.Must().Each().BeOfType<InvalidOperationException>();
            actuals.Select(_ => _.Message).Must().Each().ContainString("is malformed; it's not in a supported format and cannot be deserialized.");
        }

        [Fact]
        public static void Deserialize_T___Should_deserialize_UTC_DateTime___When_serialized_strings_have_reduced_precision()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

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

            // Act
            var actual = serializedDateTimes.Select(_ => systemUnderTest.Deserialize<DateTime>(_)).ToList();

            // Assert
            actual.Must().BeEqualTo(expected);
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_deserialize_UTC_DateTime___When_serialized_strings_have_reduced_precision()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

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

            // Act
            var actual = serializedDateTimes.Select(_ => (DateTime)systemUnderTest.Deserialize(_, typeof(DateTime))).ToList();

            // Assert
            actual.Must().BeEqualTo(expected);
        }

        [Fact]
        public static void DeserializeToDateTime___Should_deserialize_UTC_DateTime___When_serialized_strings_have_reduced_precision()
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

            // Act
            var actual = serializedDateTimes.Select(ObcDateTimeStringSerializer.DeserializeToDateTime).ToList();

            // Assert
            actual.Must().BeEqualTo(expected);
        }

        [Fact]
        public static void Deserialize_T___Should_deserialize_DateTime___When_called()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // note that this will return "+00:00" if machine is on UTC time, which is what we expect
            var offset = DateTime.Now.ToString("%K");

            var scenarios = new List<(DateTime Expected, string SerializedString)>
            {
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc), "2019-01-05T12:14:58.1920000Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(1), "2019-01-05T12:14:58.1920001Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20), "2019-01-05T12:14:58.1920020Z"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified), "2019-01-05T12:14:58.1920000"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(1), "2019-01-05T12:14:58.1920001"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(20), "2019-01-05T12:14:58.1920020"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local), "2019-01-05T12:14:58.1920000" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(1), "2019-01-05T12:14:58.1920001" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(20), "2019-01-05T12:14:58.1920020" + offset),
            };

            // Act
            var actuals = scenarios.Select(_ => systemUnderTest.Deserialize<DateTime>(_.SerializedString)).ToList();

            // Assert
            actuals.Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_deserialize_DateTime___When_called()
        {
            // Arrange
            var systemUnderTest = new ObcDateTimeStringSerializer();

            // note that this will return "+00:00" if machine is on UTC time, which is what we expect
            var offset = DateTime.Now.ToString("%K");

            var scenarios = new List<(DateTime Expected, string SerializedString)>
            {
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc), "2019-01-05T12:14:58.1920000Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(1), "2019-01-05T12:14:58.1920001Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20), "2019-01-05T12:14:58.1920020Z"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified), "2019-01-05T12:14:58.1920000"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(1), "2019-01-05T12:14:58.1920001"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(20), "2019-01-05T12:14:58.1920020"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local), "2019-01-05T12:14:58.1920000" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(1), "2019-01-05T12:14:58.1920001" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(20), "2019-01-05T12:14:58.1920020" + offset),
            };

            // Act
            var actuals = scenarios.Select(_ => (DateTime)systemUnderTest.Deserialize(_.SerializedString, typeof(DateTime))).ToList();

            // Assert
            actuals.Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void DeserializeToDateTime___Should_deserialize_DateTime___When_called()
        {
            // Arrange

            // note that this will return "+00:00" if machine is on UTC time, which is what we expect
            var offset = DateTime.Now.ToString("%K");

            var scenarios = new List<(DateTime Expected, string SerializedString)>
            {
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc), "2019-01-05T12:14:58.1920000Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(1), "2019-01-05T12:14:58.1920001Z"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20), "2019-01-05T12:14:58.1920020Z"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified), "2019-01-05T12:14:58.1920000"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(1), "2019-01-05T12:14:58.1920001"),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(20), "2019-01-05T12:14:58.1920020"),

                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local), "2019-01-05T12:14:58.1920000" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(1), "2019-01-05T12:14:58.1920001" + offset),
                (new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(20), "2019-01-05T12:14:58.1920020" + offset),
            };

            // Act
            var actuals = scenarios.Select(_ => ObcDateTimeStringSerializer.DeserializeToDateTime(_.SerializedString)).ToList();

            // Assert
            actuals.Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_UTC___Works()
        {
            // Arrange
            var expected = DateTime.UtcNow;
            var serializer = new ObcDateTimeStringSerializer();

            // Act
            var serialized = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<DateTime>(serialized);

            // Assert
            actual.Kind.Must().BeEqualTo(expected.Kind);
            actual.Must().BeEqualTo(expected);
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
            actual.Kind.Must().BeEqualTo(expected.Kind);
            actual.Must().BeEqualTo(expected);
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
            actual.Kind.Must().BeEqualTo(expected.Kind);
            actual.Must().BeEqualTo(expected);
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
            actual.Kind.Must().BeEqualTo(expected.Kind);
            actual.Must().BeEqualTo(expected);
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
            actual.Kind.Must().BeEqualTo(expected.Kind);
            actual.Must().BeEqualTo(expected);
        }
    }
}