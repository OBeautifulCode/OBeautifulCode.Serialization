﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripSerializationTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using Naos.Serialization.Bson;
    using Naos.Serialization.Domain;
    using Naos.Serialization.Json;

    using Xunit;

    using static System.FormattableString;

    public static class RoundtripSerializationTest
    {
        private static readonly NaosJsonSerializer JsonSerializerToUse = new NaosJsonSerializer();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Onlys", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_ClassWithGetterOnlysBase___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<ClassWithGetterOnlysBase>>();

            var expected = new ClassWithGetterOnlys();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as ClassWithGetterOnlys;
                actual.Should().NotBeNull();
                actual.GetMyEnumFromBase.Should().Be(expected.GetMyEnumFromBase);
                actual.GetMyEnumFromThis.Should().Be(expected.GetMyEnumFromThis);
                actual.GetMyStringFromBase.Should().Be(expected.GetMyStringFromBase);
                actual.GetMyStringFromThis.Should().Be(expected.GetMyStringFromThis);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unconfigured", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_Unconfigured_Bson___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer();

            var expected = new VanillaClass { Something = A.Dummy<string>() };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as VanillaClass;
                actual.Should().NotBeNull();
                actual.Something.Should().Be(expected.Something);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_ClassWithPrivateSetter___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<ClassWithPrivateSetter>>();

            var privateValue = A.Dummy<string>();
            var expected = new ClassWithPrivateSetter(privateValue);

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as ClassWithPrivateSetter;
                actual.Should().NotBeNull();
                actual.PrivateValue.Should().Be(expected.PrivateValue);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWrappedFields___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestWrappedFields>>();

            var expected = new TestWrappedFields
            {
                NullableDateTimeNull = null,
                NullableDateTimeWithValue = DateTime.UtcNow,
                CollectionOfDateTime = new[] { DateTime.UtcNow, },
                NullableEnumNull = null,
                NullableEnumWithValue = AnotherEnumeration.None,
            };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestWrappedFields;
                actual.Should().NotBeNull();
                actual.NullableDateTimeNull.Should().Be(expected.NullableDateTimeNull);
                actual.NullableDateTimeWithValue.Should().Be(expected.NullableDateTimeWithValue);
                actual.CollectionOfDateTime.Should().Equal(expected.CollectionOfDateTime);
                actual.NullableEnumNull.Should().Be(expected.NullableEnumNull);
                actual.NullableEnumWithValue.Should().Be(expected.NullableEnumWithValue);
                actual.EnumerableOfEnum.Should().Equal(expected.EnumerableOfEnum);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithInheritor___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestWithInheritorExtraProperty>>();

            var expected = new TestWithInheritorExtraProperty { Id = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), AnotherName = Guid.NewGuid().ToString(), };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestWithInheritorExtraProperty;
                actual.Should().NotBeNull();
                actual.Id.Should().Be(expected.Id);
                actual.Name.Should().Be(expected.Name);
                actual.AnotherName.Should().Be(expected.AnotherName);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithId___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestWithId>>();

            var expected = new TestWithId { Id = Guid.NewGuid().ToString(), };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestWithId;
                actual.Should().NotBeNull();
                actual.Id.Should().Be(expected.Id);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestMapping_with_all_defaults___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestMapping>>();

            var expected = new TestMapping();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestMapping;
                actual.Should().NotBeNull();
                actual.StringProperty.Should().BeNull();
                actual.IntProperty.Should().Be(default(int));
                actual.DateTimePropertyUtc.Should().Be(default(DateTime));
                actual.DateTimePropertyLocal.Should().Be(default(DateTime));
                actual.DateTimePropertyUnspecified.Should().Be(default(DateTime));
                actual.GuidProperty.Should().Be(Guid.Empty);
                actual.NonEnumArray.Should().BeNull();
                actual.EnumArray.Should().BeNull();
                actual.StringIntMap.Should().BeNull();
                actual.EnumIntMap.Should().BeNull();
                actual.IntIntTuple.Should().BeNull();
                actual.EnumProperty.Should().Be(TestEnumeration.None);
                actual.IntArray.Should().BeNull();
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestMapping___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestMapping>>();

            var expected = new TestMapping
                               {
                                   StringProperty = Guid.NewGuid().ToString(),
                                   IntProperty = A.Dummy<int>(),
                                   DateTimePropertyUtc = DateTime.UtcNow,
                                   DateTimePropertyLocal = DateTime.UtcNow.ToLocalTime(),
                                   DateTimePropertyUnspecified = DateTime.UtcNow.ToUnspecified(),
                                   GuidProperty = Guid.NewGuid(),
                                   NonEnumArray = new[] { A.Dummy<string>() },
                                   EnumArray = new[] { A.Dummy<TestEnumeration>(), },
                                   StringIntMap = new Dictionary<string, int> { { "key", A.Dummy<int>() } },
                                   EnumIntMap = new Dictionary<AnotherEnumeration, int> { { A.Dummy<AnotherEnumeration>(), A.Dummy<int>() } },
                                   IntIntTuple = new Tuple<int, int>(3, 4),
                                   EnumProperty = A.Dummy<TestEnumeration>(),
                                   IntArray = A.Dummy<int[]>(),
                                };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestMapping;
                actual.Should().NotBeNull();
                actual.StringProperty.Should().Be(expected.StringProperty);
                actual.IntProperty.Should().Be(expected.IntProperty);
                actual.DateTimePropertyUtc.Kind.Should().Be(DateTimeKind.Utc);
                actual.DateTimePropertyUtc.Should().Be(expected.DateTimePropertyUtc);
                actual.DateTimePropertyLocal.Should().Be(expected.DateTimePropertyLocal);
                actual.DateTimePropertyLocal.Kind.Should().Be(DateTimeKind.Local);
                actual.DateTimePropertyUnspecified.Should().Be(expected.DateTimePropertyUnspecified);
                actual.DateTimePropertyUnspecified.Kind.Should().Be(DateTimeKind.Unspecified);
                actual.GuidProperty.Should().Be(expected.GuidProperty);
                actual.NonEnumArray.Single().Should().Be(expected.NonEnumArray.Single());
                actual.EnumArray.Single().Should().Be(expected.EnumArray.Single());
                actual.StringIntMap.Single().Should().Be(expected.StringIntMap.Single());
                actual.EnumIntMap.Single().Should().Be(expected.EnumIntMap.Single());
                actual.IntIntTuple.Should().Be(expected.IntIntTuple);
                actual.EnumProperty.Should().Be(expected.EnumProperty);
                actual.IntArray.Should().Equal(expected.IntArray);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestDictionaryMapping_with_all_nulls___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestDictionaryFields>>();

            var expected = new TestDictionaryFields();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestDictionaryFields;
                actual.Should().NotBeNull();
                actual.DictionaryStringString.Should().BeNull();
                actual.IDictionaryStringString.Should().BeNull();
                actual.ReadOnlyDictionaryStringString.Should().BeNull();
                actual.IReadOnlyDictionaryStringString.Should().BeNull();
                actual.ConcurrentDictionaryStringString.Should().BeNull();
                actual.ReadOnlyDictionaryStringInt.Should().BeNull();
                actual.ReadOnlyDictionaryIntString.Should().BeNull();
                actual.IDictionaryEnumString.Should().BeNull();
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestDictionaryMapping___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestDictionaryFields>>();

            var expected = A.Dummy<TestDictionaryFields>();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestDictionaryFields;
                actual.Should().NotBeNull();
                actual.DictionaryStringString.Should().Equal(expected.DictionaryStringString);
                actual.IDictionaryStringString.Should().Equal(expected.IDictionaryStringString);
                actual.ReadOnlyDictionaryStringString.Should().Equal(expected.ReadOnlyDictionaryStringString);
                actual.IReadOnlyDictionaryStringString.Should().Equal(expected.IReadOnlyDictionaryStringString);
                actual.ConcurrentDictionaryStringString.Should().Equal(expected.ConcurrentDictionaryStringString);
                actual.ReadOnlyDictionaryStringInt.Should().Equal(expected.ReadOnlyDictionaryStringInt);
                actual.ReadOnlyDictionaryIntString.Should().Equal(expected.ReadOnlyDictionaryIntString);
                actual.IDictionaryEnumString.Should().Equal(expected.IDictionaryEnumString);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestDictionaryMixedKeyValues_with_all_nulls___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestDictionaryMixedKeyValues>>();

            var expected = new TestDictionaryMixedKeyValues();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestDictionaryMixedKeyValues;
                actual.Should().NotBeNull();

                actual.DictionaryBaseString.Should().BeNull();
                actual.IDictionaryBaseString.Should().BeNull();
                actual.ReadOnlyDictionaryBaseString.Should().BeNull();
                actual.IReadOnlyDictionaryBaseString.Should().BeNull();
                actual.ConcurrentDictionaryBaseString.Should().BeNull();

                actual.DictionaryStringConstructor.Should().BeNull();
                actual.IDictionaryStringConstructor.Should().BeNull();
                actual.ReadOnlyDictionaryStringConstructor.Should().BeNull();
                actual.IReadOnlyDictionaryStringConstructor.Should().BeNull();
                actual.ConcurrentDictionaryStringConstructor.Should().BeNull();
                
                actual.DictionaryConstructorEnum.Should().BeNull();
                actual.IDictionaryConstructorEnum.Should().BeNull();
                actual.ReadOnlyDictionaryConstructorEnum.Should().BeNull();
                actual.IReadOnlyDictionaryConstructorEnum.Should().BeNull();
                actual.ConcurrentDictionaryConstructorEnum.Should().BeNull();

                actual.DictionaryEnumBase.Should().BeNull();
                actual.IDictionaryEnumBase.Should().BeNull();
                actual.ReadOnlyDictionaryEnumBase.Should().BeNull();
                actual.IReadOnlyDictionaryEnumBase.Should().BeNull();
                actual.ConcurrentDictionaryEnumBase.Should().BeNull();

                actual.DictionaryIntPoco.Should().BeNull();
                actual.IDictionaryIntPoco.Should().BeNull();
                actual.ReadOnlyDictionaryIntPoco.Should().BeNull();
                actual.IReadOnlyDictionaryIntPoco.Should().BeNull();
                actual.ConcurrentDictionaryIntPoco.Should().BeNull();

                actual.DictionaryPocoInt.Should().BeNull();
                actual.IDictionaryPocoInt.Should().BeNull();
                actual.ReadOnlyDictionaryPocoInt.Should().BeNull();
                actual.IReadOnlyDictionaryPocoInt.Should().BeNull();
                actual.ConcurrentDictionaryPocoInt.Should().BeNull();
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestDictionaryMixedKeyValues___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestDictionaryMixedKeyValues>>();

            var expected = A.Dummy<TestDictionaryMixedKeyValues>();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestDictionaryMixedKeyValues;
                actual.Should().NotBeNull();

                actual.DictionaryBaseString.Should().Equal(expected.DictionaryBaseString);
                actual.IDictionaryBaseString.Should().Equal(expected.IDictionaryBaseString);
                actual.ReadOnlyDictionaryBaseString.Should().Equal(expected.ReadOnlyDictionaryBaseString);
                actual.IReadOnlyDictionaryBaseString.Should().Equal(expected.IReadOnlyDictionaryBaseString);
                actual.ConcurrentDictionaryBaseString.Should().Equal(expected.ConcurrentDictionaryBaseString);

                actual.DictionaryStringConstructor.Should().Equal(expected.DictionaryStringConstructor);
                actual.IDictionaryStringConstructor.Should().Equal(expected.IDictionaryStringConstructor);
                actual.ReadOnlyDictionaryStringConstructor.Should().Equal(expected.ReadOnlyDictionaryStringConstructor);
                actual.IReadOnlyDictionaryStringConstructor.Should().Equal(expected.IReadOnlyDictionaryStringConstructor);
                actual.ConcurrentDictionaryStringConstructor.Should().Equal(expected.ConcurrentDictionaryStringConstructor);

                actual.DictionaryConstructorEnum.Should().Equal(expected.DictionaryConstructorEnum);
                actual.IDictionaryConstructorEnum.Should().Equal(expected.IDictionaryConstructorEnum);
                actual.ReadOnlyDictionaryConstructorEnum.Should().Equal(expected.ReadOnlyDictionaryConstructorEnum);
                actual.IReadOnlyDictionaryConstructorEnum.Should().Equal(expected.IReadOnlyDictionaryConstructorEnum);
                actual.ConcurrentDictionaryConstructorEnum.Should().Equal(expected.ConcurrentDictionaryConstructorEnum);

                actual.DictionaryEnumBase.Should().Equal(expected.DictionaryEnumBase);
                actual.IDictionaryEnumBase.Should().Equal(expected.IDictionaryEnumBase);
                actual.ReadOnlyDictionaryEnumBase.Should().Equal(expected.ReadOnlyDictionaryEnumBase);
                actual.IReadOnlyDictionaryEnumBase.Should().Equal(expected.IReadOnlyDictionaryEnumBase);
                actual.ConcurrentDictionaryEnumBase.Should().Equal(expected.ConcurrentDictionaryEnumBase);

                actual.DictionaryIntPoco.Should().Equal(expected.DictionaryIntPoco);
                actual.IDictionaryIntPoco.Should().Equal(expected.IDictionaryIntPoco);
                actual.ReadOnlyDictionaryIntPoco.Should().Equal(expected.ReadOnlyDictionaryIntPoco);
                actual.IReadOnlyDictionaryIntPoco.Should().Equal(expected.IReadOnlyDictionaryIntPoco);
                actual.ConcurrentDictionaryIntPoco.Should().Equal(expected.ConcurrentDictionaryIntPoco);

                actual.DictionaryPocoInt.Should().Equal(expected.DictionaryPocoInt);
                actual.IDictionaryPocoInt.Should().Equal(expected.IDictionaryPocoInt);
                actual.ReadOnlyDictionaryPocoInt.Should().Equal(expected.ReadOnlyDictionaryPocoInt);
                actual.IReadOnlyDictionaryPocoInt.Should().Equal(expected.IReadOnlyDictionaryPocoInt);
                actual.ConcurrentDictionaryPocoInt.Should().Equal(expected.ConcurrentDictionaryPocoInt);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestCollectionFields_with_all_nulls___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestCollectionFields>>();

            var expected = new TestCollectionFields();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestCollectionFields;
                actual.Should().NotBeNull();
                actual.ReadOnlyCollectionDateTime.Should().BeNull();
                actual.ICollectionDateTime.Should().BeNull();
                actual.IListEnum.Should().BeNull();
                actual.IReadOnlyListString.Should().BeNull();
                actual.IReadOnlyCollectionGuid.Should().BeNull();
                actual.ListDateTime.Should().BeNull();
                actual.CollectionGuid.Should().BeNull();
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestCollectionFields___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestCollectionFields>>();

            var expected = A.Dummy<TestCollectionFields>();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestCollectionFields;
                actual.Should().NotBeNull();
                actual.ReadOnlyCollectionDateTime.Should().Equal(expected.ReadOnlyCollectionDateTime);
                actual.ICollectionDateTime.Should().Equal(expected.ICollectionDateTime);
                actual.IListEnum.Should().Equal(expected.IListEnum);
                actual.IReadOnlyListString.Should().Equal(expected.IReadOnlyListString);
                actual.IReadOnlyCollectionGuid.Should().Equal(expected.IReadOnlyCollectionGuid);
                actual.ListDateTime.Should().Equal(expected.ListDateTime);
                actual.CollectionGuid.Should().Equal(expected.CollectionGuid);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_collection_of_Interface_type___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<InvestigationConfiguration>();

            IDeduceWhoLetTheDogsOut investigator1 = new NamedInvestigator("bob", 2);
            IDeduceWhoLetTheDogsOut investigator2 = new AnonymousInvestigator(4000);

            var expected = new Investigation
            {
                Investigators = new[] { investigator1, investigator2 },
            };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as Investigation;
                actual.Should().NotBeNull();
                actual.Investigators.Count.Should().Be(2);

                var namedInvestigator = actual.Investigators.ElementAt(0) as NamedInvestigator;
                namedInvestigator.Should().NotBeNull();
                namedInvestigator.Name.Should().Be("bob");
                namedInvestigator.YearsOfPractice.Should().Be(2);

                var anonymousInvestigator = actual.Investigators.ElementAt(1) as AnonymousInvestigator;
                anonymousInvestigator.Should().NotBeNull();
                anonymousInvestigator.Fee.Should().Be(4000);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_SerializationDescription___Works()
        {
            // Arrange
            var expected = A.Dummy<SerializationDescription>();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as SerializationDescription;
                actual.Should().NotBeNull();
                actual.Should().Be(expected);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, new NaosBsonSerializer());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_ClassWithFlagsEnums___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<ClassWithFlagsEnums>>();
            var expected = new ClassWithFlagsEnums { Flags = FlagsEnumeration.SecondValue | FlagsEnumeration.ThirdValue };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as ClassWithFlagsEnums;
                actual.Should().NotBeNull();
                actual.Flags.Should().Be(expected.Flags);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithDictionaryKeyedOnEnum___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<BsonConfigurationAutoRegisterType<TestWithDictionaryKeyedOnEnum>>();

            var expected = A.Dummy<TestWithDictionaryKeyedOnEnum>();

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestWithDictionaryKeyedOnEnum;
                actual.Should().NotBeNull();
                actual.TestDictionary.Should().NotBeNull();
                actual.TestDictionary.Count.Should().NotBe(0);
                actual.TestDictionary.Should().Equal(expected.TestDictionary);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithEmptyReadOnlyCollectionOfBaseClass___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<TestWithReadOnlyCollectionOfBaseClassConfig>();

            var expected = new TestWithReadOnlyCollectionOfBaseClass { TestCollection = new List<TestBase>() };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestWithReadOnlyCollectionOfBaseClass;
                actual.Should().NotBeNull();
                actual.TestCollection.Should().NotBeNull();
                actual.TestCollection.Count.Should().Be(0);
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithReadOnlyCollectionOfBaseClass___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<TestWithReadOnlyCollectionOfBaseClassConfig>();

            var expected = new TestWithReadOnlyCollectionOfBaseClass
                               {
                                   TestCollection =
                                       new TestBase[]
                                           {
                                               new TestImplementationOne { One = "1", Message = "1 one" },
                                               new TestImplementationTwo { Two = "2", Message = "2 two" },
                                           },
                               };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestWithReadOnlyCollectionOfBaseClass;
                actual.Should().NotBeNull();
                actual.TestCollection.Should().NotBeNull();
                actual.TestCollection.Count.Should().Be(2);
                actual.TestCollection.First().GetType().Should().Be(expected.TestCollection.First().GetType());
                actual.TestCollection.Skip(1).First().GetType().Should().Be(expected.TestCollection.Skip(1).First().GetType());
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithDictionaryOfEnumToReadOnlyCollectionOfEnums___Works()
        {
            // Arrange
            var bsonSerializer = new NaosBsonSerializer<TestWithDictionaryOfEnumToReadOnlyCollectionOfEnumsConfig>();

            var expected = new TestWithDictionaryOfEnumToReadOnlyCollectionOfEnums
            {
                TestDictionary = new Dictionary<TestEnumeration, IReadOnlyCollection<AnotherEnumeration>>
                {
                    {
                        TestEnumeration.TestFirst, null
                    },
                    {
                        TestEnumeration.TestSecond, new AnotherEnumeration[0]
                    },
                    {
                        TestEnumeration.TestThird, new[] { AnotherEnumeration.AnotherFirst, AnotherEnumeration.AnotherSecond }
                    },
                },
            };

            void ThrowIfObjectsDiffer(object actualAsObject)
            {
                var actual = actualAsObject as TestWithDictionaryOfEnumToReadOnlyCollectionOfEnums;
                actual.Should().NotBeNull();
                actual.TestDictionary.Should().NotBeNull();
                actual.TestDictionary.Count.Should().Be(3);
                actual.TestDictionary.OrderBy(_ => _.Key).First().GetType().Should().Be(expected.TestDictionary.OrderBy(_ => _.Key).First().GetType());
                actual.TestDictionary.OrderBy(_ => _.Key).Skip(1).First().GetType().Should().Be(expected.TestDictionary.OrderBy(_ => _.Key).Skip(1).First().GetType());
                actual.TestDictionary.OrderBy(_ => _.Key).Skip(2).First().GetType().Should().Be(expected.TestDictionary.OrderBy(_ => _.Key).Skip(2).First().GetType());
            }

            // Act & Assert
            ActAndAssertForRoundtripSerialization(expected, ThrowIfObjectsDiffer, bsonSerializer);
        }

        private static void ActAndAssertForRoundtripSerialization(
            object expected,
            Action<object> throwIfObjectsDiffer,
            NaosBsonSerializer bsonSerializer,
            bool testBson = true,
            bool testJson = true)
        {
            var stringSerializers = new List<IStringSerializeAndDeserialize>();
            var binarySerializers = new List<IBinarySerializeAndDeserialize>();

            if (testJson)
            {
                stringSerializers.Add(JsonSerializerToUse);
                binarySerializers.Add(JsonSerializerToUse);
            }

            if (testBson)
            {
                stringSerializers.Add(bsonSerializer);
                binarySerializers.Add(bsonSerializer);
            }

            if (!stringSerializers.Any() || !binarySerializers.Any())
            {
                throw new InvalidOperationException("no serializers are being tested");
            }

            foreach (var stringSerializer in stringSerializers)
            {
                var actualString = stringSerializer.SerializeToString(expected);
                var actualObject = stringSerializer.Deserialize(actualString, expected.GetType());

                try
                {
                    throwIfObjectsDiffer(actualObject);
                }
                catch (Exception ex)
                {
                    throw new NaosSerializationException(Invariant($"Failure with {nameof(stringSerializer)} - {stringSerializer.GetType()}"), ex);
                }
            }

            foreach (var binarySerializer in binarySerializers)
            {
                var actualBytes = binarySerializer.SerializeToBytes(expected);
                var actualObject = binarySerializer.Deserialize(actualBytes, expected.GetType());

                try
                {
                    throwIfObjectsDiffer(actualObject);
                }
                catch (Exception ex)
                {
                    throw new NaosSerializationException(Invariant($"Failure with {nameof(binarySerializer)} - {binarySerializer.GetType()}"), ex);
                }
            }
        }
    }
}
