// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMapping.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Type;

#pragma warning disable SA1201 // Elements should appear in the correct order

    [Serializable]
    public class TestMapping
    {
        public string StringProperty { get; set; }

        public TestEnumeration EnumProperty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Need to test an array.")]
        public TestEnumeration[] EnumArray { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Need to test an array.")]
        public string[] NonEnumArray { get; set; }

        public Guid GuidProperty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int", Justification = "Name is correct.")]
        public int IntProperty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public Dictionary<AnotherEnumeration, int> EnumIntMap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public Dictionary<string, int> StringIntMap { get; set; }

        public Tuple<int, int> IntIntTuple { get; set; }

        public DateTime DateTimePropertyUtc { get; set; }

        public DateTime DateTimePropertyLocal { get; set; }

        public DateTime DateTimePropertyUnspecified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Just need a type to test.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public int[] IntArray { get; set; }
    }

    [Serializable]
    public class TestWithId
    {
        public string Id { get; set; }
    }

    [Serializable]
    public class TestWithInheritor
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    [Serializable]
    public class TestWithInheritorExtraProperty : TestWithInheritor
    {
        public string AnotherName { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Here for testing.")]
    public interface ITestConfigureActionFromInterface
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Here for testing.")]
    public interface ITestConfigureActionFromAuto
    {
    }

    [Serializable]
    public class TestConfigureActionFromInterface : ITestConfigureActionFromInterface
    {
    }

    [Serializable]
    public class TestConfigureActionFromAuto : ITestConfigureActionFromAuto
    {
    }

    [Serializable]
    public abstract class TestConfigureActionBaseFromSub
    {
    }

    [Serializable]
    public abstract class TestConfigureActionBaseFromAuto
    {
    }

    [Serializable]
    public class TestConfigureActionInheritedSub : TestConfigureActionBaseFromSub
    {
    }

    [Serializable]
    public class TestConfigureActionInheritedAuto : TestConfigureActionBaseFromAuto
    {
    }

    [Serializable]
    public class TestConfigureActionSingle
    {
    }

    [Serializable]
    public class TestTracking
    {
    }

    [Serializable]
    public class TestWrappedFields
    {
        public DateTime? NullableDateTimeNull { get; set; }

        public DateTime? NullableDateTimeWithValue { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public ICollection<DateTime> CollectionOfDateTime { get; set; }

        public AnotherEnumeration? NullableEnumNull { get; set; }

        public AnotherEnumeration? NullableEnumWithValue { get; set; }

        public IEnumerable<AnotherEnumeration> EnumerableOfEnum { get; set; }
    }

    [Serializable]
    public class TestCollectionFields
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public ReadOnlyCollection<DateTime> ReadOnlyCollectionDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public ICollection<string> ICollectionDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public IList<AnotherEnumeration> IListEnum { get; set; }

        public IReadOnlyList<string> IReadOnlyListString { get; set; }

        public IReadOnlyCollection<DateTime> IReadOnlyCollectionGuid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Just need a type to test.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public List<DateTime> ListDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Just need a type to test.")]
        public Collection<Guid> CollectionGuid { get; set; }
    }

    [Serializable]
    public class TestWithReadOnlyCollectionOfBaseClass
    {
        public IReadOnlyCollection<TestBase> TestCollection { get; set; }

        public TestImplementationOne RootOne { get; set; }

        public TestImplementationTwo RootTwo { get; set; }
    }

    [Serializable]
    public class TestWithReadOnlyCollectionOfBaseClassConfig : BsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new TypeToRegisterForBson[]
        {
            typeof(TestBase).ToTypeToRegisterForBson(MemberTypesToInclude.None),
            typeof(TestWithReadOnlyCollectionOfBaseClass).ToTypeToRegisterForBson(MemberTypesToInclude.None),
        };
    }

    [Serializable]
    public abstract class TestBase
    {
        public string Message { get; set; }

        public static bool operator ==(
            TestBase item1,
            TestBase item2)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var result = item1.Equals((object)item2);

            return result;
        }

        public static bool operator !=(
            TestBase item1,
            TestBase item2)
            => !(item1 == item2);

        public bool Equals(
            TestBase other)
            => this == other;

        /// <inheritdoc />
        public abstract override bool Equals(
            object obj);

        /// <inheritdoc />
        public abstract override int GetHashCode();
    }

    [Serializable]
    public class TestImplementationOne : TestBase
    {
        public string One { get; set; }

        public static bool operator ==(
            TestImplementationOne item1,
            TestImplementationOne item2)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var result =
                (item1.Message == item2.Message) &&
                (item1.One == item2.One);

            return result;
        }

        public static bool operator !=(
            TestImplementationOne item1,
            TestImplementationOne item2)
            => !(item1 == item2);

        public bool Equals(TestImplementationOne other) => this == other;

        public override bool Equals(object obj) => this == (obj as TestImplementationOne);

        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.Message)
                .Hash(this.One)
                .Value;
    }

    [Serializable]
    public class TestImplementationTwo : TestBase
    {
        public string Two { get; set; }

        public static bool operator ==(
            TestImplementationTwo item1,
            TestImplementationTwo item2)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var result =
                (item1.Message == item2.Message) &&
                (item1.Two == item2.Two);

            return result;
        }

        public static bool operator !=(
            TestImplementationTwo item1,
            TestImplementationTwo item2)
            => !(item1 == item2);

        public bool Equals(TestImplementationTwo other) => this == other;

        public override bool Equals(object obj) => this == (obj as TestImplementationTwo);

        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.Message)
                .Hash(this.Two)
                .Value;
    }

    [Serializable]
    public struct TestStruct
    {
    }

    [Serializable]
    public enum TestEnumeration
    {
        /// <summary>
        /// No value specified.
        /// </summary>
        None,

        /// <summary>
        /// First value.
        /// </summary>
        TestFirst,

        /// <summary>
        /// Second value.
        /// </summary>
        TestSecond,

        /// <summary>
        /// Third value.
        /// </summary>
        TestThird,
    }

    [Serializable]
    public enum AnotherEnumeration
    {
        /// <summary>
        /// No value specified.
        /// </summary>
        None,

        /// <summary>
        /// First value.
        /// </summary>
        AnotherFirst,

        /// <summary>
        /// Second value.
        /// </summary>
        AnotherSecond,
    }

    [Serializable]
    public class Investigation
    {
        public IReadOnlyCollection<IDeduceWhoLetTheDogsOut> Investigators { get; set; }
    }

    public interface IDeduceWhoLetTheDogsOut
    {
        string WhoLetTheDogsOut();
    }

    [Serializable]
    public class NamedInvestigator : IDeduceWhoLetTheDogsOut
    {
        public NamedInvestigator(string name, int yearsOfPractice)
        {
            this.Name = name;

            this.YearsOfPractice = yearsOfPractice;
        }

        public string Name { get; private set; }

        public int YearsOfPractice { get; private set; }

        public string WhoLetTheDogsOut()
        {
            return $"I don't know.  I, {this.Name} quit!";
        }
    }

    [Serializable]
    public class AnonymousInvestigator : IDeduceWhoLetTheDogsOut
    {
        public AnonymousInvestigator(int fee)
        {
            this.Fee = fee;
        }

        public int Fee { get; private set; }

        public string WhoLetTheDogsOut()
        {
            return FormattableString.Invariant($"Dunno.  You owe me ${this.Fee}");
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Onlys", Justification = "Spelling/name is correct.")]
    [Serializable]
    public abstract class ClassWithGetterOnlysBase
    {
        public AnotherEnumeration GetMyEnumOnlyBase { get; }

        public string GetMyStringOnlyBase { get; }

        public abstract AnotherEnumeration GetMyEnumFromBase { get; }

        public abstract string GetMyStringFromBase { get; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Onlys", Justification = "Spelling/name is correct.")]
    [Serializable]
    public class ClassWithGetterOnlys : ClassWithGetterOnlysBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is strictly for testing.")]
        public AnotherEnumeration GetMyEnumFromThis => AnotherEnumeration.AnotherFirst;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is strictly for testing.")]
        public string GetMyStringFromThis => "TurtleBusiness";

        public override AnotherEnumeration GetMyEnumFromBase => AnotherEnumeration.AnotherSecond;

        public override string GetMyStringFromBase => "MonkeyBusiness";
    }

    [Serializable]
    public class ClassWithPrivateSetter
    {
        public ClassWithPrivateSetter(string privateValue)
        {
            this.PrivateValue = privateValue;
        }

        public string PrivateValue { get; private set; }

        public static bool operator ==(
            ClassWithPrivateSetter item1,
            ClassWithPrivateSetter item2)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var result = item1.PrivateValue == item2.PrivateValue;

            return result;
        }

        public static bool operator !=(
            ClassWithPrivateSetter item1,
            ClassWithPrivateSetter item2)
            => !(item1 == item2);

        public bool Equals(ClassWithPrivateSetter other) => this == other;

        public override bool Equals(object obj) => this == (obj as ClassWithPrivateSetter);

        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.PrivateValue)
                .Value;
    }

    [Serializable]
    public class VanillaClass : IEquatable<VanillaClass>
    {
        public string Something { get; set; }

        public static bool operator ==(
            VanillaClass item1,
            VanillaClass item2)
        {
            if (ReferenceEquals(item1, item2))
            {
                return true;
            }

            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
            {
                return false;
            }

            var result = item1.Something == item2.Something;

            return result;
        }

        public static bool operator !=(
            VanillaClass item1,
            VanillaClass item2)
            => !(item1 == item2);

        public bool Equals(VanillaClass other) => this == other;

        public override bool Equals(object obj) => this == (obj as VanillaClass);

        public override int GetHashCode() =>
            HashCodeHelper.Initialize()
                .Hash(this.Something)
                .Value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames", Justification = "Spelling/name is correct.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = "Spelling/name is correct.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Spelling/name is correct.")]
    [Flags]
    [Serializable]
    public enum FlagsEnumeration
    {
        /// <summary>
        /// None value.
        /// </summary>
        None,

        /// <summary>
        /// Second value.
        /// </summary>
        SecondValue,

        /// <summary>
        /// Third value.
        /// </summary>
        ThirdValue,
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = "Spelling/name is correct.")]
    [Serializable]
    public class ClassWithFlagsEnums
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = "Spelling/name is correct.")]
        public FlagsEnumeration Flags { get; set; }
    }

    [Serializable]
    public abstract class Field
    {
        protected Field(
            string id)
        {
            this.Id = id;
        }

        public string Id { get; private set; }

        public abstract FieldDataKind FieldDataKind { get; }

        public string Title { get; set; }
    }

    [Serializable]
    public abstract class DecimalField : Field
    {
        protected DecimalField(string id)
            : base(id)
        {
        }

        public int NumberOfDecimalPlaces { get; set; }
    }

    [Serializable]
    public class CurrencyField : DecimalField
    {
        public CurrencyField(string id)
            : base(id)
        {
        }

        public override FieldDataKind FieldDataKind
        {
            get
            {
                var result = this.NumberOfDecimalPlaces == 0
                    ? FieldDataKind.CurrencyWithoutDecimals
                    : FieldDataKind.CurrencyWithDecimals;

                return result;
            }
        }
    }

    [Serializable]
    public class NumberField : DecimalField
    {
        public NumberField(string id)
            : base(id)
        {
        }

        public override FieldDataKind FieldDataKind
        {
            get
            {
                var result = this.NumberOfDecimalPlaces == 0
                    ? FieldDataKind.NumberWithoutDecimals
                    : FieldDataKind.NumberWithDecimals;

                return result;
            }
        }
    }

    [Serializable]
    public class YearField : NumberField
    {
        public YearField(string id)
            : base(id)
        {
        }

        public override FieldDataKind FieldDataKind => FieldDataKind.Year;
    }

    [Serializable]
    public enum FieldDataKind
    {
        CurrencyWithDecimals,

        CurrencyWithoutDecimals,

        NumberWithDecimals,

        NumberWithoutDecimals,

        Text,

        Date,

        Year,
    }

#pragma warning restore SA1201 // Elements should appear in the correct order
}