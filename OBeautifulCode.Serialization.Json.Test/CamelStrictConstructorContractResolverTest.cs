// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CamelStrictConstructorContractResolverTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Json.Test.Internal;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class CamelStrictConstructorContractResolverTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModelWithOptionalConstructorParameters_and_include_all_properties_in_payload___When_model_constructed_with_all_non_default_values()
        {
            // Arrange
            var stringValue1 = "my-string-value-1";
            var stringValue2 = "my-string-value-2";
            var stringValue3 = "my-string-value-3";
            var intValue1 = -39;
            var intValue2 = 4;
            var nullableIntValue1 = 22;
            var nullableIntValue2 = 28;
            var nullableIntValue3 = 32;
            var enumValue1 = TestEnum.Second;
            var enumValue2 = TestEnum.Third;
            var nullableEnumValue1 = TestEnum.Second;
            var nullableEnumValue2 = TestEnum.Third;
            var nullableEnumValue3 = TestEnum.Fourth;
            var dictionaryValue = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
            };

            var expected = new TestModelWithOptionalConstructorParameters(stringValue1, stringValue2, stringValue3, intValue1, intValue2, nullableIntValue1, nullableIntValue2, nullableIntValue3, enumValue1, enumValue2, nullableEnumValue1, nullableEnumValue2, nullableEnumValue3, dictionaryValue);

            void VerificationCallback(string serialized, SerializationFormat format, TestModelWithOptionalConstructorParameters deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"stringValue1\": \"my-string-value-1\",\r\n  \"stringValue2\": \"my-string-value-2\",\r\n  \"stringValue3\": \"my-string-value-3\",\r\n  \"intValue1\": -39,\r\n  \"intValue2\": 4,\r\n  \"nullableIntValue1\": 22,\r\n  \"nullableIntValue2\": 28,\r\n  \"nullableIntValue3\": 32,\r\n  \"enumValue1\": \"second\",\r\n  \"enumValue2\": \"third\",\r\n  \"nullableEnumValue1\": \"second\",\r\n  \"nullableEnumValue2\": \"third\",\r\n  \"nullableEnumValue3\": \"fourth\",\r\n  \"dictionaryValue\": {\r\n    \"abc\": \"def\",\r\n    \"ghi\": \"jkl\"\r\n  }\r\n}");
                }
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback, typeof(TypesToRegisterJsonSerializationConfiguration<TestModelWithOptionalConstructorParameters>));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModelWithOptionalConstructorParameters_and_include_all_properties_in_payload___When_model_constructed_with_all_default_T_values()
        {
            // Arrange
            string stringValue1 = null;
            string stringValue2 = null;
            string stringValue3 = null;
            var intValue1 = 0;
            var intValue2 = 0;
            int? nullableIntValue1 = null;
            int? nullableIntValue2 = null;
            int? nullableIntValue3 = null;
            var enumValue1 = TestEnum.First;
            var enumValue2 = TestEnum.First;
            TestEnum? nullableEnumValue1 = null;
            TestEnum? nullableEnumValue2 = null;
            TestEnum? nullableEnumValue3 = null;
            Dictionary<string, string> dictionaryValue = null;

            var expected = new TestModelWithOptionalConstructorParameters(stringValue1, stringValue2, stringValue3, intValue1, intValue2, nullableIntValue1, nullableIntValue2, nullableIntValue3, enumValue1, enumValue2, nullableEnumValue1, nullableEnumValue2, nullableEnumValue3, dictionaryValue);

            void VerificationCallback(string serialized, SerializationFormat format, TestModelWithOptionalConstructorParameters deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"stringValue1\": null,\r\n  \"stringValue2\": null,\r\n  \"stringValue3\": null,\r\n  \"intValue1\": 0,\r\n  \"intValue2\": 0,\r\n  \"nullableIntValue1\": null,\r\n  \"nullableIntValue2\": null,\r\n  \"nullableIntValue3\": null,\r\n  \"enumValue1\": \"first\",\r\n  \"enumValue2\": \"first\",\r\n  \"nullableEnumValue1\": null,\r\n  \"nullableEnumValue2\": null,\r\n  \"nullableEnumValue3\": null,\r\n  \"dictionaryValue\": null\r\n}");
                }
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback, typeof(TypesToRegisterJsonSerializationConfiguration<TestModelWithOptionalConstructorParameters>));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModelWithOptionalConstructorParameters_and_include_all_properties_in_payload___When_model_constructed_with_all_default_T_values_of_nullable_underlying_type()
        {
            // Arrange
            string stringValue1 = null;
            string stringValue2 = null;
            string stringValue3 = null;
            var intValue1 = 0;
            var intValue2 = 0;
            var nullableIntValue1 = 0;
            var nullableIntValue2 = 0;
            var nullableIntValue3 = 0;
            var enumValue1 = TestEnum.First;
            var enumValue2 = TestEnum.First;
            TestEnum? nullableEnumValue1 = TestEnum.First;
            TestEnum? nullableEnumValue2 = TestEnum.First;
            TestEnum? nullableEnumValue3 = TestEnum.First;
            Dictionary<string, string> dictionaryValue = null;

            var expected = new TestModelWithOptionalConstructorParameters(stringValue1, stringValue2, stringValue3, intValue1, intValue2, nullableIntValue1, nullableIntValue2, nullableIntValue3, enumValue1, enumValue2, nullableEnumValue1, nullableEnumValue2, nullableEnumValue3, dictionaryValue);

            void VerificationCallback(string serialized, SerializationFormat format, TestModelWithOptionalConstructorParameters deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"stringValue1\": null,\r\n  \"stringValue2\": null,\r\n  \"stringValue3\": null,\r\n  \"intValue1\": 0,\r\n  \"intValue2\": 0,\r\n  \"nullableIntValue1\": 0,\r\n  \"nullableIntValue2\": 0,\r\n  \"nullableIntValue3\": 0,\r\n  \"enumValue1\": \"first\",\r\n  \"enumValue2\": \"first\",\r\n  \"nullableEnumValue1\": \"first\",\r\n  \"nullableEnumValue2\": \"first\",\r\n  \"nullableEnumValue3\": \"first\",\r\n  \"dictionaryValue\": null\r\n}");
                }
            }

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback, typeof(TypesToRegisterJsonSerializationConfiguration<TestModelWithOptionalConstructorParameters>));
        }

        [Fact(Skip = "See comment in CreatePropertyFromConstructorParameterWithConstructorInfo about the bug in Newtonsoft.Json")]
        public static void Deserialize___Should_roundtrip_serialized_TestModelWithOptionalConstructorParameters___When_payload_is_missing_properties_for_constructor_parameters_with_default_values()
        {
            // Arrange
            var payload = "{  \"stringValue1\": \"my-string-value\" }";

            var serializer = new ObcJsonSerializer(typeof(TypesToRegisterJsonSerializationConfiguration<TestModelWithOptionalConstructorParameters>).ToJsonSerializationConfigurationType());

            var expected = new TestModelWithOptionalConstructorParameters("my-string-value");

            // Act
            var actual = serializer.Deserialize<TestModelWithOptionalConstructorParameters>(payload);

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void Deserialize___Should_roundtrip_serialized_TestModelWithOptionalConstructorParameters___When_payload_is_missing_properties_for_constructor_parameters_with_default_values_for_parameters_not_assignable_to_null_and_parameters_assignable_to_null_that_are_defaulted_to_null()
        {
            // Arrange
            var payload1 = "{\r\n    \"stringValue1\": \"my-string-value-1\",\r\n    \"stringValue3\": \"my-string-value-3\",\r\n    \"nullableIntValue2\": 28,\r\n    \"nullableIntValue3\": 32,\r\n    \"nullableEnumValue2\": \"third\",\r\n    \"nullableEnumValue3\": \"fourth\"\r\n}";
            var payload2 = "{\r\n    \"stringValue1\": \"my-string-value-1\",\r\n    \"stringValue3\": null,\r\n    \"nullableIntValue2\": null,\r\n    \"nullableIntValue3\": null,\r\n    \"nullableEnumValue2\": null,\r\n    \"nullableEnumValue3\": null\r\n}";
            var payload3 = "{\r\n    \"stringValue1\": \"my-string-value-1\",\r\n    \"stringValue3\": null,\r\n    \"nullableIntValue2\": 0,\r\n    \"nullableIntValue3\": 0,\r\n    \"nullableEnumValue2\": \"first\",\r\n    \"nullableEnumValue3\": \"first\"\r\n}";

            var serializer = new ObcJsonSerializer(typeof(TypesToRegisterJsonSerializationConfiguration<TestModelWithOptionalConstructorParameters>).ToJsonSerializationConfigurationType());

            var expected1 = new TestModelWithOptionalConstructorParameters("my-string-value-1", stringValue3: "my-string-value-3", nullableIntValue2: 28, nullableIntValue3: 32, nullableEnumValue2: TestEnum.Third, nullableEnumValue3: TestEnum.Fourth);
            var expected2 = new TestModelWithOptionalConstructorParameters("my-string-value-1", stringValue3: null, nullableIntValue2: null, nullableIntValue3: null, nullableEnumValue2: null, nullableEnumValue3: null);
            var expected3 = new TestModelWithOptionalConstructorParameters("my-string-value-1", stringValue3: null, nullableIntValue2: 0, nullableIntValue3: 0, nullableEnumValue2: TestEnum.First, nullableEnumValue3: TestEnum.First);

            // Act
            var actual1 = serializer.Deserialize<TestModelWithOptionalConstructorParameters>(payload1);
            var actual2 = serializer.Deserialize<TestModelWithOptionalConstructorParameters>(payload2);
            var actual3 = serializer.Deserialize<TestModelWithOptionalConstructorParameters>(payload3);

            // Assert
            actual1.AsTest().Must().BeEqualTo(expected1);
            actual2.AsTest().Must().BeEqualTo(expected2);
            actual3.AsTest().Must().BeEqualTo(expected3);
        }
    }

    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
    public class TestModelWithOptionalConstructorParameters : IEquatable<TestModelWithOptionalConstructorParameters>
    {
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
        public TestModelWithOptionalConstructorParameters(
            string stringValue1,
            string stringValue2 = null,
            string stringValue3 = "abcd",
            int intValue1 = 0,
            int intValue2 = 99,
            int? nullableIntValue1 = null,
            int? nullableIntValue2 = 999,
            int? nullableIntValue3 = 0,
            TestEnum enumValue1 = TestEnum.First,
            TestEnum enumValue2 = TestEnum.Second,
            TestEnum? nullableEnumValue1 = null,
            TestEnum? nullableEnumValue2 = TestEnum.First,
            TestEnum? nullableEnumValue3 = TestEnum.Second,
            Dictionary<string, string> dictionaryValue = null)
        {
            this.StringValue1 = stringValue1;
            this.StringValue2 = stringValue2;
            this.StringValue3 = stringValue3;
            this.IntValue1 = intValue1;
            this.IntValue2 = intValue2;
            this.NullableIntValue1 = nullableIntValue1;
            this.NullableIntValue2 = nullableIntValue2;
            this.NullableIntValue3 = nullableIntValue3;
            this.EnumValue1 = enumValue1;
            this.EnumValue2 = enumValue2;
            this.NullableEnumValue1 = nullableEnumValue1;
            this.NullableEnumValue2 = nullableEnumValue2;
            this.NullableEnumValue3 = nullableEnumValue3;
            this.DictionaryValue = dictionaryValue;
        }

        public string StringValue1 { get; private set; }

        public string StringValue2 { get; private set; }

        public string StringValue3 { get; private set; }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
        public int IntValue1 { get; private set; }

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
        public int IntValue2 { get; private set; }

        public int? NullableIntValue1 { get; private set; }

        public int? NullableIntValue2 { get; private set; }

        public int? NullableIntValue3 { get; private set; }

        public TestEnum EnumValue1 { get; private set; }

        public TestEnum EnumValue2 { get; private set; }

        public TestEnum? NullableEnumValue1 { get; private set; }

        public TestEnum? NullableEnumValue2 { get; private set; }

        public TestEnum? NullableEnumValue3 { get; private set; }

        public Dictionary<string, string> DictionaryValue { get; private set; }

        public static bool operator ==(TestModelWithOptionalConstructorParameters left, TestModelWithOptionalConstructorParameters right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = left.Equals(right);

            return result;
        }

        public static bool operator !=(TestModelWithOptionalConstructorParameters left, TestModelWithOptionalConstructorParameters right) => !(left == right);

        public bool Equals(TestModelWithOptionalConstructorParameters other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result =
                this.StringValue1.IsEqualTo(other.StringValue1) &&
                this.StringValue2.IsEqualTo(other.StringValue2) &&
                this.StringValue3.IsEqualTo(other.StringValue3) &&
                this.IntValue1.IsEqualTo(other.IntValue1) &&
                this.IntValue2.IsEqualTo(other.IntValue2) &&
                this.NullableIntValue1.IsEqualTo(other.NullableIntValue1) &&
                this.NullableIntValue2.IsEqualTo(other.NullableIntValue2) &&
                this.NullableIntValue3.IsEqualTo(other.NullableIntValue3) &&
                this.EnumValue1.IsEqualTo(other.EnumValue1) &&
                this.EnumValue2.IsEqualTo(other.EnumValue2) &&
                this.NullableEnumValue1.IsEqualTo(other.NullableEnumValue1) &&
                this.NullableEnumValue2.IsEqualTo(other.NullableEnumValue2) &&
                this.NullableEnumValue3.IsEqualTo(other.NullableEnumValue3) &&
                this.DictionaryValue.IsEqualTo(other.DictionaryValue);

            return result;
        }

        public override bool Equals(object obj) => this == (obj as TestModelWithOptionalConstructorParameters);

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
        public override int GetHashCode() => throw new NotImplementedException("should not get used");
    }

    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = ObcSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    #pragma warning disable SA1201 // Elements should appear in the correct order
    public enum TestEnum
    #pragma warning restore SA1201 // Elements should appear in the correct order
    {
        First,

        Second,

        Third,

        Fourth,
    }
}
