﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializerTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Serialization.Test
{
    using System;

    using FakeItEasy;

    using FluentAssertions;

    using Naos.Serialization.Domain;
    using Naos.Serialization.Json;

    using OBeautifulCode.Reflection;

    using Xunit;

    using static System.FormattableString;

    public static class JsonSerializerTest
    {
        [Fact]
        public static void NaosJsonSerializer___With_kind_Invalid___Throws()
        {
            // Arrange
            Action action = () => new NaosJsonSerializer(serializationKind: SerializationKind.Invalid);

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentOutOfRangeException>();
            exception.Message.Should().Be("Parameter 'serializationKind' is equal to the comparison value using EqualityComparer<T>.Default, where T: SerializationKind.  Specified 'comparisonValue' is 'Invalid'.");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Newtonsoft", Justification = "Spelling/name is correct.")]
        public static void NaosJsonSerializer___With_kind_Custom_and_null_Configuration_type___Throws()
        {
            // Arrange
            Action action = () => new NaosJsonSerializer(null, SerializationKind.Custom);

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Must specify configurationType if using serializationKind of SerializationKind.Custom");
        }

        [Fact]
        public static void NaosJsonSerializer___With_type_Default___Uses_default()
        {
            // Arrange
            var property1 = A.Dummy<string>();
            var property2 = A.Dummy<string>();
            var property3 = A.Dummy<string>();

            var expected = "{"
                           + Environment.NewLine
                           + Invariant($"  \"property1\": \"{property1}\",") + Environment.NewLine
                           + Invariant($"  \"property2\": \"{property2}\",") + Environment.NewLine
                           + Invariant($"  \"property3\": \"{property3}\",") + Environment.NewLine
                           + "  \"property4\": null" + Environment.NewLine
                           + "}";

            var test = new TestObject { Property1 = property1, Property2 = property2, Property3 = property3, };
            var serializer = new NaosJsonSerializer();

            // Act
            var actual = serializer.SerializeToString(test);

            // Assert
            actual.Should().Be(expected);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CompactUses", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void NaosJsonSerializer___With_type_Compact___Uses_compact()
        {
            // Arrange
            var property1 = A.Dummy<string>();
            var property2 = A.Dummy<string>();
            var property3 = A.Dummy<string>();

            var expected = "{"
                           + Invariant($"\"property1\":\"{property1}\",")
                           + Invariant($"\"property2\":\"{property2}\",")
                           + Invariant($"\"property3\":\"{property3}\",")
                           + "\"property4\":null"
                           + "}";

            var test = new TestObject { Property1 = property1, Property2 = property2, Property3 = property3, };
            var serializer = new NaosJsonSerializer(serializationKind: SerializationKind.Compact);

            // Act
            var actual = serializer.SerializeToString(test);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public static void NaosJsonSerializer___With_type_Minimal___Uses_minimal()
        {
            // Arrange
            var property1 = A.Dummy<string>();
            var property2 = A.Dummy<string>();
            var property3 = A.Dummy<string>();

            var expected = "{"
                           + Invariant($"\"property1\":\"{property1}\",")
                           + Invariant($"\"property2\":\"{property2}\",")
                           + Invariant($"\"property3\":\"{property3}\"")
                           + "}";

            var test = new TestObject { Property1 = property1, Property2 = property2, Property3 = property3, };
            var serializer = new NaosJsonSerializer(serializationKind: SerializationKind.Minimal);

            // Act
            var actual = serializer.SerializeToString(test);

            // Assert
            actual.Should().Be(expected);
        }
    }

    public class TestObject
    {
        public string Property1 { get; set; }

        public string Property2 { get; set; }

        public string Property3 { get; set; }

        public string Property4 { get; set; }
    }
}
