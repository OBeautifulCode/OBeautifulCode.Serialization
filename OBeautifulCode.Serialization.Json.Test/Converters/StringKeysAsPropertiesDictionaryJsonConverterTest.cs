// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringKeysAsPropertiesDictionaryJsonConverterTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Collections.Generic;

    using FakeItEasy;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class StringKeysAsPropertiesDictionaryJsonConverterTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_ModelWithDictionaryKeysConvertedToString___When_dictionary_properties_are_not_null_nor_contain_any_null_values()
        {
            // Arrange
            var expected = A.Dummy<ModelWithDictionaryKeysConvertedToString>();

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_ModelWithDictionaryKeysConvertedToString___When_dictionary_properties_are_all_null()
        {
            // Arrange
            var expected = new ModelWithDictionaryKeysConvertedToString(null, null, null, null, null);

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_ModelWithDictionaryKeysConvertedToString___When_dictionary_properties_are_empty()
        {
            // Arrange
            var modelToStringMap = new Dictionary<ModelThatIsConvertedToString, string>();

            var modelToDateTimeMap = new Dictionary<ModelThatIsConvertedToString, DateTime>();

            var modelToModelMap = new Dictionary<ModelThatIsConvertedToString, ModelThatIsConvertedToString>();

            var modelToOtherModelMap = new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>();

            var modelToModelToOtherModelMap = new Dictionary<ModelThatIsConvertedToString, IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>>();

            var expected = new ModelWithDictionaryKeysConvertedToString(modelToStringMap, modelToDateTimeMap, modelToModelMap, modelToOtherModelMap, modelToModelToOtherModelMap);

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_ModelWithDictionaryKeysConvertedToString___When_dictionary_properties_contain_null_values()
        {
            // Arrange
            var modelToStringMap = new Dictionary<ModelThatIsConvertedToString, string>()
            {
                { A.Dummy<ModelThatIsConvertedToString>(), A.Dummy<string>() },
                { A.Dummy<ModelThatIsConvertedToString>(), null },
            };

            var modelToDateTimeMap = new Dictionary<ModelThatIsConvertedToString, DateTime>()
            {
                { A.Dummy<ModelThatIsConvertedToString>(), A.Dummy<DateTime>() },
            };

            var modelToModelMap = new Dictionary<ModelThatIsConvertedToString, ModelThatIsConvertedToString>()
            {
                { A.Dummy<ModelThatIsConvertedToString>(), A.Dummy<ModelThatIsConvertedToString>() },
                { A.Dummy<ModelThatIsConvertedToString>(), null },
            };

            var modelToOtherModelMap = new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>()
            {
                { A.Dummy<ModelThatIsConvertedToString>(), A.Dummy<ModelThatDoesNotConvertToString>() },
                { A.Dummy<ModelThatIsConvertedToString>(), null },
            };

            var modelToModelToOtherModelMap = new Dictionary<ModelThatIsConvertedToString, IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>>()
            {
                { A.Dummy<ModelThatIsConvertedToString>(), null },
                { A.Dummy<ModelThatIsConvertedToString>(), new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>() },
                { A.Dummy<ModelThatIsConvertedToString>(), new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>() { { A.Dummy<ModelThatIsConvertedToString>(), null } } },
            };

            var expected = new ModelWithDictionaryKeysConvertedToString(modelToStringMap, modelToDateTimeMap, modelToModelMap, modelToOtherModelMap, modelToModelToOtherModelMap);

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_roundtrip_ModelWithDictionaryKeysConvertedToString___When_dictionary_contains_keys_that_convert_to_empty_string()
        {
            // Arrange
            var modelToStringMap1 = new Dictionary<ModelThatIsConvertedToString, string>()
            {
                { A.Dummy<ModelThatIsConvertedToString>(), A.Dummy<string>() },
                { new ModelThatIsConvertedToString(int.MaxValue, int.MaxValue), A.Dummy<string>() }, // key serializes to null
            };

            var modelToStringMap2 = new Dictionary<ModelThatIsConvertedToString, string>()
            {
                { A.Dummy<ModelThatIsConvertedToString>(), A.Dummy<string>() },
                { new ModelThatIsConvertedToString(int.MinValue, int.MinValue), A.Dummy<string>() }, // key serializes to empty string
            };

            var systemUnderTest1 = new ModelWithDictionaryKeysConvertedToString(modelToStringMap1, null, null, null, null);
            var systemUnderTest2 = new ModelWithDictionaryKeysConvertedToString(modelToStringMap2, null, null, null, null);

            // Act
            var actual1 = Record.Exception(() => systemUnderTest1.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration)));
            var actual2 = Record.Exception(() => systemUnderTest2.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration)));

            // Assert
            actual1.AsTest().Must().BeOfType<InvalidOperationException>();
            actual1.Message.AsTest().Must().BeEqualTo("Key in dictionary serializes to a null or white space string.  Key type is StringKeysAsPropertiesDictionaryJsonConverterTest.ModelThatIsConvertedToString.");

            actual2.AsTest().Must().BeOfType<InvalidOperationException>();
            actual2.Message.AsTest().Must().BeEqualTo("Key in dictionary serializes to a null or white space string.  Key type is StringKeysAsPropertiesDictionaryJsonConverterTest.ModelThatIsConvertedToString.");
        }

        [Fact]
        public static void Serialize___Should_write_dictionary_entries_as_properties_on_json_object____When_serializing_ModelWithDictionaryKeysConvertedToString()
        {
            // Arrange
            var modelToStringMap = new Dictionary<ModelThatIsConvertedToString, string>()
            {
                { new ModelThatIsConvertedToString(4, -5), null },
                { new ModelThatIsConvertedToString(6, 3), string.Empty },
                { new ModelThatIsConvertedToString(9, -9), "my-string" },
            };

            var modelToDateTimeMap = new Dictionary<ModelThatIsConvertedToString, DateTime>()
            {
                { new ModelThatIsConvertedToString(8, 4), new DateTime(2020, 6, 23, 5, 4, 6, DateTimeKind.Utc) },
                { new ModelThatIsConvertedToString(3, 1), new DateTime(2020, 6, 23, 5, 4, 6, DateTimeKind.Unspecified) },
            };

            var modelToModelMap = new Dictionary<ModelThatIsConvertedToString, ModelThatIsConvertedToString>()
            {
                { new ModelThatIsConvertedToString(2, -7), null },
                { new ModelThatIsConvertedToString(-8, 1), new ModelThatIsConvertedToString(-1, 1) },
            };

            var modelToOtherModelMap = new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>()
            {
                { new ModelThatIsConvertedToString(-5, 6), null },
                { new ModelThatIsConvertedToString(10, 2), new ModelThatDoesNotConvertToString(Guid.Parse("00000000-0000-0000-0000-100000000000")) },
            };

            var modelToModelToOtherModelMap = new Dictionary<ModelThatIsConvertedToString, IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>>()
            {
                { new ModelThatIsConvertedToString(1, 2), null },
                { new ModelThatIsConvertedToString(3, 4), new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>() },
                { new ModelThatIsConvertedToString(5, 6), new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>() { { new ModelThatIsConvertedToString(-3, -2), null } } },
                { new ModelThatIsConvertedToString(7, 8), new Dictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>() { { new ModelThatIsConvertedToString(-3, -2), new ModelThatDoesNotConvertToString(Guid.Parse("00000000-0000-0000-0000-200000000000")) } } },
            };

            var expected = new ModelWithDictionaryKeysConvertedToString(modelToStringMap, modelToDateTimeMap, modelToModelMap, modelToOtherModelMap, modelToModelToOtherModelMap);

            void VerificationCallback(string serialized, SerializationFormat format, ModelWithDictionaryKeysConvertedToString deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().BeEqualTo("{\r\n  \"modelToStringMap\": {\r\n    \"4,-5\": null,\r\n    \"6,3\": \"\",\r\n    \"9,-9\": \"my-string\"\r\n  },\r\n  \"modelToDateTimeMap\": {\r\n    \"8,4\": \"2020-06-23T05:04:06.0000000Z\",\r\n    \"3,1\": \"2020-06-23T05:04:06.0000000\"\r\n  },\r\n  \"modelToModelMap\": {\r\n    \"2,-7\": null,\r\n    \"-8,1\": \"-1,1\"\r\n  },\r\n  \"modelToOtherModelMap\": {\r\n    \"-5,6\": null,\r\n    \"10,2\": {\r\n      \"guid\": \"00000000-0000-0000-0000-100000000000\"\r\n    }\r\n  },\r\n  \"modelToModelToOtherModelMap\": {\r\n    \"1,2\": null,\r\n    \"3,4\": {},\r\n    \"5,6\": {\r\n      \"-3,-2\": null\r\n    },\r\n    \"7,8\": {\r\n      \"-3,-2\": {\r\n        \"guid\": \"00000000-0000-0000-0000-200000000000\"\r\n      }\r\n    }\r\n  }\r\n}");
                }
            }

            // Act & Assert
            expected.RoundtripSerializeViaJsonWithCallbackVerification(VerificationCallback, typeof(ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration));
        }

        [Serializable]
        public class ModelWithDictionaryKeysConvertedToString : IEquatable<ModelWithDictionaryKeysConvertedToString>
        {
            public ModelWithDictionaryKeysConvertedToString(
                IReadOnlyDictionary<ModelThatIsConvertedToString, string> modelToStringMap,
                IReadOnlyDictionary<ModelThatIsConvertedToString, DateTime> modelToDateTimeMap,
                IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatIsConvertedToString> modelToModelMap,
                IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString> modelToOtherModelMap,
                IReadOnlyDictionary<ModelThatIsConvertedToString, IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>> modelToModelToOtherModelMap)
            {
                this.ModelToStringMap = modelToStringMap;
                this.ModelToDateTimeMap = modelToDateTimeMap;
                this.ModelToModelMap = modelToModelMap;
                this.ModelToOtherModelMap = modelToOtherModelMap;
                this.ModelToModelToOtherModelMap = modelToModelToOtherModelMap;
            }

            public IReadOnlyDictionary<ModelThatIsConvertedToString, string> ModelToStringMap { get; private set; }

            public IReadOnlyDictionary<ModelThatIsConvertedToString, DateTime> ModelToDateTimeMap { get; private set; }

            public IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatIsConvertedToString> ModelToModelMap { get; private set; }

            public IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString> ModelToOtherModelMap { get; private set; }

            public IReadOnlyDictionary<ModelThatIsConvertedToString, IReadOnlyDictionary<ModelThatIsConvertedToString, ModelThatDoesNotConvertToString>> ModelToModelToOtherModelMap { get; private set; }

            public static bool operator ==(ModelWithDictionaryKeysConvertedToString left, ModelWithDictionaryKeysConvertedToString right)
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

            public static bool operator !=(ModelWithDictionaryKeysConvertedToString left, ModelWithDictionaryKeysConvertedToString right) => !(left == right);

            public bool Equals(ModelWithDictionaryKeysConvertedToString other)
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
                    this.ModelToStringMap.IsEqualTo(other.ModelToStringMap) &&
                    this.ModelToDateTimeMap.IsEqualTo(other.ModelToDateTimeMap) &&
                    this.ModelToModelMap.IsEqualTo(other.ModelToModelMap) &&
                    this.ModelToOtherModelMap.IsEqualTo(other.ModelToOtherModelMap) &&
                    this.ModelToModelToOtherModelMap.IsEqualTo(other.ModelToModelToOtherModelMap);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelWithDictionaryKeysConvertedToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.ModelToStringMap).Hash(this.ModelToDateTimeMap).Hash(this.ModelToModelMap).Hash(this.ModelToOtherModelMap).Hash(this.ModelToModelToOtherModelMap).Value;
        }

        [Serializable]
        public class ModelThatDoesNotConvertToString : IEquatable<ModelThatDoesNotConvertToString>
        {
            public ModelThatDoesNotConvertToString(
                Guid guid)
            {
                this.Guid = guid;
            }

            public Guid Guid { get; private set; }

            public static bool operator ==(ModelThatDoesNotConvertToString left, ModelThatDoesNotConvertToString right)
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

            public static bool operator !=(ModelThatDoesNotConvertToString left, ModelThatDoesNotConvertToString right) => !(left == right);

            public bool Equals(ModelThatDoesNotConvertToString other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.Guid == other.Guid;

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelThatDoesNotConvertToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Guid).Value;
        }

        [Serializable]
        public class ModelThatIsConvertedToString : IEquatable<ModelThatIsConvertedToString>
        {
            public ModelThatIsConvertedToString(
                int value1,
                int value2)
            {
                this.Value1 = value1;
                this.Value2 = value2;
            }

            public int Value1 { get; private set; }

            public int Value2 { get; private set; }

            public static bool operator ==(ModelThatIsConvertedToString left, ModelThatIsConvertedToString right)
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

            public static bool operator !=(ModelThatIsConvertedToString left, ModelThatIsConvertedToString right) => !(left == right);

            public bool Equals(ModelThatIsConvertedToString other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = (this.Value1 == other.Value1) && (this.Value2 == other.Value2);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelThatIsConvertedToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Value1).Hash(this.Value2).Value;
        }

        private class ModelThatIsConvertedToStringJsonConverter : JsonConverter
        {
            public override void WriteJson(
                JsonWriter writer,
                object value,
                JsonSerializer serializer)
            {
                if (value is ModelThatIsConvertedToString model)
                {
                    new { writer }.AsArg().Must().NotBeNull();

                    string stringToWrite;

                    if ((model.Value1 == int.MinValue) && (model.Value2 == int.MinValue))
                    {
                        stringToWrite = string.Empty;
                    }
                    else if ((model.Value1 == int.MaxValue) && (model.Value2 == int.MaxValue))
                    {
                        stringToWrite = null;
                    }
                    else
                    {
                        stringToWrite = model.Value1 + "," + model.Value2;
                    }

                    if (stringToWrite == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.WriteValue(stringToWrite);
                    }
                }
            }

            public override object ReadJson(
                JsonReader reader,
                Type objectType,
                object existingValue,
                JsonSerializer serializer)
            {
                new { reader }.AsArg().Must().NotBeNull();

                object result = null;

                if (reader.Value != null)
                {
                    var tokens = reader.Value.ToString().Split(',');

                    var value1 = int.Parse(tokens[0]);

                    var value2 = int.Parse(tokens[1]);

                    result = new ModelThatIsConvertedToString(value1, value2);
                }

                return result;
            }

            public override bool CanConvert(
                Type objectType)
            {
                new { objectType }.Must().NotBeNull();

                var result = objectType == typeof(ModelThatIsConvertedToString);

                return result;
            }
        }

        private class ModelWithDictionaryKeysConvertedToStringJsonSerializationConfiguration : JsonSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                new TypeToRegisterForJson(typeof(ModelThatIsConvertedToString), MemberTypesToInclude.None, RelatedTypesToInclude.None, new JsonConverterBuilder("custom-converter", () => new ModelThatIsConvertedToStringJsonConverter(), () => new ModelThatIsConvertedToStringJsonConverter(), JsonConverterOutputKind.String)),
                typeof(ModelWithDictionaryKeysConvertedToString).ToTypeToRegisterForJson(),
            };
        }
    }
}
