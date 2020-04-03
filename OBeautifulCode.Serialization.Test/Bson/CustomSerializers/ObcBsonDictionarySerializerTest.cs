// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonDictionarySerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;

    using Xunit;

    public static class ObcBsonDictionarySerializerTest
    {
        [Fact]
        public static void Deserialize___Should_recurse_through_OBC_key_and_value_serializers___When_called()
        {
            // Arrange
            var config = typeof(GenericDiscoveryBsonSerializationConfiguration<SystemDictionariesModel>);

            var serializer = new ObcBsonSerializer(config);

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

            var serialized = serializer.SerializeToString(expected);

            // Act
            var actual = serializer.Deserialize<SystemDictionariesModel>(serialized);

            // Assert
            // note that in older version of Serialization these assertions would have
            // passed UNLIKE the associated the test in ObcBsonCollectionSerializerTest
            // but we included this test for completeness
            actual.IDictionaryOfDateTime.Must().BeEqualTo(expected.IDictionaryOfDateTime);
            actual.IReadOnlyDictionaryOfDateTime.Must().BeEqualTo(expected.IReadOnlyDictionaryOfDateTime);
            actual.DictionaryOfDateTime.Must().BeEqualTo(expected.DictionaryOfDateTime);
            actual.ReadOnlyDictionaryDateTime.Must().BeEqualTo(expected.ReadOnlyDictionaryDateTime);
            actual.ConcurrentDictionaryOfDateTime.Must().BeEqualTo(expected.ConcurrentDictionaryOfDateTime);

            // The BeEqualTo assertions above are not sufficient because BeEqualTo
            // (which uses IsEqualTo) compares dictionary keys using the dictionary's
            // embedded key comparer, which determines two DateTimes to be equal if they
            // have the same number of Ticks, regardless of whether they have the same Kind.
            actual.IDictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
            actual.IReadOnlyDictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
            actual.DictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
            actual.ReadOnlyDictionaryDateTime.First().Key.Must().BeEqualTo(dateTime);
            actual.ConcurrentDictionaryOfDateTime.First().Key.Must().BeEqualTo(dateTime);
        }

        private class SystemDictionariesModel
        {
            // ReSharper disable once InconsistentNaming
            public IDictionary<DateTime, DateTime> IDictionaryOfDateTime { get; set; }

            // ReSharper disable once InconsistentNaming
            public IReadOnlyDictionary<DateTime, DateTime> IReadOnlyDictionaryOfDateTime { get; set; }

            public Dictionary<DateTime, DateTime> DictionaryOfDateTime { get; set; }

            public ReadOnlyDictionary<DateTime, DateTime> ReadOnlyDictionaryDateTime { get; set; }

            public ConcurrentDictionary<DateTime, DateTime> ConcurrentDictionaryOfDateTime { get; set; }
        }
    }
}