// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionsWorkDirectlyWithRegistrationOfGenericTypes.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;
    using OBeautifulCode.Serialization.Test;

    using Xunit;

    public static class CollectionsWorkDirectlyWithRegistrationOfGenericTypes
    {
        [Fact]
        public static void ElementTypeOfArrayIsOnlyTypeDiscovered()
        {
            // Arrange
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<RegisteredKey, RegisteredValue>);

            var expectedKey = new RegisteredKey { Property = A.Dummy<string>() };
            var expectedValue = new RegisteredValue { Property = A.Dummy<string>() };
            var expectedTuple = new Tuple<RegisteredKey, RegisteredValue>(expectedKey, expectedValue);
            var expectedDictionary = new Dictionary<RegisteredKey, RegisteredValue> { { expectedKey, expectedValue } };

            // Act, Assert
            void ThrowIfTuplesDiffer(string serialized, SerializationFormat format, Tuple<RegisteredKey, RegisteredValue> deserialized)
            {
                deserialized.Item1.Property.Should().Be(expectedTuple.Item1.Property);
                deserialized.Item2.Property.Should().Be(expectedTuple.Item2.Property);
            }

            void ThrowIfDictionariesDiffer(string serialized, SerializationFormat format, Dictionary<RegisteredKey, RegisteredValue> deserialized)
            {
                deserialized.Single().Key.Property.Should().Be(expectedDictionary.Single().Key.Property);
                deserialized.Single().Value.Property.Should().Be(expectedDictionary.Single().Value.Property);
            }

            // Act, Assert
            expectedTuple.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfTuplesDiffer, jsonConfigType);
            expectedDictionary.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfDictionariesDiffer, jsonConfigType);
        }
    }
}
