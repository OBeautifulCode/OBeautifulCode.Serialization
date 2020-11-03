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

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, Serialization.Test.SerializingAndDeserializingBehaviorOfNull.NullableObject deserialized)
            {
                serialized.Should().BeNull();

                deserialized.Should().BeNull();
            }

            // Act
            expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(ThrowIfObjectsDiffer);
        }
    }
}
