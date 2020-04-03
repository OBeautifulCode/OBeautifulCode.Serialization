// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationManagerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Bson;

    using Xunit;

    public static class SerializationConfigurationManagerTest
    {
        [Fact]
        public static void Configure___Type_not_BsonSerializationConfigurationBase___Throws()
        {
            // Arrange
            Action action = () => SerializationConfigurationManager.Configure(typeof(string));

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Provided value (name: 'typeMustBeSubclassOfSerializationConfigurationBase') is not true.  Provided value is 'False'.");
        }

        [Fact]
        public static void Configure___Type_does_not_have_default_constructor___Throws()
        {
            // Arrange
            Action action = () => SerializationConfigurationManager.Configure(typeof(TestConfigureParameterConstructor));

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Provided value (name: 'typeHasParameterLessConstructor') is not true.  Provided value is 'False'.");
        }

        [Fact]
        public static void Configure___Valid_type_as_generic___Works()
        {
            SerializationConfigurationManager.Configure<TestConfigure>();
            TestConfigure.Configured.Should().BeTrue();
        }
    }

    public class TestConfigure : BsonSerializationConfigurationBase
    {
        /// <summary>
        /// Gets a value indicating whether or not it has been configured.
        /// </summary>
        public static bool Configured { get; private set; }

        /// <inheritdoc cref="BsonSerializationConfigurationBase" />
        protected override void FinalConfiguration()
        {
            if (Configured)
            {
                throw new NotSupportedException("Configuration is not reentrant and should not have been called a second time.");
            }

            Configured = true;
        }
    }

    public class TestConfigureParameterConstructor : BsonSerializationConfigurationBase
    {
        public TestConfigureParameterConstructor(string thingy)
        {
            this.Thingy = thingy;
        }

        public string Thingy { get; set; }

        /// <inheritdoc cref="BsonSerializationConfigurationBase" />
        protected override void FinalConfiguration()
        {
            /* no-op */
        }
    }
}
