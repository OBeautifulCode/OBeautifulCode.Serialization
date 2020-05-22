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

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Bson.Test.Internal;

    using Xunit;

    public static class ObcBsonDictionarySerializerTest
    {
        [Fact]
        public static void Deserialize___Should_recurse_through_OBC_key_and_value_serializers___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<SystemDictionariesModel>);

            var dateTime = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);

            var expected = new SystemDictionariesModel
            {
                IDictionaryOfDateTime = new Dictionary<DateTime, DateTime>
                {
                    { dateTime, dateTime },
                },
                IReadOnlyDictionaryOfDateTime = new ReadOnlyDictionary<DateTime, DateTime>(new Dictionary<DateTime, DateTime>
                {
                    { dateTime, dateTime },
                }),
                DictionaryOfDateTime = new Dictionary<DateTime, DateTime>
                {
                    { dateTime, dateTime },
                },
                ReadOnlyDictionaryDateTime = new ReadOnlyDictionary<DateTime, DateTime>(new Dictionary<DateTime, DateTime>
                {
                    { dateTime, dateTime },
                }),
                ConcurrentDictionaryOfDateTime = new ConcurrentDictionary<DateTime, DateTime>(new Dictionary<DateTime, DateTime>
                {
                    { dateTime, dateTime },
                }),
            };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, SystemDictionariesModel deserialized)
            {
                // note that in older version of Serialization these assertions would have
                // passed UNLIKE the associated the test in ObcBsonCollectionSerializerTest
                // but we included this test for completeness
                deserialized.IDictionaryOfDateTime.Must().BeEqualTo(expected.IDictionaryOfDateTime);
                deserialized.IReadOnlyDictionaryOfDateTime.Must().BeEqualTo(expected.IReadOnlyDictionaryOfDateTime);
                deserialized.DictionaryOfDateTime.Must().BeEqualTo(expected.DictionaryOfDateTime);
                deserialized.ReadOnlyDictionaryDateTime.Must().BeEqualTo(expected.ReadOnlyDictionaryDateTime);
                deserialized.ConcurrentDictionaryOfDateTime.Must().BeEqualTo(expected.ConcurrentDictionaryOfDateTime);

                // The BeEqualTo assertions above are not sufficient because BeEqualTo
                // (which uses IsEqualTo) compares dictionary keys using the dictionary's
                // embedded key comparer, which determines two DateTimes to be equal if they
                // have the same number of Ticks, regardless of whether they have the same Kind.
                deserialized.IDictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
                deserialized.IReadOnlyDictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
                deserialized.DictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
                deserialized.ReadOnlyDictionaryDateTime.First().Key.Must().BeEqualTo(dateTime);
                deserialized.ConcurrentDictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallback(ThrowIfObjectsDiffer, bsonConfigType);
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class SystemDictionariesModel
        {
            // ReSharper disable once InconsistentNaming
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public IDictionary<DateTime, DateTime> IDictionaryOfDateTime { get; set; }

            // ReSharper disable once InconsistentNaming
            public IReadOnlyDictionary<DateTime, DateTime> IReadOnlyDictionaryOfDateTime { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public Dictionary<DateTime, DateTime> DictionaryOfDateTime { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ReadOnlyDictionary<DateTime, DateTime> ReadOnlyDictionaryDateTime { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ConcurrentDictionary<DateTime, DateTime> ConcurrentDictionaryOfDateTime { get; set; }
        }
    }
}