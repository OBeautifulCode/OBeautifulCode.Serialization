﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcLambdaBackedSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Json;

    using Xunit;

    using static System.FormattableString;

    public static class ObcLambdaBackedSerializerTest
    {
        [Fact]
        public static void LambdaSerializer__Passes_through()
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

            var test = new TestObjectForLambda { Property1 = property1, Property2 = property2, Property3 = property3, };
            var backingSerializer = new ObcJsonSerializer();
            var serializer = new ObcLambdaBackedSerializer(
                backingSerializer.SerializeToString,
                backingSerializer.Deserialize,
                backingSerializer.SerializeToBytes,
                backingSerializer.Deserialize);

            // Act
            var stringOut = serializer.SerializeToString(test);
            var stringRoundTrip = serializer.Deserialize(stringOut, test.GetType());
            var bytesOut = serializer.SerializeToBytes(stringRoundTrip);
            var bytesRoundTrip = serializer.Deserialize(bytesOut, stringRoundTrip.GetType());
            var actual = backingSerializer.SerializeToString(bytesRoundTrip);

            // Assert
            actual.Should().Be(expected);
            stringOut.Should().Be(expected);
        }
    }

    public class TestObjectForLambda
    {
        public string Property1 { get; set; }

        public string Property2 { get; set; }

        public string Property3 { get; set; }

        public string Property4 { get; set; }
    }
}
