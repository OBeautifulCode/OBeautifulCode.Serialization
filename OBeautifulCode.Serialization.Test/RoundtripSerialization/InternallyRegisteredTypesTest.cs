// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRegisteredTypesTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using Xunit;

    public static class InternallyRegisteredTypesTest
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_all_serialized_internally_required_types___When_called()
        {
            // Arrange
            // this is required to load all of the internal types
            Console.Write(SerializationConfigurationBase.InternallyRequiredTypes);

            // NOTE: as new open generic types are added to internally required types, specify some corresponding closed types
            // this is not something we can automate
            var closedGenericTypes = new[]
            {
                typeof(ConstantExpressionRepresentation<string>),
                typeof(ConstantExpressionRepresentation<DateTime>),
                typeof(ObjectWrapper<string>),
                typeof(ObjectWrapper<decimal>),
            };

            var modelTypes = AssemblyLoader
                .GetLoadedAssemblies()
                .GetTypesFromAssemblies()
                .Where(_ => !_.ContainsGenericParameters)
                .Where(_ => _.IsAssignableTo(typeof(IModel)))
                .Where(_ => _ != typeof(IModel))
                .Where(_ => _ != typeof(DynamicTypePlaceholder))
                .Concat(closedGenericTypes)
                .OrderBy(_ => _.Name)
                .ToList();

            // Act, Assert
            foreach (var modelType in modelTypes)
            {
                var expected = AD.ummy(modelType);

                var bsonConfigType = typeof(ThrowOnUnregisteredTypeBsonSerializationConfiguration<NullBsonSerializationConfiguration>);

                var jsonConfigType = typeof(ThrowOnUnregisteredTypeJsonSerializationConfiguration<NullJsonSerializationConfiguration>);

                expected.RoundtripSerializeWithBeEqualToAssertion(bsonConfigType, jsonConfigType, testBson: false, formats: new[] { SerializationFormat.String }, appDomainScenarios: AppDomainScenarios.RoundtripInCurrentAppDomain);
            }
        }

        [Fact(Skip = "testing")]
        public static void Test()
        {
            var serializer = new ObcJsonSerializer<TestJsonSerializationConfiguration>();

            ////var item = new MyClass
            ////{
            ////    StringProperty = "abcd",
            ////    ObjectProperty = new DateTime(2020, 01, 02, 10, 33, 59, DateTimeKind.Utc),
            ////    MyClass2 = new MyClass2(5),
            ////};

            var item = new MyClass
            {
                ////Property = new Dictionary<TestClassAbstract, string>
                ////{
                ////    { new TestClassConcrete(), "abcd" },
                ////},
                ////Property5 = new Dictionary<TestClassAbstract, string>
                ////{
                ////    { new TestClassConcrete(), "abcd" },
                ////},
                ////Property2 = new List<TestClassConcrete>() { new TestClassConcrete() },

                ////Property3 = new List<TestClassAbstract>(){ new TestClassConcrete() },
                Property4 = new TestClassConcrete[] { new TestClassConcrete() },
            };

            object dateTime = DateTime.Now;
            var test = serializer.SerializeToString(item);
            var test2 = serializer.Deserialize<MyClass>(test);

            var json = serializer.SerializeToString(item);

            var deserialized = serializer.Deserialize<MyClass>(json);

            // var result = item.ObjectProperty.IsEqualTo(deserialized.ObjectProperty);
            // test dictionary<class-without-string-converter, anything> where property is declared as object
            // test dictionary<class-with-string-converter, anything> where property is declared as object
            // test roundtrip object() as top level type.  how about property of a top level type?
        }

        public class TestClassConcrete : TestClassAbstract
        {
        }

        public abstract class TestClassAbstract
        {
        }

        ////public class MyClass
        ////{
        ////    public string StringProperty { get; set; }

        ////    public object ObjectProperty { get; set; }

        ////    public MyClass2 MyClass2 { get; set; }
        ////}

        public class MyClass
        {
            ////public IReadOnlyDictionary<TestClassAbstract, string> Property { get; set; }

            ////public Dictionary<TestClassAbstract, string> Property5 { get; set; }

            public IReadOnlyList<TestClassAbstract> Property2 { get; set; }

            ////public List<TestClassAbstract> Property3 { get; set; }

            public TestClassAbstract[] Property4 { get; set; }
        }

        public class MyClass2
        {
            public MyClass2(int someProperty)
            {
                this.SomeProperty = someProperty;
            }

            public int SomeProperty { get; private set; }
        }

        public class TestJsonSerializationConfiguration : JsonSerializationConfigurationBase
        {
            /// <inheritdoc />
            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(MyClass).ToTypeToRegisterForJson(),
                typeof(MyClass2).ToTypeToRegisterForJsonUsingStringSerializer(new MyClass2StringSerializer()),
            };
        }

        public class MyClass2StringSerializer : IStringSerializeAndDeserialize
        {
            public string SerializeToString(
                object objectToSerialize)
            {
                string result;

                if (objectToSerialize == null)
                {
                    result = null;
                }
                else if (objectToSerialize is MyClass2 item)
                {
                    result = item.SomeProperty.ToString();
                }
                else
                {
                    throw new ArgumentException();
                }

                return result;
            }

            /// <inheritdoc />
            public T Deserialize<T>(
                string serializedString)
            {
                var result = (T)this.Deserialize(serializedString, typeof(T));

                return result;
            }

            /// <inheritdoc />
            public object Deserialize(
                string serializedString,
                Type type)
            {
                var result = new MyClass2(int.Parse(serializedString));

                return result;
            }
        }
    }
}