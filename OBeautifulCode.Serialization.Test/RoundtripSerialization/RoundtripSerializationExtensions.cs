// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripSerializationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.PropertyBag;
    using OBeautifulCode.Serialization.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    public delegate void RoundtripSerializationCallback<in T>(
        DescribedSerialization yieldedDescribedSerialization,
        T deserializedObject);

    public static class RoundtripSerializationExtensions
    {
        public static void RoundtripSerializeUsingTypesToRegisterConfigWithEquatableAssertion<T>(
            this T expected,
            bool testBson = true,
            bool testJson = true,
            bool testPropertyBag = false,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithEquatableAssertion(
                typeof(TypesToRegisterBsonSerializationConfiguration<T>),
                typeof(TypesToRegisterJsonSerializationConfiguration<T>),
                typeof(TypesToRegisterPropertyBagSerializationConfiguration<T>),
                testBson,
                testJson,
                testPropertyBag,
                formats);
        }

        public static void RoundtripSerializeWithEquatableAssertion<T>(
            this T expected,
            Type bsonSerializationConfigurationType = null,
            Type jsonSerializationConfigurationType = null,
            Type propertyBagSerializationConfigurationType = null,
            bool testBson = true,
            bool testJson = true,
            bool testPropertyBag = false,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            RoundtripSerializeWithCallback(
                expected,
                (yieldedDescribedSerialization, deserializedObject) => deserializedObject.AsTest().Must().BeEqualTo(expected),
                bsonSerializationConfigurationType,
                jsonSerializationConfigurationType,
                propertyBagSerializationConfigurationType,
                testBson,
                testJson,
                testPropertyBag,
                formats);
        }

        public static void RoundtripSerializeWithCallback<T>(
            this T expected,
            RoundtripSerializationCallback<T> validationCallback,
            Type bsonSerializationConfigurationType = null,
            Type jsonSerializationConfigurationType = null,
            Type propertyBagSerializationConfigurationType = null,
            bool testBson = true,
            bool testJson = true,
            bool testPropertyBag = false,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            formats = formats ?? new[] { SerializationFormat.String, SerializationFormat.Binary };

            formats.AsArg().Must().NotBeNullNorEmptyEnumerable();

            var serializerRepresentations = new List<SerializerRepresentation>();

            if (testBson)
            {
                var serializerDescription = new SerializerRepresentation(SerializationKind.Bson, bsonSerializationConfigurationType?.ToRepresentation());

                serializerRepresentations.Add(serializerDescription);
            }

            if (testJson)
            {
                var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, jsonSerializationConfigurationType?.ToRepresentation());

                serializerRepresentations.Add(serializerRepresentation);
            }

            if (testPropertyBag)
            {
                var serializerDescription = new SerializerRepresentation(SerializationKind.PropertyBag, propertyBagSerializationConfigurationType?.ToRepresentation());

                serializerRepresentations.Add(serializerDescription);
            }

            if (!serializerRepresentations.Any())
            {
                throw new InvalidOperationException("No serializers are being tested.");
            }

            Func<SerializerRepresentation, SerializationFormat, object, DescribedSerialization> serializeFunc = Serialize;

            Func<DescribedSerialization, T> deserializeFunc = Deserialize<T>;

            Func<SerializerRepresentation, SerializationFormat, object, Tuple<DescribedSerialization, T>> serializeAndDeserializeFunc = SerializeAndDeserialize<T>;

            foreach (var serializerRepresentation in serializerRepresentations)
            {
                foreach (var format in formats)
                {
                    // serialize in a new app domain
                    var describedSerialization = serializeFunc.ExecuteInNewAppDomain(serializerRepresentation, format, expected);

                    // deserialize in a new app domain
                    var actual = deserializeFunc.ExecuteInNewAppDomain(describedSerialization);

                    try
                    {
                        validationCallback(describedSerialization, actual);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(Invariant($"Failed to roundtrip specified object to/from {serializerRepresentation.SerializationKind} {format} using {serializerRepresentation.SerializationConfigType.ResolveFromLoadedTypes().ToStringReadable()} when serializing in a new AppDomain and deserializing in a new AppDomain.  Deserialized object is: {actual}."), ex);
                    }

                    // serialize and deserialize in the same, new app domain
                    var describedSerializationAndActual = serializeAndDeserializeFunc.ExecuteInNewAppDomain(serializerRepresentation, format, expected);

                    try
                    {
                        validationCallback(describedSerializationAndActual.Item1, describedSerializationAndActual.Item2);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(Invariant($"Failed to roundtrip specified object to/from {serializerRepresentation.SerializationKind} {format} using {serializerRepresentation.SerializationConfigType.ResolveFromLoadedTypes().ToStringReadable()} when serializing and deserializing in the same, new AppDomain.  Deserialized object is: {actual}."), ex);
                    }
                }
            }
        }

        private static DescribedSerialization Serialize(
            SerializerRepresentation serializerRepresentation,
            SerializationFormat serializationFormat,
            object objectToSerialize)
        {
            var result = objectToSerialize.ToDescribedSerialization(serializerRepresentation, serializationFormat);

            return result;
        }

        private static T Deserialize<T>(
            DescribedSerialization describedSerialization)
        {
            var result = describedSerialization.DeserializePayload<T>();

            return result;
        }

        private static Tuple<DescribedSerialization, T> SerializeAndDeserialize<T>(
            SerializerRepresentation serializerRepresentation,
            SerializationFormat serializationFormat,
            object objectToSerialize)
        {
            var describedSerialization = objectToSerialize.ToDescribedSerialization(serializerRepresentation, serializationFormat);

            var actual = describedSerialization.DeserializePayload<T>();

            // note that we cannot return a ValueTuple (DescribedSerialization DescribedSerialization, T actual)
            // here because ValueTuple is not [Serializable]
            var result = new Tuple<DescribedSerialization, T>(describedSerialization, actual);

            return result;
        }
    }
}
