// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializerFactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using Xunit;

    public static class JsonSerializerFactoryTest
    {
        [Fact]
        public static void BuildSerializer___Should_throw_ArgumentNullException___When_parameter_serializerRepresentation_is_null()
        {
            // Arrange
            var subjectUnderTest = new JsonSerializerFactory();

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(null, A.Dummy<VersionMatchStrategy>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation");
        }

        [Fact]
        public static void BuildSerializer___Should_throw_ArgumentOutOfRangeException___When_parameter_serializerRepresentation_CompressionKind_is_not_CompressionKind_None()
        {
            // Arrange
            var subjectUnderTest = new JsonSerializerFactory();

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(
                A.Dummy<SerializerRepresentation>().Whose(_ => _.CompressionKind != CompressionKind.None),
                A.Dummy<VersionMatchStrategy>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation.CompressionKind");
        }

        [Fact]
        public static void BuildSerializer___Should_throw_NotSupportedException___When_parameter_serializerRepresentation_SerializationKind_is_not_Json()
        {
            // Arrange
            var subjectUnderTest = new JsonSerializerFactory();

            var serializerRepresentation = new SerializerRepresentation(
                A.Dummy<SerializationKind>().ThatIsNot(SerializationKind.Json));

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(serializerRepresentation));

            // Assert
            actual.AsTest().Must().BeOfType<NotSupportedException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation from enumeration SerializationKind");
        }

        [Fact]
        public static void BuildSerializer___Should_return_ObcJsonSerializer___When_called()
        {
            // Arrange
            var subjectUnderTest = new JsonSerializerFactory();

            var configType = typeof(TypesToRegisterJsonSerializationConfiguration<TestClass>);

            var serializerRepresentation = new SerializerRepresentation(
                SerializationKind.Json,
                configType.ToRepresentation());

            // Act
            var actual = subjectUnderTest.BuildSerializer(serializerRepresentation);

            // Assert
            actual.AsTest().Must().BeOfType<ObcJsonSerializer>();
            ((ObcJsonSerializer)actual).SerializationConfigurationType.AsTest().Must().BeEqualTo((SerializationConfigurationType)new JsonSerializationConfigurationType(configType));
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = ObcSuppressBecause.CA1812_AvoidUninstantiatedInternalClasses_ClassExistsToUseItsTypeInUnitTests)]
        private class TestClass
        {
        }
    }
}