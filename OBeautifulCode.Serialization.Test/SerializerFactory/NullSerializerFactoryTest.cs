// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSerializerFactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class NullSerializerFactoryTest
    {
        [Fact]
        public static void BuildSerializer___Should_return_ObcNullSerializer___When_called()
        {
            // Arrange
            var subjectUnderTest = new NullSerializerFactory();

            // Act
            var actual = subjectUnderTest.BuildSerializer(A.Dummy<SerializerRepresentation>(), A.Dummy<VersionMatchStrategy>());

            // Assert
            actual.AsTest().Must().BeOfType<ObcNullSerializer>();
        }
    }
}