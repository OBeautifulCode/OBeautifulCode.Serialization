// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcVersionStringSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;

    using Xunit;

    public static class ObcVersionStringSerializerTest
    {
        [Fact]
        public static void SerializeToString___Should_return_null___When_parameter_value_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            // Act
            var actual = systemUnderTest.SerializeToString(null);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void SerializeToString___Should_throw_ArgumentException___When_parameter_value_is_not_Version()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            var values = new[] { new object(), "1.0", A.Dummy<string>(), A.Dummy<int>(), A.Dummy<Guid>() };

            // Act
            var actual = values.Select(_ => Record.Exception(() => systemUnderTest.SerializeToString(_))).ToList();

            // Assert
            actual.Must().Each().BeOfType<ArgumentException>();
            actual.Select(_ => _.Message).Must().Each().ContainString("objectToSerialize is not a Version; it is a");
        }

        [Fact]
        public static void SerializeToString___Should_return_serialized_string_representation_of_value___When_called()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            var scenarios = new List<(Version Value, string Expected)>
            {
                (new Version(), "0.0"),
                (new Version(5, 20), "5.20"),
                (new Version(5, 20, 43), "5.20.43"),
                (new Version(5, 20, 43, 69), "5.20.43.69"),
            };

            // Act
            var actuals = scenarios.Select(_ => systemUnderTest.SerializeToString(_.Value)).ToList();

            // Assert
            actuals.Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_ArgumentNullException___When_parameter_type_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            // Act
            var actual = Record.Exception(() => systemUnderTest.Deserialize("1.0", null));

            // Assert
            actual.Must().BeOfType<ArgumentNullException>();
            actual.Message.Must().ContainString("type");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_ArgumentException___When_parameter_type_is_not_Version()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            // Act
            var actual1 = Record.Exception(() => systemUnderTest.Deserialize("1.2.0", typeof(object)));
            var actual2 = Record.Exception(() => systemUnderTest.Deserialize("1.2.0", typeof(DateTime)));

            // Assert
            actual1.Must().BeOfType<ArgumentException>();
            actual1.Message.Must().BeEqualTo("type != typeof(Version); 'type' is of type 'object'");

            actual2.Must().BeOfType<ArgumentException>();
            actual2.Message.Must().BeEqualTo("type != typeof(Version); 'type' is of type 'DateTime'");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_return_null___When_parameter_serializedString_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            // Act
            var actual = systemUnderTest.Deserialize(null, typeof(Version));

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_throw_InvalidOperationException___When_parameter_serializedString_is_malformed()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            var serializedStrings = new[]
            {
                string.Empty,
                "not-a-version",
                "-1.2.3",
            };

            // Act
            var actual = serializedStrings.Select(_ => Record.Exception(() => systemUnderTest.Deserialize(_, typeof(Version)))).ToList();

            // Assert
            actual.AsTest().Must().Each().BeOfType<InvalidOperationException>();
            actual.Select(_ => _.Message).AsTest().Must().Each().ContainString("The serialized Version is malformed");
        }

        [Fact]
        public static void Deserialize_serializedString_type___Should_deserialize_version___When_called()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            var scenarios = new List<(Version Expected, string SerializedString)>
            {
                (new Version(), "0.0"),
                (new Version(5, 20), "5.20"),
                (new Version(5, 20, 43), "5.20.43"),
                (new Version(5, 20, 43, 69), "5.20.43.69"),
            };

            // Act
            var actual = scenarios.Select(_ => (Version)systemUnderTest.Deserialize(_.SerializedString, typeof(Version))).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void Deserialize_T___Should_throw_ArgumentException___When_type_parameter_is_not_Version()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            // Act
            var actual1 = Record.Exception(() => systemUnderTest.Deserialize<object>("1.2.0"));
            var actual2 = Record.Exception(() => systemUnderTest.Deserialize<DateTime>("1.2.0"));

            // Assert
            actual1.Must().BeOfType<ArgumentException>();
            actual1.Message.Must().BeEqualTo("type != typeof(Version); 'type' is of type 'object'");

            actual2.Must().BeOfType<ArgumentException>();
            actual2.Message.Must().BeEqualTo("type != typeof(Version); 'type' is of type 'DateTime'");
        }

        [Fact]
        public static void Deserialize_T___Should_return_null___When_parameter_serializedString_is_null()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            // Act
            var actual = systemUnderTest.Deserialize<Version>(null);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void Deserialize_T___Should_throw_InvalidOperationException___When_parameter_serializedString_is_malformed()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            var serializedStrings = new[]
            {
                string.Empty,
                "not-a-version",
                "-1.2.3",
            };

            // Act
            var actual = serializedStrings.Select(_ => Record.Exception(() => systemUnderTest.Deserialize<Version>(_))).ToList();

            // Assert
            actual.AsTest().Must().Each().BeOfType<InvalidOperationException>();
            actual.Select(_ => _.Message).AsTest().Must().Each().ContainString("The serialized Version is malformed");
        }

        [Fact]
        public static void Deserialize_T___Should_deserialize_version___When_called()
        {
            // Arrange
            var systemUnderTest = new ObcVersionStringSerializer();

            var scenarios = new List<(Version Expected, string SerializedString)>
            {
                (new Version(), "0.0"),
                (new Version(5, 20), "5.20"),
                (new Version(5, 20, 43), "5.20.43"),
                (new Version(5, 20, 43, 69), "5.20.43.69"),
            };

            // Act
            var actual = scenarios.Select(_ => systemUnderTest.Deserialize<Version>(_.SerializedString)).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }
    }
}