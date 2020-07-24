// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryJsonConverterBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Json.Test.Internal;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    using static System.FormattableString;

    public static class DictionaryJsonConverterBaseTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModel___When_dictionary_properties_are_not_null_nor_contain_any_null_values()
        {
            // Arrange
            var expected = A.Dummy<TestModel>();

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(NoneConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(DictionaryKeyConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModel___When_using_ObjectAndDictionaryKeyConvertingSerializationConfiguration_and_dictionary_properties_are_all_null()
        {
            // Arrange
            var expected = new TestModel(null, null, null, null, null, null, null, null, null, null, null, null, null);

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(NoneConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(DictionaryKeyConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModel___When_using_ObjectAndDictionaryKeyConvertingSerializationConfiguration_and_dictionary_properties_are_empty()
        {
            // Arrange
            var enumToOtherModelMap = new Dictionary<Position, ModelThatCannotBeConvertToString>();
            var nullableEnumToModelMap = new Dictionary<Position?, ModelThatCanBeConvertedToString>();
            var stringToIntMap = new Dictionary<string, int>();
            var dateTimeToNullableDateTimeMap = new Dictionary<DateTime, DateTime?>();
            var nullableDateTimeToDateTimeMap = new Dictionary<DateTime?, DateTime>();
            var intToStringMap = new Dictionary<int, string>();
            var modelToNullableEnumMap = new Dictionary<ModelThatCanBeConvertedToString, Position?>();
            var otherModelToEnumMap = new Dictionary<ModelThatCannotBeConvertToString, Position>();
            var modelToModelMap = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString>();
            var otherModelToOtherModelMap = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCannotBeConvertToString>();
            var modelToOtherModelMap = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>();
            var otherModelToModelMap = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCanBeConvertedToString>();
            var modelToModelToOtherModelMap = new Dictionary<ModelThatCanBeConvertedToString, IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>();

            var expected = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(NoneConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(DictionaryKeyConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModel___When_using_ObjectAndDictionaryKeyConvertingSerializationConfiguration_and_dictionary_properties_contain_null_values()
        {
            // Arrange
            var enumToOtherModelMap1 = new Dictionary<Position, ModelThatCannotBeConvertToString>
            {
                { Position.First, null },
                { Position.Second, A.Dummy<ModelThatCannotBeConvertToString>() },
                { Position.Third, A.Dummy<ModelThatCannotBeConvertToString>() },
            };

            var enumToOtherModelMap2 = new Dictionary<Position, ModelThatCannotBeConvertToString>
            {
                { Position.First, A.Dummy<ModelThatCannotBeConvertToString>() },
                { Position.Second, null },
                { Position.Third, A.Dummy<ModelThatCannotBeConvertToString>() },
            };

            var enumToOtherModelMap3 = new Dictionary<Position, ModelThatCannotBeConvertToString>
            {
                { Position.First, A.Dummy<ModelThatCannotBeConvertToString>() },
                { Position.Second, A.Dummy<ModelThatCannotBeConvertToString>() },
                { Position.Third, null },
            };

            var nullableEnumToModelMap1 = new Dictionary<Position?, ModelThatCanBeConvertedToString>
            {
                { Position.First, null },
                { Position.Second, A.Dummy<ModelThatCanBeConvertedToString>() },
                { Position.Third, A.Dummy<ModelThatCanBeConvertedToString>() },
            };

            var nullableEnumToModelMap2 = new Dictionary<Position?, ModelThatCanBeConvertedToString>
            {
                { Position.First, A.Dummy<ModelThatCanBeConvertedToString>() },
                { Position.Second, null },
                { Position.Third, A.Dummy<ModelThatCanBeConvertedToString>() },
            };

            var nullableEnumToModelMap3 = new Dictionary<Position?, ModelThatCanBeConvertedToString>
            {
                { Position.First, A.Dummy<ModelThatCanBeConvertedToString>() },
                { Position.Second, A.Dummy<ModelThatCanBeConvertedToString>() },
                { Position.Third, null },
            };

            var stringToIntMap = new Dictionary<string, int>
            {
                { A.Dummy<string>(), A.Dummy<int>() },
            };

            var dateTimeToNullableDateTimeMap1 = new Dictionary<DateTime, DateTime?>
            {
                { A.Dummy<DateTime>(), null },
                { A.Dummy<DateTime>(), A.Dummy<DateTime?>() },
                { A.Dummy<DateTime>(), A.Dummy<DateTime?>() },
            };

            var dateTimeToNullableDateTimeMap2 = new Dictionary<DateTime, DateTime?>
            {
                { A.Dummy<DateTime>(), A.Dummy<DateTime?>() },
                { A.Dummy<DateTime>(), null },
                { A.Dummy<DateTime>(), A.Dummy<DateTime?>() },
            };

            var dateTimeToNullableDateTimeMap3 = new Dictionary<DateTime, DateTime?>
            {
                { A.Dummy<DateTime>(), A.Dummy<DateTime?>() },
                { A.Dummy<DateTime>(), A.Dummy<DateTime?>() },
                { A.Dummy<DateTime>(), null },
            };

            var nullableDateTimeToDateTimeMap = new Dictionary<DateTime?, DateTime>
            {
                { A.Dummy<DateTime>(), A.Dummy<DateTime>() },
            };

            var intToStringMap1 = new Dictionary<int, string>
            {
                { A.Dummy<int>(), null },
                { A.Dummy<int>(), A.Dummy<string>() },
                { A.Dummy<int>(), A.Dummy<string>() },
            };

            var intToStringMap2 = new Dictionary<int, string>
            {
                { A.Dummy<int>(), A.Dummy<string>() },
                { A.Dummy<int>(), null },
                { A.Dummy<int>(), A.Dummy<string>() },
            };

            var intToStringMap3 = new Dictionary<int, string>
            {
                { A.Dummy<int>(), A.Dummy<string>() },
                { A.Dummy<int>(), A.Dummy<string>() },
                { A.Dummy<int>(), null },
            };

            var modelToNullableEnumMap1 = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position>() },
            };

            var modelToNullableEnumMap2 = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position>() },
            };

            var modelToNullableEnumMap3 = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
            };

            var otherModelToEnumMap = new Dictionary<ModelThatCannotBeConvertToString, Position>
            {
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<Position>() },
            };

            var modelToModelMap1 = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
            };

            var modelToModelMap2 = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
            };

            var modelToModelMap3 = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
            };

            var otherModelToOtherModelMap1 = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCannotBeConvertToString>
            {
                { A.Dummy<ModelThatCannotBeConvertToString>(), null },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
            };

            var otherModelToOtherModelMap2 = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCannotBeConvertToString>
            {
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), null },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
            };

            var otherModelToOtherModelMap3 = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCannotBeConvertToString>
            {
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), null },
            };

            var modelToOtherModelMap1 = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
            };

            var modelToOtherModelMap2 = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
            };

            var modelToOtherModelMap3 = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<ModelThatCannotBeConvertToString>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
            };

            var otherModelToModelMap1 = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCanBeConvertedToString>
            {
                { A.Dummy<ModelThatCannotBeConvertToString>(), null },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
            };

            var otherModelToModelMap2 = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCanBeConvertedToString>
            {
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), null },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
            };

            var otherModelToModelMap3 = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCanBeConvertedToString>
            {
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), A.Dummy<ModelThatCanBeConvertedToString>() },
                { A.Dummy<ModelThatCannotBeConvertToString>(), null },
            };

            var modelToModelToOtherModelMap1 = new Dictionary<ModelThatCanBeConvertedToString, IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>() },
            };

            var modelToModelToOtherModelMap2 = new Dictionary<ModelThatCanBeConvertedToString, IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>() },
            };

            var modelToModelToOtherModelMap3 = new Dictionary<ModelThatCanBeConvertedToString, IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>() },
                { A.Dummy<ModelThatCanBeConvertedToString>(), null },
            };

            var expected1 = new TestModel(enumToOtherModelMap1, nullableEnumToModelMap1, stringToIntMap, dateTimeToNullableDateTimeMap1, nullableDateTimeToDateTimeMap, intToStringMap1, modelToNullableEnumMap1, otherModelToEnumMap, modelToModelMap1, otherModelToOtherModelMap1, modelToOtherModelMap1, otherModelToModelMap1, modelToModelToOtherModelMap1);
            var expected2 = new TestModel(enumToOtherModelMap2, nullableEnumToModelMap2, stringToIntMap, dateTimeToNullableDateTimeMap2, nullableDateTimeToDateTimeMap, intToStringMap2, modelToNullableEnumMap2, otherModelToEnumMap, modelToModelMap2, otherModelToOtherModelMap2, modelToOtherModelMap2, otherModelToModelMap2, modelToModelToOtherModelMap2);
            var expected3 = new TestModel(enumToOtherModelMap3, nullableEnumToModelMap3, stringToIntMap, dateTimeToNullableDateTimeMap3, nullableDateTimeToDateTimeMap, intToStringMap3, modelToNullableEnumMap3, otherModelToEnumMap, modelToModelMap3, otherModelToOtherModelMap3, modelToOtherModelMap3, otherModelToModelMap3, modelToModelToOtherModelMap3);

            // Act & Assert
            expected1.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(NoneConvertingSerializationConfiguration));
            expected1.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectConvertingSerializationConfiguration));
            expected1.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(DictionaryKeyConvertingSerializationConfiguration));
            expected1.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration));

            expected2.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(NoneConvertingSerializationConfiguration));
            expected2.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectConvertingSerializationConfiguration));
            expected2.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(DictionaryKeyConvertingSerializationConfiguration));
            expected2.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration));

            expected3.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(NoneConvertingSerializationConfiguration));
            expected3.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectConvertingSerializationConfiguration));
            expected3.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(DictionaryKeyConvertingSerializationConfiguration));
            expected3.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_throw_InvalidOperationException___When_dictionary_contains_keys_that_convert_to_null_or_empty_string()
        {
            // Arrange, Act
            var enumToOtherModelMap = A.Dummy<TestModel>().EnumToOtherModelMap;

            var nullableEnumToModelMap = A.Dummy<TestModel>().NullableEnumToModelMap;

            var stringToIntMap = A.Dummy<TestModel>().StringToIntMap;

            var stringToIntMap1 = new Dictionary<string, int>
            {
                { string.Empty, A.Dummy<int>() },
                { A.Dummy<string>(), A.Dummy<int>() },
            };

            var stringToIntMap2 = new Dictionary<string, int>
            {
                { A.Dummy<string>(), A.Dummy<int>() },
                { string.Empty, A.Dummy<int>() },
            };

            var dateTimeToNullableDateTimeMap = A.Dummy<TestModel>().DateTimeToNullableDateTimeMap;

            var nullableDateTimeToDateTimeMap = A.Dummy<TestModel>().NullableDateTimeToDateTimeMap;

            var intToStringMap = A.Dummy<TestModel>().IntToStringMap;

            var modelToNullableEnumMap = A.Dummy<TestModel>().ModelToNullableEnumMap;

            var modelToNullableEnumMap1 = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { new ModelThatCanBeConvertedToString(int.MaxValue, int.MaxValue), A.Dummy<Position?>() }, // key serializes to null
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position?>() },
            };

            var modelToNullableEnumMap2 = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position?>() },
                { new ModelThatCanBeConvertedToString(int.MaxValue, int.MaxValue), A.Dummy<Position?>() }, // key serializes to null
            };

            var modelToNullableEnumMap3 = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { new ModelThatCanBeConvertedToString(int.MinValue, int.MinValue), A.Dummy<Position?>() }, // key serializes to empty string
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position?>() },
            };

            var modelToNullableEnumMap4 = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { A.Dummy<ModelThatCanBeConvertedToString>(), A.Dummy<Position?>() },
                { new ModelThatCanBeConvertedToString(int.MinValue, int.MinValue), A.Dummy<Position?>() }, // key serializes to empty string
            };

            var otherModelToEnumMap = A.Dummy<TestModel>().OtherModelToEnumMap;

            var modelToModelMap = A.Dummy<TestModel>().ModelToModelMap;

            var otherModelToOtherModelMap = A.Dummy<TestModel>().OtherModelToOtherModelMap;

            var modelToOtherModelMap = A.Dummy<TestModel>().ModelToOtherModelMap;

            var otherModelToModelMap = A.Dummy<TestModel>().OtherModelToModelMap;

            var modelToModelToOtherModelMap = A.Dummy<TestModel>().ModelToModelToOtherModelMap;

            var expected1 = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap1, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);
            var expected2 = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap2, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);

            var expecteds = new[] { expected1, expected2 };

            var configTypes = new[]
            {
                typeof(NoneConvertingSerializationConfiguration),
                typeof(ObjectConvertingSerializationConfiguration),
                typeof(DictionaryKeyConvertingSerializationConfiguration),
                typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration),
            };

            var actuals = new List<Exception>();

            foreach (var configType in configTypes)
            {
                foreach (var expected in expecteds)
                {
                    actuals.Add(Record.Exception(() => expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(configType)));
                }
            }

            var expected3 = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap1, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);
            var expected4 = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap2, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);
            var expected5 = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap3, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);
            var expected6 = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap4, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);

            expecteds = new[] { expected3, expected4, expected5, expected6 };

            configTypes = new[]
            {
                typeof(DictionaryKeyConvertingSupportingNullOrEmptySerializationConfiguration),
                typeof(ObjectAndDictionaryKeyConvertingSupportingNullOrEmptySerializationConfiguration),
            };

            foreach (var configType in configTypes)
            {
                foreach (var expected in expecteds)
                {
                    actuals.Add(Record.Exception(() => expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(configType)));
                }
            }

            // Assert
            foreach (var actual in actuals)
            {
                actual.AsTest().Must().BeOfType<InvalidOperationException>();
                actual.Message.AsTest().Must().StartWith("Key in dictionary serializes to a null or white space string.  Key type is");
            }
        }

        [Fact]
        public static void Serialize___Should_write_dictionary_entries_as_properties_on_json_object____When_using_ObjectAndDictionaryKeyConvertingSerializationConfiguration_and_serializing_TestModel()
        {
            // Arrange
            var enumToOtherModelMap = new Dictionary<Position, ModelThatCannotBeConvertToString>
            {
                { Position.First, null },
                { Position.Second, new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-100000000000")) },
            };

            var nullableEnumToModelMap = new Dictionary<Position?, ModelThatCanBeConvertedToString>
            {
                { Position.Third, new ModelThatCanBeConvertedToString(1, 2) },
                { Position.Fourth, null },
                { Position.Fifth, new ModelThatCanBeConvertedToString(3, 4) },
            };

            var stringToIntMap = new Dictionary<string, int>
            {
                { "my-string-1", -90 },
            };

            var dateTimeToNullableDateTimeMap = new Dictionary<DateTime, DateTime?>
            {
                { new DateTime(2020, 6, 23, 4, 5, 6, DateTimeKind.Utc), null },
                { new DateTime(2020, 6, 23, 4, 5, 7, DateTimeKind.Utc), new DateTime(2020, 6, 23, 4, 5, 8, DateTimeKind.Utc) },
            };

            var nullableDateTimeToDateTimeMap = new Dictionary<DateTime?, DateTime>
            {
                { new DateTime(2020, 6, 23, 4, 5, 9, DateTimeKind.Utc), new DateTime(2020, 6, 23, 4, 5, 10, DateTimeKind.Utc) },
            };

            var intToStringMap = new Dictionary<int, string>
            {
                { -89, null },
                { -88, string.Empty },
                { -87, "my-string-2" },
            };

            var modelToNullableEnumMap = new Dictionary<ModelThatCanBeConvertedToString, Position?>
            {
                { new ModelThatCanBeConvertedToString(5, 6), null },
                { new ModelThatCanBeConvertedToString(7, 8), Position.Sixth },
            };

            var otherModelToEnumMap = new Dictionary<ModelThatCannotBeConvertToString, Position>
            {
                { new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-200000000000")), Position.Seventh },
                { new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-300000000000")), Position.Eighth },
            };

            var modelToModelMap = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString>
            {
                { new ModelThatCanBeConvertedToString(9, 10), null },
                { new ModelThatCanBeConvertedToString(11, 12), new ModelThatCanBeConvertedToString(13, 14) },
            };

            var otherModelToOtherModelMap = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCannotBeConvertToString>
            {
                { new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-400000000000")), new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-500000000000")) },
                { new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-600000000000")), null },
            };

            var modelToOtherModelMap = new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>
            {
                { new ModelThatCanBeConvertedToString(15, 16), null },
                { new ModelThatCanBeConvertedToString(17, 18), new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-700000000000")) },
            };

            var otherModelToModelMap = new Dictionary<ModelThatCannotBeConvertToString, ModelThatCanBeConvertedToString>
            {
                { new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-800000000000")), null },
                { new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0000-900000000000")), new ModelThatCanBeConvertedToString(19, 20) },
            };

            var modelToModelToOtherModelMap = new Dictionary<ModelThatCanBeConvertedToString, IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>>
            {
                { new ModelThatCanBeConvertedToString(21, 22), null },
                { new ModelThatCanBeConvertedToString(23, 24), new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>() },
                { new ModelThatCanBeConvertedToString(25, 26), new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>() { { new ModelThatCanBeConvertedToString(29, 30), null } } },
                { new ModelThatCanBeConvertedToString(27, 28), new Dictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>() { { new ModelThatCanBeConvertedToString(31, 32), new ModelThatCannotBeConvertToString(Guid.Parse("00000000-0000-0000-0001-000000000000")) } } },
            };

            var expected = new TestModel(enumToOtherModelMap, nullableEnumToModelMap, stringToIntMap, dateTimeToNullableDateTimeMap, nullableDateTimeToDateTimeMap, intToStringMap, modelToNullableEnumMap, otherModelToEnumMap, modelToModelMap, otherModelToOtherModelMap, modelToOtherModelMap, otherModelToModelMap, modelToModelToOtherModelMap);

            void VerificationCallback1(string serialized, SerializationFormat format, TestModel deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"enumToOtherModelMap\": {\r\n    \"first\": null,\r\n    \"second\": {\r\n      \"id\": \"00000000-0000-0000-0000-100000000000\"\r\n    }\r\n  },\r\n  \"nullableEnumToModelMap\": {\r\n    \"third\": {\r\n      \"value\": 1,\r\n      \"value2\": 2\r\n    },\r\n    \"fourth\": null,\r\n    \"fifth\": {\r\n      \"value\": 3,\r\n      \"value2\": 4\r\n    }\r\n  },\r\n  \"stringToIntMap\": {\r\n    \"my-string-1\": -90\r\n  },\r\n  \"dateTimeToNullableDateTimeMap\": {\r\n    \"2020-06-23T04:05:06.0000000Z\": null,\r\n    \"2020-06-23T04:05:07.0000000Z\": \"2020-06-23T04:05:08.0000000Z\"\r\n  },\r\n  \"nullableDateTimeToDateTimeMap\": {\r\n    \"2020-06-23T04:05:09.0000000Z\": \"2020-06-23T04:05:10.0000000Z\"\r\n  },\r\n  \"intToStringMap\": {\r\n    \"-89\": null,\r\n    \"-88\": \"\",\r\n    \"-87\": \"my-string-2\"\r\n  },\r\n  \"modelToNullableEnumMap\": [\r\n    {\r\n      \"key\": {\r\n        \"value\": 5,\r\n        \"value2\": 6\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"value\": 7,\r\n        \"value2\": 8\r\n      },\r\n      \"value\": \"sixth\"\r\n    }\r\n  ],\r\n  \"otherModelToEnumMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-200000000000\"\r\n      },\r\n      \"value\": \"seventh\"\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-300000000000\"\r\n      },\r\n      \"value\": \"eighth\"\r\n    }\r\n  ],\r\n  \"modelToModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"value\": 9,\r\n        \"value2\": 10\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"value\": 11,\r\n        \"value2\": 12\r\n      },\r\n      \"value\": {\r\n        \"value\": 13,\r\n        \"value2\": 14\r\n      }\r\n    }\r\n  ],\r\n  \"otherModelToOtherModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-400000000000\"\r\n      },\r\n      \"value\": {\r\n        \"id\": \"00000000-0000-0000-0000-500000000000\"\r\n      }\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-600000000000\"\r\n      },\r\n      \"value\": null\r\n    }\r\n  ],\r\n  \"modelToOtherModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"value\": 15,\r\n        \"value2\": 16\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"value\": 17,\r\n        \"value2\": 18\r\n      },\r\n      \"value\": {\r\n        \"id\": \"00000000-0000-0000-0000-700000000000\"\r\n      }\r\n    }\r\n  ],\r\n  \"otherModelToModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-800000000000\"\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-900000000000\"\r\n      },\r\n      \"value\": {\r\n        \"value\": 19,\r\n        \"value2\": 20\r\n      }\r\n    }\r\n  ],\r\n  \"modelToModelToOtherModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"value\": 21,\r\n        \"value2\": 22\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"value\": 23,\r\n        \"value2\": 24\r\n      },\r\n      \"value\": []\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"value\": 25,\r\n        \"value2\": 26\r\n      },\r\n      \"value\": [\r\n        {\r\n          \"key\": {\r\n            \"value\": 29,\r\n            \"value2\": 30\r\n          },\r\n          \"value\": null\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"value\": 27,\r\n        \"value2\": 28\r\n      },\r\n      \"value\": [\r\n        {\r\n          \"key\": {\r\n            \"value\": 31,\r\n            \"value2\": 32\r\n          },\r\n          \"value\": {\r\n            \"id\": \"00000000-0000-0000-0001-000000000000\"\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}");
                }
            }

            void VerificationCallback2(string serialized, SerializationFormat format, TestModel deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"enumToOtherModelMap\": {\r\n    \"first\": null,\r\n    \"second\": {\r\n      \"id\": \"00000000-0000-0000-0000-100000000000\"\r\n    }\r\n  },\r\n  \"nullableEnumToModelMap\": {\r\n    \"third\": \"1,2\",\r\n    \"fourth\": null,\r\n    \"fifth\": \"3,4\"\r\n  },\r\n  \"stringToIntMap\": {\r\n    \"my-string-1\": -90\r\n  },\r\n  \"dateTimeToNullableDateTimeMap\": {\r\n    \"2020-06-23T04:05:06.0000000Z\": null,\r\n    \"2020-06-23T04:05:07.0000000Z\": \"2020-06-23T04:05:08.0000000Z\"\r\n  },\r\n  \"nullableDateTimeToDateTimeMap\": {\r\n    \"2020-06-23T04:05:09.0000000Z\": \"2020-06-23T04:05:10.0000000Z\"\r\n  },\r\n  \"intToStringMap\": {\r\n    \"-89\": null,\r\n    \"-88\": \"\",\r\n    \"-87\": \"my-string-2\"\r\n  },\r\n  \"modelToNullableEnumMap\": [\r\n    {\r\n      \"key\": \"5,6\",\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": \"7,8\",\r\n      \"value\": \"sixth\"\r\n    }\r\n  ],\r\n  \"otherModelToEnumMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-200000000000\"\r\n      },\r\n      \"value\": \"seventh\"\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-300000000000\"\r\n      },\r\n      \"value\": \"eighth\"\r\n    }\r\n  ],\r\n  \"modelToModelMap\": [\r\n    {\r\n      \"key\": \"9,10\",\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": \"11,12\",\r\n      \"value\": \"13,14\"\r\n    }\r\n  ],\r\n  \"otherModelToOtherModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-400000000000\"\r\n      },\r\n      \"value\": {\r\n        \"id\": \"00000000-0000-0000-0000-500000000000\"\r\n      }\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-600000000000\"\r\n      },\r\n      \"value\": null\r\n    }\r\n  ],\r\n  \"modelToOtherModelMap\": [\r\n    {\r\n      \"key\": \"15,16\",\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": \"17,18\",\r\n      \"value\": {\r\n        \"id\": \"00000000-0000-0000-0000-700000000000\"\r\n      }\r\n    }\r\n  ],\r\n  \"otherModelToModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-800000000000\"\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-900000000000\"\r\n      },\r\n      \"value\": \"19,20\"\r\n    }\r\n  ],\r\n  \"modelToModelToOtherModelMap\": [\r\n    {\r\n      \"key\": \"21,22\",\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": \"23,24\",\r\n      \"value\": []\r\n    },\r\n    {\r\n      \"key\": \"25,26\",\r\n      \"value\": [\r\n        {\r\n          \"key\": \"29,30\",\r\n          \"value\": null\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"key\": \"27,28\",\r\n      \"value\": [\r\n        {\r\n          \"key\": \"31,32\",\r\n          \"value\": {\r\n            \"id\": \"00000000-0000-0000-0001-000000000000\"\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}");
                }
            }

            void VerificationCallback3(string serialized, SerializationFormat format, TestModel deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"enumToOtherModelMap\": {\r\n    \"first\": null,\r\n    \"second\": {\r\n      \"id\": \"00000000-0000-0000-0000-100000000000\"\r\n    }\r\n  },\r\n  \"nullableEnumToModelMap\": {\r\n    \"third\": {\r\n      \"value\": 1,\r\n      \"value2\": 2\r\n    },\r\n    \"fourth\": null,\r\n    \"fifth\": {\r\n      \"value\": 3,\r\n      \"value2\": 4\r\n    }\r\n  },\r\n  \"stringToIntMap\": {\r\n    \"my-string-1\": -90\r\n  },\r\n  \"dateTimeToNullableDateTimeMap\": {\r\n    \"2020-06-23T04:05:06.0000000Z\": null,\r\n    \"2020-06-23T04:05:07.0000000Z\": \"2020-06-23T04:05:08.0000000Z\"\r\n  },\r\n  \"nullableDateTimeToDateTimeMap\": {\r\n    \"2020-06-23T04:05:09.0000000Z\": \"2020-06-23T04:05:10.0000000Z\"\r\n  },\r\n  \"intToStringMap\": {\r\n    \"-89\": null,\r\n    \"-88\": \"\",\r\n    \"-87\": \"my-string-2\"\r\n  },\r\n  \"modelToNullableEnumMap\": {\r\n    \"5,6\": null,\r\n    \"7,8\": \"sixth\"\r\n  },\r\n  \"otherModelToEnumMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-200000000000\"\r\n      },\r\n      \"value\": \"seventh\"\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-300000000000\"\r\n      },\r\n      \"value\": \"eighth\"\r\n    }\r\n  ],\r\n  \"modelToModelMap\": {\r\n    \"9,10\": null,\r\n    \"11,12\": {\r\n      \"value\": 13,\r\n      \"value2\": 14\r\n    }\r\n  },\r\n  \"otherModelToOtherModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-400000000000\"\r\n      },\r\n      \"value\": {\r\n        \"id\": \"00000000-0000-0000-0000-500000000000\"\r\n      }\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-600000000000\"\r\n      },\r\n      \"value\": null\r\n    }\r\n  ],\r\n  \"modelToOtherModelMap\": {\r\n    \"15,16\": null,\r\n    \"17,18\": {\r\n      \"id\": \"00000000-0000-0000-0000-700000000000\"\r\n    }\r\n  },\r\n  \"otherModelToModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-800000000000\"\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-900000000000\"\r\n      },\r\n      \"value\": {\r\n        \"value\": 19,\r\n        \"value2\": 20\r\n      }\r\n    }\r\n  ],\r\n  \"modelToModelToOtherModelMap\": {\r\n    \"21,22\": null,\r\n    \"23,24\": {},\r\n    \"25,26\": {\r\n      \"29,30\": null\r\n    },\r\n    \"27,28\": {\r\n      \"31,32\": {\r\n        \"id\": \"00000000-0000-0000-0001-000000000000\"\r\n      }\r\n    }\r\n  }\r\n}");
                }
            }

            void VerificationCallback4(string serialized, SerializationFormat format, TestModel deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"enumToOtherModelMap\": {\r\n    \"first\": null,\r\n    \"second\": {\r\n      \"id\": \"00000000-0000-0000-0000-100000000000\"\r\n    }\r\n  },\r\n  \"nullableEnumToModelMap\": {\r\n    \"third\": \"1,2\",\r\n    \"fourth\": null,\r\n    \"fifth\": \"3,4\"\r\n  },\r\n  \"stringToIntMap\": {\r\n    \"my-string-1\": -90\r\n  },\r\n  \"dateTimeToNullableDateTimeMap\": {\r\n    \"2020-06-23T04:05:06.0000000Z\": null,\r\n    \"2020-06-23T04:05:07.0000000Z\": \"2020-06-23T04:05:08.0000000Z\"\r\n  },\r\n  \"nullableDateTimeToDateTimeMap\": {\r\n    \"2020-06-23T04:05:09.0000000Z\": \"2020-06-23T04:05:10.0000000Z\"\r\n  },\r\n  \"intToStringMap\": {\r\n    \"-89\": null,\r\n    \"-88\": \"\",\r\n    \"-87\": \"my-string-2\"\r\n  },\r\n  \"modelToNullableEnumMap\": {\r\n    \"5,6\": null,\r\n    \"7,8\": \"sixth\"\r\n  },\r\n  \"otherModelToEnumMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-200000000000\"\r\n      },\r\n      \"value\": \"seventh\"\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-300000000000\"\r\n      },\r\n      \"value\": \"eighth\"\r\n    }\r\n  ],\r\n  \"modelToModelMap\": {\r\n    \"9,10\": null,\r\n    \"11,12\": \"13,14\"\r\n  },\r\n  \"otherModelToOtherModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-400000000000\"\r\n      },\r\n      \"value\": {\r\n        \"id\": \"00000000-0000-0000-0000-500000000000\"\r\n      }\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-600000000000\"\r\n      },\r\n      \"value\": null\r\n    }\r\n  ],\r\n  \"modelToOtherModelMap\": {\r\n    \"15,16\": null,\r\n    \"17,18\": {\r\n      \"id\": \"00000000-0000-0000-0000-700000000000\"\r\n    }\r\n  },\r\n  \"otherModelToModelMap\": [\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-800000000000\"\r\n      },\r\n      \"value\": null\r\n    },\r\n    {\r\n      \"key\": {\r\n        \"id\": \"00000000-0000-0000-0000-900000000000\"\r\n      },\r\n      \"value\": \"19,20\"\r\n    }\r\n  ],\r\n  \"modelToModelToOtherModelMap\": {\r\n    \"21,22\": null,\r\n    \"23,24\": {},\r\n    \"25,26\": {\r\n      \"29,30\": null\r\n    },\r\n    \"27,28\": {\r\n      \"31,32\": {\r\n        \"id\": \"00000000-0000-0000-0001-000000000000\"\r\n      }\r\n    }\r\n  }\r\n}");
                }
            }

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback1, typeof(NoneConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback2, typeof(ObjectConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback3, typeof(DictionaryKeyConvertingSerializationConfiguration));
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback4, typeof(ObjectAndDictionaryKeyConvertingSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_TestModelWithMixedCaseKeys_and_serialize_without_changing_case_of_keys___When_model_contains_dictionaries_keyed_on_string_or_convertible_to_string_and_keys_have_mixed_case()
        {
            var stringToIntMap = new Dictionary<string, int>
            {
                { "abc", 1 },
                { "Abc", 2 },
                { "AbC", 3 },
            };

            var otherModelToIntMap = new Dictionary<OtherModelThatCanBeConvertedToString, int>
            {
                { new OtherModelThatCanBeConvertedToString("abc"), 1 },
                { new OtherModelThatCanBeConvertedToString("Abc"), 2 },
                { new OtherModelThatCanBeConvertedToString("AbC"), 3 },
            };

            var expected = new TestModelWithMixedCaseKeys(stringToIntMap, otherModelToIntMap);

            void VerificationCallback(string serialized, SerializationFormat format, TestModelWithMixedCaseKeys deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"stringToIntMap\": {\r\n    \"abc\": 1,\r\n    \"Abc\": 2,\r\n    \"AbC\": 3\r\n  },\r\n  \"otherModelToIntMap\": {\r\n    \"abc\": 1,\r\n    \"Abc\": 2,\r\n    \"AbC\": 3\r\n  }\r\n}");
                }
            }

            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback, typeof(MixedCaseKeysSerializationConfiguration));
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        #pragma warning disable SA1201 // Elements should appear in the correct order
        public enum Position
        {
            First,

            Second,

            Third,

            Fourth,

            Fifth,

            Sixth,

            Seventh,

            Eighth,

            Ninth,

            Tenth,
        }
        #pragma warning restore SA1201 // Elements should appear in the correct order

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class TestModel : IEquatable<TestModel>
        {
            public TestModel(
                IReadOnlyDictionary<Position, ModelThatCannotBeConvertToString> enumToOtherModelMap,
                IReadOnlyDictionary<Position?, ModelThatCanBeConvertedToString> nullableEnumToModelMap,
                IReadOnlyDictionary<string, int> stringToIntMap,
                IReadOnlyDictionary<DateTime, DateTime?> dateTimeToNullableDateTimeMap,
                IReadOnlyDictionary<DateTime?, DateTime> nullableDateTimeToDateTimeMap,
                IReadOnlyDictionary<int, string> intToStringMap,
                IReadOnlyDictionary<ModelThatCanBeConvertedToString, Position?> modelToNullableEnumMap,
                IReadOnlyDictionary<ModelThatCannotBeConvertToString, Position> otherModelToEnumMap,
                IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString> modelToModelMap,
                IReadOnlyDictionary<ModelThatCannotBeConvertToString, ModelThatCannotBeConvertToString> otherModelToOtherModelMap,
                IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString> modelToOtherModelMap,
                IReadOnlyDictionary<ModelThatCannotBeConvertToString, ModelThatCanBeConvertedToString> otherModelToModelMap,
                IReadOnlyDictionary<ModelThatCanBeConvertedToString, IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>> modelToModelToOtherModelMap)
            {
                this.EnumToOtherModelMap = enumToOtherModelMap;
                this.NullableEnumToModelMap = nullableEnumToModelMap;
                this.StringToIntMap = stringToIntMap;
                this.DateTimeToNullableDateTimeMap = dateTimeToNullableDateTimeMap;
                this.NullableDateTimeToDateTimeMap = nullableDateTimeToDateTimeMap;
                this.IntToStringMap = intToStringMap;
                this.ModelToNullableEnumMap = modelToNullableEnumMap;
                this.OtherModelToEnumMap = otherModelToEnumMap;
                this.ModelToModelMap = modelToModelMap;
                this.OtherModelToOtherModelMap = otherModelToOtherModelMap;
                this.ModelToOtherModelMap = modelToOtherModelMap;
                this.OtherModelToModelMap = otherModelToModelMap;
                this.ModelToModelToOtherModelMap = modelToModelToOtherModelMap;
            }

            public IReadOnlyDictionary<Position, ModelThatCannotBeConvertToString> EnumToOtherModelMap { get; private set; }

            public IReadOnlyDictionary<Position?, ModelThatCanBeConvertedToString> NullableEnumToModelMap { get; private set; }

            public IReadOnlyDictionary<string, int> StringToIntMap { get; private set; }

            public IReadOnlyDictionary<DateTime, DateTime?> DateTimeToNullableDateTimeMap { get; private set; }

            public IReadOnlyDictionary<DateTime?, DateTime> NullableDateTimeToDateTimeMap { get; private set; }

            public IReadOnlyDictionary<int, string> IntToStringMap { get; private set; }

            public IReadOnlyDictionary<ModelThatCanBeConvertedToString, Position?> ModelToNullableEnumMap { get; private set; }

            public IReadOnlyDictionary<ModelThatCannotBeConvertToString, Position> OtherModelToEnumMap { get; private set; }

            public IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString> ModelToModelMap { get; private set; }

            public IReadOnlyDictionary<ModelThatCannotBeConvertToString, ModelThatCannotBeConvertToString> OtherModelToOtherModelMap { get; private set; }

            public IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString> ModelToOtherModelMap { get; private set; }

            public IReadOnlyDictionary<ModelThatCannotBeConvertToString, ModelThatCanBeConvertedToString> OtherModelToModelMap { get; private set; }

            public IReadOnlyDictionary<ModelThatCanBeConvertedToString, IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCannotBeConvertToString>> ModelToModelToOtherModelMap { get; private set; }

            public static bool operator ==(TestModel left, TestModel right)
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

            public static bool operator !=(TestModel left, TestModel right) => !(left == right);

            public bool Equals(TestModel other)
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
                    this.EnumToOtherModelMap.IsEqualTo(other.EnumToOtherModelMap) &&
                    this.NullableEnumToModelMap.IsEqualTo(other.NullableEnumToModelMap) &&
                    this.StringToIntMap.IsEqualTo(other.StringToIntMap) &&
                    this.DateTimeToNullableDateTimeMap.IsEqualTo(other.DateTimeToNullableDateTimeMap) &&
                    this.NullableDateTimeToDateTimeMap.IsEqualTo(other.NullableDateTimeToDateTimeMap) &&
                    this.IntToStringMap.IsEqualTo(other.IntToStringMap) &&
                    this.ModelToNullableEnumMap.IsEqualTo(other.ModelToNullableEnumMap) &&
                    this.OtherModelToEnumMap.IsEqualTo(other.OtherModelToEnumMap) &&
                    this.ModelToModelMap.IsEqualTo(other.ModelToModelMap) &&
                    this.OtherModelToOtherModelMap.IsEqualTo(other.OtherModelToOtherModelMap) &&
                    this.ModelToOtherModelMap.IsEqualTo(other.ModelToOtherModelMap) &&
                    this.OtherModelToModelMap.IsEqualTo(other.OtherModelToModelMap) &&
                    this.ModelToModelToOtherModelMap.IsEqualTo(other.ModelToModelToOtherModelMap);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as TestModel);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => throw new NotImplementedException("should not get used");
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class TestModelWithMixedCaseKeys : IEquatable<TestModelWithMixedCaseKeys>
        {
            public TestModelWithMixedCaseKeys(
                IReadOnlyDictionary<string, int> stringToIntMap,
                IReadOnlyDictionary<OtherModelThatCanBeConvertedToString, int> otherModelToIntMap)
            {
                this.StringToIntMap = stringToIntMap;
                this.OtherModelToIntMap = otherModelToIntMap;
            }

            public IReadOnlyDictionary<string, int> StringToIntMap { get; private set; }

            public IReadOnlyDictionary<OtherModelThatCanBeConvertedToString, int> OtherModelToIntMap { get; private set; }

            public static bool operator ==(TestModelWithMixedCaseKeys left, TestModelWithMixedCaseKeys right)
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

            public static bool operator !=(TestModelWithMixedCaseKeys left, TestModelWithMixedCaseKeys right) => !(left == right);

            public bool Equals(TestModelWithMixedCaseKeys other)
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
                    this.StringToIntMap.IsEqualTo(other.StringToIntMap) &&
                    this.OtherModelToIntMap.IsEqualTo(other.OtherModelToIntMap);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as TestModelWithMixedCaseKeys);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => throw new NotImplementedException("should not get used");
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ModelThatCannotBeConvertToString : IEquatable<ModelThatCannotBeConvertToString>
        {
            public ModelThatCannotBeConvertToString(
                Guid id)
            {
                this.Id = id;
            }

            public Guid Id { get; private set; }

            public static bool operator ==(ModelThatCannotBeConvertToString left, ModelThatCannotBeConvertToString right)
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

            public static bool operator !=(ModelThatCannotBeConvertToString left, ModelThatCannotBeConvertToString right) => !(left == right);

            public bool Equals(ModelThatCannotBeConvertToString other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.Id == other.Id;

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelThatCannotBeConvertToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Id).Value;
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ModelThatCanBeConvertedToString : IEquatable<ModelThatCanBeConvertedToString>
        {
            public ModelThatCanBeConvertedToString(
                int value,
                int value2)
            {
                this.Value = value;
                this.Value2 = value2;
            }

            public int Value { get; private set; }

            public int Value2 { get; private set; }

            public static bool operator ==(ModelThatCanBeConvertedToString left, ModelThatCanBeConvertedToString right)
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

            public static bool operator !=(ModelThatCanBeConvertedToString left, ModelThatCanBeConvertedToString right) => !(left == right);

            public bool Equals(ModelThatCanBeConvertedToString other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = (this.Value == other.Value) && (this.Value2 == other.Value2);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelThatCanBeConvertedToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Value).Hash(this.Value2).Value;
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class OtherModelThatCanBeConvertedToString : IEquatable<OtherModelThatCanBeConvertedToString>
        {
            public OtherModelThatCanBeConvertedToString(
                string value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.Value = value;
            }

            public string Value { get; private set; }

            public static bool operator ==(OtherModelThatCanBeConvertedToString left, OtherModelThatCanBeConvertedToString right)
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

            public static bool operator !=(OtherModelThatCanBeConvertedToString left, OtherModelThatCanBeConvertedToString right) => !(left == right);

            public bool Equals(OtherModelThatCanBeConvertedToString other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.Value == other.Value;

                return result;
            }

            public override bool Equals(object obj) => this == (obj as OtherModelThatCanBeConvertedToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Value).Value;
        }

        private class ModelThatCanBeConvertedToStringSerializer : IStringSerializeAndDeserialize
        {
            private bool outputNullOrEmpty;

            public ModelThatCanBeConvertedToStringSerializer(
                bool outputNullOrEmpty = false)
            {
                this.outputNullOrEmpty = outputNullOrEmpty;
            }

            public string SerializeToString(object objectToSerialize)
            {
                if (objectToSerialize == null)
                {
                    return null;
                }

                string result;

                if (objectToSerialize is ModelThatCanBeConvertedToString model)
                {
                    if ((model.Value == int.MinValue) && (model.Value2 == int.MinValue) && this.outputNullOrEmpty)
                    {
                        result = string.Empty;
                    }
                    else if ((model.Value == int.MaxValue) && (model.Value2 == int.MaxValue) && this.outputNullOrEmpty)
                    {
                        result = null;
                    }
                    else
                    {
                        result = model.Value + "," + model.Value2;
                    }
                }
                else
                {
                    throw new ArgumentException(Invariant($"{nameof(objectToSerialize)} is not a {nameof(ModelThatCanBeConvertedToString)}"));
                }

                return result;
            }

            public T Deserialize<T>(string serializedString)
            {
                var result = (T)this.Deserialize(serializedString, typeof(T));

                return result;
            }

            public object Deserialize(string serializedString, Type type)
            {
                if (serializedString == null)
                {
                    return null;
                }

                var tokens = serializedString.Split(',');

                var value = int.Parse(tokens[0]);

                var value2 = int.Parse(tokens[1]);

                var result = new ModelThatCanBeConvertedToString(value, value2);

                return result;
            }
        }

        private class OtherModelThatCanBeConvertedToStringSerializer : IStringSerializeAndDeserialize
        {
            public string SerializeToString(object objectToSerialize)
            {
                if (objectToSerialize == null)
                {
                    return null;
                }

                string result;

                if (objectToSerialize is OtherModelThatCanBeConvertedToString model)
                {
                    result = model.Value;
                }
                else
                {
                    throw new ArgumentException(Invariant($"{nameof(objectToSerialize)} is not a {nameof(OtherModelThatCanBeConvertedToString)}"));
                }

                return result;
            }

            public T Deserialize<T>(string serializedString)
            {
                var result = (T)this.Deserialize(serializedString, typeof(T));

                return result;
            }

            public object Deserialize(string serializedString, Type type)
            {
                if (serializedString == null)
                {
                    return null;
                }

                var result = new OtherModelThatCanBeConvertedToString(serializedString);

                return result;
            }
        }

        private class NoneConvertingSerializationConfiguration : JsonSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(TestModel).ToTypeToRegisterForJson(),
                typeof(ModelThatCanBeConvertedToString).ToTypeToRegisterForJson(),
            };
        }

        private class ObjectConvertingSerializationConfiguration : JsonSerializationConfigurationBase
        {
            private static readonly ModelThatCanBeConvertedToStringSerializer ModelThatCanBeConvertedToStringSerializer = new ModelThatCanBeConvertedToStringSerializer();

            private static readonly JsonConverterBuilder ModelThatIsConvertedToStringJsonConverterBuilder = new JsonConverterBuilder(
                Guid.NewGuid().ToString(),
                () => new StringSerializerBackedJsonConverter<ModelThatCanBeConvertedToString>(ModelThatCanBeConvertedToStringSerializer),
                () => new StringSerializerBackedJsonConverter<ModelThatCanBeConvertedToString>(ModelThatCanBeConvertedToStringSerializer));

            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(TestModel).ToTypeToRegisterForJson(),
                new TypeToRegisterForJson(typeof(ModelThatCanBeConvertedToString), MemberTypesToInclude.None, RelatedTypesToInclude.None, ModelThatIsConvertedToStringJsonConverterBuilder, null),
            };
        }

        private class DictionaryKeyConvertingSerializationConfiguration : JsonSerializationConfigurationBase
        {
            private static readonly ModelThatCanBeConvertedToStringSerializer ModelThatCanBeConvertedToStringSerializer = new ModelThatCanBeConvertedToStringSerializer();

            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(TestModel).ToTypeToRegisterForJson(),
                typeof(ModelThatCanBeConvertedToString).ToTypeToRegisterForJsonUsingKeyInDictionaryStringSerializer(ModelThatCanBeConvertedToStringSerializer),
            };
        }

        private class ObjectAndDictionaryKeyConvertingSerializationConfiguration : JsonSerializationConfigurationBase
        {
            private static readonly ModelThatCanBeConvertedToStringSerializer ModelThatCanBeConvertedToStringSerializer = new ModelThatCanBeConvertedToStringSerializer();

            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(TestModel).ToTypeToRegisterForJson(),
                typeof(ModelThatCanBeConvertedToString).ToTypeToRegisterForJsonUsingStringSerializer(ModelThatCanBeConvertedToStringSerializer),
            };
        }

        private class DictionaryKeyConvertingSupportingNullOrEmptySerializationConfiguration : JsonSerializationConfigurationBase
        {
            private static readonly ModelThatCanBeConvertedToStringSerializer ModelThatCanBeConvertedToStringSerializer = new ModelThatCanBeConvertedToStringSerializer(outputNullOrEmpty: true);

            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(TestModel).ToTypeToRegisterForJson(),
                typeof(ModelThatCanBeConvertedToString).ToTypeToRegisterForJsonUsingKeyInDictionaryStringSerializer(ModelThatCanBeConvertedToStringSerializer),
            };
        }

        private class ObjectAndDictionaryKeyConvertingSupportingNullOrEmptySerializationConfiguration : JsonSerializationConfigurationBase
        {
            private static readonly ModelThatCanBeConvertedToStringSerializer ModelThatCanBeConvertedToStringSerializer = new ModelThatCanBeConvertedToStringSerializer(outputNullOrEmpty: true);

            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(TestModel).ToTypeToRegisterForJson(),
                typeof(ModelThatCanBeConvertedToString).ToTypeToRegisterForJsonUsingStringSerializer(ModelThatCanBeConvertedToStringSerializer),
            };
        }

        private class MixedCaseKeysSerializationConfiguration : JsonSerializationConfigurationBase
        {
            private static readonly OtherModelThatCanBeConvertedToStringSerializer OtherModelThatCanBeConvertedToStringSerializer = new OtherModelThatCanBeConvertedToStringSerializer();

            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(TestModelWithMixedCaseKeys).ToTypeToRegisterForJson(),
                typeof(OtherModelThatCanBeConvertedToString).ToTypeToRegisterForJsonUsingStringSerializer(OtherModelThatCanBeConvertedToStringSerializer),
            };
        }
    }
}
