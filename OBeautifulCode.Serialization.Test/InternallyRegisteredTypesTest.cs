// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRegisteredTypesTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using MongoDB.Bson.Serialization;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using Xunit;

    public static class InternallyRegisteredTypesTest
    {
        private static readonly ObcJsonSerializer JsonSerializer = new ObcJsonSerializer(unregisteredTypeEncounteredStrategy: UnregisteredTypeEncounteredStrategy.Throw);

        // private static readonly ObcBsonSerializer BsonSerializer = new ObcBsonSerializer(unregisteredTypeEncounteredStrategy: UnregisteredTypeEncounteredStrategy.Throw);
        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_UtcDateTimeRangeInclusive___When_called()
        {
            var test = typeof(TestChild).BaseType;

            var modelTypes = AssemblyLoader
                .GetLoadedAssemblies()
                .GetTypesFromAssemblies()
                .Where(_ => !_.ContainsGenericParameters)
                .Where(_ => _.IsAssignableTo(typeof(IModel)))
                .Where(_ => _.IsClass)
                .Where(_ => _ != typeof(DynamicTypePlaceholder))
                .ToList();

            modelTypes = modelTypes.Where(_ => _ == typeof(ElementInitRepresentation)).ToList();

            foreach (var modelType in modelTypes)
            {
                // Arrange
                var expected = AD.ummy(modelType);

                var serialized1 = JsonSerializer.SerializeToString(expected);

                // var serialized2 = BsonSerializer.SerializeToString(expected);

                // Act
                var actual1 = JsonSerializer.Deserialize(serialized1, expected.GetType());

                // var actual2 = BsonSerializer.Deserialize(serialized2, expected.GetType());

                // Assert
                actual1.AsTest().Must().BeEqualTo(expected);

                // actual2.AsTest().Must().BeEqualTo(expected);
            }
        }

        [Fact]
        public static void Test()
        {
            var baseType = typeof(List<>).MakeArrayType();

            var type1 = typeof(Dictionary<string, int>);

            var output2 = type1.FullName + ", " + type1.Assembly.GetName().Name;

            var test = new TestGeneric<int>()
            {
                Wrapped = 54,
            };

            var output = test.GetType().FullName + ", " + test.GetType().Assembly.GetName().Name;

            var type = Type.GetType(output);

            var json = JsonConvert.SerializeObject(test);

            var result = JsonConvert.DeserializeObject<TestGeneric<int>>(json);

            Console.Write(type);

            var bsonClassMap = new BsonClassMap(type);

            bsonClassMap.MapMember(typeof(TestGeneric<>).GetProperty("Wrapped"));

            BsonClassMap.RegisterClassMap(bsonClassMap);

            var document = ObcBsonSerializerExtensions.SerializeToDocument(test);

            var roundtrip = ObcBsonSerializerExtensions.DeserializeFromDocument<TestGeneric<int>>(document);

            Console.Write(roundtrip);
        }

        private class TestGeneric<T>
        {
            public T Wrapped { get; set; }
        }

        private abstract class TestGenericParent<T>
        {
            public T Wrapped { get; set; }
        }

        private class TestChild : TestGenericParent<string>
        {
            public int SomeInt { get; set; }
        }

        private class TestChild2<T> : TestGenericParent<T>
        {
            public int SomeInt { get; set; }
        }

        private abstract class TestParent
        {
            public string SomeString { get; set; }
        }

        private class TestGenericChild<T> : TestParent
        {
            public T Wrapped { get; set; }
        }
    }
}
