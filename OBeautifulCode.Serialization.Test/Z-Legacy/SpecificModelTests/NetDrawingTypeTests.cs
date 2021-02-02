// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetDrawingTypeTests.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Drawing;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class NetDrawingTypeTests
    {
        [Fact]
        public static void RegularColorRoundtrip()
        {
            // Arrange
            var expected = new ObjectWithNetDrawingTypes
            {
                Color = A.Dummy<Color>(),
                NullableWithValueColor = A.Dummy<Color>(),
                NullableWithoutValueColor = null,
            };

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, ObjectWithNetDrawingTypes deserialized)
            {
                deserialized.Color.Should().Be(expected.Color);
                deserialized.NullableWithValueColor.Should().Be(expected.NullableWithValueColor);
                deserialized.NullableWithoutValueColor.Should().BeNull();
            }

            // Act, Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer);
        }
    }

    [Serializable]
    public class ObjectWithNetDrawingTypes
    {
        public Color Color { get; set; }

        public Color? NullableWithValueColor { get; set; }

        public Color? NullableWithoutValueColor { get; set; }
    }
}
