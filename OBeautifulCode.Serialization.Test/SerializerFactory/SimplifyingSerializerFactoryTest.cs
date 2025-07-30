// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimplifyingSerializerFactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Linq;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class SimplifyingSerializerFactoryTest
    {
        [Fact]
        public static void Constructor___Should_throw_ArgumentNullException___When_parameter_fallbackSerializer_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new SimplifyingSerializerFactory(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("fallbackSerializer");
        }

        [Fact]
        public static void BuildSerializer___Should_throw_ArgumentNullException___When_parameter_serializerRepresentation_is_null()
        {
            // Arrange
            var subjectUnderTest = new SimplifyingSerializerFactory(new TestSerializerFactory());

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(null, A.Dummy<VersionMatchStrategy>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation");
        }

        [Fact]
        public static void BuildSerializer___Should_return_ObcSimplifyingSerializer_wrapping_serializer_returned_by_FallbackSerializerFactory___When_called()
        {
            // Arrange
            var subjectUnderTest = new SimplifyingSerializerFactory(new TestSerializerFactory());
            var expectedSerializedObjects = TestSerializerFactory.BuildSerializerTestParametersAndResults.Select(_ => _.SerializerId).ToList();

            // Act
            var actual = TestSerializerFactory.BuildSerializerTestParametersAndResults
                .Select(_ => subjectUnderTest.BuildSerializer(_.SerializerRepresentation, _.AssemblyVersionMatchStrategy))
                .ToList();

            // Assert
            actual.AsTest().Must().Each().BeOfType<ObcSimplifyingSerializer>();
            var actualSerializedObjects = actual.Cast<ObcSimplifyingSerializer>().Select(_ => _.FallbackSerializer.SerializeToString(new object())).ToList();
            actualSerializedObjects.AsTest().Must().BeEqualTo(expectedSerializedObjects);
        }
    }
}