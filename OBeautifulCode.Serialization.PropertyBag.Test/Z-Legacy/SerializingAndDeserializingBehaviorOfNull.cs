// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializingAndDeserializingBehaviorOfNull.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag.Test
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
        public static void PropertyBagCanSerializeNull()
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

            // Act
            var exception1 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaPropertyBagWithCallbackVerification(null, formats: new[] { SerializationFormat.String }));
            var exception2 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaPropertyBagWithCallbackVerification(null, formats: new[] { SerializationFormat.Binary }));

            // Assert
            expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(ThrowIfObjectsDiffer);

            exception1.Should().BeOfType<NotSupportedException>();
            exception1.Message.Should().Be("String is not supported as a type for this serializer.");

            exception2.Should().BeOfType<NotSupportedException>();
            exception2.Message.Should().Be("String is not supported as a type for this serializer.");
        }
    }
}
