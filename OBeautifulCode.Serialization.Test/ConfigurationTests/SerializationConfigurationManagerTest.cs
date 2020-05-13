// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationManagerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FluentAssertions;
    using MongoDB.Bson;
    using OBeautifulCode.Serialization.Bson;

    using Xunit;

    public static class SerializationConfigurationManagerTest
    {
        [Fact]
        public static void Configure___Valid_type_as_generic___Works()
        {
            SerializationConfigurationManager.GetOrAddSerializationConfiguration(typeof(TestConfigure).ToBsonSerializationConfigurationType());
            TestConfigure.Configured.Should().BeTrue();
        }
    }

    public class TestConfigure : BsonSerializationConfigurationBase
    {
        public static bool Configured { get; private set; }

        protected override void FinalizeInitialization()
        {
            if (Configured)
            {
                throw new NotSupportedException("Configuration is not reentrant and should not have been called a second time.");
            }

            Configured = true;
        }
    }
}
