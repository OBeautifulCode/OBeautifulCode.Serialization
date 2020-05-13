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

            var formatUnspecifiedSerializerDescriptions = new List<SerializerDescription>();

            if (testJson)
            {
                var serializerDescription = new SerializerDescription(SerializationKind.Json, SerializationFormat.String, jsonSerializationConfigurationType?.ToRepresentation());

                formatUnspecifiedSerializerDescriptions.Add(serializerDescription);
            }

            if (testBson)
            {
                var serializerDescription = new SerializerDescription(SerializationKind.Bson, SerializationFormat.String, bsonSerializationConfigurationType?.ToRepresentation());

                formatUnspecifiedSerializerDescriptions.Add(serializerDescription);
            }

            if (testPropertyBag)
            {
                var serializerDescription = new SerializerDescription(SerializationKind.PropertyBag, SerializationFormat.String, propertyBagSerializationConfigurationType?.ToRepresentation());

                formatUnspecifiedSerializerDescriptions.Add(serializerDescription);
            }

            if (!formatUnspecifiedSerializerDescriptions.Any())
            {
                throw new InvalidOperationException("No serializers are being tested.");
            }

            Func<SerializerDescription, object, DescribedSerialization> serializeFunc = Serialize;

            Func<DescribedSerialization, T> deserializeFunc = Deserialize<T>;

            Func<SerializerDescription, object, Tuple<DescribedSerialization, T>> serializeAndDeserializeFunc = SerializeAndDeserialize<T>;

            foreach (var formatUnspecifiedSerializerDescription in formatUnspecifiedSerializerDescriptions)
            {
                var stringSerializerDescription = formatUnspecifiedSerializerDescription.DeepCloneWithSerializationFormat(SerializationFormat.String);

                var binarySerializerDescription = formatUnspecifiedSerializerDescription.DeepCloneWithSerializationFormat(SerializationFormat.Binary);

                var serializerDescriptions = new[] { stringSerializerDescription, binarySerializerDescription };

                foreach (var serializerDescription in serializerDescriptions)
                {
                    // serialize in a new app domain
                    var describedSerialization = serializeFunc.ExecuteInNewAppDomain(serializerDescription, expected);

                    // deserialize in a new app domain
                    var actual = deserializeFunc.ExecuteInNewAppDomain(describedSerialization);

                    try
                    {
                        validationCallback(describedSerialization, actual);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(Invariant($"Failed to roundtrip specified object to/from {serializerDescription.SerializationKind} {serializerDescription.SerializationFormat} using {serializerDescription.SerializationConfigType.ResolveFromLoadedTypes().ToStringReadable()} when serializing in a new AppDomain and deserializing in a new AppDomain.  Deserialized object is: {actual}."), ex);
                    }

                    // serialize and deserialize in the same, new app domain
                    var describedSerializationAndActual = serializeAndDeserializeFunc.ExecuteInNewAppDomain(serializerDescription, expected);

                    try
                    {
                        validationCallback(describedSerializationAndActual.Item1, describedSerializationAndActual.Item2);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(Invariant($"Failed to roundtrip specified object to/from {serializerDescription.SerializationKind} {serializerDescription.SerializationFormat} using {serializerDescription.SerializationConfigType.ResolveFromLoadedTypes().ToStringReadable()} when serializing and deserializing in the same, new AppDomain.  Deserialized object is: {actual}."), ex);
                    }
                }
            }
        }

        private static DescribedSerialization Serialize(
            SerializerDescription serializerDescription,
            object objectToSerialize)
        {
            var result = objectToSerialize.ToDescribedSerialization(serializerDescription);

            return result;
        }

        private static T Deserialize<T>(
            DescribedSerialization describedSerialization)
        {
            var result = describedSerialization.DeserializePayload<T>();

            return result;
        }

        private static Tuple<DescribedSerialization, T> SerializeAndDeserialize<T>(
            SerializerDescription serializerDescription,
            object objectToSerialize)
        {
            var describedSerialization = objectToSerialize.ToDescribedSerialization(serializerDescription);

            var actual = describedSerialization.DeserializePayload<T>();

            // note that we cannot return a ValueTuple (DescribedSerialization DescribedSerialization, T actual)
            // here because ValueTuple is not [Serializable]
            var result = new Tuple<DescribedSerialization, T>(describedSerialization, actual);

            return result;
        }
    }
}
