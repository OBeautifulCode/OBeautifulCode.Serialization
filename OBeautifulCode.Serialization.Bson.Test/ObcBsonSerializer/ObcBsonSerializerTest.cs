// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using FakeItEasy;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    using static System.FormattableString;

    public static class ObcBsonSerializerTest
    {
        #pragma warning disable SA1202 // Elements should be ordered by access
        [Fact]
        public static void GetSerializationConfigurationInUseForDeserialization___Should_return_serialization_configuration_of_serializer_being_used_to_deserialize_payload___When_called()
        {
            // When the BSON serializer hits the AbstractClass property, it will call ObcBsonDiscriminatorConvention.GetActualType
            // which will de-reference ConcreteClassForThreadStaticTest and then check whether that type is registered by calling
            // GetSerializationConfigurationInUseForDeserialization().ThrowOnUnregisteredTypeIfAppropriate(), before it moves on.
            // We are using that to test whether the right serialization configuration is returned by running multiple parallel
            // de-serialization operations with a configuration that will throw and one that will not throw when checking
            // ConcreteClassForThreadStaticTest.
            Action test = () =>
            {
                // Arrange
                var payload = "{ \"_t\" : \"OBeautifulCode.Serialization.Bson.Test.ObcBsonSerializerTest+ModelObjectForThreadStaticTest, OBeautifulCode.Serialization.Bson.Test\", \"AbstractClass\" : { \"_t\" : \"OBeautifulCode.Serialization.Bson.Test.ObcBsonSerializerTest+ConcreteClassForThreadStaticTest, OBeautifulCode.Serialization.Bson.Test\", \"StringProperty\" : \"my-string\", \"IntProperty\" : -392 } }";

                var throwingSerializer = new ObcBsonSerializer<ThrowingSerializationConfigurationForThreadStaticTest>();

                var notThrowingSerializer = new ObcBsonSerializer<NotThrowingSerializationConfigurationForThreadStaticTest>();

                var expected = new ModelObjectForThreadStaticTest(new ConcreteClassForThreadStaticTest("my-string", -392));

                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 10,
                };

                // Act
                Parallel.For(0, 1000, parallelOptions, index =>
                {
                    if ((index % 2) == 0)
                    {
                        var actual = Record.Exception(() => throwingSerializer.Deserialize<ModelObjectForThreadStaticTest>(payload));

                        // ReSharper disable once PossibleNullReferenceException
                        actual.InnerException.AsTest().Must().BeOfType<InvalidOperationException>();

                        // ReSharper disable once PossibleNullReferenceException
                        actual.InnerException.Message.Must().BeEqualTo("Attempted to perform operation on unregistered type 'ObcBsonSerializerTest.ConcreteClassForThreadStaticTest'.");
                    }
                    else
                    {
                        var actual = notThrowingSerializer.Deserialize<ModelObjectForThreadStaticTest>(payload);

                        actual.AsTest().Must().BeEqualTo(expected);
                    }
                });
            };

            test.ExecuteInNewAppDomain();
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_model_using_RootObjectThatSerializesToStringWrapper___When_model_type_is_registered_with_an_IBsonSerializer_that_serializes_to_string()
        {
            // Arrange
            var expected = A.Dummy<ModelObjectThatSerializesToString>();

            void VerificationCallback(
                string serialized,
                SerializationFormat format,
                ModelObjectThatSerializesToString deserialized)
            {
                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().ContainString(typeof(RootObjectThatSerializesToStringWrapper<>).Name);
                }
                else if (format == SerializationFormat.Binary)
                {
                    // skip verifying binary
                }
                else
                {
                    throw new NotSupportedException("This format is not supported: " + format);
                }

                deserialized.AsTest().Must().BeEqualTo(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(VerificationCallback, typeof(ModelObjectThatSerializesToStringSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_string_using_RootObjectThatSerializesToStringWrapper___When_called()
        {
            // Arrange
            var expected = A.Dummy<string>();

            void VerificationCallback(
                string serialized,
                SerializationFormat format,
                string deserialized)
            {
                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().ContainString(typeof(RootObjectThatSerializesToStringWrapper<>).Name);
                }
                else if (format == SerializationFormat.Binary)
                {
                    // skip verifying binary
                }
                else
                {
                    throw new NotSupportedException("This format is not supported: " + format);
                }

                deserialized.AsTest().Must().BeEqualTo(expected);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(VerificationCallback, typeof(ModelObjectThatSerializesToStringSerializationConfiguration));
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = ObcSuppressBecause.CA1812_AvoidUninstantiatedInternalClasses_SerializationConfigurationClassInstantedBySerializer)]
        private class NotThrowingSerializationConfigurationForThreadStaticTest : BsonSerializationConfigurationBase
        {
            /// <inheritdoc />
            public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

            /// <inheritdoc />
            protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(ThrowingSerializationConfigurationForThreadStaticTest).ToBsonSerializationConfigurationType() };

            /// <inheritdoc />
            protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
            {
                new TypeToRegisterForBson(typeof(ConcreteClassForThreadStaticTest), MemberTypesToInclude.None, RelatedTypesToInclude.None, null, null),
            };
        }

        private class ThrowingSerializationConfigurationForThreadStaticTest : BsonSerializationConfigurationBase
        {
            /// <inheritdoc />
            public override UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy => UnregisteredTypeEncounteredStrategy.Throw;

            /// <inheritdoc />
            protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
            {
                new TypeToRegisterForBson(typeof(ModelObjectForThreadStaticTest), MemberTypesToInclude.None, RelatedTypesToInclude.None, null, null),
                new TypeToRegisterForBson(typeof(AbstractClassForThreadStaticTest), MemberTypesToInclude.None, RelatedTypesToInclude.None, null, null),
            };
        }

        [Serializable]
        private class ModelObjectForThreadStaticTest : IEquatable<ModelObjectForThreadStaticTest>
        {
            public ModelObjectForThreadStaticTest(
                AbstractClassForThreadStaticTest abstractClass)
            {
                this.AbstractClass = abstractClass;
            }

            public AbstractClassForThreadStaticTest AbstractClass { get; private set; }

            public bool Equals(
                ModelObjectForThreadStaticTest other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                var thisConcreteClass = (ConcreteClassForThreadStaticTest)this.AbstractClass;
                var otherConcreteClass = (ConcreteClassForThreadStaticTest)other.AbstractClass;

                var result = (thisConcreteClass.IntProperty == otherConcreteClass.IntProperty) &&
                             (thisConcreteClass.StringProperty == otherConcreteClass.StringProperty);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelObjectForThreadStaticTest);

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        }

        [Serializable]
        private abstract class AbstractClassForThreadStaticTest
        {
            protected AbstractClassForThreadStaticTest(
                string stringProperty)
            {
                this.StringProperty = stringProperty;
            }

            public string StringProperty { get; private set; }
        }

        [Serializable]
        private class ConcreteClassForThreadStaticTest : AbstractClassForThreadStaticTest
        {
            public ConcreteClassForThreadStaticTest(
                string stringProperty,
                int intProperty)
                : base(stringProperty)
            {
                this.IntProperty = intProperty;
            }

            public int IntProperty { get; private set; }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ModelObjectThatSerializesToString : IEquatable<ModelObjectThatSerializesToString>
        {
            public ModelObjectThatSerializesToString(
                string value)
            {
                this.Value = value;
            }

            public string Value { get; private set; }

            public bool Equals(
                ModelObjectThatSerializesToString other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                var result = this.Value == other.Value;

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelObjectThatSerializesToString);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode()
            {
                throw new NotSupportedException();
            }
        }

        private class ModelObjectThatSerializesToStringDependentSerializationConfiguration : BsonSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
            {
                new TypeToRegisterForBson(
                    typeof(ModelObjectThatSerializesToString),
                    MemberTypesToInclude.None,
                    RelatedTypesToInclude.None,
                    new BsonSerializerBuilder(
                        () => new ModelObjectThatSerializesToStringSerializer(),
                        BsonSerializerOutputKind.String),
                    null),
            };
        }

        private class ModelObjectThatSerializesToStringSerializationConfiguration : BsonSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(ModelObjectThatSerializesToStringDependentSerializationConfiguration).ToBsonSerializationConfigurationType() };
        }

        private class ModelObjectThatSerializesToStringSerializer : SerializerBase<ModelObjectThatSerializesToString>
        {
            /// <inheritdoc />
            public override ModelObjectThatSerializesToString Deserialize(
                BsonDeserializationContext context,
                BsonDeserializationArgs args)
            {
                new { context }.AsArg().Must().NotBeNull();

                var type = context.Reader.GetCurrentBsonType();

                ModelObjectThatSerializesToString result;

                switch (type)
                {
                    case BsonType.String:
                        var value = context.Reader.ReadString();
                        result = new ModelObjectThatSerializesToString(value);
                        break;
                    case BsonType.Null:
                        context.Reader.ReadNull();
                        result = null;
                        break;
                    default:
                        throw new NotSupportedException(Invariant($"Cannot convert a {type} to a {nameof(ModelObjectThatSerializesToString)}."));
                }

                return result;
            }

            /// <inheritdoc />
            public override void Serialize(
                BsonSerializationContext context,
                BsonSerializationArgs args,
                ModelObjectThatSerializesToString value)
            {
                new { context }.AsArg().Must().NotBeNull();

                if (value == null)
                {
                    context.Writer.WriteNull();
                }
                else
                {
                    context.Writer.WriteString(value.Value);
                }
            }
        }
        #pragma warning restore SA1202 // Elements should be ordered by access
    }
}
