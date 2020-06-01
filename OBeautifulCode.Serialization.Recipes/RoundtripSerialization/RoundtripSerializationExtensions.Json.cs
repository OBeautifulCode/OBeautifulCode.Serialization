﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripSerializationExtensions.Json.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Recipes
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Json;

    public static partial class RoundtripSerializationExtensions
    {
        /// <summary>
        /// Test roundtrip serialization to/from JSON, asserting that the expected/provided value is equal to the deserialized value using
        /// <see cref="Verifications.BeEqualTo{T}(AssertionTracker, T, string, ApplyBecause, System.Collections.IDictionary)"/>.
        /// Use the following the serialization configuration wrapper for the type being tested:
        /// <see cref="TypesToRegisterJsonSerializationConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="formats">The serialization formats to test.</param>
        /// <param name="appDomainScenarios">Optional value that specifies various scenarios of serializing and de-serializing in the current App Domain or a new App Domain.  DEFAULT is test the roundtrip in a new App Domain and also to serialize in a new App Domain and de-serialize in a new, but different App Domain.</param>
        public static void RoundtripSerializeViaJsonUsingTypesToRegisterConfigWithBeEqualToAssertion<T>(
            this T expected,
            IReadOnlyCollection<SerializationFormat> formats = null,
            AppDomainScenarios appDomainScenarios = DefaultAppDomainScenarios)
        {
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(
                typeof(TypesToRegisterJsonSerializationConfiguration<T>),
                formats,
                appDomainScenarios);
        }

        /// <summary>
        /// Test roundtrip serialization to/from JSON, asserting that the expected/provided value is equal to the deserialized value using
        /// <see cref="Verifications.BeEqualTo{T}(AssertionTracker, T, string, ApplyBecause, System.Collections.IDictionary)"/>,
        /// with the serialization configuration type specified.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="jsonSerializationConfigurationType">Optional type of the serialization configuration to use for JSON testing.  DEFAULT is null; <see cref="NullJsonSerializationConfiguration"/> will be used.</param>
        /// <param name="formats">The serialization formats to test.</param>
        /// <param name="appDomainScenarios">Optional value that specifies various scenarios of serializing and de-serializing in the current App Domain or a new App Domain.  DEFAULT is test the roundtrip in a new App Domain and also to serialize in a new App Domain and de-serialize in a new, but different App Domain.</param>
        public static void RoundtripSerializeViaJsonWithBeEqualToAssertion<T>(
            this T expected,
            Type jsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null,
            AppDomainScenarios appDomainScenarios = DefaultAppDomainScenarios)
        {
            expected.RoundtripSerializeWithBeEqualToAssertion(
                null,
                jsonSerializationConfigurationType,
                null,
                false,
                true,
                false,
                formats,
                appDomainScenarios);
        }

        /// <summary>
        /// Test roundtrip serialization to/from JSON, asserting that the expected/provided value is equal to the deserialized value using
        /// the specified callback, with the serialization configuration type specified.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="verificationCallback">Callback to verify that the expected/provided value is equal to the deserialized value.</param>
        /// <param name="jsonSerializationConfigurationType">Optional type of the serialization configuration to use for JSON testing.  DEFAULT is null; <see cref="NullJsonSerializationConfiguration"/> will be used.</param>
        /// <param name="formats">The serialization formats to test.</param>
        /// <param name="appDomainScenarios">Optional value that specifies various scenarios of serializing and de-serializing in the current App Domain or a new App Domain.  DEFAULT is test the roundtrip in a new App Domain and also to serialize in a new App Domain and de-serialize in a new, but different App Domain.</param>
        public static void RoundtripSerializeViaJsonWithCallbackVerification<T>(
            this T expected,
            RoundtripSerializationVerification<T> verificationCallback,
            Type jsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null,
            AppDomainScenarios appDomainScenarios = DefaultAppDomainScenarios)
        {
            expected.RoundtripSerializeWithCallbackVerification(
                verificationCallback,
                null,
                jsonSerializationConfigurationType,
                null,
                false,
                true,
                false,
                formats,
                appDomainScenarios);
        }
    }
}
