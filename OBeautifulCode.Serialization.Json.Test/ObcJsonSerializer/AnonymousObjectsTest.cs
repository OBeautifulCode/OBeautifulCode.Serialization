// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnonymousObjectsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;

    using OBeautifulCode.Assertion.Recipes;

    using Xunit;

    public static class AnonymousObjectsTest
    {
        [Fact]
        public static void Serialize___Should_serialize_anonymous_object_using_same_settings_as_non_anonymous_object___When_called()
        {
            // Arrange
            var serializer = new ObcJsonSerializer(typeof(AttemptOnUnregisteredTypeJsonSerializationConfiguration<TypesToRegisterJsonSerializationConfiguration<TestBaseModel>>).ToJsonSerializationConfigurationType());

            var model = new
            {
                AnonymousProperty = new TestBaseModel[]
                {
                    new TestDerivedModel
                    {
                        DateTimeProperty = new DateTime(2020, 4, 8, 13, 43, 52, 101, DateTimeKind.Utc),
                        TestEnum = TestEnum.Second,
                    },
                },
            };

            var expected = "{\r\n  \"anonymousProperty\": [\r\n    {\r\n      \"dateTimeProperty\": \"2020-04-08T13:43:52.1010000Z\",\r\n      \"testEnum\": \"second\",\r\n      \"$concreteType\": \"OBeautifulCode.Serialization.Json.Test.AnonymousObjectsTest+TestDerivedModel, OBeautifulCode.Serialization.Json.Test\"\r\n    }\r\n  ]\r\n}";

            // Act
            var actual = serializer.SerializeToString(model);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        #pragma warning disable SA1201 // Elements should appear in the correct order
        private enum TestEnum
        {
            First,

            Second,

            Third,
        }

        private abstract class TestBaseModel
        {
        }

        private class TestDerivedModel : TestBaseModel
        {
            public DateTime DateTimeProperty { get; set; }

            public TestEnum TestEnum { get; set; }
        }
        #pragma warning restore SA1201 // Elements should appear in the correct order
    }
}
