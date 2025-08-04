// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlwaysThrowingSerializerFactoryTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using FakeItEasy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class AlwaysThrowingSerializerFactoryTest
    {
        [Fact]
        public static void BuildSerializer___Should_throw_NotSupportedException___When_called()
        {
            // Arrange
            var subjectUnderTest = new AlwaysThrowingSerializerFactory();

            // Act
            var actual = Record.Exception(() => subjectUnderTest.BuildSerializer(A.Dummy<SerializerRepresentation>(), A.Dummy<VersionMatchStrategy>()));

            // Assert
            actual.AsTest().Must().BeOfType<NotSupportedException>();
        }
    }
}