// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripJsonSerializationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Test;

    public static class RoundtripJsonSerializationExtensions
    {
        public static void RoundtripSerializeViaJsonUsingTypesToRegisterConfigWithEquatableAssertion<T>(
            this T expected,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            RoundtripSerializeViaJsonWithEquatableAssertion(expected, typeof(TypesToRegisterJsonSerializationConfiguration<T>), formats);
        }

        public static void RoundtripSerializeViaJsonWithEquatableAssertion<T>(
            this T expected,
            Type jsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithEquatableAssertion(
                null,
                jsonSerializationConfigurationType,
                null,
                false,
                true,
                false,
                formats);
        }

        public static void RoundtripSerializeViaJsonWithCallback<T>(
            this T expected,
            RoundtripSerializationCallback<T> validationCallback,
            Type jsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithCallback(
                validationCallback,
                null,
                jsonSerializationConfigurationType,
                null,
                false,
                true,
                false,
                formats);
        }
    }
}
