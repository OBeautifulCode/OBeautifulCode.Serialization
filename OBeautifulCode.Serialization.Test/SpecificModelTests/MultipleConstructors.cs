// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleConstructors.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FakeItEasy;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;

    using Xunit;

    public static class MultipleConstructors
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_UnchainedNoDefaultConstructorModel___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonConfiguration<UnchainedNoDefaultConstructorModel>);
            var jsonConfigType = typeof(GenericDiscoveryJsonConfiguration<UnchainedNoDefaultConstructorModel>);

            var expected = A.Dummy<UnchainedNoDefaultConstructorModel>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, UnchainedNoDefaultConstructorModel deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.StartDateTimeInUtc.AsTest().Must().BeEqualTo(expected.StartDateTimeInUtc);
                deserialized.EndDateTimeInUtc.AsTest().Must().BeEqualTo(expected.EndDateTimeInUtc);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_ChainedNoDefaultConstructorModel___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonConfiguration<ChainedNoDefaultConstructorModel>);
            var jsonConfigType = typeof(GenericDiscoveryJsonConfiguration<ChainedNoDefaultConstructorModel>);

            var expected = A.Dummy<ChainedNoDefaultConstructorModel>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ChainedNoDefaultConstructorModel deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.StartDateTimeInUtc.AsTest().Must().BeEqualTo(expected.StartDateTimeInUtc);
                deserialized.EndDateTimeInUtc.AsTest().Must().BeEqualTo(expected.EndDateTimeInUtc);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_UnchainedDefaultConstructorModel___When_called()
        {
            // Arrange
            var expectedParameterless = new UnchainedDefaultConstructorModel();
            var expectedParameter = new UnchainedDefaultConstructorModel(A.Dummy<string>());

            // Act & Assert
            expectedParameter.RoundtripSerializeWithEquatableAssertion();
            expectedParameterless.RoundtripSerializeWithEquatableAssertion();
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_AttributedConstructorModel___When_called()
        {
            // Arrange
            var expectedParameterless = new AttributedConstructorModel();
            var expectedParameter = new AttributedConstructorModel(A.Dummy<string>());

            // Act & Assert
            expectedParameter.RoundtripSerializeWithEquatableAssertion();
            expectedParameterless.RoundtripSerializeWithEquatableAssertion();
        }
    }

    public class UnchainedNoDefaultConstructorModel
    {
        public UnchainedNoDefaultConstructorModel(
            UtcDateTimeRangeInclusive utcDateTimeRangeInclusive)
        {
            this.StartDateTimeInUtc = utcDateTimeRangeInclusive.StartDateTimeInUtc;
            this.EndDateTimeInUtc = utcDateTimeRangeInclusive.EndDateTimeInUtc;
        }

        public UnchainedNoDefaultConstructorModel(
            DateTime startDateTimeInUtc,
            DateTime endDateTimeInUtc)
        {
            this.StartDateTimeInUtc = startDateTimeInUtc;
            this.EndDateTimeInUtc = endDateTimeInUtc;
        }

        public DateTime EndDateTimeInUtc { get; set; }

        public DateTime StartDateTimeInUtc { get; set; }
    }

    public class ChainedNoDefaultConstructorModel
    {
        public ChainedNoDefaultConstructorModel(
            UtcDateTimeRangeInclusive utcDateTimeRangeInclusive)
            : this(utcDateTimeRangeInclusive.StartDateTimeInUtc, utcDateTimeRangeInclusive.EndDateTimeInUtc)
        {
        }

        public ChainedNoDefaultConstructorModel(
            DateTime startDateTimeInUtc,
            DateTime endDateTimeInUtc)
        {
            this.StartDateTimeInUtc = startDateTimeInUtc;
            this.EndDateTimeInUtc = endDateTimeInUtc;
        }

        public DateTime EndDateTimeInUtc { get; set; }

        public DateTime StartDateTimeInUtc { get; set; }
    }

    public class UnchainedDefaultConstructorModel : IEquatable<UnchainedDefaultConstructorModel>
    {
        private const string ParameterlessConstructorValue = "OhNos";

        private const string ParameterConstructorValue = "OhYes";

        public UnchainedDefaultConstructorModel()
        {
            this.SomeValue = ParameterlessConstructorValue;
        }

        public UnchainedDefaultConstructorModel(string someValue)
        {
            this.SomeValue = someValue; // ParameterConstructorValue;
        }

        public string SomeValue { get; private set; }

        public bool Equals(UnchainedDefaultConstructorModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.SomeValue, other.SomeValue);
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

            return this.Equals((UnchainedDefaultConstructorModel)obj);
        }

        public override int GetHashCode()
        {
            return this.SomeValue != null ? this.SomeValue.GetHashCode() : 0;
        }
    }

    public class AttributedConstructorModel : IEquatable<AttributedConstructorModel>
    {
        private const string ParameterlessConstructorValue = "OhNos";

        private const string ParameterConstructorValue = "OhYes";

        public AttributedConstructorModel()
        {
            this.SomeValue = ParameterlessConstructorValue;
        }

        [JsonConstructor]
        public AttributedConstructorModel(string someValue)
        {
            this.SomeValue = someValue; // ParameterConstructorValue;
        }

        public string SomeValue { get; private set; }

        public bool Equals(AttributedConstructorModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.SomeValue, other.SomeValue);
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

            return this.Equals((AttributedConstructorModel)obj);
        }

        public override int GetHashCode()
        {
            return this.SomeValue != null ? this.SomeValue.GetHashCode() : 0;
        }
    }
}
