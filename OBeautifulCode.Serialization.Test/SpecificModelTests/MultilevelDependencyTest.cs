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
            var middleSerializer = new ObcJsonSerializer(typeof(ConfigMiddle));
            middleSerializer.SerializeToString(null);

            var topSerializer = new ObcJsonSerializer(typeof(ConfigTop));
            topSerializer.SerializeToString(null);
        }
    }

    public class ConfigTop : JsonSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentConfigurationTypes => new[] { typeof(ConfigMiddle) };
    }

    public class ConfigMiddle : JsonSerializationConfigurationBase
    {
        public override IReadOnlyCollection<Type> DependentConfigurationTypes => new[] { typeof(ConfigBottom1), typeof(ConfigBottom2), };
    }

    public class ConfigBottom1 : JsonSerializationConfigurationBase
    {
    }

    public class ConfigBottom2 : JsonSerializationConfigurationBase
    {
    }
}
