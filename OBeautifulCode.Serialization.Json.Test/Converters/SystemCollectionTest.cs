// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemCollectionTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class SystemCollectionTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_return_types_that_honor_mutability_contracts___When_deserializing_various_system_collection_types()
        {
            // Arrange
            var modelThatSerializesToString = A.Dummy<TestModel>();

            var expected = new CollectionsOfModelThatSerializesToStringModel
            {
                CollectionInterface = new List<TestModel> { modelThatSerializesToString },
                ReadOnlyCollectionInterface = new HashSet<TestModel> { modelThatSerializesToString },
                ListInterface = new List<TestModel> { modelThatSerializesToString },
                ReadOnlyListInterface = new[] { modelThatSerializesToString },
                List = new List<TestModel> { modelThatSerializesToString },
                Collection = new Collection<TestModel> { modelThatSerializesToString },
                ReadOnlyCollection = new ReadOnlyCollection<TestModel>(new List<TestModel> { modelThatSerializesToString }),
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
            expected.RoundtripSerializeViaJsonWithCallbackVerification(ThrowIfObjectsDiffer, typeof(CollectionsOfTestModelJsonSerializationConfiguration));
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class CollectionsOfModelThatSerializesToStringModel : IEquatable<CollectionsOfModelThatSerializesToStringModel>
        {
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ICollection<TestModel> CollectionInterface { get; set; }

            public IReadOnlyCollection<TestModel> ReadOnlyCollectionInterface { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public IList<TestModel> ListInterface { get; set; }

            public IReadOnlyList<TestModel> ReadOnlyListInterface { get; set; }

            [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = ObcSuppressBecause.CA1002_DoNotExposeGenericLists_GenericListRequiredForTesting)]
            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public List<TestModel> List { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public Collection<TestModel> Collection { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ReadOnlyCollection<TestModel> ReadOnlyCollection { get; set; }

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
        public class TestModel : IEquatable<TestModel>
        {
            public TestModel(
                int value,
                int value2)
            {
                this.Value = value;
                this.Value2 = value2;
            }

            public int Value { get; private set; }

            public int Value2 { get; private set; }

            public static bool operator ==(TestModel left, TestModel right)
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

            public static bool operator !=(TestModel left, TestModel right) => !(left == right);

            public bool Equals(TestModel other)
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

            public override bool Equals(object obj) => this == (obj as TestModel);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Value).Hash(this.Value2).Value;
        }

        private class CollectionsOfTestModelJsonSerializationConfiguration : JsonSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(CollectionsOfModelThatSerializesToStringModel).ToTypeToRegisterForJson(),
            };
        }
    }
}