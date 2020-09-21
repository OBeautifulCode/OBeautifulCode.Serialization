// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonCollectionSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    using static System.FormattableString;

    public static class ObcBsonCollectionSerializerTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_use_element_serializer___When_serializers_returned_by_GetAppropriateSerializer()
        {
            // Arrange

            // note that in older version of Serialization these assertions would have
            // failed because our the ObcBsonDateTimeSerializer was not being called at
            // de-serialization time and it resulted in DateTimes with Kind = Local, instead of Unspecified.
            var dateTime = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);

            var expected = new CollectionsOfDateTimeModel
            {
                CollectionInterface = new List<DateTime> { dateTime },
                ReadOnlyCollectionInterface = new HashSet<DateTime> { dateTime },
                ListInterface = new List<DateTime> { dateTime },
                ReadOnlyListInterface = new[] { dateTime },
                List = new List<DateTime> { dateTime },
                Collection = new Collection<DateTime> { dateTime },
                ReadOnlyCollection = new ReadOnlyCollection<DateTime>(new List<DateTime> { dateTime }),
            };

            // Act, Assert
            expected.RoundtripSerializeViaBsonUsingTypesToRegisterConfigWithBeEqualToAssertion();
        }

        [Fact]
        public static void RoundtripSerialize___Should_use_element_serializer___When_serializers_are_registered()
        {
            // Arrange
            var modelThatSerializesToString = new ModelThatSerializesToString(-603, 329);

            var expected1 = new CollectionsOfModelThatSerializesToStringModel
            {
                CollectionInterface = new List<ModelThatSerializesToString> { modelThatSerializesToString },
                ReadOnlyCollectionInterface = new HashSet<ModelThatSerializesToString> { modelThatSerializesToString },
                ListInterface = new List<ModelThatSerializesToString> { modelThatSerializesToString },
                ReadOnlyListInterface = new[] { modelThatSerializesToString },
                List = new List<ModelThatSerializesToString> { modelThatSerializesToString },
                Collection = new Collection<ModelThatSerializesToString> { modelThatSerializesToString },
                ReadOnlyCollection = new ReadOnlyCollection<ModelThatSerializesToString>(new List<ModelThatSerializesToString> { modelThatSerializesToString }),
            };

            var expected2 = A.Dummy<CollectionsOfModelThatSerializesToStringModel>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, CollectionsOfModelThatSerializesToStringModel deserialized)
            {
                deserialized.AsTest().Must().BeEqualTo(expected1);

                if (format == SerializationFormat.String)
                {
                    serialized.AsTest().Must().ContainString("-603,329");
                }
            }

            // Act, Assert
            expected1.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer, typeof(CollectionsOfModelThatSerializesToStringBsonSerializationConfiguration));
            expected2.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(CollectionsOfModelThatSerializesToStringBsonSerializationConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_return_types_that_honor_mutability_contracts___When_deserializing_various_system_collection_types()
        {
            // Arrange
            var modelThatSerializesToString = A.Dummy<ModelThatSerializesToString>();

            var expected = new CollectionsOfModelThatSerializesToStringModel
            {
                CollectionInterface = new List<ModelThatSerializesToString> { modelThatSerializesToString },
                ReadOnlyCollectionInterface = new HashSet<ModelThatSerializesToString> { modelThatSerializesToString },
                ListInterface = new List<ModelThatSerializesToString> { modelThatSerializesToString },
                ReadOnlyListInterface = new[] { modelThatSerializesToString },
                List = new List<ModelThatSerializesToString> { modelThatSerializesToString },
                Collection = new Collection<ModelThatSerializesToString> { modelThatSerializesToString },
                ReadOnlyCollection = new ReadOnlyCollection<ModelThatSerializesToString>(new List<ModelThatSerializesToString> { modelThatSerializesToString }),
            };

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, CollectionsOfModelThatSerializesToStringModel deserialized)
            {
                // these types are mutable; we should be able to add to them
                deserialized.Collection.Add(null);
                deserialized.CollectionInterface.Add(null);
                deserialized.List.Add(null);
                deserialized.ListInterface.Add(null);
            }

            // Act, Assert
            expected.RoundtripSerializeViaBsonWithCallbackVerification(ThrowIfObjectsDiffer, typeof(CollectionsOfModelThatSerializesToStringBsonSerializationConfiguration));
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class CollectionsOfDateTimeModel : IEquatable<CollectionsOfDateTimeModel>
        {
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ICollection<DateTime> CollectionInterface { get; set; }

            public IReadOnlyCollection<DateTime> ReadOnlyCollectionInterface { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public IList<DateTime> ListInterface { get; set; }

            public IReadOnlyList<DateTime> ReadOnlyListInterface { get; set; }

            [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = ObcSuppressBecause.CA1002_DoNotExposeGenericLists_GenericListRequiredForTesting)]
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public List<DateTime> List { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public Collection<DateTime> Collection { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ReadOnlyCollection<DateTime> ReadOnlyCollection { get; set; }

            public static bool operator ==(CollectionsOfDateTimeModel left, CollectionsOfDateTimeModel right)
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

            public static bool operator !=(CollectionsOfDateTimeModel left, CollectionsOfDateTimeModel right) => !(left == right);

            public bool Equals(CollectionsOfDateTimeModel other)
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
                    this.CollectionInterface.IsEqualTo(other.CollectionInterface) &&
                    this.ReadOnlyCollectionInterface.IsEqualTo(other.ReadOnlyCollectionInterface) &&
                    this.ListInterface.IsEqualTo(other.ListInterface) &&
                    this.ReadOnlyListInterface.IsEqualTo(other.ReadOnlyListInterface) &&
                    this.List.IsEqualTo(other.List) &&
                    this.Collection.IsEqualTo(other.Collection) &&
                    this.ReadOnlyCollection.IsEqualTo(other.ReadOnlyCollection);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as CollectionsOfDateTimeModel);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => throw new NotImplementedException("should not get used");
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class CollectionsOfModelThatSerializesToStringModel : IEquatable<CollectionsOfModelThatSerializesToStringModel>
        {
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ICollection<ModelThatSerializesToString> CollectionInterface { get; set; }

            public IReadOnlyCollection<ModelThatSerializesToString> ReadOnlyCollectionInterface { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public IList<ModelThatSerializesToString> ListInterface { get; set; }

            public IReadOnlyList<ModelThatSerializesToString> ReadOnlyListInterface { get; set; }

            [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = ObcSuppressBecause.CA1002_DoNotExposeGenericLists_GenericListRequiredForTesting)]
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public List<ModelThatSerializesToString> List { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public Collection<ModelThatSerializesToString> Collection { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ReadOnlyCollection<ModelThatSerializesToString> ReadOnlyCollection { get; set; }

            public static bool operator ==(CollectionsOfModelThatSerializesToStringModel left, CollectionsOfModelThatSerializesToStringModel right)
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

            public static bool operator !=(CollectionsOfModelThatSerializesToStringModel left, CollectionsOfModelThatSerializesToStringModel right) => !(left == right);

            public bool Equals(CollectionsOfModelThatSerializesToStringModel other)
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
                    this.CollectionInterface.IsEqualTo(other.CollectionInterface) &&
                    this.ReadOnlyCollectionInterface.IsEqualTo(other.ReadOnlyCollectionInterface) &&
                    this.ListInterface.IsEqualTo(other.ListInterface) &&
                    this.ReadOnlyListInterface.IsEqualTo(other.ReadOnlyListInterface) &&
                    this.List.IsEqualTo(other.List) &&
                    this.Collection.IsEqualTo(other.Collection) &&
                    this.ReadOnlyCollection.IsEqualTo(other.ReadOnlyCollection);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as CollectionsOfModelThatSerializesToStringModel);

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

        private class CollectionsOfModelThatSerializesToStringBsonSerializationConfiguration : BsonSerializationConfigurationBase
        {
            private static readonly ModelThatSerializesToStringSerializer ModelThatSerializesToStringSerializer = new ModelThatSerializesToStringSerializer();

            protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
            {
                typeof(CollectionsOfModelThatSerializesToStringModel).ToTypeToRegisterForBson(),
                typeof(ModelThatSerializesToString).ToTypeToRegisterForBsonUsingStringSerializer(ModelThatSerializesToStringSerializer),
            };
        }
    }
}