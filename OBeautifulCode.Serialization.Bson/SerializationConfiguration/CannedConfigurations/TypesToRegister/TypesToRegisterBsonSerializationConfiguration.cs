// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesToRegisterBsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Help methods for creating BSON serialization configuration types that set <see cref="BsonSerializationConfigurationBase.TypesToRegisterForBson"/>.
    /// </summary>
    public static class TypesToRegisterBsonSerializationConfiguration
    {
        /// <summary>
        /// Gets the type of a BSON serialization configuration that registers the specified <paramref name="typeToRegister"/>,
        /// using the default settings for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
        /// </summary>
        /// <param name="typeToRegister">The type to register.</param>
        /// <returns>
        /// The requested BSON serialization configuration type.
        /// </returns>
        public static Type GetType(
            Type typeToRegister)
        {
            new { typeToRegister }.AsArg().Must().NotBeNull();

            var result = typeof(TypesToRegisterBsonSerializationConfiguration<>).MakeGenericType(typeToRegister);

            return result;
        }

        /// <summary>
        /// Gets the type of a BSON serialization configuration that registers the specified <paramref name="typeToRegister1"/> and <paramref name="typeToRegister2"/>,
        /// using the default settings for <see cref="MemberTypesToInclude"/> and <see cref="RelatedTypesToInclude"/>.
        /// </summary>
        /// <param name="typeToRegister1">The first type to register.</param>
        /// <param name="typeToRegister2">The second type to register.</param>
        /// <returns>
        /// The requested BSON serialization configuration type.
        /// </returns>
        public static Type GetType(
            Type typeToRegister1,
            Type typeToRegister2)
        {
            new { typeToRegister1 }.AsArg().Must().NotBeNull();
            new { typeToRegister2 }.AsArg().Must().NotBeNull();

            var result = typeof(TypesToRegisterBsonSerializationConfiguration<,>).MakeGenericType(typeToRegister1, typeToRegister2);

            return result;
        }
    }
}