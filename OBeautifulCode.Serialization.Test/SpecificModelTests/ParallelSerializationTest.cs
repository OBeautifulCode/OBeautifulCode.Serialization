// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParallelSerializationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;

    using Xunit;

    using static System.FormattableString;

    public static class ParallelSerializationTest
    {
        [Fact(Skip = "Long running")]
        public static void TestDictionaryMixedKeyValues()
        {
            var tasks = Enumerable.Range(1, 10000).Select(_ => A.Dummy<TestDictionaryMixedKeyValues>())
                .Select(_ => new Task(() =>
                {
                    var serializer = new ObcJsonSerializer(typeof(GenericDiscoveryJsonConfiguration<TestDictionaryMixedKeyValues>));
                    serializer.SerializeToString(_);
                })).ToArray();
            Parallel.ForEach(tasks, _ => _.Start());
            Task.WaitAll(tasks);
        }

        [Fact(Skip = "Long running")]
        public static void TestBaseOther()
        {
            var tasks = Enumerable.Range(1, 10000).Select(_ => A.Dummy<TestBase>())
                .Select(_ => new Task(() =>
                {
                    var serializer = new ObcJsonSerializer(typeof(GenericDiscoveryJsonConfiguration<TestBase>));
                    serializer.SerializeToString(_);
                })).ToArray();
            Parallel.ForEach(tasks, _ => _.Start());
            Task.WaitAll(tasks);
        }

        [Fact(Skip = "Long running")]
        public static void TestBase()
        {
            var serializer = new ObcJsonSerializer(typeof(GenericDiscoveryJsonConfiguration<TestBase>));
            var tasks = Enumerable.Range(1, 100).Select(_ => A.Dummy<TestBase>())
                .Select(_ => new Task(() => serializer.SerializeToString(_))).ToArray();
            Parallel.ForEach(tasks, _ => _.Start());
            Task.WaitAll(tasks);
        }
    }
}
