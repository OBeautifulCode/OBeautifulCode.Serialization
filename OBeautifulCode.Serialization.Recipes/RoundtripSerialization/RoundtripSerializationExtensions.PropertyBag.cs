// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripSerializationExtensions.PropertyBag.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Recipes
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.PropertyBag;

    public static partial class RoundtripSerializationExtensions
    {
        /// <summary>
        /// Test roundtrip serialization to/from a Property Bag, asserting that the expected/provided value is equal to the deserialized value using
        /// <see cref="Verifications.BeEqualTo{T}(AssertionTracker, T, string, ApplyBecause, System.Collections.IDictionary)"/>.
        /// Use the following the serialization configuration wrapper for the type being tested:
        /// <see cref="TypesToRegisterPropertyBagSerializationConfiguration{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="formats">The serialization formats to test.</param>
        public static void RoundtripSerializeViaPropertyBagUsingTypesToRegisterConfigWithBeEqualToAssertion<T>(
            this T expected,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            RoundtripSerializeViaPropertyBagWithBeEqualToAssertion(expected, typeof(TypesToRegisterPropertyBagSerializationConfiguration<T>), formats);
        }

        /// <summary>
        /// Test roundtrip serialization to/from a Property Bag, asserting that the expected/provided value is equal to the deserialized value using
        /// <see cref="Verifications.BeEqualTo{T}(AssertionTracker, T, string, ApplyBecause, System.Collections.IDictionary)"/>,
        /// with the serialization configuration type specified.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="propertyBagSerializationConfigurationType">Optional type of the serialization configuration to use for Property Bag testing.  DEFAULT is null; <see cref="NullPropertyBagSerializationConfiguration"/> will be used.</param>
        /// <param name="formats">The serialization formats to test.</param>
        public static void RoundtripSerializeViaPropertyBagWithBeEqualToAssertion<T>(
            this T expected,
            Type propertyBagSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithBeEqualToAssertion(
                null,
                null,
                propertyBagSerializationConfigurationType,
                false,
                false,
                true,
                formats);
        }

        /// <summary>
        /// Test roundtrip serialization to/from a Property Bag, asserting that the expected/provided value is equal to the deserialized value using
        /// the specified callback, with the serialization configuration type specified.
        /// </summary>
        /// <typeparam name="T">The type being tested.</typeparam>
        /// <param name="expected">The value to serialize, which should be equal to the resulting deserialized object.</param>
        /// <param name="verificationCallback">Callback to verify that the expected/provided value is equal to the deserialized value.</param>
        /// <param name="propertyBagSerializationConfigurationType">Optional type of the serialization configuration to use for Property Bag testing.  DEFAULT is null; <see cref="NullPropertyBagSerializationConfiguration"/> will be used.</param>
        /// <param name="formats">The serialization formats to test.</param>
        public static void RoundtripSerializeViaPropertyBagWithCallbackVerification<T>(
            this T expected,
            RoundtripSerializationVerification<T> verificationCallback,
            Type propertyBagSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithCallbackVerification(
                verificationCallback,
                null,
                null,
                propertyBagSerializationConfigurationType,
                false,
                false,
                true,
                formats);
        }
    }
}
