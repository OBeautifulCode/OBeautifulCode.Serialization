// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcConfigurationBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class ObcConfigurationBaseTest
    {
        [Fact]
        public static void Deserialize___Should_recurse_through_OBC_serializers_for_generic_arguments___When_called()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<MultilevelGenericsModel>);

            var dateTime = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);

            var expected = new MultilevelGenericsModel
            {
                ListOfDictionary = new List<IReadOnlyDictionary<DateTime, string>>
                {
                    new Dictionary<DateTime, string>
                    {
                        { dateTime, "whatever" },
                    },
                },
                DictionaryOfDictionary = new Dictionary<string, IReadOnlyDictionary<DateTime, string>>
                {
                    {
                        "whatever",
                        new Dictionary<DateTime, string>
                        {
                            { dateTime, "whatever" },
                        }
                    },
                },
                ListOfList = new List<IReadOnlyList<DateTime>>
                {
                    new List<DateTime>
                    {
                        dateTime,
                    },
                },
            };

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, MultilevelGenericsModel deserialized)
            {
                // note that in older version of Serialization these assertions would have
                // failed because our the ObcBsonDateTimeSerializer was not being called at
                // de-serialization time and it resulted in DateTimes with Kind = Local, instead of Unspecified.
                deserialized.ListOfDictionary.Must().BeEqualTo(expected.ListOfDictionary);
                deserialized.DictionaryOfDictionary.Must().BeEqualTo(expected.DictionaryOfDictionary);
                deserialized.ListOfList.Must().BeEqualTo(expected.ListOfList);

                // The BeEqualTo assertions above are not sufficient because BeEqualTo
                // (which uses IsEqualTo) compares dictionary keys using the dictionary's
                // embedded key comparer, which determines two DateTimes to be equal if they
                // have the same number of Ticks, regardless of whether they have the same Kind.
                deserialized.ListOfDictionary.First().First().Key.Must().BeEqualTo(dateTime);
                deserialized.DictionaryOfDictionary.First().Value.First().Key.Must().BeEqualTo(dateTime);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType);
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class MultilevelGenericsModel
        {
            public IReadOnlyList<IReadOnlyDictionary<DateTime, string>> ListOfDictionary { get; set; }

            public IReadOnlyDictionary<string, IReadOnlyDictionary<DateTime, string>> DictionaryOfDictionary { get; set; }

            public IReadOnlyList<IReadOnlyList<DateTime>> ListOfList { get; set; }
        }
    }
}