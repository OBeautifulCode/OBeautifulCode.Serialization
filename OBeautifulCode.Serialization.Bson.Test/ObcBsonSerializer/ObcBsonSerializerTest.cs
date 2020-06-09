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

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Bson.Test.Internal;

    using Xunit;

    public static class ObcBsonSerializerTest
    {
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

                        actual.InnerException.AsTest().Must().BeOfType<InvalidOperationException>();
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
    }
}
