// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonCollectionSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;

    using Xunit;

    public static class ObcBsonCollectionSerializerTest
    {
        [Fact]
        public static void Deserialize___Should_recurse_through_OBC_element_serializer___When_called()
        {
            // Arrange
            var config = typeof(GenericDiscoveryBsonSerializationConfiguration<SystemCollectionsModel>);

            var serializer = new ObcBsonSerializer(config);

            var dateTime = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);

            var expected = new SystemCollectionsModel
            {
                ICollectionOfDateTime = new List<DateTime>
                {
                    dateTime,
                },
                IReadOnlyCollectionOfDateTime = new HashSet<DateTime>
                {
                    dateTime,
                },
                IListOfDateTime = new List<DateTime>
                {
                    dateTime,
                },
                IReadOnlyListOfDateTime = new[]
                {
                    dateTime,
                },
                ListOfDateTime = new List<DateTime>
                {
                    dateTime,
                },
                CollectionOfDateTime = new Collection<DateTime>
                {
                    dateTime,
                },
                ReadOnlyCollectionOfDateTime = new ReadOnlyCollection<DateTime>(new List<DateTime>
                {
                    dateTime,
                }),
            };

            var serialized = serializer.SerializeToString(expected);

            // Act
            var actual = serializer.Deserialize<SystemCollectionsModel>(serialized);

            // Assert
            // note that in older version of Serialization these assertions would have
            // failed because our the ObcBsonDateTimeSerializer was not being called at
            // de-serialization time and it resulted in DateTimes with Kind = Local, instead of Unspecified.
            actual.ICollectionOfDateTime.Must().BeEqualTo(expected.ICollectionOfDateTime);
            actual.IReadOnlyCollectionOfDateTime.Must().BeEqualTo(expected.IReadOnlyCollectionOfDateTime);
            actual.IListOfDateTime.Must().BeEqualTo(expected.IListOfDateTime);
            actual.IReadOnlyListOfDateTime.Must().BeEqualTo(expected.IReadOnlyListOfDateTime);
            actual.ListOfDateTime.Must().BeEqualTo(expected.ListOfDateTime);
            actual.CollectionOfDateTime.Must().BeEqualTo(expected.CollectionOfDateTime);
            actual.ReadOnlyCollectionOfDateTime.Must().BeEqualTo(expected.ReadOnlyCollectionOfDateTime);
        }

        private class SystemCollectionsModel
        {
            // ReSharper disable once InconsistentNaming
            public ICollection<DateTime> ICollectionOfDateTime { get; set; }

            // ReSharper disable once InconsistentNaming
            public IReadOnlyCollection<DateTime> IReadOnlyCollectionOfDateTime { get; set; }

            // ReSharper disable once InconsistentNaming
            public IList<DateTime> IListOfDateTime { get; set; }

            // ReSharper disable once InconsistentNaming
            public IReadOnlyList<DateTime> IReadOnlyListOfDateTime { get; set; }

            public List<DateTime> ListOfDateTime { get; set; }

            public Collection<DateTime> CollectionOfDateTime { get; set; }

            public ReadOnlyCollection<DateTime> ReadOnlyCollectionOfDateTime { get; set; }
        }
    }
}