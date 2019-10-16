// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetDrawingTypeTests.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System.Drawing;
    using FakeItEasy;
    using FluentAssertions;
    using OBeautifulCode.Serialization.Bson;
    using Xunit;

    public static class NetDrawingTypeTests
    {
        [Fact]
        public static void RegularColorRoundtrip()
        {
            // Arrange
            var serializer = new ObcBsonSerializer();
            var expected = new ObjectWithNetDrawingTypes
            {
                Color = A.Dummy<Color>(),
                NullableWithValueColor = A.Dummy<Color>(),
                NullableWithoutValueColor = null,
            };

            // Act
            var actualString = serializer.SerializeToString(expected);
            var actual = serializer.Deserialize<ObjectWithNetDrawingTypes>(actualString);

            // Assert
            actual.Color.Should().Be(expected.Color);
            actual.NullableWithValueColor.Should().Be(expected.NullableWithValueColor);
            actual.NullableWithoutValueColor.Should().BeNull();
        }
    }

    public class ObjectWithNetDrawingTypes
    {
        public Color Color { get; set; }

        public Color? NullableWithValueColor { get; set; }

        public Color? NullableWithoutValueColor { get; set; }
    }
}
