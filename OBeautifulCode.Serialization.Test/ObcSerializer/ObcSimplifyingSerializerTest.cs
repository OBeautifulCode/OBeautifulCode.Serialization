// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSimplifyingSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;

    using Xunit;

    public static class ObcSimplifyingSerializerTest
    {
        [Fact]
        public static void Deserialize___Should_shortcut_FallbackSerializer_and_roundtrip_StringSimplifiedTypes___When_deserializing_payload_generated_by_the_serializer()
        {
            // Arrange
            var subjectUnderTest = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

            var expected = ObcSimplifyingSerializer.StringSimplifiedTypes.Select(AD.ummy).ToList();

            var stringSerializedObjects = expected.Select(_ => new { Serialized = subjectUnderTest.SerializeToString(_), Type = _.GetType() }).ToList();

            // Act
            var actual = stringSerializedObjects.Select(_ => subjectUnderTest.Deserialize(_.Serialized, _.Type)).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void Deserialize___Should_shortcut_FallbackSerializer_and_roundtrip_a_byte_array___When_deserializing_bytes_generated_by_the_serializer()
        {
            // Arrange
            var subjectUnderTest = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

            var expected = A.Dummy<byte[]>();

            var serialized = subjectUnderTest.SerializeToBytes(expected);

            // Act
            var actual = subjectUnderTest.Deserialize<byte[]>(serialized);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }
    }
}
