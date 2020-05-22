// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripBsonSerializationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson.Test
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Test;

    public static class RoundtripBsonSerializationExtensions
    {
        public static void RoundtripSerializeViaBsonUsingTypesToRegisterConfigWithEquatableAssertion<T>(
            this T expected,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeViaBsonWithEquatableAssertion(
                typeof(TypesToRegisterBsonSerializationConfiguration<T>),
                formats);
        }

        public static void RoundtripSerializeViaBsonWithEquatableAssertion<T>(
            this T expected,
            Type bsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithEquatableAssertion(
                bsonSerializationConfigurationType,
                null,
                null,
                true,
                false,
                false,
                formats);
        }

        public static void RoundtripSerializeViaBsonWithCallback<T>(
            this T expected,
            RoundtripSerializationCallback<T> validationCallback,
            Type bsonSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithCallback(
                validationCallback,
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
