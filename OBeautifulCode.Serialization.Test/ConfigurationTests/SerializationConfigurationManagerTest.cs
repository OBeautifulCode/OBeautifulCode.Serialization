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
        public static void Configure___Valid_type_as_generic___Works()
        {
            SerializationConfigurationManager.GetOrAddSerializationConfiguration<TestConfigure>();
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
}
