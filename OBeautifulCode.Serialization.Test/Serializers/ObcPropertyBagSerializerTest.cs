// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializerTest.cs" company="OBeautifulCode">
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

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.PropertyBag;
    using OBeautifulCode.Serialization.Test.Internal;
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
            var serializationConfigurationType = typeof(PropertyBagConfigForInheritTypeBase);

            var serializer = new ObcPropertyBagSerializer(serializationConfigurationType.ToPropertyBagSerializationConfigurationType());

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

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ComplicatedObject deserialized)
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

            var serializedPropertyBag = serializer.SerializeToPropertyBag(expected);

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallback(ThrowIfObjectsDiffer, serializationConfigurationType);

            var actualPropertyBag = serializer.Deserialize<ComplicatedObject>(serializedPropertyBag);
            ThrowIfObjectsDiffer(null, actualPropertyBag);
        }

        [Fact]
        public static void Deserializing_constructors___When_properties_exist___Works()
        {
            // Arrange
            var expected = new ConstructorWithProperties(A.Dummy<string>(), A.Dummy<string>(), A.Dummy<string>());

            // Act
            void ThrowIfObjectsDiffer(DescribedSerialization serialized, ConstructorWithProperties deserialized)
            {
                deserialized.PropertyGetOnly.Should().Be(expected.PropertyGetOnly);
                deserialized.PropertyPrivateSet.Should().Be(expected.PropertyPrivateSet);
                deserialized.PropertyPublicSet.Should().Be(expected.PropertyPublicSet);
            }

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallback(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void Deserializing_collections___When_serialized_string_has_a_comma___Works()
        {
            // Arrange
            var expected = new HasSerializesWithComma { WithCommas = new[] { new SerializesWithComma(), new SerializesWithComma() }.ToList() };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, HasSerializesWithComma deserialized)
            {
                deserialized.WithCommas.Count.Should().Be(expected.WithCommas.Count);
                deserialized.WithCommas.ToList().ForEach(_ => _.Should().NotBeNull());
            }

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallback(ThrowIfObjectsDiffer);
        }

        [Fact]
        public static void Deserializing_constructors___When_properties_do_not_exist___Throws()
        {
            // Arrange
            var expected = new ConstructorWithoutProperties(A.Dummy<string>(), A.Dummy<string>());

            // Act
            var actual1 = Record.Exception(() => expected.RoundtripSerializeViaPropertyBagWithCallback(null, formats: new[] { SerializationFormat.String }));
            var actual2 = Record.Exception(() => expected.RoundtripSerializeViaPropertyBagWithCallback(null, formats: new[] { SerializationFormat.Binary }));

            // Assert
            actual1.Message.Should().Be("Could not find a parameterless constructor or a constructor whose parameter names matched the properties provided; type: OBeautifulCode.Serialization.Test.ObcPropertyBagSerializerTest+ConstructorWithoutProperties, properties: Property,ToString,GetType.");
            actual2.Message.Should().Be("Could not find a parameterless constructor or a constructor whose parameter names matched the properties provided; type: OBeautifulCode.Serialization.Test.ObcPropertyBagSerializerTest+ConstructorWithoutProperties, properties: Property,ToString,GetType.");
        }

        [Fact]
        public static void Configuration___Specifying_type___Works()
        {
            // Arrange
            var propertyBagConfig = typeof(AttemptOnUnregisteredTypePropertyBagSerializationConfiguration<PropertyBagConfig>);

            var expected = new TypeWithCustomPropertyBagSerializerWrapper { CustomTypeWrapper = new TypeWithCustomPropertyBagSerializer() };

            void ThrowIfObjectsDiffer(DescribedSerialization serialized, TypeWithCustomPropertyBagSerializerWrapper deserialized)
            {
            }

            // Act, Assert
            expected.RoundtripSerializeViaPropertyBagWithCallback(ThrowIfObjectsDiffer, propertyBagConfig);
        }

        [Serializable]
        public class ConstructorWithProperties
        {
            public ConstructorWithProperties(string propertyGetOnly, string propertyPrivateSet, string propertyPublicSet)
            {
                this.PropertyGetOnly = propertyGetOnly;
                this.PropertyPrivateSet = propertyPrivateSet;
                this.PropertyPublicSet = propertyPublicSet;
            }

            public string PropertyGetOnly { get; }

            public string PropertyPrivateSet { get; private set; }

            public string PropertyPublicSet { get; set; }
        }

        [Serializable]
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
        public class PropertyBagConfigDepend : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new TypeToRegisterForPropertyBag[]
            {
                typeof(TypeWithCustomPropertyBagSerializer).ToTypeToRegisterForPropertyBag(MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new TypeWithCustomPropertyBagSerializerSerializer()),
            };
        }

        [Serializable]
        public class PropertyBagConfigForInheritTypeBase : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag => new TypeToRegisterForPropertyBag[]
            {
                typeof(ComplicatedObject).ToTypeToRegisterForPropertyBag(),
                typeof(InheritTypeBase).ToTypeToRegisterForPropertyBag(MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new InheritTestSerializer()),
                typeof(InheritTypeDerive).ToTypeToRegisterForPropertyBag(MemberTypesToInclude.None, RelatedTypesToInclude.None, () => new InheritTestSerializer()),
            };
        }

        [Serializable]
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
        public class TypeWithCustomPropertyBagSerializerWrapper
        {
            public TypeWithCustomPropertyBagSerializer CustomTypeWrapper { get; set; }
        }

        [Serializable]
        public class ComplicatedObject
        {
            public decimal? NullableDecimal { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public decimal? NullableDecimalDefault { get; set; }

            public InheritTypeBase BaseVersion { get; set; }

            public InheritTypeDerive DeriveVersion { get; set; }

            public InheritTypeDerive[] DeriveVersionArray { get; set; }

            public IReadOnlyCollection<InheritTypeDerive> DeriveVersionCollection { get; set; }

            public string String { get; set; }

            public int Int { get; set; }

            public TimeSpan TimeSpan { get; set; }

            public DateTime DateTime { get; set; }

            public DateTime? DateTimeNullable { get; set; }

            public CustomWithoutInterface CustomWithoutInterface { get; set; }

            public CustomWithInterface CustomWithInterface { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public string StringDefault { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public int IntDefault { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public TimeSpan TimeSpanDefault { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public DateTime DateTimeDefault { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public DateTime? DateTimeNullableDefault { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public CustomWithoutInterface CustomWithoutInterfaceDefault { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public CustomWithInterface CustomWithInterfaceDefault { get; set; }

            public string[] StringArray { get; set; }

            public IEnumerable<string> StringCollection { get; set; }

            public IReadOnlyCollection<int> IntCollection { get; set; }

            public ICollection<TimeSpan> TimeSpanCollection { get; set; }

            public IList<DateTime> DateTimeCollection { get; set; }

            public IReadOnlyList<CustomWithoutInterface> CustomWithoutInterfaceCollection { get; set; }

            public IEnumerable<CustomWithInterface> CustomWithInterfaceCollection { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public EnumParse EnumDefault { get; set; }

            public EnumParse EnumParse { get; set; }

            public IReadOnlyCollection<EnumParse> EnumParseCollection { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for default value testing.")]
            public IReadOnlyCollection<string> StringCollectionDefault { get; set; }

            public IReadOnlyCollection<string> StringCollectionWithSingleEmptyString { get; set; }

            public IReadOnlyCollection<string> StringCollectionWithNulls { get; set; }
        }

        public enum EnumParse { Default, Value, }

        public enum EnumAttributeProperty { Default, Value, Replaced, }

        [Serializable]
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
        public class HasSerializesWithComma
        {
            public IReadOnlyCollection<SerializesWithComma> WithCommas { get; set; }
        }

        [Serializable]
        public class SerializesWithComma
        {
            public const string CustomToString = "This is my tostring with a , comma...";

            public override string ToString()
            {
                return CustomToString;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for reflection call.")]
            public static SerializesWithComma Parse(string input)
            {
                new { input }.AsArg().Must().BeEqualTo(CustomToString);
                return new SerializesWithComma();
            }
        }

        [Serializable]
        public class CustomWithoutInterface
        {
            public const string CustomToString = "This is my default tostring.";

            public override string ToString()
            {
                return CustomToString;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping for reflection call.")]
            public static CustomWithoutInterface Parse(string input)
            {
                new { input }.AsArg().Must().BeEqualTo(CustomToString);
                return new CustomWithoutInterface();
            }
        }

        [Serializable]
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

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Keeping.")]
            public static CustomWithInterface Parse(string input)
            {
                new { input }.AsArg().Must().BeEqualTo(CustomSerializedString);
                return new CustomWithInterface();
            }
        }

        [Serializable]
        public abstract class InheritTypeBase
        {
            public override string ToString()
            {
                return this.GetType().Name;
            }
        }

        [Serializable]
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
