// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSimplifyingSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;

    using Xunit;

    public static class ObcSimplifyingSerializerTest
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_null___When_deserializing_null_objectToSerialize_serialized_using_SerializeToString()
        {
            // Arrange
            var subjectUnderTest = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

            var serialized = subjectUnderTest.SerializeToString(null);

            // Act
            var actual = subjectUnderTest.Deserialize(serialized, A.Dummy<Type>());

            // Assert
            serialized.AsTest().Must().BeNull();
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_null___When_deserializing_null_objectToSerialize_serialized_using_SerializeToBytes()
        {
            // Arrange
            var subjectUnderTest = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

            var serialized = subjectUnderTest.SerializeToBytes(null);

            // Act
            var actual = subjectUnderTest.Deserialize(serialized, A.Dummy<Type>());

            // Assert
            serialized.AsTest().Must().BeNull();
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void Deserialize___Should_shortcut_FallbackSerializer_and_roundtrip_StringSimplifiedTypes___When_deserializing_payload_generated_by_the_serializer()
        {
            // Arrange
            var subjectUnderTest = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

            var expectedWithType = ObcSimplifyingSerializer.StringSimplifiedTypes.Select(_ => new { Expected = AD.ummy(_), Type = _ }).ToList();

            var stringSerializedObjects = expectedWithType.Select(_ => new { Serialized = subjectUnderTest.SerializeToString(_.Expected), Type = _.Type }).ToList();

            // Act
            var actual = stringSerializedObjects.Select(_ => subjectUnderTest.Deserialize(_.Serialized, _.Type)).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expectedWithType.Select(_ => _.Expected).ToList());
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

        [Fact]
        public static void Deserialize___Should_roundtrip_float_with_no_precision_issues___When_deserializing_float_serialized_to_string()
        {
            // Arrange
            var expected = 1.292834724f;

            var subjectUnderTest = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

            var serialized = subjectUnderTest.SerializeToString(expected);

            // Act
            var actual = subjectUnderTest.Deserialize<float>(serialized);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_double_with_no_precision_issues___When_deserializing_float_serialized_to_string()
        {
            // Arrange
            var expected = 1.29283472488347248d;

            var subjectUnderTest = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

            var serialized = subjectUnderTest.SerializeToString(expected);

            // Act
            var actual = subjectUnderTest.Deserialize<double>(serialized);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }
    }
}
