// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBase.Static.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    using static System.FormattableString;

    public abstract partial class BsonSerializationConfigurationBase
    {
        private const string DefaultIdMemberName = "Id";

        /// <summary>
        /// Configures a <see cref="BsonClassMap"/> automatically based on the members of the provided type.
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="constrainToProperties">Optional list of properties to constrain type members to (null or 0 will mean all).</param>
        /// <returns>
        /// Configured <see cref="BsonClassMap"/>.
        /// </returns>
        protected static BsonClassMap AutomaticallyBuildBsonClassMap(
            Type type,
            IReadOnlyCollection<string> constrainToProperties = null)
        {
            new { type }.AsArg().Must().NotBeNull();

            var result = new BsonClassMap(type);

            var constraintsAreNullOrEmpty = constrainToProperties == null || constrainToProperties.Count == 0;

            var allMembers = type.GetMembersToAutomap();

            var members = allMembers.Where(_ => constraintsAreNullOrEmpty || constrainToProperties.Contains(_.Name)).ToList();

            if (!constraintsAreNullOrEmpty)
            {
                var allMemberNames = allMembers.Select(_ => _.Name).ToList();

                constrainToProperties.Any(_ => !allMemberNames.Contains(_)).AsArg("constrainedPropertyDoesNotExistOnType").Must().BeFalse();
            }

            foreach (var member in members)
            {
                var memberType = member.GetUnderlyingType();

                memberType.AsArg(Invariant($"{member.Name}-{nameof(MemberInfo.DeclaringType)}")).Must().NotBeNull();

                try
                {
                    var memberMap = MapMember(result, member);

                    var serializer = memberType.GetAppropriateSerializer(defaultToObjectSerializer: false);

                    if (serializer != null)
                    {
                        memberMap.SetSerializer(serializer);
                    }
                }
                catch (Exception ex)
                {
                    throw new BsonSerializationConfigurationException(Invariant($"Error automatically mapping; type: {type}, member: {member}"), ex);
                }
            }

            return result;
        }

        private static BsonMemberMap MapMember(
            BsonClassMap bsonClassMap,
            MemberInfo member)
        {
            var result = DefaultIdMemberName.Equals(member.Name, StringComparison.OrdinalIgnoreCase)
                ? bsonClassMap.MapIdMember(member) // TODO: add logic to make sure ID is of acceptable type here...
                : bsonClassMap.MapMember(member);

            return result;
        }
    }
}