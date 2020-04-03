// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Reflection;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods for use with Bson serialization.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Sets the automatically-chosen serializer.
        /// </summary>
        /// <param name="map">Member the extension is on.</param>
        /// <returns>Updated <see cref="BsonMemberMap"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Prefer to use in the generic sense.")]
        public static BsonMemberMap SetObcSerializer(this BsonMemberMap map)
        {
            new { map }.AsArg().Must().NotBeNull();

            var serializer = BsonSerializationConfigurationBase.GetAppropriateSerializer(map.MemberType);

            return map.SetSerializer(serializer);
        }

        /// <summary>
        /// Gets the underlying type of a <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member"><see cref="MemberInfo"/> to check.</param>
        /// <returns>Type of the member.</returns>
        internal static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default: throw new ArgumentException("Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
            }
        }
    }
}