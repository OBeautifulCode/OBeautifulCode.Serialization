// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializingAndDeserializingBehaviorOfNull.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;

    using FluentAssertions;
    using MongoDB.Bson.Serialization;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class SerializingAndDeserializingBehaviorOfNull
    {
        [Fact]
        public static void BsonCanSerializeNull()
        {
            // Arrange
            Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject expected = null;

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject deserialized)
            {
                if (format == SerializationFormat.String)
                {
                    serialized.Should().Be(ObcBsonSerializer.SerializedRepresentationOfNull);
                }
                else if (format == SerializationFormat.Binary)
                {
                    serialized.Should().BeNull();
                }
                else
                {
                    throw new NotSupportedException("This format is not supported: " + format);
                }

                deserialized.Should().BeNull();
            }

            void ThrowIfStringsDiffer(string serialized, SerializationFormat format, string deserialized)
            {
                deserialized.Should().Be(ObcBsonSerializer.SerializedRepresentationOfNull);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer);
            ObcBsonSerializer.SerializedRepresentationOfNull.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfStringsDiffer);
        }
    }
}
