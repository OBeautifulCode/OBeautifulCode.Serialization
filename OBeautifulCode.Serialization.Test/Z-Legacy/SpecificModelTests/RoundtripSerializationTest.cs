// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripSerializationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class RoundtripSerializationTest
    {
        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Onlys", Justification = "Spelling/name is correct.")]
        public static void RoundtripSerializeDeserialize___Using_ClassWithGetterOnlysBase___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ClassWithGetterOnlys>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ClassWithGetterOnlys>);

            var expected = new ClassWithGetterOnlys();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ClassWithGetterOnlys deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.GetMyEnumFromBase.Should().Be(expected.GetMyEnumFromBase);
                deserialized.GetMyEnumFromThis.Should().Be(expected.GetMyEnumFromThis);
                deserialized.GetMyStringFromBase.Should().Be(expected.GetMyStringFromBase);
                deserialized.GetMyStringFromThis.Should().Be(expected.GetMyStringFromThis);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_ClassWithPrivateSetter___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ClassWithPrivateSetter>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ClassWithPrivateSetter>);

            var privateValue = A.Dummy<string>();
            var expected = new ClassWithPrivateSetter(privateValue);

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ClassWithPrivateSetter deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.PrivateValue.Should().Be(expected.PrivateValue);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWrappedFields___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestWrappedFields>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestWrappedFields>);

            var expected = new TestWrappedFields
            {
                NullableDateTimeNull = null,
                NullableDateTimeWithValue = DateTime.UtcNow,
                CollectionOfDateTime = new[] { DateTime.UtcNow, },
                NullableEnumNull = null,
                NullableEnumWithValue = AnotherEnumeration.None,
            };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestWrappedFields deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.NullableDateTimeNull.Should().Be(expected.NullableDateTimeNull);
                deserialized.NullableDateTimeWithValue.Should().Be(expected.NullableDateTimeWithValue);
                deserialized.CollectionOfDateTime.Should().Equal(expected.CollectionOfDateTime);
                deserialized.NullableEnumNull.Should().Be(expected.NullableEnumNull);
                deserialized.NullableEnumWithValue.Should().Be(expected.NullableEnumWithValue);
                deserialized.EnumerableOfEnum.Should().Equal(expected.EnumerableOfEnum);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithInheritor___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestWithInheritorExtraProperty>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestWithInheritorExtraProperty>);

            var expected = new TestWithInheritorExtraProperty { Id = Guid.NewGuid().ToString(), Name = Guid.NewGuid().ToString(), AnotherName = Guid.NewGuid().ToString(), };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestWithInheritorExtraProperty deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.Id.Should().Be(expected.Id);
                deserialized.Name.Should().Be(expected.Name);
                deserialized.AnotherName.Should().Be(expected.AnotherName);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithId___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestWithId>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestWithId>);

            var expected = new TestWithId { Id = Guid.NewGuid().ToString(), };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestWithId deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.Id.Should().Be(expected.Id);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestMapping_with_all_defaults___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestMapping>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestMapping>);

            var expected = new TestMapping();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestMapping deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.StringProperty.Should().BeNull();
                deserialized.IntProperty.Should().Be(default(int));
                deserialized.DateTimePropertyUtc.Should().Be(default(DateTime));
                deserialized.DateTimePropertyLocal.Should().Be(default(DateTime));
                deserialized.DateTimePropertyUnspecified.Should().Be(default(DateTime));
                deserialized.GuidProperty.Should().Be(Guid.Empty);
                deserialized.NonEnumArray.Should().BeNull();
                deserialized.EnumArray.Should().BeNull();
                deserialized.StringIntMap.Should().BeNull();
                deserialized.EnumIntMap.Should().BeNull();
                deserialized.IntIntTuple.Should().BeNull();
                deserialized.EnumProperty.Should().Be(TestEnumeration.None);
                deserialized.IntArray.Should().BeNull();
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestMapping___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestMapping>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestMapping>);

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
                EnumIntMap = new Dictionary<AnotherEnumeration, int>
                    { { A.Dummy<AnotherEnumeration>(), A.Dummy<int>() } },
                IntIntTuple = new Tuple<int, int>(3, 4),
                EnumProperty = A.Dummy<TestEnumeration>(),
                IntArray = A.Dummy<int[]>(),
            };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestMapping deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.StringProperty.Should().Be(expected.StringProperty);
                deserialized.IntProperty.Should().Be(expected.IntProperty);
                deserialized.DateTimePropertyUtc.Kind.Should().Be(DateTimeKind.Utc);
                deserialized.DateTimePropertyUtc.Should().Be(expected.DateTimePropertyUtc);
                deserialized.DateTimePropertyLocal.Should().Be(expected.DateTimePropertyLocal);
                deserialized.DateTimePropertyLocal.Kind.Should().Be(DateTimeKind.Local);
                deserialized.DateTimePropertyUnspecified.Should().Be(expected.DateTimePropertyUnspecified);
                deserialized.DateTimePropertyUnspecified.Kind.Should().Be(DateTimeKind.Unspecified);
                deserialized.GuidProperty.Should().Be(expected.GuidProperty);
                deserialized.NonEnumArray.Single().Should().Be(expected.NonEnumArray.Single());
                deserialized.EnumArray.Single().Should().Be(expected.EnumArray.Single());
                deserialized.StringIntMap.Single().Should().Be(expected.StringIntMap.Single());
                deserialized.EnumIntMap.Single().Should().Be(expected.EnumIntMap.Single());
                deserialized.IntIntTuple.Should().Be(expected.IntIntTuple);
                deserialized.EnumProperty.Should().Be(expected.EnumProperty);
                deserialized.IntArray.Should().Equal(expected.IntArray);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestCollectionFields_with_all_nulls___Works()
        {
            // Arrange
            var bsonConfigType = typeof(AttemptOnUnregisteredTypeBsonSerializationConfiguration<TypesToRegisterBsonSerializationConfiguration<TestCollectionFields>>);
            var jsonConfigType = typeof(AttemptOnUnregisteredTypeJsonSerializationConfiguration<TypesToRegisterJsonSerializationConfiguration<TestCollectionFields>>);

            var expected = new TestCollectionFields();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestCollectionFields deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.ReadOnlyCollectionDateTime.Should().BeNull();
                deserialized.ICollectionDateTime.Should().BeNull();
                deserialized.IListEnum.Should().BeNull();
                deserialized.IReadOnlyListString.Should().BeNull();
                deserialized.IReadOnlyCollectionGuid.Should().BeNull();
                deserialized.ListDateTime.Should().BeNull();
                deserialized.CollectionGuid.Should().BeNull();
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestCollectionFields___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestCollectionFields>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestCollectionFields>);

            var expected = A.Dummy<TestCollectionFields>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestCollectionFields deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.ReadOnlyCollectionDateTime.Should().Equal(expected.ReadOnlyCollectionDateTime);
                deserialized.ICollectionDateTime.Should().Equal(expected.ICollectionDateTime);
                deserialized.IListEnum.Should().Equal(expected.IListEnum);
                deserialized.IReadOnlyListString.Should().Equal(expected.IReadOnlyListString);
                deserialized.IReadOnlyCollectionGuid.Should().Equal(expected.IReadOnlyCollectionGuid);
                deserialized.ListDateTime.Should().Equal(expected.ListDateTime);
                deserialized.CollectionGuid.Should().Equal(expected.CollectionGuid);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_collection_of_Interface_type___Works()
        {
            // Arrange
            var bsonConfigType = typeof(AttemptOnUnregisteredTypeBsonSerializationConfiguration<TypesToRegisterBsonSerializationConfiguration<IDeduceWhoLetTheDogsOut>>);
            var jsonConfigType = typeof(AttemptOnUnregisteredTypeJsonSerializationConfiguration<TypesToRegisterJsonSerializationConfiguration<IDeduceWhoLetTheDogsOut>>);

            IDeduceWhoLetTheDogsOut investigator1 = new NamedInvestigator("bob", 2);
            IDeduceWhoLetTheDogsOut investigator2 = new AnonymousInvestigator(4000);

            var expected = new Investigation
            {
                Investigators = new[] { investigator1, investigator2 },
            };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, Investigation deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.Investigators.Count.Should().Be(2);

                var namedInvestigator = deserialized.Investigators.ElementAt(0) as NamedInvestigator;
                namedInvestigator.Should().NotBeNull();
                namedInvestigator.Name.Should().Be("bob");
                namedInvestigator.YearsOfPractice.Should().Be(2);

                var anonymousInvestigator = deserialized.Investigators.ElementAt(1) as AnonymousInvestigator;
                anonymousInvestigator.Should().NotBeNull();
                anonymousInvestigator.Fee.Should().Be(4000);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_SerializerRepresentation___Works()
        {
            // Arrange
            var expected = A.Dummy<SerializerRepresentation>();

            // Act & Assert
            expected.RoundtripSerializeWithBeEqualToAssertion();
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void RoundtripSerializeDeserialize___Using_ClassWithFlagsEnums___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ClassWithFlagsEnums>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ClassWithFlagsEnums>);

            var expected = new ClassWithFlagsEnums { Flags = FlagsEnumeration.SecondValue | FlagsEnumeration.ThirdValue };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ClassWithFlagsEnums deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.Flags.Should().Be(expected.Flags);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithEmptyReadOnlyCollectionOfBaseClass___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestWithReadOnlyCollectionOfBaseClass>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestWithReadOnlyCollectionOfBaseClass>);

            var expected = new TestWithReadOnlyCollectionOfBaseClass { TestCollection = new List<TestBase>() };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestWithReadOnlyCollectionOfBaseClass deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.TestCollection.Should().NotBeNull();
                deserialized.TestCollection.Count.Should().Be(0);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_TestWithReadOnlyCollectionOfBaseClass___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<TestWithReadOnlyCollectionOfBaseClass>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<TestWithReadOnlyCollectionOfBaseClass>);

            var expected = new TestWithReadOnlyCollectionOfBaseClass
                               {
                                   TestCollection =
                                       new TestBase[]
                                           {
                                               new TestImplementationOne { One = "1", Message = "1 one" },
                                               new TestImplementationTwo { Two = "2", Message = "2 two" },
                                           },
                               };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TestWithReadOnlyCollectionOfBaseClass deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.TestCollection.Should().NotBeNull();
                deserialized.TestCollection.Count.Should().Be(2);
                deserialized.TestCollection.First().GetType().Should().Be(expected.TestCollection.First().GetType());
                deserialized.TestCollection.Skip(1).First().GetType().Should().Be(expected.TestCollection.Skip(1).First().GetType());
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "atop", Justification = "Spelling/name is correct.")]
        [Fact]
        public static void When_registering_a_top_level_interface___An_implementation_of_an_interface_that_implements_top_interface___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ITopInterface>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ITopInterface>);

            var expected = A.Dummy<BottomClass>();

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, BottomClass deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.Name.Should().Be(expected.Name);
                deserialized.Species.Should().Be(expected.Species);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "FieldWorks", Justification = "This is spelled correctly.")]
        public static void RoundtripSerializeDeserialize___Using_Field_NumberField_YearField__Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<Field>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<Field>);

            var expectedId1 = "my-field-1";
            var expectedId2 = "my-field-2";

            var expectedTitle1 = "my-title-1";
            var expectedTitle2 = "my-title-2";

            var expectedNumberOfDecimalPlaces1 = 2;
            var expectedNumberOfDecimalPlaces2 = 4;

            var expected1 = new NumberField(expectedId1)
            {
                Title = expectedTitle1,
                NumberOfDecimalPlaces = expectedNumberOfDecimalPlaces1,
            };

            var expected2 = new YearField(expectedId2)
            {
                Title = expectedTitle2,
                NumberOfDecimalPlaces = expectedNumberOfDecimalPlaces2,
            };

            void ThrowIfObjectsDiffer1(DescribedSerialization serialized, NumberField deserialized)
            {
                (deserialized is YearField).Should().BeFalse();

                deserialized.Should().NotBeNull();
                deserialized.Id.Should().Be(expectedId1);
                deserialized.FieldDataKind.Should().Be(FieldDataKind.NumberWithDecimals);
                deserialized.NumberOfDecimalPlaces.Should().Be(expectedNumberOfDecimalPlaces1);
                deserialized.Title.Should().Be(expectedTitle1);
            }

            void ThrowIfObjectsDiffer2(DescribedSerialization serialized, YearField deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.Id.Should().Be(expectedId2);
                deserialized.FieldDataKind.Should().Be(FieldDataKind.Year);
                deserialized.NumberOfDecimalPlaces.Should().Be(expectedNumberOfDecimalPlaces2);
                deserialized.Title.Should().Be(expectedTitle2);
            }

            // Act & Assert
            expected1.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer1, bsonConfigType, jsonConfigType);
            expected2.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer2, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_ModelWithNullableProperties_with_nulls___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ModelWithNullableProperties>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ModelWithNullableProperties>);

            var expected = new ModelWithNullableProperties
            {
            };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ModelWithNullableProperties deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.NullableDateTime.Should().BeNull();
                deserialized.NullableGuid.Should().BeNull();
                deserialized.NullableInt.Should().BeNull();
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }

        [Fact]
        public static void RoundtripSerializeDeserialize___Using_ModelWithNullableProperties_with_non_nulls___Works()
        {
            // Arrange
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ModelWithNullableProperties>);
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ModelWithNullableProperties>);

            var expected = new ModelWithNullableProperties
            {
                NullableDateTime = A.Dummy<DateTime>(),
                NullableGuid = A.Dummy<Guid>(),
                NullableInt = A.Dummy<int>(),
            };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ModelWithNullableProperties deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.NullableDateTime.Should().Be(expected.NullableDateTime);
                deserialized.NullableGuid.Should().Be(expected.NullableGuid);
                deserialized.NullableInt.Should().Be(expected.NullableInt);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }
    }
}
