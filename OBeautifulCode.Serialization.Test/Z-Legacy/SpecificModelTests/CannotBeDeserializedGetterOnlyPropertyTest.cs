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
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class CannotBeDeserializedGetterOnlyPropertyTest
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_InitializedNoConstructor___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedNoConstructor, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedNoConstructor, AssemblyMatchStrategy>);

            var expected = A.Dummy<InitializedNoConstructor>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, InitializedNoConstructor deserialized)
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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodyNoConstructor, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodyNoConstructor, AssemblyMatchStrategy>);

            var expected = A.Dummy<ExpressionBodyNoConstructor>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, ExpressionBodyNoConstructor deserialized)
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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedSingleParameterizedConstructor, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedSingleParameterizedConstructor, AssemblyMatchStrategy>);

            var expected = A.Dummy<InitializedSingleParameterizedConstructor>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, InitializedSingleParameterizedConstructor deserialized)
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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodySingleParameterizedConstructor, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodySingleParameterizedConstructor, AssemblyMatchStrategy>);

            var expected = A.Dummy<ExpressionBodySingleParameterizedConstructor>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, ExpressionBodySingleParameterizedConstructor deserialized)
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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedMultipleConstructorsWithDefault, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedMultipleConstructorsWithDefault, AssemblyMatchStrategy>);

            var expected = A.Dummy<InitializedMultipleConstructorsWithDefault>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, InitializedMultipleConstructorsWithDefault deserialized)
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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithDefault, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithDefault, AssemblyMatchStrategy>);

            var expected = A.Dummy<ExpressionBodyMultipleConstructorsWithDefault>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, ExpressionBodyMultipleConstructorsWithDefault deserialized)
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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<InitializedMultipleConstructorsWithoutDefault, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<InitializedMultipleConstructorsWithoutDefault, AssemblyMatchStrategy>);

            var expected = A.Dummy<InitializedMultipleConstructorsWithoutDefault>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, InitializedMultipleConstructorsWithoutDefault deserialized)
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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithoutDefault, AssemblyMatchStrategy>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ExpressionBodyMultipleConstructorsWithoutDefault, AssemblyMatchStrategy>);

            var expected = A.Dummy<ExpressionBodyMultipleConstructorsWithoutDefault>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, ExpressionBodyMultipleConstructorsWithoutDefault deserialized)
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

        public Enum Trigger { get; } = AssemblyMatchStrategy.AnySingleVersion;
    }

    [Serializable]
    public class ExpressionBodyNoConstructor
    {
        public string OtherProperty { get; set; }

        public Enum Trigger => AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger { get; } = AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger => AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger { get; } = AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger => AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger { get; } = AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger => AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger { get; } = AssemblyMatchStrategy.AnySingleVersion;
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

        public Enum Trigger => AssemblyMatchStrategy.AnySingleVersion;
    }
}
