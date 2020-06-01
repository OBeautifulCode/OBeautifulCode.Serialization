// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializingAndDeserializingBehaviorOfNull.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Text;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Recipes;
    using OBeautifulCode.String.Recipes;

    using Xunit;

    public static class SerializingAndDeserializingBehaviorOfNull
    {
        [Fact]
        public static void JsonCanSerializeNull()
        {
            // Arrange
            Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject expected = null;

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject deserialized)
            {
                if (format == SerializationFormat.String)
                {
                    serialized.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
                }
                else if (format == SerializationFormat.Binary)
                {
                    serialized.Should().Be(Convert.ToBase64String(SerializationConfigurationBase.NullSerializedStringValue.ToBytes(Encoding.UTF8)));
                }
                else
                {
                    throw new NotSupportedException("This format is not supported: " + format);
                }

                deserialized.Should().BeNull();
            }

            void ThrowIfStringsDiffer(string serialized, SerializationFormat format, string deserialized)
            {
                deserialized.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
            SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfStringsDiffer);
        }
    }
}
