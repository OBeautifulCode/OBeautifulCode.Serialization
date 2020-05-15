// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRegisteredTypesTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FakeItEasy;
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using Xunit;

    public static class InternallyRegisteredTypesTest
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_a_serialized_UtcDateTimeRangeInclusive___When_called()
        {
            // this is required to load all of the internal types
            var serializer = new ObcBsonSerializer();

            var modelTypes = AssemblyLoader
                .GetLoadedAssemblies()
                .GetTypesFromAssemblies()
                .Where(_ => !_.ContainsGenericParameters)
                .Where(_ => _.IsAssignableTo(typeof(IModel)))
                .Where(_ => _.IsClass)
                .Where(_ => _ != typeof(DynamicTypePlaceholder))
                .ToList();

            // DELETE THIS LINE---->
            modelTypes = modelTypes.Where(_ => _ == typeof(ElementInitRepresentation)).ToList();

            Func<object, string> serializeToJsonStringFunc = SerializeToJsonString;

            Func<object, string> serializeToBsonStringFunc = SerializeToBsonString;

            Func<string, Type, object> deserializeFromJsonStringFunc = DeserializeFromJsonString;

            Func<string, Type, object> deserializeFromBsonStringFunc = DeserializeFromBsonString;

            foreach (var modelType in modelTypes)
            {
                // Arrange
                var expected = AD.ummy(modelType);

                // var serialized1 = JsonSerializer.SerializeToString(expected);
                var serialized2 = serializeToBsonStringFunc.ExecuteInNewAppDomain(expected);

                // Act
                // var actual1 = JsonSerializer.Deserialize(serialized1, modelType);
                var actual2 = deserializeFromBsonStringFunc.ExecuteInNewAppDomain(serialized2, modelType);

                // Assert
                // actual1.AsTest().Must().BeEqualTo(expected);
                actual2.AsTest().Must().BeEqualTo(expected);
            }
        }

        [Fact]
        public static void Test1()
        {
            var serializer1 = new ObcJsonSerializer<TypesToRegisterJsonSerializationConfiguration<DummyClassType>>();

            serializer1.SerializeToString(A.Dummy<DummyClassType>());

            var stopwatch = new System.Diagnostics.Stopwatch();

            var values = new List<double>();

            for (var x = 0; x < 1000; x++)
            {
                var obj = A.Dummy<DummyClassType>();

                var bson = serializer1.SerializeToString(obj);

                stopwatch.Start();

                serializer1.Deserialize<DummyClassType>(bson);
                stopwatch.Stop();

                values.Add(stopwatch.Elapsed.TotalMilliseconds);

                stopwatch.Reset();
            }

            File.AppendAllText("d:\\no-backup\\bson-deserialize-recurse-simple.txt", values.Select(_ => _.ToString()).ToNewLineDelimited());
        }

        private static void Test()
        {
            // Arrange
            var type = typeof(ExpressionRepresentationBase);

            // Act
            var actualAttributes = type.GetCustomAttributes(typeof(SerializableAttribute), false);

            // Assert
            actualAttributes.AsTest().Must().NotBeEmptyEnumerable();
        }

        private static string SerializeToJsonString(
            object objectToSerialize)
        {
           var serializer = new ObcJsonSerializer();

           var result = serializer.SerializeToString(objectToSerialize);

           return result;
        }

        private static string SerializeToBsonString(
            object objectToSerialize)
        {
            var serializer = new ObcBsonSerializer();

            var result = serializer.SerializeToString(objectToSerialize);

            return result;
        }

        private static object DeserializeFromJsonString(
            string serializedString,
            Type type)
        {
            var serializer = new ObcJsonSerializer();

            var result = serializer.Deserialize(serializedString, type);

            return result;
        }

        private static object DeserializeFromBsonString(
            string serializedString,
            Type type)
        {
            var serializer = new ObcBsonSerializer();

            var result = serializer.Deserialize(serializedString, type);

            return result;
        }

        public class DummyClassType
        {
            public string String1 { get; set; }
        }
    }
}