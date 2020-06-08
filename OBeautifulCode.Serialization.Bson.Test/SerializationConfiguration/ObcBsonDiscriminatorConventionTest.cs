// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonDiscriminatorConventionTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    using Xunit;

    public static class ObcBsonDiscriminatorConventionTest
    {
        [Fact]
        public static void GetActualType___Should_return_serialized_runtime_types___When_objects_were_serialized_using_mix_of_legacy_and_OBC_discriminator_conventions_and_method_is_called_on_legacy_payload_before_current_payload()
        {
            Action test = () =>
            {
                // Arrange
                var legacyPayload = "{ \"_t\" : \"ModelObjectForDiscriminatorConventionTest\", \"AbstractClass\" : { \"_t\" : \"ConcreteClassForDiscriminatorConventionTest\", \"StringProperty\" : \"my-string\", \"IntProperty\" : -392 } }";

                var currentPayload = "{ \"_t\" : \"OBeautifulCode.Serialization.Bson.Test.ObcBsonDiscriminatorConventionTest+ModelObjectForDiscriminatorConventionTest, OBeautifulCode.Serialization.Bson.Test\", \"AbstractClass\" : { \"_t\" : \"OBeautifulCode.Serialization.Bson.Test.ObcBsonDiscriminatorConventionTest+ConcreteClassForDiscriminatorConventionTest, OBeautifulCode.Serialization.Bson.Test\", \"StringProperty\" : \"my-string\", \"IntProperty\" : -392 } }";

                var serializer = new ObcBsonSerializer<TypesToRegisterBsonSerializationConfiguration<ModelObjectForDiscriminatorConventionTest>>();

                var expected = new ModelObjectForDiscriminatorConventionTest(new ConcreteClassForDiscriminatorConventionTest("my-string", -392));

                // Act
                var actual1 = serializer.Deserialize<ModelObjectForDiscriminatorConventionTest>(legacyPayload);

                var actual2 = serializer.Deserialize<ModelObjectForDiscriminatorConventionTest>(currentPayload);

                // Assert
                actual1.AsTest().Must().BeEqualTo(expected);
                actual2.AsTest().Must().BeEqualTo(expected);
            };

            test.ExecuteInNewAppDomain();
        }

        [Fact]
        public static void GetActualType___Should_return_serialized_runtime_types___When_objects_were_serialized_using_mix_of_legacy_and_OBC_discriminator_conventions_and_method_is_called_on_current_payload_before_legacy_payload()
        {
            Action test = () =>
            {
                // Arrange
                var legacyPayload = "{ \"_t\" : \"ModelObjectForDiscriminatorConventionTest\", \"AbstractClass\" : { \"_t\" : \"ConcreteClassForDiscriminatorConventionTest\", \"StringProperty\" : \"my-string\", \"IntProperty\" : -392 } }";

                var currentPayload = "{ \"_t\" : \"OBeautifulCode.Serialization.Bson.Test.ObcBsonDiscriminatorConventionTest+ModelObjectForDiscriminatorConventionTest, OBeautifulCode.Serialization.Bson.Test\", \"AbstractClass\" : { \"_t\" : \"OBeautifulCode.Serialization.Bson.Test.ObcBsonDiscriminatorConventionTest+ConcreteClassForDiscriminatorConventionTest, OBeautifulCode.Serialization.Bson.Test\", \"StringProperty\" : \"my-string\", \"IntProperty\" : -392 } }";

                var serializer = new ObcBsonSerializer<TypesToRegisterBsonSerializationConfiguration<ModelObjectForDiscriminatorConventionTest>>();

                var expected = new ModelObjectForDiscriminatorConventionTest(new ConcreteClassForDiscriminatorConventionTest("my-string", -392));

                // Act
                var actual1 = serializer.Deserialize<ModelObjectForDiscriminatorConventionTest>(currentPayload);

                var actual2 = serializer.Deserialize<ModelObjectForDiscriminatorConventionTest>(legacyPayload);

                // Assert
                actual1.AsTest().Must().BeEqualTo(expected);
                actual2.AsTest().Must().BeEqualTo(expected);
            };

            test.ExecuteInNewAppDomain();
        }

        [Serializable]
        private class ModelObjectForDiscriminatorConventionTest : IEquatable<ModelObjectForDiscriminatorConventionTest>
        {
            public ModelObjectForDiscriminatorConventionTest(
                AbstractClassForDiscriminatorConventionTest abstractClass)
            {
                this.AbstractClass = abstractClass;
            }

            public AbstractClassForDiscriminatorConventionTest AbstractClass { get; private set; }

            public bool Equals(
                ModelObjectForDiscriminatorConventionTest other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                var thisConcreteClass = (ConcreteClassForDiscriminatorConventionTest)this.AbstractClass;
                var otherConcreteClass = (ConcreteClassForDiscriminatorConventionTest)other.AbstractClass;

                var result = (thisConcreteClass.IntProperty == otherConcreteClass.IntProperty) &&
                    (thisConcreteClass.StringProperty == otherConcreteClass.StringProperty);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelObjectForDiscriminatorConventionTest);

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        }

        [Serializable]
        private abstract class AbstractClassForDiscriminatorConventionTest
        {
            protected AbstractClassForDiscriminatorConventionTest(
                string stringProperty)
            {
                this.StringProperty = stringProperty;
            }

            public string StringProperty { get; private set; }
        }

        [Serializable]
        private class ConcreteClassForDiscriminatorConventionTest : AbstractClassForDiscriminatorConventionTest
        {
            public ConcreteClassForDiscriminatorConventionTest(
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
