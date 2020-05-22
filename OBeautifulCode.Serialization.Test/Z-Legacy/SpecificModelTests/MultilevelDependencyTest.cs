// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultilevelDependencyTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class MultilevelDependencyTest
    {
        [Fact]
        public static void RootFirstWorks()
        {
            var middleSerializer = new ObcJsonSerializer(typeof(ConfigMiddle).ToJsonSerializationConfigurationType());
            middleSerializer.SerializeToString(null);

            var topSerializer = new ObcJsonSerializer(typeof(ConfigTop).ToJsonSerializationConfigurationType());
            topSerializer.SerializeToString(null);
        }
    }

    public class ConfigTop : JsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new[] { typeof(ConfigMiddle).ToJsonSerializationConfigurationType() };
    }

    public class ConfigMiddle : JsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<JsonSerializationConfigurationType> DependentJsonSerializationConfigurationTypes => new[] { typeof(ConfigBottom1).ToJsonSerializationConfigurationType(), typeof(ConfigBottom2).ToJsonSerializationConfigurationType(), };
    }

    public class ConfigBottom1 : JsonSerializationConfigurationBase
    {
    }

    public class ConfigBottom2 : JsonSerializationConfigurationBase
    {
    }
}
