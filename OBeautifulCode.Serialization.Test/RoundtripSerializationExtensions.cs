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

    public static class RoundtripSerializationExtensions
    {
        public static void RoundtripSerializeWithEquatableAssertion<T>(
            this T expected,
            bool shouldUseSerializationConfiguration = true)
            where T : IEquatable<T>
        {
            RoundtripSerializeWithCallback(
                expected,
                (yieldedDescribedSerialization, deserializedObject) => deserializedObject.AsTest().Must().BeEqualTo(expected),
                shouldUseSerializationConfiguration ? typeof(TypesToRegisterJsonSerializationConfiguration<T>) : null,
                shouldUseSerializationConfiguration ? typeof(TypesToRegisterBsonSerializationConfiguration<T>) : null);
        }

        public static void RoundtripSerializeWithCallback<T>(
            this T expected,
            RoundtripSerializationCallback<T> validationCallback,
            Type jsonSerializationConfigurationType = null,
            Type bsonSerializationConfigurationType = null,
            Type propertyBagSerializationConfigurationType = null,
            bool testBson = true,
            bool testJson = true,
            bool testPropertyBag = false)
        {
            new { validationCallback }.AsArg().Must().NotBeNull();

            var serializerRepresentations = new List<SerializerRepresentation>();

            if (testJson)
            {
                var serializerRepresentation = new SerializerRepresentation(SerializationKind.Json, jsonSerializationConfigurationType?.ToRepresentation());

                serializerRepresentations.Add(serializerRepresentation);
            }

            if (testBson)
            {
                var serializerDescription = new SerializerRepresentation(SerializationKind.Bson, bsonSerializationConfigurationType?.ToRepresentation());

                serializerRepresentations.Add(serializerDescription);
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
                var formats = new[] { SerializationFormat.String, SerializationFormat.Binary };

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
                        throw new Exception(Invariant($"Failed to roundtrip specified object to/from {serializerRepresentation.SerializationKind} {format} using {serializerRepresentation.SerializationConfigType.ResolveFromLoadedTypes().ToStringReadable()} when serializing in a new AppDomain and deserializing in a new AppDomain.  Deserialized object is: {actual}."), ex);
                    }

                    // serialize and deserialize in the same, new app domain
                    var describedSerializationAndActual = serializeAndDeserializeFunc.ExecuteInNewAppDomain(serializerRepresentation, format, expected);

                    try
                    {
                        validationCallback(describedSerializationAndActual.Item1, describedSerializationAndActual.Item2);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(Invariant($"Failed to roundtrip specified object to/from {serializerRepresentation.SerializationKind} {format} using {serializerRepresentation.SerializationConfigType.ResolveFromLoadedTypes().ToStringReadable()} when serializing and deserializing in the same, new AppDomain.  Deserialized object is: {actual}."), ex);
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
