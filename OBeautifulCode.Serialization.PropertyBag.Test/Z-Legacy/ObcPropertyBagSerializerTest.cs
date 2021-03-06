﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization.PropertyBag;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class ObcPropertyBagSerializerTest
    {
#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable SA1202 // Public members should come before protected members
#pragma warning disable SA1203 // Constant fields should appear before non-constant fields
#pragma warning disable SA1204 // Static members should appear before non-static members
#pragma warning disable SA1602 // Enumeration items should be documented
#pragma warning disable SA1136 // Enum values should be on separate lines
#pragma warning disable SA1502 // Element should not be on a single line

        [Fact]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "RoundTrip", Justification = "Name/spelling is correct.")]
        public static void RoundTrip___Supported_scenarios___Works()
        {
            // Arrange
            var serializationConfigurationType1 = typeof(PropertyBagConfigIncludingAssemblyQualifiedNameForInheritTypeBase);
            var serializationConfigurationType2 = typeof(PropertyBagConfigExcludingAssemblyQualifiedNameForInheritTypeBase);

            var serializer1 = new ObcPropertyBagSerializer(serializationConfigurationType1.ToPropertyBagSerializationConfigurationType());
            var serializer2 = new ObcPropertyBagSerializer(serializationConfigurationType2.ToPropertyBagSerializationConfigurationType());

            var expected = new ComplicatedObject
            {
                NullableDecimal = 29m,
                BaseVersion = new InheritTypeDerive(),
                DeriveVersion = new InheritTypeDerive(),
                DeriveVersionArray = new[] { new InheritTypeDerive(), },
                DeriveVersionCollection = new[] { new InheritTypeDerive(), }.ToList(),
                String = A.Dummy<string>(),
                Int = A.Dummy<int>(),
                TimeSpan = A.Dummy<TimeSpan>(),
                DateTime = A.Dummy<DateTime>(),
                DateTimeNullable = A.Dummy<DateTime>(),
                CustomWithoutInterface = new CustomWithoutInterface(),
                CustomWithInterface = new CustomWithInterface(),
                StringArray = new[] { A.Dummy<string>(), },
                StringCollection = new[] { A.Dummy<string>(), }.ToList(),
                IntCollection = new[] { A.Dummy<int>(), }.ToList(),
                TimeSpanCollection = new[] { A.Dummy<TimeSpan>(), }.ToList(),
                DateTimeCollection = new[] { A.Dummy<DateTime>(), }.ToList(),
                CustomWithoutInterfaceCollection = new[] { new CustomWithoutInterface(), new CustomWithoutInterface(), }.ToList(),
                CustomWithInterfaceCollection = new[] { new CustomWithInterface(), new CustomWithInterface(), }.ToList(),
                EnumParseCollection = new[] { EnumParse.Default, EnumParse.Value, },
                EnumParse = EnumParse.Value,
                StringCollectionWithSingleEmptyString = new[] { string.Empty },
                StringCollectionWithNulls = new[] { string.Empty, A.Dummy<string>(), null, string.Empty, null, A.Dummy<string>() },
            };

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, ComplicatedObject deserialized)
            {
                deserialized.NullableDecimal.Should().Be(expected.NullableDecimal);
                deserialized.NullableDecimalDefault.Should().BeNull();
                deserialized.BaseVersion.Should().NotBeNull();
                deserialized.DeriveVersion.Should().NotBeNull();
                deserialized.DeriveVersionArray.Count().Should().Be(expected.DeriveVersionArray.Count());
                deserialized.DeriveVersionCollection.Count().Should().Be(expected.DeriveVersionCollection.Count());
                deserialized.String.Should().Be(expected.String);
                deserialized.Int.Should().Be(expected.Int);
                deserialized.TimeSpan.Should().Be(expected.TimeSpan);
                deserialized.DateTime.Should().Be(expected.DateTime);
                deserialized.DateTimeNullable.Should().Be(expected.DateTimeNullable);
                deserialized.CustomWithoutInterface.Should().NotBeNull();
                deserialized.CustomWithInterface.Should().NotBeNull();
                deserialized.StringDefault.Should().BeNull();
                deserialized.IntDefault.Should().Be(default(int));
                deserialized.TimeSpanDefault.Should().Be(default(TimeSpan));
                deserialized.DateTimeDefault.Should().Be(default(DateTime));
                deserialized.DateTimeNullableDefault.Should().BeNull();
                deserialized.CustomWithoutInterfaceDefault.Should().BeNull();
                deserialized.CustomWithInterfaceDefault.Should().BeNull();
                deserialized.StringArray.Should().Equal(expected.StringArray);
                deserialized.StringCollection.Should().Equal(expected.StringCollection);
                deserialized.IntCollection.Should().Equal(expected.IntCollection);
                deserialized.TimeSpanCollection.Should().Equal(expected.TimeSpanCollection);
                deserialized.DateTimeCollection.Should().Equal(expected.DateTimeCollection);
                deserialized.CustomWithoutInterfaceCollection.Should().HaveCount(expected.CustomWithoutInterfaceCollection.Count());
                deserialized.CustomWithInterfaceCollection.Should().HaveCount(expected.CustomWithInterfaceCollection.Count());
                deserialized.EnumParseCollection.Should().Equal(expected.EnumParseCollection);
                deserialized.EnumDefault.Should().Be(EnumParse.Default);
                deserialized.EnumParse.Should().Be(expected.EnumParse);
                deserialized.StringCollectionDefault.Should().BeNull();
                deserialized.StringCollectionWithSingleEmptyString.Should().Equal(expected.StringCollectionWithSingleEmptyString);
                deserialized.StringCollectionWithNulls.Should().Equal(expected.StringCollectionWithNulls);
            }

            var serializedPropertyBag1a = serializer1.SerializeToNamedPropertyBagWithStringValues(expected);
            var serializedPropertyBag1b = serializer1.SerializeToNamedPropertyBagWithObjectValues(expected);
            var serializedPropertyBag1c = serializer1.SerializeToOrdinalPropertyBagWithStringValues(expected);
            var serializedPropertyBag1d = serializer1.SerializeToOrdinalPropertyBagWithObjectValues(expected);

            var serializedPropertyBag2a = serializer2.SerializeToNamedPropertyBagWithStringValues(expected);
            var serializedPropertyBag2b = serializer2.SerializeToNamedPropertyBagWithObjectValues(expected);
            var serializedPropertyBag2c = serializer2.SerializeToOrdinalPropertyBagWithStringValues(expected);
            var serializedPropertyBag2d = serializer2.SerializeToOrdinalPropertyBagWithObjectValues(expected);

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(ThrowIfObjectsDiffer, serializationConfigurationType1);
            expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(ThrowIfObjectsDiffer, serializationConfigurationType2);

            var actualPropertyBag1a = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag1a);
            var actualPropertyBag1b = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag1b);
            var actualPropertyBag1c = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag1c);
            var actualPropertyBag1d = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag1d);

            var actualPropertyBag2a = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag2a);
            var actualPropertyBag2b = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag2b);
            var actualPropertyBag2c = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag2c);
            var actualPropertyBag2d = serializer1.Deserialize<ComplicatedObject>(serializedPropertyBag2d);

            ThrowIfObjectsDiffer(null, actualPropertyBag1a);
            ThrowIfObjectsDiffer(null, actualPropertyBag1b);
            ThrowIfObjectsDiffer(null, actualPropertyBag1c);
            ThrowIfObjectsDiffer(null, actualPropertyBag1d);

            ThrowIfObjectsDiffer(null, actualPropertyBag2a);
            ThrowIfObjectsDiffer(null, actualPropertyBag2b);
            ThrowIfObjectsDiffer(null, actualPropertyBag2c);
            ThrowIfObjectsDiffer(null, actualPropertyBag2d);
        }

        [Fact]
        public static void Deserializing_constructors___When_properties_exist___Works()
        {
            // Arrange
            var serializationConfigurationType1 = typeof(IncludeVersionlessAssemblyQualifiedNamePropertyBagSerializationConfiguration<TypesToRegisterPropertyBagSerializationConfiguration<ConstructorWithProperties>>);
            var serializationConfigurationType2 = typeof(ExcludeVersionlessAssemblyQualifiedNamePropertyBagSerializationConfiguration<TypesToRegisterPropertyBagSerializationConfiguration<ConstructorWithProperties>>);

            var serializer1 = new ObcPropertyBagSerializer(serializationConfigurationType1.ToPropertyBagSerializationConfigurationType());
            var serializer2 = new ObcPropertyBagSerializer(serializationConfigurationType2.ToPropertyBagSerializationConfigurationType());

            var expected = new ConstructorWithProperties(A.Dummy<string>(), A.Dummy<int>(), A.Dummy<DateTime>());

            var serializedPropertyBag1a = serializer1.SerializeToNamedPropertyBagWithStringValues(expected);
            var serializedPropertyBag1b = serializer1.SerializeToNamedPropertyBagWithObjectValues(expected);
            var serializedPropertyBag1c = serializer1.SerializeToOrdinalPropertyBagWithStringValues(expected);
            var serializedPropertyBag1d = serializer1.SerializeToOrdinalPropertyBagWithObjectValues(expected);

            var serializedPropertyBag2a = serializer2.SerializeToNamedPropertyBagWithStringValues(expected);
            var serializedPropertyBag2b = serializer2.SerializeToNamedPropertyBagWithObjectValues(expected);
            var serializedPropertyBag2c = serializer2.SerializeToOrdinalPropertyBagWithStringValues(expected);
            var serializedPropertyBag2d = serializer2.SerializeToOrdinalPropertyBagWithObjectValues(expected);

            // Act
            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, ConstructorWithProperties deserialized)
            {
                deserialized.Property1.AsTest().Must().BeEqualTo(expected.Property1);
                deserialized.Property2.AsTest().Must().BeEqualTo(expected.Property2);
                deserialized.Property3.AsTest().Must().BeEqualTo(expected.Property3);
            }

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(ThrowIfObjectsDiffer);

            var actualPropertyBag1a = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag1a);
            var actualPropertyBag1b = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag1b);
            var actualPropertyBag1c = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag1c);
            var actualPropertyBag1d = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag1d);

            var actualPropertyBag2a = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag2a);
            var actualPropertyBag2b = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag2b);
            var actualPropertyBag2c = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag2c);
            var actualPropertyBag2d = serializer1.Deserialize<ConstructorWithProperties>(serializedPropertyBag2d);

            ThrowIfObjectsDiffer(null, actualPropertyBag1a);
            ThrowIfObjectsDiffer(null, actualPropertyBag1b);
            ThrowIfObjectsDiffer(null, actualPropertyBag1c);
            ThrowIfObjectsDiffer(null, actualPropertyBag1d);

            ThrowIfObjectsDiffer(null, actualPropertyBag2a);
            ThrowIfObjectsDiffer(null, actualPropertyBag2b);
            ThrowIfObjectsDiffer(null, actualPropertyBag2c);
            ThrowIfObjectsDiffer(null, actualPropertyBag2d);
        }

        [Fact]
        public static void Deserializing_collections___When_serialized_string_has_a_comma___Works()
        {
            // Arrange
            var expected = new HasSerializesWithComma { WithCommas = new[] { new SerializesWithComma(), new SerializesWithComma() }.ToList() };

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, HasSerializesWithComma deserialized)
            {
                deserialized.WithCommas.Count.Should().Be(expected.WithCommas.Count);
                deserialized.WithCommas.ToList().ForEach(_ => _.Should().NotBeNull());
            }

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void Deserializing_constructors___When_properties_do_not_exist___Throws()
        {
            // Arrange
            var expected = new ConstructorWithoutProperties(A.Dummy<string>(), A.Dummy<string>());

            // Act
            var actual1 = Record.Exception(() => expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(null, formats: new[] { SerializationFormat.String }));
            var actual2 = Record.Exception(() => expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(null, formats: new[] { SerializationFormat.Binary }));

            // Assert
            actual1.Message.Should().Be("Could not serialize/deserialize a 'ObcPropertyBagSerializerTest.ConstructorWithoutProperties'; none of its public constructors have parameters where all parameters have a matching property (public, inherited or declared, writable, instance) by name and type.");
            actual2.Message.Should().Be("Could not serialize/deserialize a 'ObcPropertyBagSerializerTest.ConstructorWithoutProperties'; none of its public constructors have parameters where all parameters have a matching property (public, inherited or declared, writable, instance) by name and type.");
        }

        [Fact]
        public static void Configuration___Specifying_type___Works()
        {
            // Arrange
            var propertyBagConfig = typeof(AttemptOnUnregisteredTypePropertyBagSerializationConfiguration<PropertyBagConfig>);

            var expected = new TypeWithCustomPropertyBagSerializerWrapper { CustomTypeWrapper = new TypeWithCustomPropertyBagSerializer() };

            void ThrowIfObjectsDiffer(DescribedSerializationBase describedSerialization, TypeWithCustomPropertyBagSerializerWrapper deserialized)
            {
            }

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallbackVerification(ThrowIfObjectsDiffer, propertyBagConfig);
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ConstructorWithProperties
        {
            public ConstructorWithProperties(string property1, int property2, DateTime property3)
            {
                this.Property1 = property1;
                this.Property2 = property2;
                this.Property3 = property3;
            }

            public string Property1 { get; private set; }

            public int Property2 { get; private set; }

            public DateTime Property3 { get; private set; }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ConstructorWithoutProperties
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "propertyDoesNotExist", Justification = "Here to test reflection.")]
            public ConstructorWithoutProperties(string property, string propertyDoesNotExist)
            {
                this.Property = property;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Here to test reflection.")]
            public string Property { get; }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = ObcSuppressBecause.CA1812_AvoidUninstantiatedInternalClasses_ClassExistsToUseItsTypeInUnitTests)]
        public class PropertyBagConfig : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new[] { typeof(PropertyBagConfigDepend).ToPropertyBagSerializationConfigurationType() };
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class TypeWithCustomPropertyBagSerializer
        {
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class PropertyBagConfigDepend : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new TypeToRegisterForPropertyBag[]
            {
                new TypeToRegisterForPropertyBag(typeof(TypeWithCustomPropertyBagSerializer), MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new TypeWithCustomPropertyBagSerializerSerializer()),
            };
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class PropertyBagConfigIncludingAssemblyQualifiedNameForInheritTypeBase : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new TypeToRegisterForPropertyBag[]
            {
                typeof(ComplicatedObject).ToTypeToRegisterForPropertyBag(),
                new TypeToRegisterForPropertyBag(typeof(InheritTypeBase), MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new InheritTestSerializer()),
                new TypeToRegisterForPropertyBag(typeof(InheritTypeDerive), MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new InheritTestSerializer()),
            };

            public override bool IncludeVersionlessAssemblyQualifiedNameAsProperty => true;
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class PropertyBagConfigExcludingAssemblyQualifiedNameForInheritTypeBase : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new TypeToRegisterForPropertyBag[]
            {
                typeof(ComplicatedObject).ToTypeToRegisterForPropertyBag(),
                new TypeToRegisterForPropertyBag(typeof(InheritTypeBase), MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new InheritTestSerializer()),
                new TypeToRegisterForPropertyBag(typeof(InheritTypeDerive), MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new InheritTestSerializer()),
            };

            public override bool IncludeVersionlessAssemblyQualifiedNameAsProperty => false;
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class InheritTestSerializer : IStringSerializeAndDeserialize
        {
            public const string CustomSerializedString = "We have a serializer inherited.";

            public SerializationConfigurationType SerializationConfigurationType => null;

            public string SerializeToString(object objectToSerialize)
            {
                return CustomSerializedString;
            }

            public T Deserialize<T>(string serializedString)
            {
                return (T)this.Deserialize(serializedString, typeof(T));
            }

            public object Deserialize(string serializedString, Type type)
            {
                new { serializedString }.AsArg().Must().BeEqualTo(CustomSerializedString);
                (type == typeof(InheritTypeBase) || type == typeof(InheritTypeDerive)).AsArg().Must().BeTrue();
                return new InheritTypeDerive();
            }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class TypeWithCustomPropertyBagSerializerWrapper
        {
            public TypeWithCustomPropertyBagSerializer CustomTypeWrapper { get; set; }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ComplicatedObject
        {
            public decimal? NullableDecimal { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public decimal? NullableDecimalDefault { get; set; }

            public InheritTypeBase BaseVersion { get; set; }

            public InheritTypeDerive DeriveVersion { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = ObcSuppressBecause.CA1819_PropertiesShouldNotReturnArrays_ArrayPropertyRequiredForTesting)]
            public InheritTypeDerive[] DeriveVersionArray { get; set; }

            public IReadOnlyCollection<InheritTypeDerive> DeriveVersionCollection { get; set; }

            public string String { get; set; }

            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            public int Int { get; set; }

            public TimeSpan TimeSpan { get; set; }

            public DateTime DateTime { get; set; }

            public DateTime? DateTimeNullable { get; set; }

            public CustomWithoutInterface CustomWithoutInterface { get; set; }

            public CustomWithInterface CustomWithInterface { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public string StringDefault { get; set; }

            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public int IntDefault { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public TimeSpan TimeSpanDefault { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public DateTime DateTimeDefault { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public DateTime? DateTimeNullableDefault { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public CustomWithoutInterface CustomWithoutInterfaceDefault { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public CustomWithInterface CustomWithInterfaceDefault { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = ObcSuppressBecause.CA1819_PropertiesShouldNotReturnArrays_ArrayPropertyRequiredForTesting)]
            public string[] StringArray { get; set; }

            public IEnumerable<string> StringCollection { get; set; }

            public IReadOnlyCollection<int> IntCollection { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public ICollection<TimeSpan> TimeSpanCollection { get; set; }

            [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = ObcSuppressBecause.CA2227_CollectionPropertiesShouldBeReadOnly_SetterIsRequiredForTesting)]
            public IList<DateTime> DateTimeCollection { get; set; }

            public IReadOnlyList<CustomWithoutInterface> CustomWithoutInterfaceCollection { get; set; }

            public IEnumerable<CustomWithInterface> CustomWithInterfaceCollection { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public EnumParse EnumDefault { get; set; }

            public EnumParse EnumParse { get; set; }

            public IReadOnlyCollection<EnumParse> EnumParseCollection { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public IReadOnlyCollection<string> StringCollectionDefault { get; set; }

            public IReadOnlyCollection<string> StringCollectionWithSingleEmptyString { get; set; }

            public IReadOnlyCollection<string> StringCollectionWithNulls { get; set; }
        }

        public enum EnumParse { Default, Value, }

        public enum EnumAttributeProperty { Default, Value, Replaced, }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class TypeWithCustomPropertyBagSerializerSerializer : IStringSerializeAndDeserialize
        {
            public const string CustomSerializedString = "This is the string representation of a TypeWithCustomPropertyBagSerializer object.";

            public SerializationConfigurationType SerializationConfigurationType => null;

            public string SerializeToString(object objectToSerialize)
            {
                objectToSerialize.GetType().AsArg().Must().BeEqualTo(typeof(TypeWithCustomPropertyBagSerializer));

                return CustomSerializedString;
            }

            public T Deserialize<T>(string serializedString)
            {
                return (T)this.Deserialize(serializedString, typeof(T));
            }

            public object Deserialize(string serializedString, Type type)
            {
                new { serializedString }.AsArg().Must().BeEqualTo(CustomSerializedString);
                new { type }.AsArg().Must().BeEqualTo(typeof(TypeWithCustomPropertyBagSerializer));

                return new TypeWithCustomPropertyBagSerializer();
            }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class HasSerializesWithComma
        {
            public IReadOnlyCollection<SerializesWithComma> WithCommas { get; set; }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class SerializesWithComma
        {
            public const string CustomToString = "This is my tostring with a , comma...";

            public override string ToString()
            {
                return CustomToString;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for reflection call.")]
            public static SerializesWithComma Parse(string input)
            {
                new { input }.AsArg().Must().BeEqualTo(CustomToString);
                return new SerializesWithComma();
            }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class CustomWithoutInterface
        {
            public const string CustomToString = "This is my default tostring.";

            public override string ToString()
            {
                return CustomToString;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for reflection call.")]
            public static CustomWithoutInterface Parse(string input)
            {
                new { input }.AsArg().Must().BeEqualTo(CustomToString);
                return new CustomWithoutInterface();
            }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class CustomWithInterface : ISerializeToString
        {
            public const string CustomToString = "This is my default tostring.";

            public const string CustomSerializedString = "This is my serialized string.";

            public override string ToString()
            {
                return CustomToString;
            }

            public string SerializeToString()
            {
                return CustomSerializedString;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping.")]
            public static CustomWithInterface Parse(string input)
            {
                new { input }.AsArg().Must().BeEqualTo(CustomSerializedString);
                return new CustomWithInterface();
            }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public abstract class InheritTypeBase
        {
            public override string ToString()
            {
                return this.GetType().Name;
            }
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class InheritTypeDerive : InheritTypeBase
        {
            public override string ToString()
            {
                return this.GetType().Name;
            }
        }
#pragma warning restore SA1201 // Elements should appear in the correct order
#pragma warning restore SA1202 // Public members should come before protected members
#pragma warning restore SA1203 // Constant fields should appear before non-constant fields
#pragma warning restore SA1204 // Static members should appear before non-static members
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore SA1136 // Enum values should be on separate lines
#pragma warning restore SA1502 // Element should not be on a single line
    }
}
