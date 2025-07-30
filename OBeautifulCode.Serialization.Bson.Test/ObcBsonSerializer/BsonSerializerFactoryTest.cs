// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializerFactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
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

    public static class BsonSerializerFactoryTest
    {
        [Fact]
        public static void BuildSerializer___Should_throw_ArgumentNullException___When_parameter_serializerRepresentation_is_null()
        {
            // Arrange
            var subjectUnderTest = new BsonSerializerFactory();

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
            var subjectUnderTest = new BsonSerializerFactory();

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(
                A.Dummy<SerializerRepresentation>().Whose(_ => _.CompressionKind != CompressionKind.None),
                A.Dummy<VersionMatchStrategy>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation.CompressionKind");
        }

        [Fact]
        public static void BuildSerializer___Should_throw_NotSupportedException___When_parameter_serializerRepresentation_SerializationKind_is_not_Bson()
        {
            // Arrange
            var subjectUnderTest = new BsonSerializerFactory();

            var serializerRepresentation = new SerializerRepresentation(
                A.Dummy<SerializationKind>().ThatIsNot(SerializationKind.Bson));

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(serializerRepresentation));

            // Assert
            actual.AsTest().Must().BeOfType<NotSupportedException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation from enumeration SerializationKind");
        }

        [Fact]
        public static void BuildSerializer___Should_return_ObcBsonSerializer___When_called()
        {
            // Arrange
            var subjectUnderTest = new BsonSerializerFactory();

            var configType = typeof(TypesToRegisterBsonSerializationConfiguration<TestClass>);

            var serializerRepresentation = new SerializerRepresentation(
                SerializationKind.Bson,
                configType.ToRepresentation());

            // Act
            var actual = subjectUnderTest.BuildSerializer(serializerRepresentation);

            // Assert
            actual.AsTest().Must().BeOfType<ObcBsonSerializer>();
            ((ObcBsonSerializer)actual).SerializationConfigurationType.AsTest().Must().BeEqualTo((SerializationConfigurationType)new BsonSerializationConfigurationType(configType));
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = ObcSuppressBecause.CA1812_AvoidUninstantiatedInternalClasses_ClassExistsToUseItsTypeInUnitTests)]
        private class TestClass
        {
        }
    }
}