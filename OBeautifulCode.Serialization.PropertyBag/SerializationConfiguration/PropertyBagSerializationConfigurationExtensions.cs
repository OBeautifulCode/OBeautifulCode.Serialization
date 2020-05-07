// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;

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
        /// Builds a <see cref="TypeToRegisterForPropertyBag"/> from a type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultMemberTypesToInclude"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="TypeToRegisterConstants.DefaultRelatedTypesToInclude"/>.</param>
        /// <param name="stringSerializerBuilderFunc">Optional func that builds the <see cref="IStringSerializeAndDeserialize"/>.  DEFAULT is null (no serializer).</param>
        /// <returns>
        /// The type to register for property bag serialization.
        /// </returns>
        public static TypeToRegisterForPropertyBag ToTypeToRegisterForPropertyBag(
            this Type type,
            MemberTypesToInclude memberTypesToInclude = TypeToRegisterConstants.DefaultMemberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude = TypeToRegisterConstants.DefaultRelatedTypesToInclude,
            Func<IStringSerializeAndDeserialize> stringSerializerBuilderFunc = null)
        {
            var result = new TypeToRegisterForPropertyBag(type, memberTypesToInclude, relatedTypesToInclude, stringSerializerBuilderFunc);

            return result;
        }
    }
}