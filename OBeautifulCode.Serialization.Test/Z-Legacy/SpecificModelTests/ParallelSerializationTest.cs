﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParallelSerializationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System.Linq;
    using System.Threading.Tasks;

    using FakeItEasy;

    using OBeautifulCode.Serialization.Json;

    using Xunit;

    public static class ParallelSerializationTest
    {
        [Fact(Skip = "Long running")]
        public static void TestDictionaryMixedKeyValues()
        {
            var tasks = Enumerable.Range(1, 10000).Select(_ => A.Dummy<TestDictionaryMixedKeyValues>())
                .Select(_ => new Task(() =>
                {
                    var serializer = new ObcJsonSerializer(typeof(TypesToRegisterJsonSerializationConfiguration<TestDictionaryMixedKeyValues>).ToJsonSerializationConfigurationType());
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
                    var serializer = new ObcJsonSerializer(typeof(TypesToRegisterJsonSerializationConfiguration<TestBase>).ToJsonSerializationConfigurationType());
                    serializer.SerializeToString(_);
                })).ToArray();
            Parallel.ForEach(tasks, _ => _.Start());
            Task.WaitAll(tasks);
        }

        [Fact(Skip = "Long running")]
        public static void TestBase()
        {
            var serializer = new ObcJsonSerializer(typeof(TypesToRegisterJsonSerializationConfiguration<TestBase>).ToJsonSerializationConfigurationType());
            var tasks = Enumerable.Range(1, 100).Select(_ => A.Dummy<TestBase>())
                .Select(_ => new Task(() => serializer.SerializeToString(_))).ToArray();
            Parallel.ForEach(tasks, _ => _.Start());
            Task.WaitAll(tasks);
        }
    }
}
