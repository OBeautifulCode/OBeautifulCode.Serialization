// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CannotBeDeserializedGetterOnlyPropertyTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class CannotBeDeserializedGetterOnlyPropertyTest
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedNoConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<InitializedNoConstructor>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<InitializedNoConstructor>);

            var expected = A.Dummy<InitializedNoConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, InitializedNoConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty.AsTest().Must().BeEqualTo(expected.OtherProperty);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodyNoConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<ExpressionBodyNoConstructor>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<ExpressionBodyNoConstructor>);

            var expected = A.Dummy<ExpressionBodyNoConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ExpressionBodyNoConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty.AsTest().Must().BeEqualTo(expected.OtherProperty);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).")]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedSingleParameterizedConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<InitializedSingleParameterizedConstructor>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<InitializedSingleParameterizedConstructor>);

            var expected = A.Dummy<InitializedSingleParameterizedConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, InitializedSingleParameterizedConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodySingleParameterizedConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<ExpressionBodySingleParameterizedConstructor>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<ExpressionBodySingleParameterizedConstructor>);

            var expected = A.Dummy<ExpressionBodySingleParameterizedConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ExpressionBodySingleParameterizedConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedMultipleConstructorsWithDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<InitializedMultipleConstructorsWithDefault>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<InitializedMultipleConstructorsWithDefault>);

            var expected = A.Dummy<InitializedMultipleConstructorsWithDefault>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, InitializedMultipleConstructorsWithDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodyMultipleConstructorsWithDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithDefault>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithDefault>);

            var expected = A.Dummy<ExpressionBodyMultipleConstructorsWithDefault>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ExpressionBodyMultipleConstructorsWithDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).")]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedMultipleConstructorsWithoutDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<InitializedMultipleConstructorsWithoutDefault>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<InitializedMultipleConstructorsWithoutDefault>);

            var expected = A.Dummy<InitializedMultipleConstructorsWithoutDefault>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, InitializedMultipleConstructorsWithoutDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodyMultipleConstructorsWithoutDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(GenericDiscoveryBsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithoutDefault>);
            var jsonConfigType = typeof(GenericDiscoveryJsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithoutDefault>);

            var expected = A.Dummy<ExpressionBodyMultipleConstructorsWithoutDefault>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ExpressionBodyMultipleConstructorsWithoutDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(ThrowIfObjectsDiffer, jsonConfigType, bsonConfigType);
        }
    }

    public class InitializedNoConstructor
    {
        public string OtherProperty { get; set; }

        public Enum Trigger { get; } = MultipleMatchStrategy.NewestVersion;
    }

    public class ExpressionBodyNoConstructor
    {
        public string OtherProperty { get; set; }

        public Enum Trigger => MultipleMatchStrategy.NewestVersion;
    }

    public class InitializedSingleParameterizedConstructor
    {
        public InitializedSingleParameterizedConstructor(
            string otherProperty1)
        {
            this.OtherProperty1 = otherProperty1;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger { get; } = MultipleMatchStrategy.NewestVersion;
    }

    public class ExpressionBodySingleParameterizedConstructor
    {
        public ExpressionBodySingleParameterizedConstructor(
            string otherProperty1)
        {
            this.OtherProperty1 = otherProperty1;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger => MultipleMatchStrategy.NewestVersion;
    }

    public class InitializedMultipleConstructorsWithDefault
    {
        public InitializedMultipleConstructorsWithDefault()
        {
        }

        public InitializedMultipleConstructorsWithDefault(
            string otherProperty1,
            string otherProperty2)
        {
            this.OtherProperty1 = otherProperty1;
            this.OtherProperty2 = otherProperty2;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger { get; } = MultipleMatchStrategy.NewestVersion;
    }

    public class ExpressionBodyMultipleConstructorsWithDefault
    {
        public ExpressionBodyMultipleConstructorsWithDefault()
        {
        }

        public ExpressionBodyMultipleConstructorsWithDefault(
            string otherProperty1,
            string otherProperty2)
        {
            this.OtherProperty1 = otherProperty1;
            this.OtherProperty2 = otherProperty2;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger => MultipleMatchStrategy.NewestVersion;
    }

    public class InitializedMultipleConstructorsWithoutDefault
    {
        public InitializedMultipleConstructorsWithoutDefault(
            string otherProperty1)
            : this(otherProperty1, null)
        {
        }

        public InitializedMultipleConstructorsWithoutDefault(
            string otherProperty1,
            string otherProperty2)
        {
            this.OtherProperty1 = otherProperty1;
            this.OtherProperty2 = otherProperty2;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger { get; } = MultipleMatchStrategy.NewestVersion;
    }

    public class ExpressionBodyMultipleConstructorsWithoutDefault
    {
        public ExpressionBodyMultipleConstructorsWithoutDefault(
            string otherProperty1)
            : this(otherProperty1, null)
        {
        }

        public ExpressionBodyMultipleConstructorsWithoutDefault(
            string otherProperty1,
            string otherProperty2)
        {
            this.OtherProperty1 = otherProperty1;
            this.OtherProperty2 = otherProperty2;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger => MultipleMatchStrategy.NewestVersion;
    }

    public class InitializedPublicSetProperty
    {
        public InitializedPublicSetProperty(
            string otherProperty1,
            string otherProperty2)
        {
            this.OtherProperty1 = otherProperty1;
            this.OtherProperty2 = otherProperty2;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public string PublicSet { get; set; }

        public Enum Trigger { get; } = MultipleMatchStrategy.NewestVersion;
    }

    public class ExpressionBodyPublicSetProperty
    {
        public ExpressionBodyPublicSetProperty(
            string otherProperty1)
            : this(otherProperty1, null)
        {
        }

        public ExpressionBodyPublicSetProperty(
            string otherProperty1,
            string otherProperty2)
        {
            this.OtherProperty1 = otherProperty1;
            this.OtherProperty2 = otherProperty2;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public string PublicSet { get; set; }

        public Enum Trigger => MultipleMatchStrategy.NewestVersion;
    }
}
