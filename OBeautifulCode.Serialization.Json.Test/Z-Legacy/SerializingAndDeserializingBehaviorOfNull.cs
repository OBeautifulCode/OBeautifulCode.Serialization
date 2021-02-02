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
    using OBeautifulCode.Type.Recipes;
    using Xunit;

    public static class SerializingAndDeserializingBehaviorOfNull
    {
        private const string NewtonsoftSerializedNullRepresentation = "null";

        [Fact]
        public static void JsonCanSerializeNull()
        {
            // Arrange
            Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject expected = null;

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject deserialized)
            {
                if (describedSerialization is StringDescribedSerialization stringDescribedSerialization)
                {
                    stringDescribedSerialization.SerializedPayload.Should().Be(NewtonsoftSerializedNullRepresentation);
                }
                else if (describedSerialization is BinaryDescribedSerialization binaryDescribedSerialization)
                {
                    var expectedPayload = NewtonsoftSerializedNullRepresentation.ToBytes(Encoding.UTF8);

                    binaryDescribedSerialization.SerializedPayload.Should().Equal(expectedPayload);
                }
                else
                {
                    throw new NotSupportedException("This type of described serialization is not supported: " + describedSerialization.GetType().ToStringReadable());
                }

                deserialized.Should().BeNull();
            }

            void ThrowIfStringsDiffer(DescribedSerializationBase describedSerialization, string deserialized)
            {
                deserialized.Should().Be(NewtonsoftSerializedNullRepresentation);
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer);
            NewtonsoftSerializedNullRepresentation.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfStringsDiffer);
        }
    }
}
