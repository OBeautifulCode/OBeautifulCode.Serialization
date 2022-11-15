// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using Xunit;

    public static class SerializationConfigurationBaseTest
    {
        [Fact]
        public static void Two_Dependent_Configurations_Have_Same_Registered_Type_Via_Post_Initialization_Registrations()
        {
            // Arrange
            var serializer1 = new ObcJsonSerializer<TestConfig1>();
            serializer1.SerializeToString(new TestEvent1(1, DateTime.UtcNow));

            var serializer2 = new ObcJsonSerializer<TestConfig2>();
            serializer2.SerializeToString(new TestEvent2(2, DateTime.UtcNow));

            // Act
            var actual = Record.Exception(() => new ObcJsonSerializer<DependencyOnlyJsonSerializationConfiguration<TestConfig1, TestConfig2>>());

            // Assert
            actual.AsTest().Must().BeNull();
        }
    }

    public class TestConfig1 : JsonSerializationConfigurationBase
    {
        /// <inheritdoc cref="JsonSerializationConfigurationBase" />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
        {
            typeof(TestEvent1).ToTypeToRegisterForJson(),
        };
    }

    public class TestConfig2 : JsonSerializationConfigurationBase
    {
        /// <inheritdoc cref="JsonSerializationConfigurationBase" />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
        {
            typeof(TestEvent2).ToTypeToRegisterForJson(),
        };
    }

    public class TestEvent1 : EventBase<short>
    {
        public TestEvent1(short id, DateTime timestampUtc)
            : base(id, timestampUtc)
        {
        }
    }

    public class TestEvent2 : EventBase<short>
    {
        public TestEvent2(short id, DateTime timestampUtc)
            : base(id, timestampUtc)
        {
        }
    }
}