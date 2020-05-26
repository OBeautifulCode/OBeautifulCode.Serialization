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

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject deserialized)
            {
                if (serialized.SerializationFormat == SerializationFormat.String)
                {
                    serialized.SerializedPayload.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
                }
                else if (serialized.SerializationFormat == SerializationFormat.Binary)
                {
                    serialized.SerializedPayload.Should().Be(Convert.ToBase64String(SerializationConfigurationBase.NullSerializedStringValue.ToBytes(Encoding.UTF8)));
                }
                else
                {
                    throw new NotSupportedException("This format is not supported: " + serialized.SerializationFormat);
                }

                deserialized.Should().BeNull();
            }

            void ThrowIfStringsDiffer(DescribedSerialization serialized, string deserialized)
            {
                deserialized.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
            SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfStringsDiffer);
        }
    }
}
