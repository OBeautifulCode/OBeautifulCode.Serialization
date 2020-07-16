// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullableBsonSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Bson.Test.Internal;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class NullableBsonSerializerTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModelWithNullableTypes___When_property_values_are_all_null()
        {
            // Arrange
            var expected = new TestModelWithNullableTypes(null, null, null, null);

            // Act, Assert
            expected.RoundtripSerializeViaBsonUsingTypesToRegisterConfigWithBeEqualToAssertion();
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModelWithNullableTypes___When_property_values_are_all_not_null()
        {
            // Arrange
            var expected = new TestModelWithNullableTypes(A.Dummy<bool>(), A.Dummy<decimal>(), A.Dummy<DateTime>(), A.Dummy<Guid>());

            // Act, Assert
            expected.RoundtripSerializeViaBsonUsingTypesToRegisterConfigWithBeEqualToAssertion();
        }

        [Fact]
        public static void Deserialize___Should_deserialize_string_into_expected_nullable_type___When_called()
        {
            // Arrange
            // see comment in Deserialize about MongoDB.  We don't have a good way to create a situation where
            // the properties are written as strings (we'd have to wire-up Mongo), so instead of performing
            // a property roundtrip-serialization tests, we are just going to test the ChangeType method for some
            // observations from Mongo, to test that code path.
            var tests = new[]
            {
                new { Expected = (object)1.439382m, ExpectedType = typeof(decimal), Input = "1.439382" },
                new { Expected = (object)-1.439382m, ExpectedType = typeof(decimal), Input = "-1.439382" },
                new { Expected = (object)1392, ExpectedType = typeof(int), Input = "1392" },
                new { Expected = (object)-1392, ExpectedType = typeof(int), Input = "-1392" },
            };

            // Act, Assert
            foreach (var test in tests)
            {
                var actual = Convert.ChangeType(test.Input, test.ExpectedType, CultureInfo.InvariantCulture);

                actual.AsTest().Must().BeEqualTo(test.Expected);
            }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class TestModelWithNullableTypes : IEquatable<TestModelWithNullableTypes>
        {
            public TestModelWithNullableTypes(
                bool? boolValue,
                decimal? decimalValue,
                DateTime? dateTimeValue,
                Guid? guidValue)
            {
                this.BoolValue = boolValue;
                this.DecimalValue = decimalValue;
                this.DateTimeValue = dateTimeValue;
                this.GuidValue = guidValue;
            }

            public bool? BoolValue { get; private set; }

            public decimal? DecimalValue { get; private set; }

            public DateTime? DateTimeValue { get; private set; }

            public Guid? GuidValue { get; private set; }

            public static bool operator ==(TestModelWithNullableTypes left, TestModelWithNullableTypes right)
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

            public static bool operator !=(TestModelWithNullableTypes left, TestModelWithNullableTypes right) => !(left == right);

            public bool Equals(TestModelWithNullableTypes other)
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
                    (this.BoolValue == other.BoolValue) &&
                    (this.DecimalValue == other.DecimalValue) &&
                    (this.DateTimeValue == other.DateTimeValue) &&
                    (this.GuidValue == other.GuidValue);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as TestModelWithNullableTypes);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.BoolValue).Hash(this.DecimalValue).Hash(this.DateTimeValue).Hash(this.GuidValue).Value;
        }
    }
}