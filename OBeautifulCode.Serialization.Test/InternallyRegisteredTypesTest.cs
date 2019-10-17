// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRegisteredTypesTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;

    using Xunit;

    public static class InternallyRegisteredTypesTest
    {
        private static readonly ObcJsonSerializer JsonSerializer = new ObcJsonSerializer();

        private static readonly ObcBsonSerializer BsonSerializer = new ObcBsonSerializer();

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_UtcDateTimeRangeInclusive___When_called()
        {
            // Arrange
            var expected = A.Dummy<UtcDateTimeRangeInclusive>();

            var serialized1 = JsonSerializer.SerializeToString(expected);
            var serialized2 = BsonSerializer.SerializeToString(expected);

            // Act
            var actual1 = JsonSerializer.Deserialize<UtcDateTimeRangeInclusive>(serialized1);
            var actual2 = BsonSerializer.Deserialize<UtcDateTimeRangeInclusive>(serialized2);

            // Assert
            actual1.Must().BeEqualTo(expected);
            actual2.Must().BeEqualTo(expected);
        }
    }
}
