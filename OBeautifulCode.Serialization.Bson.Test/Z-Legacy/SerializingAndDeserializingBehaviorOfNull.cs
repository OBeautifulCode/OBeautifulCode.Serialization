// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializingAndDeserializingBehaviorOfNull.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class SerializingAndDeserializingBehaviorOfNull
    {
        [Fact]
        public static void BsonCanSerializeNull()
        {
            Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject expected = null;

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject deserialized)
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
            var exception1 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaBsonWithCallbackVerification(null, formats: new[] { SerializationFormat.String }));
            var exception2 = Record.Exception(() => SerializationConfigurationBase.NullSerializedStringValue.RoundtripSerializeViaBsonWithCallbackVerification(null, formats: new[] { SerializationFormat.Binary }));

            // Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer);
            exception1.Should().BeOfType<NotSupportedException>();
            exception2.Should().BeOfType<NotSupportedException>();
        }
    }
}
