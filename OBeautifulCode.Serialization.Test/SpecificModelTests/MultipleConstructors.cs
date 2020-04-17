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
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;

    using Xunit;

    public static class MultipleConstructors
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel>);

            var expected = A.Dummy<UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.StartDateTimeInUtc.AsTest().Must().BeEqualTo(expected.StartDateTimeInUtc);
                deserialized.EndDateTimeInUtc.AsTest().Must().BeEqualTo(expected.EndDateTimeInUtc);
                deserialized.ExpressionBodiedProperty.AsTest().Must().BeEqualTo(expected.ExpressionBodiedProperty);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_ChainedNoDefaultConstructorWithExpressionBodyPropertyModel___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ChainedNoDefaultConstructorWithExpressionBodyPropertyModel>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ChainedNoDefaultConstructorWithExpressionBodyPropertyModel>);

            var expected = A.Dummy<ChainedNoDefaultConstructorWithExpressionBodyPropertyModel>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ChainedNoDefaultConstructorWithExpressionBodyPropertyModel deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.StartDateTimeInUtc.AsTest().Must().BeEqualTo(expected.StartDateTimeInUtc);
                deserialized.EndDateTimeInUtc.AsTest().Must().BeEqualTo(expected.EndDateTimeInUtc);
                deserialized.ExpressionBodiedProperty.AsTest().Must().BeEqualTo(expected.ExpressionBodiedProperty);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_UnchainedDefaultConstructorWithExpressionBodyPropertyModel___When_called()
        {
            // Arrange
            var expectedParameterless = new UnchainedDefaultConstructorWithExpressionBodyPropertyModel();
            var expectedParameter = new UnchainedDefaultConstructorWithExpressionBodyPropertyModel(A.Dummy<string>());

            // Act & Assert
            expectedParameter.RoundtripSerializeWithEquatableAssertion();
            expectedParameterless.RoundtripSerializeWithEquatableAssertion();
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_ChainedDefaultConstructorWithExpressionBodyPropertyModel___When_called()
        {
            // Arrange
            var expectedParameterless = new ChainedDefaultConstructorWithExpressionBodyPropertyModel();
            var expectedParameter = new ChainedDefaultConstructorWithExpressionBodyPropertyModel(A.Dummy<string>());

            // Act & Assert
            expectedParameter.RoundtripSerializeWithEquatableAssertion();
            expectedParameterless.RoundtripSerializeWithEquatableAssertion();
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel___When_called()
        {
            // Arrange
            var expectedParameterless = new AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel();
            var expectedParameter = new AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel(A.Dummy<string>());

            // Act & Assert
            expectedParameter.RoundtripSerializeWithEquatableAssertion();
            expectedParameterless.RoundtripSerializeWithEquatableAssertion();
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel___When_called()
        {
            // Arrange
            var expectedParameter1 = new AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel(A.Dummy<string>());
            var expectedParameter2 = new AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel(A.Dummy<string>(), A.Dummy<string>());

            // Act & Assert
            expectedParameter1.RoundtripSerializeWithEquatableAssertion();
            expectedParameter2.RoundtripSerializeWithEquatableAssertion();
        }

        [Fact(Skip = "Some models with initialized getter-only properties (e.g. public DateTime MyProperty { get; } = DateTime.Now) do not roundtrip to/from BSON. JSON seems to work just fine.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).  If there is no default constructor the initialized property is not initialized.")]
        public static void Deserialize___Should_roundtrip_a_serialized_UnchainedNoDefaultConstructorWithInitializedGetterOnlyPropertyModel___When_called()
        {
        }

        [Fact(Skip = "Some models with initialized getter-only properties (e.g. public DateTime MyProperty { get; } = DateTime.Now) do not roundtrip to/from BSON. JSON seems to work just fine.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).  If there is no default constructor the initialized property is not initialized.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ChainedNoDefaultConstructorWithInitializedGetterOnlyPropertyModel___When_called()
        {
        }

        [Fact(Skip = "Some models with initialized getter-only properties (e.g. public DateTime MyProperty { get; } = DateTime.Now) do not roundtrip to/from BSON. JSON seems to work just fine.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).  If there is no default constructor the initialized property is not initialized.")]
        public static void Deserialize___Should_roundtrip_a_serialized_UnchainedDefaultConstructorWithInitializedGetterOnlyPropertyModel___When_called()
        {
        }

        [Fact(Skip = "Some models with initialized getter-only properties (e.g. public DateTime MyProperty { get; } = DateTime.Now) do not roundtrip to/from BSON. JSON seems to work just fine.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).  If there is no default constructor the initialized property is not initialized.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ChainedDefaultConstructorWithInitializedGetterOnlyPropertyModel___When_called()
        {
        }

        [Fact(Skip = "Some models with initialized getter-only properties (e.g. public DateTime MyProperty { get; } = DateTime.Now) do not roundtrip to/from BSON. JSON seems to work just fine.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).  If there is no default constructor the initialized property is not initialized.")]
        public static void Deserialize___Should_roundtrip_a_serialized_AttributedUnchainedDefaultConstructorWithInitializedGetterOnlyPropertyModel___When_called()
        {
        }

        [Fact(Skip = "Some models with initialized getter-only properties (e.g. public DateTime MyProperty { get; } = DateTime.Now) do not roundtrip to/from BSON. JSON seems to work just fine.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).  If there is no default constructor the initialized property is not initialized.")]
        public static void Deserialize___Should_roundtrip_a_serialized_AttributedChainedNoDefaultConstructorWithInitializedGetterOnlyPropertyModel___When_called()
        {
        }
    }

    public class UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel
    {
        public UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel(
            UtcDateTimeRangeInclusive utcDateTimeRangeInclusive)
        {
            this.StartDateTimeInUtc = utcDateTimeRangeInclusive.StartDateTimeInUtc;
            this.EndDateTimeInUtc = utcDateTimeRangeInclusive.EndDateTimeInUtc;
        }

        public UnchainedNoDefaultConstructorWithExpressionBodyPropertyModel(
            DateTime startDateTimeInUtc,
            DateTime endDateTimeInUtc)
        {
            this.StartDateTimeInUtc = startDateTimeInUtc;
            this.EndDateTimeInUtc = endDateTimeInUtc;
        }

        public DateTime EndDateTimeInUtc { get; set; }

        public DateTime StartDateTimeInUtc { get; set; }

        public DateTime ExpressionBodiedProperty => new DateTime(2020, 1, 5, 3, 4, 9, DateTimeKind.Utc);
    }

    public class ChainedNoDefaultConstructorWithExpressionBodyPropertyModel
    {
        public ChainedNoDefaultConstructorWithExpressionBodyPropertyModel(
            UtcDateTimeRangeInclusive utcDateTimeRangeInclusive)
            : this(utcDateTimeRangeInclusive.StartDateTimeInUtc, utcDateTimeRangeInclusive.EndDateTimeInUtc)
        {
        }

        public ChainedNoDefaultConstructorWithExpressionBodyPropertyModel(
            DateTime startDateTimeInUtc,
            DateTime endDateTimeInUtc)
        {
            this.StartDateTimeInUtc = startDateTimeInUtc;
            this.EndDateTimeInUtc = endDateTimeInUtc;
        }

        public DateTime EndDateTimeInUtc { get; set; }

        public DateTime StartDateTimeInUtc { get; set; }

        public DateTime ExpressionBodiedProperty => new DateTime(2020, 1, 5, 3, 4, 9, DateTimeKind.Utc);
    }

    public class UnchainedDefaultConstructorWithExpressionBodyPropertyModel : IEquatable<UnchainedDefaultConstructorWithExpressionBodyPropertyModel>
    {
        private const string ParameterlessConstructorValue = "OhNos";

        private const string ParameterConstructorValue = "OhYes";

        public UnchainedDefaultConstructorWithExpressionBodyPropertyModel()
        {
            this.SomeValue = ParameterlessConstructorValue;
        }

        public UnchainedDefaultConstructorWithExpressionBodyPropertyModel(string someValue)
        {
            this.SomeValue = someValue; // ParameterConstructorValue;
        }

        public string SomeValue { get; private set; }

        public DateTime ExpressionBodiedProperty => new DateTime(2020, 1, 5, 3, 4, 9, DateTimeKind.Utc);

        public bool Equals(UnchainedDefaultConstructorWithExpressionBodyPropertyModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var result = string.Equals(this.SomeValue, other.SomeValue)
                         && this.ExpressionBodiedProperty.IsEqualTo(other.ExpressionBodiedProperty);

            return result;
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

            return this.Equals((UnchainedDefaultConstructorWithExpressionBodyPropertyModel)obj);
        }

        public override int GetHashCode()
        {
            var result = HashCodeHelper.Initialize().Hash(this.SomeValue).Hash(this.ExpressionBodiedProperty).Value;

            return result;
        }
    }

    public class ChainedDefaultConstructorWithExpressionBodyPropertyModel : IEquatable<ChainedDefaultConstructorWithExpressionBodyPropertyModel>
    {
        private const string ParameterlessConstructorValue = "OhNos";

        private const string ParameterConstructorValue = "OhYes";

        public ChainedDefaultConstructorWithExpressionBodyPropertyModel()
            : this(ParameterlessConstructorValue)
        {
        }

        public ChainedDefaultConstructorWithExpressionBodyPropertyModel(string someValue)
        {
            this.SomeValue = someValue; // ParameterConstructorValue;
        }

        public string SomeValue { get; private set; }

        public DateTime ExpressionBodiedProperty => new DateTime(2020, 1, 5, 3, 4, 9, DateTimeKind.Utc);

        public bool Equals(ChainedDefaultConstructorWithExpressionBodyPropertyModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var result = string.Equals(this.SomeValue, other.SomeValue)
                         && this.ExpressionBodiedProperty.IsEqualTo(other.ExpressionBodiedProperty);

            return result;
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

            return this.Equals((ChainedDefaultConstructorWithExpressionBodyPropertyModel)obj);
        }

        public override int GetHashCode()
        {
            var result = HashCodeHelper.Initialize().Hash(this.SomeValue).Hash(this.ExpressionBodiedProperty).Value;

            return result;
        }
    }

    public class AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel : IEquatable<AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel>
    {
        private const string ParameterlessConstructorValue = "OhNos";

        private const string ParameterConstructorValue = "OhYes";

        public AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel()
        {
            this.SomeValue = ParameterlessConstructorValue;
        }

        [JsonConstructor]
        public AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel(string someValue)
        {
            this.SomeValue = someValue; // ParameterConstructorValue;
        }

        public string SomeValue { get; private set; }

        public DateTime ExpressionBodiedProperty => new DateTime(2020, 1, 5, 3, 4, 9, DateTimeKind.Utc);

        public bool Equals(AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var result = string.Equals(this.SomeValue, other.SomeValue)
                         && (this.ExpressionBodiedProperty == other.ExpressionBodiedProperty);

            return result;
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

            return this.Equals((AttributedUnchainedDefaultConstructorWithExpressionBodyPropertyModel)obj);
        }

        public override int GetHashCode()
        {
            var result = HashCodeHelper.Initialize().Hash(this.SomeValue).Hash(this.ExpressionBodiedProperty).Value;

            return result;
        }
    }

    public class AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel : IEquatable<AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel>
    {
        private const string ParameterlessConstructorValue = "OhNos";

        private const string ParameterConstructorValue = "OhYes";

        public AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel(string someValue)
            : this(someValue, ParameterlessConstructorValue)
        {
        }

        [JsonConstructor]
        public AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel(string someValue, string someValue2)
        {
            this.SomeValue = someValue;
            this.SomeValue2 = someValue2;
        }

        public string SomeValue { get; private set; }

        public string SomeValue2 { get; private set; }

        public DateTime ExpressionBodiedProperty => new DateTime(2020, 1, 5, 3, 4, 9, DateTimeKind.Utc);

        public bool Equals(AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            var result = string.Equals(this.SomeValue, other.SomeValue)
                         && string.Equals(this.SomeValue2, other.SomeValue2)
                         && (this.ExpressionBodiedProperty == other.ExpressionBodiedProperty);

            return result;
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

            return this.Equals((AttributedChainedNoDefaultConstructorWithExpressionBodyPropertyModel)obj);
        }

        public override int GetHashCode()
        {
            var result = HashCodeHelper.Initialize().Hash(this.SomeValue).Hash(this.SomeValue2).Hash(this.ExpressionBodiedProperty).Value;

            return result;
        }
    }
}
