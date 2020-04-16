// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson.Serialization;

    /// <summary>
    /// Extension methods related to BSON serialization configuration.
    /// </summary>
    public static class BsonSerializationConfigurationExtensions
    {
        /// <summary>
        /// Gets the <see cref="BsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </summary>
        /// <param name="bsonSerializationConfigurationType">The type of the BSON serialization configuration.</param>
        /// <returns>
        /// The <see cref="BsonSerializationConfigurationType"/> corresponding to the specified configuration type.
        /// </returns>
        public static BsonSerializationConfigurationType ToBsonSerializationConfigurationType(
            this Type bsonSerializationConfigurationType)
        {
            var result = new BsonSerializationConfigurationType(bsonSerializationConfigurationType);

            return result;
        }

        /// <summary>
        /// Builds a <see cref="TypeToRegisterForBson"/> from a type.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Optional <see cref="MemberTypesToInclude"/>.  DEFAULT is <see cref="MemberTypesToInclude.All"/>.</param>
        /// <param name="relatedTypesToInclude">Optional <see cref="RelatedTypesToInclude"/>.  DEFAULT is <see cref="RelatedTypesToInclude.Descendants"/>.</param>
        /// <param name="serializerBuilderFunc">Optional func that builds the <see cref="IBsonSerializer"/>.  DEFAULT is null (no serializer).</param>
        /// <param name="propertyNameWhitelist">Optional names of the properties to constrain the registration to.  DEFAULT is null (no whitelist).</param>
        /// <returns>
        /// The type to register for BSON serialization.
        /// </returns>
        public static TypeToRegisterForBson ToTypeToRegisterForBson(
            this Type type,
            MemberTypesToInclude memberTypesToInclude = MemberTypesToInclude.All,
            RelatedTypesToInclude relatedTypesToInclude = RelatedTypesToInclude.Descendants,
            Func<IBsonSerializer> serializerBuilderFunc = null,
            IReadOnlyCollection<string> propertyNameWhitelist = null)
        {
            var result = new TypeToRegisterForBson(type, memberTypesToInclude, relatedTypesToInclude, serializerBuilderFunc, propertyNameWhitelist);

            return result;
        }
    }
}