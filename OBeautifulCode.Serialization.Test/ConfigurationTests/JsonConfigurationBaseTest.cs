﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConfigurationBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Newtonsoft.Json.Serialization;

    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class JsonConfigurationBaseTest
    {
        [Fact]
        public static void JsonConfigurationBase___With_contract_override___Works()
        {
            // Arrange & Act
            var actual = SerializationConfigurationManager.ConfigureWithReturn<DefaultTestConfiguration>();

            // Assert
            actual.Should().NotBeNull();
            actual.BuildJsonSerializerSettings(SerializationDirection.Serialize).ContractResolver.Should().BeOfType<DefaultContractResolver>();
            actual.BuildJsonSerializerSettings(SerializationDirection.Deserialize).ContractResolver.Should().BeOfType<DefaultContractResolver>();
        }

        [Fact]
        public static void JsonConfigurationBase___With_null_implementation___Works()
        {
            // Arrange & Act
            var actual = SerializationConfigurationManager.ConfigureWithReturn<NullJsonConfiguration>();

            // Assert
            actual.Should().NotBeNull();
            actual.BuildJsonSerializerSettings(SerializationDirection.Serialize).ContractResolver.GetType().FullName.Should().Be("OBeautifulCode.Serialization.Json.CamelStrictConstructorContractResolver"); // this type is not public so we can't use nameof()
            actual.BuildJsonSerializerSettings(SerializationDirection.Deserialize).ContractResolver.GetType().FullName.Should().Be("OBeautifulCode.Serialization.Json.CamelStrictConstructorContractResolver"); // this type is not public so we can't use nameof()
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called via reflection.")]
    internal class DefaultTestConfiguration : JsonConfigurationBase
    {
        protected override IReadOnlyDictionary<SerializationDirection, RegisteredContractResolver> OverrideContractResolver =>
            new Dictionary<SerializationDirection, RegisteredContractResolver>
            {
                { SerializationDirection.Serialize, new RegisteredContractResolver(() => new DefaultContractResolver()) },
                { SerializationDirection.Deserialize, new RegisteredContractResolver(() => new DefaultContractResolver()) },
            };
    }
}