// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Test.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.IO;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class Test
    {
        private static readonly string BsonPerfTestFilePath = "d:\\no-backup\\perf-issue-bson-tests.txt";

        private static readonly string JsonPerfTestFilePath = "d:\\no-backup\\perf-issue-json-tests.txt";

        [Fact]
        public static void BsonTest()
        {
            var bsonSetup = GetBsonSetup();

            var watch = new System.Diagnostics.Stopwatch();

            for (int x = 0; x < 10; x++)
            {
                watch.Start();

                bsonSetup.serializer.Deserialize(bsonSetup.describedSerialization.SerializedPayload, typeof(TestMapping));

                watch.Stop();

                File.AppendAllText(BsonPerfTestFilePath, "direct-deserialize: " + watch.Elapsed.TotalMilliseconds + Environment.NewLine);

                watch.Reset();
            }
        }

        [Fact]
        public static void BsonTest2()
        {
            var bsonSetup = GetBsonSetup();

            var watch = new System.Diagnostics.Stopwatch();

            for (int x = 0; x < 10; x++)
            {
                watch.Start();

                bsonSetup.describedSerialization.DeserializePayloadUsingSpecificFactory(SerializerFactory.Instance);

                watch.Stop();

                File.AppendAllText(BsonPerfTestFilePath, "described-deserialize: " + watch.Elapsed.TotalMilliseconds + Environment.NewLine);

                watch.Reset();
            }
        }

        [Fact]
        public static void JsonTest()
        {
            var jsonSetup = GetJsonSetup();

            var watch = new System.Diagnostics.Stopwatch();

            for (int x = 0; x < 10; x++)
            {
                watch.Start();

                jsonSetup.serializer.Deserialize(jsonSetup.describedSerialization.SerializedPayload, typeof(TestMapping));

                watch.Stop();

                File.AppendAllText(JsonPerfTestFilePath, "direct-deserialize: " + watch.Elapsed.TotalMilliseconds + Environment.NewLine);

                watch.Reset();
            }
        }

        [Fact]
        public static void JsonTest2()
        {
            var jsonSetup = GetJsonSetup();

            var watch = new System.Diagnostics.Stopwatch();

            for (int x = 0; x < 10; x++)
            {
                watch.Start();

                jsonSetup.describedSerialization.DeserializePayloadUsingSpecificFactory(SerializerFactory.Instance);

                watch.Stop();

                File.AppendAllText(JsonPerfTestFilePath, "described-deserialize: " + watch.Elapsed.TotalMilliseconds + Environment.NewLine);

                watch.Reset();
            }
        }

        private static (ObcBsonSerializer serializer, DescribedSerialization describedSerialization) GetBsonSetup()
        {
            var serializer = new ObcBsonSerializer<TypesToRegisterBsonSerializationConfiguration<TestMapping>>();

            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Bson, serializer.SerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToRepresentation());

            var serializedPayload = File.ReadAllText("d:\\no-backup\\payload-for-testing.bson");

            var describedSerialization = new DescribedSerialization(typeof(TestMapping).ToRepresentation(), serializedPayload, serializerRepresentation, SerializationFormat.String);

            return (serializer, describedSerialization);
        }

        private static (ObcJsonSerializer serializer, DescribedSerialization describedSerialization) GetJsonSetup()
        {
            var serializer = new ObcJsonSerializer<TypesToRegisterJsonSerializationConfiguration<TestMapping>>();

            var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, serializer.SerializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.ToRepresentation());

            var serializedPayload = File.ReadAllText("d:\\no-backup\\payload-for-testing.json");

            var describedSerialization = new DescribedSerialization(typeof(TestMapping).ToRepresentation(), serializedPayload, serializerRepresentation, SerializationFormat.String);

            return (serializer, describedSerialization);
        }
    }
}