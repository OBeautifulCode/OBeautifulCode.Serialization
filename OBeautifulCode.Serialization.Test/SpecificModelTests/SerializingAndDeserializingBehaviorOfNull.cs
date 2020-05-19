// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializingAndDeserializingBehaviorOfNull.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using FluentAssertions;

    using OBeautifulCode.String.Recipes;

    using Xunit;

    public static class SerializingAndDeserializingBehaviorOfNull
    {
        [Fact]
        public static void PropertyBagCanSerializeNull()
        {
            // Arrange
            NullableObject expected = null;

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, NullableObject deserialized)
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
            var exception1 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaPropertyBagWithCallback(null, formats: new[] { SerializationFormat.String }));
            var exception2 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaPropertyBagWithCallback(null, formats: new[] { SerializationFormat.Binary }));

            // Assert
            expected.RoundtripSerializeViaPropertyBagWithCallback(ThrowIfObjectsDiffer);

            exception1.Should().BeOfType<NotSupportedException>();
            exception1.Message.Should().Be("String is not supported as a type for this serializer.");

            exception2.Should().BeOfType<NotSupportedException>();
            exception2.Message.Should().Be("String is not supported as a type for this serializer.");
        }

        [Fact]
        public static void JsonCanSerializeNull()
        {
            // Arrange
            NullableObject expected = null;

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, NullableObject deserialized)
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
            expected.RoundtripSerializeViaJsonWithCallback(ThrowIfObjectsDiffer);
            SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaJsonWithCallback(ThrowIfStringsDiffer);
        }

        [Fact]
        public static void BsonCanSerializeNull()
        {
            NullableObject expected = null;

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, NullableObject deserialized)
            {
                if (serialized.SerializationFormat == SerializationFormat.String)
                {
                    serialized.SerializedPayload.Should().Be(SerializationConfigurationBase.NullSerializedStringValue);
                }
                else if (serialized.SerializationFormat == SerializationFormat.Binary)
                {
                    serialized.SerializedPayload.Should().BeNull();
                }
                else
                {
                    throw new NotSupportedException("This format is not supported: " + serialized.SerializationFormat);
                }

                deserialized.Should().BeNull();
            }

            // Act
            var exception1 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaBsonWithCallback(null, formats: new[] { SerializationFormat.String }));
            var exception2 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaBsonWithCallback(null, formats: new[] { SerializationFormat.Binary }));

            // Assert
            expected.RoundtripSerializeViaBsonWithCallback(ThrowIfObjectsDiffer);
            exception1.Should().BeOfType<NotSupportedException>();
            exception2.Should().BeOfType<NotSupportedException>();
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Not important.")]
        public class NullableObject
        {
        }
    }
}
