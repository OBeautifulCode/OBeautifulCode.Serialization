// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundtripPropertyBagSerializationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag.Test
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization.PropertyBag;
    using OBeautifulCode.Serialization.Test;

    public static class RoundtripPropertyBagSerializationExtensions
    {
        public static void RoundtripSerializeViaPropertyBagUsingTypesToRegisterConfigWithEquatableAssertion<T>(
            this T expected,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            RoundtripSerializeViaPropertyBagWithEquatableAssertion(expected, typeof(TypesToRegisterPropertyBagSerializationConfiguration<T>), formats);
        }

        public static void RoundtripSerializeViaPropertyBagWithEquatableAssertion<T>(
            this T expected,
            Type propertyBagSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithEquatableAssertion(
                null,
                null,
                propertyBagSerializationConfigurationType,
                false,
                false,
                true,
                formats);
        }

        public static void RoundtripSerializeViaPropertyBagWithCallback<T>(
            this T expected,
            RoundtripSerializationCallback<T> validationCallback,
            Type propertyBagSerializationConfigurationType = null,
            IReadOnlyCollection<SerializationFormat> formats = null)
        {
            expected.RoundtripSerializeWithCallback(
                validationCallback,
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
