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
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<SystemCollectionsModel>);

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

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, SystemCollectionsModel deserialized)
            {
                // note that in older version of Serialization these assertions would have
                // failed because our the ObcBsonDateTimeSerializer was not being called at
                // de-serialization time and it resulted in DateTimes with Kind = Local, instead of Unspecified.
                deserialized.ICollectionOfDateTime.Must().BeEqualTo(expected.ICollectionOfDateTime);
                deserialized.IReadOnlyCollectionOfDateTime.Must().BeEqualTo(expected.IReadOnlyCollectionOfDateTime);
                deserialized.IListOfDateTime.Must().BeEqualTo(expected.IListOfDateTime);
                deserialized.IReadOnlyListOfDateTime.Must().BeEqualTo(expected.IReadOnlyListOfDateTime);
                deserialized.ListOfDateTime.Must().BeEqualTo(expected.ListOfDateTime);
                deserialized.CollectionOfDateTime.Must().BeEqualTo(expected.CollectionOfDateTime);
                deserialized.ReadOnlyCollectionOfDateTime.Must().BeEqualTo(expected.ReadOnlyCollectionOfDateTime);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallback(ThrowIfObjectsDiffer, bsonConfigType);
        }

        [Serializable]
        public class SystemCollectionsModel
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