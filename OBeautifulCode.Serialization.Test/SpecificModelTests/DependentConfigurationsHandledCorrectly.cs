﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependentConfigurationsHandledCorrectly.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using FluentAssertions;
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.PropertyBag;

    using Xunit;

    using static System.FormattableString;

    using JsonReader = Newtonsoft.Json.JsonReader;
    using JsonToken = Newtonsoft.Json.JsonToken;
    using JsonWriter = Newtonsoft.Json.JsonWriter;

    public static class DependentConfigurationsHandledCorrectly
    {
        [Fact]
        public static void DependentConfigurationsAreDuplicatingStuffs()
        {
            // Arrange
            var bsonConfigType = typeof(BsonConfigA);
            var jsonConfigType = typeof(JsonConfigA);
            var propBagConfigType = typeof(PropBagConfigA);

            var expected = A.Dummy<TestingDependentConfigAbstractTypeInheritor>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestingDependentConfigAbstractTypeInheritor deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.Property.SomeValue.Should().Be(expected.Property.SomeValue);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallback(
                ThrowIfObjectsDiffer,
                jsonConfigType,
                bsonConfigType,
                propBagConfigType,
                UnregisteredTypeEncounteredStrategy.Throw,
                true,
                true,
                true);
        }
    }

    public class JsonConfigA : JsonSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentSerializationConfigurationTypes =>
            new[] { typeof(JsonConfigB), typeof(JsonConfigC) };
    }

    public class BsonConfigA : BsonSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentSerializationConfigurationTypes =>
            new[] { typeof(BsonConfigB), typeof(BsonConfigC) };
    }

    public class PropBagConfigA : PropertyBagSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentSerializationConfigurationTypes =>
            new[] { typeof(PropBagConfigB), typeof(PropBagConfigC) };
    }

    public class JsonConfigB : JsonSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentSerializationConfigurationTypes =>
            new[] { typeof(JsonConfigC) };
    }

    public class BsonConfigB : BsonSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentSerializationConfigurationTypes =>
            new[] { typeof(BsonConfigC) };
    }

    public class PropBagConfigB : PropertyBagSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentSerializationConfigurationTypes =>
            new[] { typeof(PropBagConfigC) };
    }

    public class JsonConfigC : JsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery =>
            new[] { typeof(TestingDependentConfigAbstractType) };

        protected override IReadOnlyCollection<RegisteredJsonConverter> ConvertersToRegister => new[]
        {
            new RegisteredJsonConverter(
                () => new TestingDependentConverter(),
                () => new TestingDependentConverter(),
                A.Dummy<RegisteredJsonConverterOutputKind>(),
                new[] { typeof(TestingDependentConfigType) }),
        };
    }

    public class TestingDependentConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            var payload = ((TestingDependentConfigType)value).SomeValue;

            var payloadObject = new JValue(payload);
            payloadObject.WriteTo(writer);
        }

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var payload = reader.Value;
            var stringValue = payload.ToString();
            var result = new TestingDependentConfigType { SomeValue = stringValue };

            return result;
        }

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType)
        {
            return objectType == typeof(TestingDependentConfigType);
        }
    }

    public class BsonConfigC : BsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery =>
            new[] { typeof(TestingDependentConfigAbstractType) };

        protected override IReadOnlyCollection<RegisteredBsonSerializer> SerializersToRegister => new[]
        {
            new RegisteredBsonSerializer(
                () => new TestingDependentSerializer(),
                new[] { typeof(TestingDependentConfigType) }),
        };
    }

    public class TestingDependentSerializer : SerializerBase<TestingDependentConfigType>
    {
        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TestingDependentConfigType value)
        {
            new { context }.AsArg().Must().NotBeNull();
            new { value }.AsArg().Must().NotBeNull();

            context.Writer.WriteStartDocument();
            context.Writer.WriteName(nameof(value.SomeValue));
            context.Writer.WriteString(value.SomeValue);
            context.Writer.WriteEndDocument();
        }

        /// <inheritdoc />
        public override TestingDependentConfigType Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();

            context.Reader.ReadStartDocument();
            context.Reader.ReadName(new Utf8NameDecoder());
            var type = context.Reader.GetCurrentBsonType();

            TestingDependentConfigType result;
            switch (type)
            {
                case BsonType.String:
                    result = new TestingDependentConfigType { SomeValue = context.Reader.ReadString() };
                    break;
                default:
                    throw new NotSupportedException(Invariant($"Cannot convert a {type} to a {nameof(TestingDependentConfigType)}."));
            }

            context.Reader.ReadEndDocument();

            return result;
        }
    }

    public class PropBagConfigC : PropertyBagSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<Type> TypesToAutoRegisterWithDiscovery =>
            new[] { typeof(TestingDependentConfigAbstractType) };

        protected override IReadOnlyCollection<RegisteredStringSerializer> SerializersToRegister => new[]
        {
            new RegisteredStringSerializer(
                () => new TestingDependentPropBagSerializer(),
                new[] { typeof(TestingDependentConfigType) }),
        };
    }

    public class TestingDependentPropBagSerializer : IStringSerializeAndDeserialize
    {
        public Type SerializationConfigurationType => typeof(NullPropertyBagConfiguration);

        public string SerializeToString(object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }
            else if (objectToSerialize.GetType() != typeof(TestingDependentConfigType))
            {
                throw new NotSupportedException(Invariant($"Type: {objectToSerialize.GetType()} is not supported by this serializer: {this.GetType()}.  Confirm your configuration is correct."));
            }
            else
            {
                return ((TestingDependentConfigType)objectToSerialize).SomeValue;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.Convert.ChangeType(System.Object,System.Type)", Justification = "This isn't real.")]
        public T Deserialize<T>(string serializedString)
        {
            var result = this.Deserialize(serializedString, typeof(T));
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public object Deserialize(string serializedString, Type type)
        {
            if (type != typeof(TestingDependentConfigType))
            {
                throw new NotSupportedException(Invariant($"Type: {type} is not supported by this serializer: {this.GetType()}.  Confirm your configuration is correct."));
            }
            else if (serializedString == null)
            {
                return null;
            }
            else
            {
                return new TestingDependentConfigType { SomeValue = serializedString };
            }
        }
    }

    public class TestingDependentConfigType
    {
        public string SomeValue { get; set; }
    }

    public abstract class TestingDependentConfigAbstractType
    {
        public TestingDependentConfigType Property { get; set; }
    }

    public class TestingDependentConfigAbstractTypeInheritor : TestingDependentConfigAbstractType
    {
    }
}
