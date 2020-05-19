﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionsWorkDirectlyWithRegistrationOfGenericTypes.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class CollectionsWorkDirectlyWithRegistrationOfGenericTypes
    {
        [Fact]
        public static void ElementTypeOfArrayIsOnlyTypeDiscovered()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<RegisteredKey, RegisteredValue>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<RegisteredKey, RegisteredValue>);

            var expectedKey = new RegisteredKey { Property = A.Dummy<string>() };
            var expectedValue = new RegisteredValue { Property = A.Dummy<string>() };
            var expectedTuple = new Tuple<RegisteredKey, RegisteredValue>(expectedKey, expectedValue);
            var expectedDictionary = new Dictionary<RegisteredKey, RegisteredValue> { { expectedKey, expectedValue } };
            var expectedList = new List<RegisteredKey>(new[] { expectedKey });
            var expectedArray = new[] { expectedValue };

            // Act, Assert
            void ThrowIfTuplesDiffer(DescribedSerialization serialized, Tuple<RegisteredKey, RegisteredValue> deserialized)
            {
                deserialized.Item1.Property.Should().Be(expectedTuple.Item1.Property);
                deserialized.Item2.Property.Should().Be(expectedTuple.Item2.Property);
            }

            void ThrowIfDictionariesDiffer(DescribedSerialization serialized, Dictionary<RegisteredKey, RegisteredValue> deserialized)
            {
                deserialized.Single().Key.Property.Should().Be(expectedDictionary.Single().Key.Property);
                deserialized.Single().Value.Property.Should().Be(expectedDictionary.Single().Value.Property);
            }

            void ThrowIfListsDiffer(DescribedSerialization serialized, List<RegisteredKey> deserialized)
            {
                deserialized.Single().Property.Should().Be(expectedList.Single().Property);
            }

            void ThrowIfArraysDiffer(DescribedSerialization serialized, RegisteredValue[] deserialized)
            {
                deserialized.Single().Property.Should().Be(expectedArray.Single().Property);
            }

            // Act, Assert
            expectedTuple.RoundtripSerializeViaJsonWithCallback(ThrowIfTuplesDiffer, jsonConfigType);
            expectedDictionary.RoundtripSerializeViaJsonWithCallback(ThrowIfDictionariesDiffer, jsonConfigType);
            expectedList.RoundtripSerializeWithCallback(ThrowIfListsDiffer, bsonConfigType, jsonConfigType);
            expectedArray.RoundtripSerializeWithCallback(ThrowIfArraysDiffer, bsonConfigType, jsonConfigType);
        }
    }

    [Serializable]
    public class RegisteredKey : IEquatable<RegisteredKey>
    {
        public string Property { get; set; }

        public bool Equals(RegisteredKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Property, other.Property);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((RegisteredKey)obj);
        }

        public override int GetHashCode()
        {
            return this.Property != null ? this.Property.GetHashCode() : 0;
        }
    }

    [Serializable]
    public class RegisteredValue
    {
        public string Property { get; set; }
    }
}
