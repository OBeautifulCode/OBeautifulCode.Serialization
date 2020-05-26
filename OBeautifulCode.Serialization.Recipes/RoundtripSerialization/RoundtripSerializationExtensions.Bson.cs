// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripSerializationExtensions.Bson.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Recipes
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;

    public static partial class RoundtripSerializationExtensions
    {
        /// <summary>
        /// Test roundtrip serialization to/from BSON, asserting that the expected/provided value is equal to the deserialized value using
        /// <see cref="Verifications.BeEqualTo{T}(AssertionTracker, T, string, ApplyBecause, System.Collections.IDictionary)"/>.
        /// Use the following the serialization configuration wrapper for the type being tested:
        /// <see cref="TypesToRegisterBsonSerializationConfiguration{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="formats">The serialization formats to test.</param>
        public static void RoundtripSerializeViaBsonUsingTypesToRegisterConfigWithBeEqualToAssertion<T>(
            this T expected,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(
                typeof(TypesToRegisterBsonSerializationConfiguration<T>),
                formats);
        }

        /// <summary>
        /// Test roundtrip serialization to/from BSON, asserting that the expected/provided value is equal to the deserialized value using
        /// <see cref="Verifications.BeEqualTo{T}(AssertionTracker, T, string, ApplyBecause, System.Collections.IDictionary)"/>,
        /// with the serialization configuration type specified.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="bsonSerializationConfigurationType">Optional type of the serialization configuration to use for BSON testing.  DEFAULT is null; <see cref="NullBsonSerializationConfiguration"/> will be used.</param>
        /// <param name="formats">The serialization formats to test.</param>
        public static void RoundtripSerializeViaBsonWithBeEqualToAssertion<T>(
            this T expected,
            Type bsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithBeEqualToAssertion(
                bsonSerializationConfigurationType,
                null,
                null,
                true,
                false,
                false,
                formats);
        }

        /// <summary>
        /// Test roundtrip serialization to/from BSON, asserting that the expected/provided value is equal to the deserialized value using
        /// the specified callback, with the serialization configuration type specified.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="verificationCallback">Callback to verify that the expected/provided value is equal to the deserialized value.</param>
        /// <param name="bsonSerializationConfigurationType">Optional type of the serialization configuration to use for BSON testing.  DEFAULT is null; <see cref="NullBsonSerializationConfiguration"/> will be used.</param>
        /// <param name="formats">The serialization formats to test.</param>
        public static void RoundtripSerializeViaBsonWithCallbackVerification<T>(
            this T expected,
            RoundtripSerializationVerification<T> verificationCallback,
            Type bsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithCallbackVerification(
                verificationCallback,
                bsonSerializationConfigurationType,
                null,
                null,
                true,
                false,
                false,
                formats);
        }
    }
}
