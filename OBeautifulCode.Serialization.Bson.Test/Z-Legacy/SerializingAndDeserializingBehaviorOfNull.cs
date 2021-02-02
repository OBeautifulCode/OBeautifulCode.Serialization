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
    using OBeautifulCode.Type.Recipes;

    using Xunit;

    public static class SerializingAndDeserializingBehaviorOfNull
    {
        [Fact]
        public static void BsonCanSerializeNull()
        {
            // Arrange
            Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject expected = null;

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject deserialized)
            {
                if (describedSerialization is StringDescribedSerialization stringDescribedSerialization)
                {
                    stringDescribedSerialization.SerializedPayload.Should().Be(ObcBsonSerializer.SerializedRepresentationOfNull);
                }
                else if (describedSerialization is BinaryDescribedSerialization binaryDescribedSerialization)
                {
                    binaryDescribedSerialization.SerializedPayload.Should().BeNull();
                }
                else
                {
                    throw new NotSupportedException("This type of described serialization is not supported: " + describedSerialization.GetType().ToStringReadable());
                }

                deserialized.Should().BeNull();
            }

            void ThrowIfStringsDiffer(DescribedSerializationBase describedSerialization, string deserialized)
            {
                deserialized.Should().Be(ObcBsonSerializer.SerializedRepresentationOfNull);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer);
            ObcBsonSerializer.SerializedRepresentationOfNull.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfStringsDiffer);
        }
    }
}
