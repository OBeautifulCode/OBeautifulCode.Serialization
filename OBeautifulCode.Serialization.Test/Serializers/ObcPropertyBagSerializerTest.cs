// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.PropertyBag;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "RoundTrip", Justification = "Name/spelling is correct.")]
        [Fact]
        public static void RoundTrip___Supported_scenarios___Works()
        {
            // Arrange
            var serializer = new ObcPropertyBagSerializer();
            var input = new ComplicatedObject
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

            // Act
            var serializedPropertyBag = serializer.SerializeToPropertyBag(input);
            var serializedString = serializer.SerializeToString(input);
            var serializedBytes = serializer.SerializeToBytes(input);

            var actualPropertyBag = serializer.Deserialize<ComplicatedObject>(serializedPropertyBag);
            var actualString = serializer.Deserialize<ComplicatedObject>(serializedString);
            var actualBytes = serializer.Deserialize<ComplicatedObject>(serializedBytes);

            // Assert
            void AssertCorrect(ComplicatedObject actual)
            {
                actual.NullableDecimal.Should().Be(input.NullableDecimal);
                actual.NullableDecimalDefault.Should().BeNull();
                actual.BaseVersion.Should().NotBeNull();
                actual.DeriveVersion.Should().NotBeNull();
                actual.DeriveVersionArray.Count().Should().Be(input.DeriveVersionArray.Count());
                actual.DeriveVersionCollection.Count().Should().Be(input.DeriveVersionCollection.Count());
                actual.String.Should().Be(input.String);
                actual.Int.Should().Be(input.Int);
                actual.TimeSpan.Should().Be(input.TimeSpan);
                actual.DateTime.Should().Be(input.DateTime);
                actual.DateTimeNullable.Should().Be(input.DateTimeNullable);
                actual.CustomWithoutInterface.Should().NotBeNull();
                actual.CustomWithInterface.Should().NotBeNull();
                actual.StringDefault.Should().BeNull();
                actual.IntDefault.Should().Be(default(int));
                actual.TimeSpanDefault.Should().Be(default(TimeSpan));
                actual.DateTimeDefault.Should().Be(default(DateTime));
                actual.DateTimeNullableDefault.Should().BeNull();
                actual.CustomWithoutInterfaceDefault.Should().BeNull();
                actual.CustomWithInterfaceDefault.Should().BeNull();
                actual.StringArray.Should().Equal(input.StringArray);
                actual.StringCollection.Should().Equal(input.StringCollection);
                actual.IntCollection.Should().Equal(input.IntCollection);
                actual.TimeSpanCollection.Should().Equal(input.TimeSpanCollection);
                actual.DateTimeCollection.Should().Equal(input.DateTimeCollection);
                actual.CustomWithoutInterfaceCollection.Should().HaveCount(input.CustomWithoutInterfaceCollection.Count());
                actual.CustomWithInterfaceCollection.Should().HaveCount(input.CustomWithInterfaceCollection.Count());
                actual.EnumParseCollection.Should().Equal(input.EnumParseCollection);
                actual.EnumDefault.Should().Be(EnumParse.Default);
                actual.EnumParse.Should().Be(input.EnumParse);
                actual.StringCollectionDefault.Should().BeNull();
                actual.StringCollectionWithSingleEmptyString.Should().Equal(input.StringCollectionWithSingleEmptyString);
                actual.StringCollectionWithNulls.Should().Equal(input.StringCollectionWithNulls);
            }

            AssertCorrect(actualPropertyBag);
            AssertCorrect(actualString);
            AssertCorrect(actualBytes);
        }

        [Fact]
        public static void Deserializing_constructors___When_properties_exist___Works()
        {
            // Arrange
            var serializer = new ObcPropertyBagSerializer();
            var input = new ConstructorWithProperties(A.Dummy<string>(), A.Dummy<string>(), A.Dummy<string>());

            // Act
            var serializedString = serializer.SerializeToString(input);
            var actual = serializer.Deserialize<ConstructorWithProperties>(serializedString);

            // Act
            actual.PropertyGetOnly.Should().Be(input.PropertyGetOnly);
            actual.PropertyPrivateSet.Should().Be(input.PropertyPrivateSet);
            actual.PropertyPublicSet.Should().Be(input.PropertyPublicSet);
        }

        [Fact]
        public static void Deserializing_collections___When_serialized_string_has_a_comma___Works()
        {
            // Arrange
            var serializer = new ObcPropertyBagSerializer();
            var input = new HasSerializesWithComma { WithCommas = new[] { new SerializesWithComma(), new SerializesWithComma() }.ToList() };

            // Act
            var serializedString = serializer.SerializeToString(input);
            var actual = serializer.Deserialize<HasSerializesWithComma>(serializedString);

            // Act
            actual.WithCommas.Count.Should().Be(input.WithCommas.Count);
            actual.WithCommas.ToList().ForEach(_ => _.Should().NotBeNull());
        }

        [Fact]
        public static void Deserializing_constructors___When_properties_do_not_exist___Throws()
        {
            // Arrange
            var serializer = new ObcPropertyBagSerializer();
            var input = new ConstructorWithoutProperties(A.Dummy<string>(), A.Dummy<string>());

            // Act
            var serializedString = serializer.SerializeToString(input);
            var exception = Record.Exception(() => serializer.Deserialize<ConstructorWithProperties>(serializedString));

            // Act
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Could not find a parameterless constructor or a constructor whose parameter names matched the properties provided; type: OBeautifulCode.Serialization.Test.ObcPropertyBagSerializerTest+ConstructorWithoutProperties, properties: Property,ToString,GetType.");
        }

        [Fact]
        public static void Configuration___Specifying_type___Works()
        {
            // Arrange
            var configurationType = typeof(PropertyBagConfig);
            var serializer = new ObcPropertyBagSerializer<PropertyBagConfig>(UnregisteredTypeEncounteredStrategy.Attempt);
            var input = new StringProperty { StringItem = A.Dummy<string>() };

            // Act
            var serializedString = serializer.SerializeToString(input);
            var actual = serializer.Deserialize<StringProperty>(serializedString);

            // Act
        }

        private class ConstructorWithProperties
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

        private class ConstructorWithoutProperties
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "propertyDoesNotExist", Justification = "Here to test reflection.")]
            public ConstructorWithoutProperties(string property, string propertyDoesNotExist)
            {
                this.Property = property;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Here to test reflection.")]
            public string Property { get; }
        }

        private class PropertyBagConfig : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new[] { typeof(PropertyBagConfigDepend).ToPropertyBagSerializationConfigurationType() };
        }

        private class PropertyBagConfigDepend : PropertyBagSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<RegisteredStringSerializer> SerializersToRegister =>
                new[]
                {
                    new RegisteredStringSerializer(() => new CustomStringSerializer(), new[] { typeof(string) }),
                };
        }

        private class StringProperty
        {
            public string StringItem { get; set; }
        }

        private class ComplicatedObject
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

        private class CustomStringSerializer : IStringSerializeAndDeserialize
        {
            public const string CustomReplacementString = "We have a string overwriting.";

            public const string CustomSerializedString = "We have a string overwriting serializer attribute.";

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
                new { type }.AsArg().Must().BeEqualTo(typeof(string));

                return CustomReplacementString;
            }
        }

        private class CustomElement
        {
        }

        private class CustomElementSerializer : IStringSerializeAndDeserialize
        {
            public const string CustomSerializedString = "Array of these.";

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
                new { type }.AsArg().Must().BeEqualTo(typeof(CustomElement));

                return new CustomElement();
            }
        }

        private class HasSerializesWithComma
        {
            public IReadOnlyCollection<SerializesWithComma> WithCommas { get; set; }
        }

        private class SerializesWithComma
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

        private class CustomWithoutInterface
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

        private class CustomWithInterface : ISerializeToString
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

        private abstract class InheritTypeBase
        {
            public override string ToString()
            {
                return this.GetType().Name;
            }
        }

        private class InheritTypeDerive : InheritTypeBase
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
