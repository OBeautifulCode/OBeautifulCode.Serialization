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

    using FluentAssertions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.PropertyBag;

    using static System.FormattableString;

    public static class RoundtripSerializationExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "OK in testing.")]
        public delegate void RoundtripSerializationCallback<in T>(DescribedSerialization yieldedDescribedSerialization, T deserializedObject);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Want parameters this way.")]
        public static void RoundtripSerializeWithEquatableAssertion<T>(
            this T expected,
            bool shouldUseConfiguration = true)
            where T : IEquatable<T>
        {
            RoundtripSerializeWithCallback(
                expected,
                (yieldedDescribedSerialization, deserializedObject) => deserializedObject.Should().Be(expected),
                shouldUseConfiguration ? typeof(GenericDiscoveryJsonSerializationConfiguration<T>) : null,
                shouldUseConfiguration ? typeof(GenericDiscoveryBsonSerializationConfiguration<T>) : null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Want parameters this way.")]
        public static void RoundtripSerializeWithCallback<T>(
            this T expected,
            RoundtripSerializationCallback<T> validationCallback,
            Type jsonConfigType = null,
            Type bsonConfigType = null,
            Type propBagConfigType = null,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default,
            bool testBson = true,
            bool testJson = true,
            bool testPropBag = false)
        {
            new { validationCallback }.AsArg().Must().NotBeNull();

            var serializers = new List<ISerializeAndDeserialize>();

            if (testJson)
            {
                var jsonSerializer = new ObcJsonSerializer(jsonConfigType, unregisteredTypeEncounteredStrategy);
                serializers.Add(jsonSerializer);
            }

            if (testBson)
            {
                var bsonSerializer = new ObcBsonSerializer(bsonConfigType, unregisteredTypeEncounteredStrategy);
                serializers.Add(bsonSerializer);
            }

            if (testPropBag)
            {
                var propBagSerializer = new ObcPropertyBagSerializer(propBagConfigType, unregisteredTypeEncounteredStrategy);
                serializers.Add(propBagSerializer);
            }

            if (!serializers.Any())
            {
                throw new InvalidOperationException("No serializers are being tested.");
            }

            foreach (var serializer in serializers)
            {
                var configurationTypeRepresentation = serializer.SerializationConfigurationType.ToRepresentation();

                var stringDescription = new SerializerDescription(
                    serializer.SerializationKind,
                    SerializationFormat.String,
                    configurationTypeRepresentation);

                var binaryDescription = new SerializerDescription(
                    serializer.SerializationKind,
                    SerializationFormat.Binary,
                    configurationTypeRepresentation);

                var actualString = expected.ToDescribedSerializationUsingSpecificSerializer(stringDescription, serializer);
                var actualBinary = expected.ToDescribedSerializationUsingSpecificSerializer(binaryDescription, serializer);

                var actualFromString = actualString.DeserializePayloadUsingSpecificSerializer(serializer);
                var actualFromBinary = actualBinary.DeserializePayloadUsingSpecificSerializer(serializer);

                try
                {
                    validationCallback(actualString, (T)actualFromString);
                }
                catch (Exception ex)
                {
                    throw new ObcSerializationException(Invariant($"Failure confirming from string with {nameof(serializer)} - {serializer.GetType()} - {actualFromString}"), ex);
                }

                try
                {
                    validationCallback(actualBinary, (T)actualFromBinary);
                }
                catch (Exception ex)
                {
                    throw new ObcSerializationException(Invariant($"Failure confirming from binary with {nameof(serializer)} - {serializer.GetType()} - {actualFromBinary}"), ex);
                }
            }
        }
    }
}
