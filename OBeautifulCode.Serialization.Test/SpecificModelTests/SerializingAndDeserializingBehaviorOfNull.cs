// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializingAndDeserializingBehaviorOfNull.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using FluentAssertions;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.PropertyBag;
    using Xunit;

    public static class SerializingAndDeserializingBehaviorOfNull
    {
        [Fact]
        public static void PropertyBagCanSerializeNull()
        {
            // Arrange
            var serializer = new ObcPropertyBagSerializer();

            // Act
            var actualNullString = serializer.SerializeToString(null);
            var actualNullBytes = serializer.SerializeToBytes(null);
            var actualStringException = Record.Exception(() => serializer.SerializeToString(SerializationConfigurationBase.NullSerializedStringValue));
            var actualNullFromString = serializer.Deserialize<NullableObject>(actualNullString);
            var actualNullFromBytes = serializer.Deserialize<NullableObject>(actualNullBytes);

            // Assert
            actualNullString.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
            actualNullFromString.Should().BeNull();
            actualNullFromBytes.Should().BeNull();
            actualStringException.Should().NotBeNull();
            actualStringException.Should().BeOfType<NotSupportedException>();
            actualStringException.Message.Should().Be("String is not supported as a type for this serializer.");
        }

        [Fact]
        public static void JsonCanSerializeNull()
        {
            // Arrange
            var serializer = new ObcJsonSerializer();

            // Act
            var actualNullString = serializer.SerializeToString(null);
            var actualNullBytes = serializer.SerializeToBytes(null);
            var actualString = serializer.SerializeToString(SerializationConfigurationBase.NullSerializedStringValue);
            var actualNullFromString = serializer.Deserialize<NullableObject>(actualNullString);
            var actualNullFromBytes = serializer.Deserialize<NullableObject>(actualNullBytes);
            var actual = serializer.Deserialize<string>(actualString);

            // Assert
            actualNullString.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
            actualNullFromString.Should().BeNull();
            actualNullFromBytes.Should().BeNull();
            actualString.Should().NotBe(SerializationConfigurationBase.NullSerializedStringValue);
            actual.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
        }

        [Fact]
        public static void BsonCanSerializeNull()
        {
            // Arrange
            var serializer = new ObcBsonSerializer();

            // Act
            var actualNullString = serializer.SerializeToString(null);
            var actualNullBytes = serializer.SerializeToBytes(null);
            var actualStringException = Record.Exception(() => serializer.SerializeToString(SerializationConfigurationBase.NullSerializedStringValue));
            var actualNullFromString = serializer.Deserialize<NullableObject>(actualNullString);
            var actualNullFromBytes = serializer.Deserialize<NullableObject>(actualNullBytes);

            // Assert
            actualNullString.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
            actualNullFromString.Should().BeNull();
            actualNullFromBytes.Should().BeNull();
            actualStringException.Should().NotBeNull();
            actualStringException.Should().BeOfType<NotSupportedException>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Not important.")]
        public class NullableObject
        {
        }
    }
}
