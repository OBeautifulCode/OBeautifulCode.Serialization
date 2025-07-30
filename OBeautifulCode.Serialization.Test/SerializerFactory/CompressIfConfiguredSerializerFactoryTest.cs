// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompressIfConfiguredSerializerFactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Compression.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class CompressIfConfiguredSerializerFactoryTest
    {
        [Fact]
        public static void Constructor___Should_throw_ArgumentNullException___When_parameter_backingSerializer_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new CompressIfConfiguredSerializerFactory(null, OBeautifulCode.Compression.Recipes.CompressorFactory.Instance));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("backingSerializer");
        }

        [Fact]
        public static void Constructor___Should_throw_ArgumentNullException___When_parameter_compressorFactory_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new CompressIfConfiguredSerializerFactory(new TestSerializerFactory(), null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("compressorFactory");
        }

        [Fact]
        public static void BuildSerializer___Should_throw_ArgumentNullException___When_parameter_serializerRepresentation_is_null()
        {
            // Arrange
            var subjectUnderTest = new CompressIfConfiguredSerializerFactory(new TestSerializerFactory(), OBeautifulCode.Compression.Recipes.CompressorFactory.Instance);

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(null, A.Dummy<VersionMatchStrategy>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("serializerRepresentation");
        }

        [Fact]
        public static void BuildSerializer___Should_return_serializer_returned_by_BackingSerializerFactory___When_serializerRepresentation_CompressionKind_is_None()
        {
            // Arrange
            var subjectUnderTest = new CompressIfConfiguredSerializerFactory(new TestSerializerFactory(), OBeautifulCode.Compression.Recipes.CompressorFactory.Instance);
            var expected1 = TestSerializerFactory.BuildSerializerTestParametersAndResult1.ResultFunc().SerializeToString(new object());
            var expected2 = TestSerializerFactory.BuildSerializerTestParametersAndResult2.ResultFunc().SerializeToString(new object());

            // Act
            var actual1 = subjectUnderTest.BuildSerializer(TestSerializerFactory.BuildSerializerTestParametersAndResult1.SerializerRepresentation, TestSerializerFactory.BuildSerializerTestParametersAndResult1.AssemblyVersionMatchStrategy).SerializeToString(new object());
            var actual2 = subjectUnderTest.BuildSerializer(TestSerializerFactory.BuildSerializerTestParametersAndResult2.SerializerRepresentation, TestSerializerFactory.BuildSerializerTestParametersAndResult2.AssemblyVersionMatchStrategy).SerializeToString(new object());

            // Assert
            actual1.AsTest().Must().BeEqualTo(expected1);
            actual2.AsTest().Must().BeEqualTo(expected2);
        }

        [Fact]
        public static void BuildSerializer___Should_return_ObcCompressingSerializer_wrapping_serializer_returned_by_BackingSerializerFactory_with_compression_kind_stripped_off_SerializerRepresentation___When_serializerRepresentation_CompressionKind_is_DotNetZip()
        {
            // Arrange
            var subjectUnderTest = new CompressIfConfiguredSerializerFactory(new TestSerializerFactory(), OBeautifulCode.Compression.Recipes.CompressorFactory.Instance);
            var expectedSerializedObject1 = TestSerializerFactory.BuildSerializerTestParametersAndResult5.ResultFunc().SerializeToString(new object());
            var expectedSerializedObject2 = TestSerializerFactory.BuildSerializerTestParametersAndResult6.ResultFunc().SerializeToString(new object());

            // Act
            var actualSerializer1 = subjectUnderTest.BuildSerializer(TestSerializerFactory.BuildSerializerTestParametersAndResult3.SerializerRepresentation, TestSerializerFactory.BuildSerializerTestParametersAndResult3.AssemblyVersionMatchStrategy);
            var actualSerializer2 = subjectUnderTest.BuildSerializer(TestSerializerFactory.BuildSerializerTestParametersAndResult4.SerializerRepresentation, TestSerializerFactory.BuildSerializerTestParametersAndResult4.AssemblyVersionMatchStrategy);

            // Assert
            actualSerializer1.AsTest().Must().BeOfType<ObcCompressingSerializer>();
            actualSerializer2.AsTest().Must().BeOfType<ObcCompressingSerializer>();

            var compressingSerializer1 = (ObcCompressingSerializer)actualSerializer1;
            var compressingSerializer2 = (ObcCompressingSerializer)actualSerializer2;

            compressingSerializer1.Compressor.Must().BeOfType<DotNetZipCompressor>();
            compressingSerializer2.Compressor.Must().BeOfType<DotNetZipCompressor>();

            var actualSerializedObject1 = compressingSerializer1.BackingSerializer.SerializeToString(new object());
            var actualSerializedObject2 = compressingSerializer2.BackingSerializer.SerializeToString(new object());
            actualSerializedObject1.AsTest().Must().BeEqualTo(expectedSerializedObject1);
            actualSerializedObject2.AsTest().Must().BeEqualTo(expectedSerializedObject2);
        }
    }
}