// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonDictionarySerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Cryptography;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Bson.Test.Internal;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    using static System.FormattableString;

    public static class ObcBsonDictionarySerializerTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_use_dictionary_key_and_value_serializers___When_serializers_returned_by_GetAppropriateSerializer()
        {
            // Arrange
            var serializationConfigurationType = typeof(TypesToRegisterBsonSerializationConfiguration<DictionariesOfDateTimeModel>);

            var dateTime = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);

            var expected = new DictionariesOfDateTimeModel
            {
                DictionaryInterface = new Dictionary<DateTime, DateTime> { { dateTime, dateTime } },
                ReadOnlyDictionaryInterface = new ReadOnlyDictionary<DateTime, DateTime>(
                    new Dictionary<DateTime, DateTime> { { dateTime, dateTime } }),
                Dictionary = new Dictionary<DateTime, DateTime> { { dateTime, dateTime } },
                ReadOnlyDictionary = new ReadOnlyDictionary<DateTime, DateTime>(
                    new Dictionary<DateTime, DateTime> { { dateTime, dateTime } }),
                ConcurrentDictionary = new ConcurrentDictionary<DateTime, DateTime>(
                    new Dictionary<DateTime, DateTime> { { dateTime, dateTime } }),
            };

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, DictionariesOfDateTimeModel deserialized)
            {
                // note that in older version of Serialization these assertions would have
                // passed UNLIKE the associated the test in ObcBsonCollectionSerializerTest
                // but we included this test for completeness
                deserialized.DictionaryInterface.Must().BeEqualTo(expected.DictionaryInterface);
                deserialized.ReadOnlyDictionaryInterface.Must().BeEqualTo(expected.ReadOnlyDictionaryInterface);
                deserialized.Dictionary.Must().BeEqualTo(expected.Dictionary);
                deserialized.ReadOnlyDictionary.Must().BeEqualTo(expected.ReadOnlyDictionary);
                deserialized.ConcurrentDictionary.Must().BeEqualTo(expected.ConcurrentDictionary);

                // The BeEqualTo assertions above are not sufficient because BeEqualTo
                // (which uses IsEqualTo) compares dictionary keys using the dictionary's
                // embedded key comparer, which determines two DateTimes to be equal if they
                // have the same number of Ticks, regardless of whether they have the same Kind.
                deserialized.DictionaryInterface.First().Key.Must().BeEqualTo(dateTime);
                deserialized.ReadOnlyDictionaryInterface.First().Key.Must().BeEqualTo(dateTime);
                deserialized.Dictionary.First().Key.Must().BeEqualTo(dateTime);
                deserialized.ReadOnlyDictionary.First().Key.Must().BeEqualTo(dateTime);
                deserialized.ConcurrentDictionary.First().Key.Must().BeEqualTo(dateTime);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer, serializationConfigurationType);
        }

        [Fact]
        public static void RoundtripSerialize___Should_use_dictionary_key_and_value_serializers___When_serializers_are_registered()
        {
            // Arrange
            var modelThatSerializesToStringForKey = new ModelThatSerializesToString(-892, 441);
            var modelThatSerializesToStringForValue = new ModelThatSerializesToString(228, -761);

            var expected1 = new DictionariesOfModelThatSerializesToStringModel
            {
                DictionaryInterface = new Dictionary<ModelThatSerializesToString, ModelThatSerializesToString> { { modelThatSerializesToStringForKey, modelThatSerializesToStringForValue } },
                ReadOnlyDictionaryInterface = new ReadOnlyDictionary<ModelThatSerializesToString, ModelThatSerializesToString>(
                    new Dictionary<ModelThatSerializesToString, ModelThatSerializesToString> { { modelThatSerializesToStringForKey, modelThatSerializesToStringForValue } }),
                Dictionary = new Dictionary<ModelThatSerializesToString, ModelThatSerializesToString> { { modelThatSerializesToStringForKey, modelThatSerializesToStringForValue } },
                ReadOnlyDictionary = new ReadOnlyDictionary<ModelThatSerializesToString, ModelThatSerializesToString>(
                    new Dictionary<ModelThatSerializesToString, ModelThatSerializesToString> { { modelThatSerializesToStringForKey, modelThatSerializesToStringForValue } }),
                ConcurrentDictionary = new ConcurrentDictionary<ModelThatSerializesToString, ModelThatSerializesToString>(
                    new Dictionary<ModelThatSerializesToString, ModelThatSerializesToString> { { modelThatSerializesToStringForKey, modelThatSerializesToStringForValue } }),
            };

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, DictionariesOfModelThatSerializesToStringModel deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected1);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().ContainString("-892,441");
                    serialized.AsTest().Must().ContainString("228,-761");
                }
            }

            var expected2 = A.Dummy<DictionariesOfModelThatSerializesToStringModel>();

            // Act, Assert
            expected1.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer, typeof(DictionariesOfModelThatSerializesToStringBsonSerializationConfiguration));
            expected2.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(DictionariesOfModelThatSerializesToStringBsonSerializationConfiguration));
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class DictionariesOfDateTimeModel
        {
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public IDictionary<DateTime, DateTime> DictionaryInterface { get; set; }

            public IReadOnlyDictionary<DateTime, DateTime> ReadOnlyDictionaryInterface { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public Dictionary<DateTime, DateTime> Dictionary { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ReadOnlyDictionary<DateTime, DateTime> ReadOnlyDictionary { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ConcurrentDictionary<DateTime, DateTime> ConcurrentDictionary { get; set; }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class DictionariesOfModelThatSerializesToStringModel : IEquatable<DictionariesOfModelThatSerializesToStringModel>
        {
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public IDictionary<ModelThatSerializesToString, ModelThatSerializesToString> DictionaryInterface { get; set; }

            public IReadOnlyDictionary<ModelThatSerializesToString, ModelThatSerializesToString> ReadOnlyDictionaryInterface { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public Dictionary<ModelThatSerializesToString, ModelThatSerializesToString> Dictionary { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ReadOnlyDictionary<ModelThatSerializesToString, ModelThatSerializesToString> ReadOnlyDictionary { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ConcurrentDictionary<ModelThatSerializesToString, ModelThatSerializesToString> ConcurrentDictionary { get; set; }

            public static bool operator ==(DictionariesOfModelThatSerializesToStringModel left, DictionariesOfModelThatSerializesToStringModel right)
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

            public static bool operator !=(DictionariesOfModelThatSerializesToStringModel left, DictionariesOfModelThatSerializesToStringModel right) => !(left == right);

            public bool Equals(DictionariesOfModelThatSerializesToStringModel other)
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
                    this.DictionaryInterface.IsEqualTo(other.DictionaryInterface) &&
                    this.ReadOnlyDictionaryInterface.IsEqualTo(other.ReadOnlyDictionaryInterface) &&
                    this.Dictionary.IsEqualTo(other.Dictionary) &&
                    this.ReadOnlyDictionary.IsEqualTo(other.ReadOnlyDictionary) &&
                    this.ConcurrentDictionary.IsEqualTo(other.ConcurrentDictionary);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as DictionariesOfModelThatSerializesToStringModel);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => throw new NotImplementedException("should not get used");
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ModelThatSerializesToString : IEquatable<ModelThatSerializesToString>
        {
            public ModelThatSerializesToString(
                int value,
                int value2)
            {
                this.Value = value;
                this.Value2 = value2;
            }

            public int Value { get; private set; }

            public int Value2 { get; private set; }

            public static bool operator ==(ModelThatSerializesToString left, ModelThatSerializesToString right)
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

            public static bool operator !=(ModelThatSerializesToString left, ModelThatSerializesToString right) => !(left == right);

            public bool Equals(ModelThatSerializesToString other)
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

            public override bool Equals(object obj) => this == (obj as ModelThatSerializesToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Value).Hash(this.Value2).Value;
        }

        private class ModelThatSerializesToStringSerializer : IStringSerializeAndDeserialize
        {
            public string SerializeToString(object objectToSerialize)
            {
                if (objectToSerialize == null)
                {
                    return null;
                }

                string result;

                if (objectToSerialize is ModelThatSerializesToString model)
                {
                    result = model.Value + "," + model.Value2;
                }
                else
                {
                    throw new ArgumentException(Invariant($"{nameof(objectToSerialize)} is not a {nameof(ModelThatSerializesToString)}"));
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

                var result = new ModelThatSerializesToString(value, value2);

                return result;
            }
        }

        private class DictionariesOfModelThatSerializesToStringBsonSerializationConfiguration : BsonSerializationConfigurationBase
        {
            private static readonly ModelThatSerializesToStringSerializer ModelThatSerializesToStringSerializer = new ModelThatSerializesToStringSerializer();

            protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
            {
                typeof(DictionariesOfModelThatSerializesToStringModel).ToTypeToRegisterForBson(),
                typeof(ModelThatSerializesToString).ToTypeToRegisterForBsonUsingStringSerializer(ModelThatSerializesToStringSerializer),
            };
        }
    }
}