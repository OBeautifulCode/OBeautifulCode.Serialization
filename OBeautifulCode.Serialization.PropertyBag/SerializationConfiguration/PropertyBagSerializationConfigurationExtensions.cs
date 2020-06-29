// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods related to property bag serialization configuration.
    /// </summary>
    public static class PropertyBagSerializationConfigurationExtensions
    {
        /// <summary>
        /// Gets the <see cref="PropertyBagSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </summary>
        /// <param name="propertyBagSerializationConfigurationType">The type of the Property Bag serialization configuration.</param>
        /// <returns>
        /// The <see cref="PropertyBagSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </returns>
        public static PropertyBagSerializationConfigurationType ToPropertyBagSerializationConfigurationType(
            this Type propertyBagSerializationConfigurationType)
        {
            var result = new PropertyBagSerializationConfigurationType(propertyBagSerializationConfigurationType);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForPropertyBag"/> from a type using the most sensible settings.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <returns>
        /// The type to register for property bag serialization.
        /// </returns>
        public static TypeToRegisterForPropertyBag ToTypeToRegisterForPropertyBag(
            this Type type)
        {
            new { type }.AsArg().Must().NotBeNull();

            var result = new TypeToRegisterForPropertyBag(type, MemberTypesToInclude.All, RelatedTypesToInclude.Default, null);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForPropertyBag"/> from a type using the most sensible settings,
        /// with a specified <see cref="IStringSerializeAndDeserialize"/> to use everywhere the type appears.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="stringSerializerBuilderFunc">Func that builds the <see cref="IStringSerializeAndDeserialize"/>.  DEFAULT is null (no serializer).</param>
        /// <returns>
        /// The type to register for property bag serialization.
        /// </returns>
        public static TypeToRegisterForPropertyBag ToTypeToRegisterForPropertyBagUsingStringSerializer(
            this Type type,
            Func<IStringSerializeAndDeserialize> stringSerializerBuilderFunc)
        {
            new { type }.AsArg().Must().NotBeNull();
            new { stringSerializerBuilderFunc }.AsArg().Must().NotBeNull();

            var result = new TypeToRegisterForPropertyBag(type, MemberTypesToInclude.None, RelatedTypesToInclude.Default, stringSerializerBuilderFunc);

            return result;
        }
    }
}