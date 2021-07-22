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
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;
    using OBeautifulCode.Type;

    using Xunit;

    public static class CannotBeDeserializedGetterOnlyPropertyTest
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedNoConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedNoConstructor, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedNoConstructor, VersionMatchStrategy>);

            var expected = A.Dummy<InitializedNoConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, InitializedNoConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty.AsTest().Must().BeEqualTo(expected.OtherProperty);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodyNoConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodyNoConstructor, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodyNoConstructor, VersionMatchStrategy>);

            var expected = A.Dummy<ExpressionBodyNoConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, ExpressionBodyNoConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty.AsTest().Must().BeEqualTo(expected.OtherProperty);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).")]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedSingleParameterizedConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedSingleParameterizedConstructor, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedSingleParameterizedConstructor, VersionMatchStrategy>);

            var expected = A.Dummy<InitializedSingleParameterizedConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, InitializedSingleParameterizedConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodySingleParameterizedConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodySingleParameterizedConstructor, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodySingleParameterizedConstructor, VersionMatchStrategy>);

            var expected = A.Dummy<ExpressionBodySingleParameterizedConstructor>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, ExpressionBodySingleParameterizedConstructor deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedMultipleConstructorsWithDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedMultipleConstructorsWithDefault, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedMultipleConstructorsWithDefault, VersionMatchStrategy>);

            var expected = A.Dummy<InitializedMultipleConstructorsWithDefault>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, InitializedMultipleConstructorsWithDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodyMultipleConstructorsWithDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithDefault, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithDefault, VersionMatchStrategy>);

            var expected = A.Dummy<ExpressionBodyMultipleConstructorsWithDefault>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, ExpressionBodyMultipleConstructorsWithDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  In BSON, the issue is specifically with models that do not have a default constructor (regardless of whether there are multiple constructors or a single parameterized constructor).")]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedMultipleConstructorsWithoutDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedMultipleConstructorsWithoutDefault, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedMultipleConstructorsWithoutDefault, VersionMatchStrategy>);

            var expected = A.Dummy<InitializedMultipleConstructorsWithoutDefault>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, InitializedMultipleConstructorsWithoutDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact(Skip = "Throws in JSON, see note in CamelStrictConstructorContractResolver.CreateObjectContract.  Works fine in BSON.")]
        public static void Deserialize___Should_roundtrip_a_serialized_ExpressionBodyMultipleConstructorsWithoutDefault___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithoutDefault, VersionMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithoutDefault, VersionMatchStrategy>);

            var expected = A.Dummy<ExpressionBodyMultipleConstructorsWithoutDefault>();

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, ExpressionBodyMultipleConstructorsWithoutDefault deserialized)
            {
                deserialized.AsTest().Must().NotBeNull();

                deserialized.OtherProperty1.AsTest().Must().BeEqualTo(expected.OtherProperty1);

                deserialized.OtherProperty2.AsTest().Must().BeEqualTo(expected.OtherProperty2);

                deserialized.Trigger.AsTest().Must().BeEqualTo(expected.Trigger);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }
    }

    [Serializable]
    public class InitializedNoConstructor
    {
        public string OtherProperty { get; set; }

        public Enum Trigger { get; } = VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
    public class ExpressionBodyNoConstructor
    {
        public string OtherProperty { get; set; }

        public Enum Trigger => VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
    public class InitializedSingleParameterizedConstructor
    {
        public InitializedSingleParameterizedConstructor(
            string otherProperty1)
        {
            this.OtherProperty1 = otherProperty1;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger { get; } = VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
    public class ExpressionBodySingleParameterizedConstructor
    {
        public ExpressionBodySingleParameterizedConstructor(
            string otherProperty1)
        {
            this.OtherProperty1 = otherProperty1;
        }

        public string OtherProperty1 { get; private set; }

        public string OtherProperty2 { get; private set; }

        public Enum Trigger => VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
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

        public Enum Trigger { get; } = VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
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

        public Enum Trigger => VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
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

        public Enum Trigger { get; } = VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
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

        public Enum Trigger => VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
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

        public Enum Trigger { get; } = VersionMatchStrategy.AnySingleVersion;
    }

    [Serializable]
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

        public Enum Trigger => VersionMatchStrategy.AnySingleVersion;
    }
}
