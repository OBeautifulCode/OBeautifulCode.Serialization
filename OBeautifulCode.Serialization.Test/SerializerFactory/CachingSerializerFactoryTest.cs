// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachingSerializerFactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Linq;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class CachingSerializerFactoryTest
    {
        [Fact]
        public static void Constructor___Should_throw_ArgumentNullException___When_parameter_backingSerializer_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new CachingSerializerFactory(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("backingSerializer");
        }

        [Fact]
        public static void BuildSerializer___Should_throw_ArgumentNullException___When_parameter_serializerRepresentation_is_null()
        {
            // Arrange
            var subjectUnderTest = new CachingSerializerFactory(new TestSerializerFactory());

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(null, A.Dummy<VersionMatchStrategy>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation");
        }

        [Fact]
        public static void BuildSerializer___Should_return_serializer_returned_by_BackingSerializerFactory___When_called()
        {
            // Arrange
            var subjectUnderTest = new CachingSerializerFactory(new TestSerializerFactory());
            var expected = TestSerializerFactory.BuildSerializerTestParametersAndResults.Select(_ => _.SerializerId).ToList();

            // Act
            var actual = TestSerializerFactory.BuildSerializerTestParametersAndResults
                .Select(_ => subjectUnderTest.BuildSerializer(_.SerializerRepresentation, _.AssemblyVersionMatchStrategy))
                .Select(_ => _.SerializeToString(new object()))
                .ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void BuildSerializer___Should_return_cached_serializer___When_called()
        {
            // Arrange
            var subjectUnderTest = new CachingSerializerFactory(new TestSerializerFactory());
            var expected = TestSerializerFactory.BuildSerializerTestParametersAndResults
                .Select(_ => subjectUnderTest.BuildSerializer(_.SerializerRepresentation, _.AssemblyVersionMatchStrategy))
                .ToList();

            // Act
            var actual = TestSerializerFactory.BuildSerializerTestParametersAndResults
                .Select(_ => subjectUnderTest.BuildSerializer(_.SerializerRepresentation, _.AssemblyVersionMatchStrategy))
                .ToList();

            // Assert
            // serializers are not IEquatable so this will do a reference equality check:
            actual.AsTest().Must().BeEqualTo(expected);
        }
    }
}