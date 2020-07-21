﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterPropertyBagSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    /// <summary>
    /// Help methods for creating Property Bag serialization configuration types that set <see cref="PropertyBagSerializationConfigurationBase.TypesToRegisterForPropertyBag"/>.
    /// </summary>
    public static class TypesToRegisterPropertyBagSerializationConfiguration
    {
        /// <summary>
        /// Gets the type of a Property Bag serialization configuration that registers the specified <paramref name="typeToRegister"/>,
        /// using the default settings for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
        /// </summary>
        /// <param name="typeToRegister">The type to register.</param>
        /// <returns>
        /// The requested Property Bag serialization configuration type.
        /// </returns>
        public static Type GetType(
            Type typeToRegister)
        {
            if (typeToRegister == null)
            {
                throw new ArgumentNullException(nameof(typeToRegister));
            }

            var result = typeof(TypesToRegisterPropertyBagSerializationConfiguration<>).MakeGenericType(typeToRegister);

            return result;
        }

        /// <summary>
        /// Gets the type of a Property Bag serialization configuration that registers the specified <paramref name="typeToRegister1"/> and <paramref name="typeToRegister2"/>,
        /// using the default settings for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
        /// </summary>
        /// <param name="typeToRegister1">The first type to register.</param>
        /// <param name="typeToRegister2">The second type to register.</param>
        /// <returns>
        /// The requested Property Bag serialization configuration type.
        /// </returns>
        public static Type GetType(
            Type typeToRegister1,
            Type typeToRegister2)
        {
            if (typeToRegister1 == null)
            {
                throw new ArgumentNullException(nameof(typeToRegister1));
            }

            if (typeToRegister2 == null)
            {
                throw new ArgumentNullException(nameof(typeToRegister2));
            }

            var result = typeof(TypesToRegisterPropertyBagSerializationConfiguration<,>).MakeGenericType(typeToRegister1, typeToRegister2);

            return result;
        }
    }
}