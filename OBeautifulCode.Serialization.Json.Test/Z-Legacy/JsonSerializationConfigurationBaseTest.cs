// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;

    using Newtonsoft.Json.Serialization;

    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class JsonSerializationConfigurationBaseTest
    {
        [Fact]
        public static void JsonSerializationConfigurationBase___With_contract_override___Works()
        {
            // Arrange & Act
            var actual = (DefaultTestConfiguration)SerializationConfigurationManager.GetOrAddSerializationConfiguration(typeof(DefaultTestConfiguration).ToJsonSerializationConfigurationType());

            // Assert
            actual.Should().NotBeNull();
            actual.BuildJsonSerializerSettings(SerializationDirection.Serialize, actual).ContractResolver.Should().BeOfType<DefaultContractResolver>();
            actual.BuildJsonSerializerSettings(SerializationDirection.Deserialize, actual).ContractResolver.Should().BeOfType<DefaultContractResolver>();
        }

        [Fact]
        public static void JsonSerializationConfigurationBase___With_null_implementation___Works()
        {
            // Arrange & Act
            var actual = (NullJsonSerializationConfiguration)SerializationConfigurationManager.GetOrAddSerializationConfiguration(typeof(NullJsonSerializationConfiguration).ToJsonSerializationConfigurationType());

            // Assert
            actual.Should().NotBeNull();
            actual.BuildJsonSerializerSettings(SerializationDirection.Serialize, actual).ContractResolver.GetType().FullName.Should().Be("OBeautifulCode.Serialization.Json.CamelStrictConstructorContractResolver"); // this type is not public so we can't use nameof()
            actual.BuildJsonSerializerSettings(SerializationDirection.Deserialize, actual).ContractResolver.GetType().FullName.Should().Be("OBeautifulCode.Serialization.Json.CamelStrictConstructorContractResolver"); // this type is not public so we can't use nameof()
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Called via reflection.")]
    internal class DefaultTestConfiguration : JsonSerializationConfigurationBase
    {
        public override IReadOnlyDictionary<SerializationDirection, RegisteredContractResolver> OverrideContractResolver =>
            new Dictionary<SerializationDirection, RegisteredContractResolver>
            {
                { SerializationDirection.Serialize, new RegisteredContractResolver(_ => new DefaultContractResolver()) },
                { SerializationDirection.Deserialize, new RegisteredContractResolver(_ => new DefaultContractResolver()) },
            };
    }
}
