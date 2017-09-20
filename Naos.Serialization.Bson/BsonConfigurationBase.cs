﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonConfigurationBase.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using MongoDB.Bson.Serialization;

    using Spritely.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating
    /// </summary>
    public abstract class BsonConfigurationBase
    {
        private const BindingFlags DefaultMongoBsonMemberSelectionBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private const string RegisterClassMapMethodName = nameof(BsonClassMap.RegisterClassMap);

        private static readonly MethodInfo RegisterClassMapGenericMethod = typeof(BsonClassMap).GetMethods().Single(_ => (_.Name == RegisterClassMapMethodName) && (!_.GetParameters().Any()) && _.IsGenericMethod);

        private readonly object syncConfigure = new object();

        private bool configured;

        /// <summary>
        /// Run configuration logic.
        /// </summary>
        public void Configure()
        {
            if (!this.configured)
            {
                lock (this.syncConfigure)
                {
                    if (!this.configured)
                    {
                        if (this.ShouldRegisterEnumConvention)
                        {
                            NaosBsonConventions.RegisterEnumAsStringConventionIfNotRegistered();
                        }

                        foreach (var dependantMapperType in this.DependentMapperTypes)
                        {
                            BsonConfigurationManager.Configure(dependantMapperType);
                        }

                        this.CustomConfiguration();

                        this.configured = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not to run <see cref="NaosBsonConventions.RegisterEnumAsStringConventionIfNotRegistered"/>.  Optionally overrideable, DEFAULT is true.
        /// </summary>
        protected virtual bool ShouldRegisterEnumConvention => true;

        /// <summary>
        /// Gets a list of <see cref="BsonConfigurationBase"/>'s that are needed for the current implemenation of <see cref="BsonConfigurationBase"/>.  Optionally overrideable, DEFAULT is empty collection.
        /// </summary>
        protected virtual IReadOnlyCollection<Type> DependentMapperTypes => new Type[0];

        /// <summary>
        /// Template method to override and specify custom logic.
        /// </summary>
        protected abstract void CustomConfiguration();

        /// <summary>
        /// Method to use relection and call <see cref="BsonClassMap.RegisterClassMap{TClass}()"/> using the <see cref="Type"/> as a parameter.
        /// </summary>
        /// <param name="type">Type to register.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterClassMapForType(Type type)
        {
            new { type }.Must().NotBeNull().OrThrowFirstFailure();

            try
            {
                var genericRegisterClassMapMethod = RegisterClassMapGenericMethod.MakeGenericMethod(type);
                genericRegisterClassMapMethod.Invoke(null, null);
            }
            catch (Exception ex)
            {
                throw new BsonConfigurationException(Invariant($"Failed to run {RegisterClassMapMethodName} on {type.FullName}"), ex);
            }
        }

        /// <summary>
        /// Method to use relection and call <see cref="BsonClassMap.RegisterClassMap{TClass}()"/> using the <see cref="Type"/> as a parameter.
        /// </summary>
        /// <param name="types">Types to register.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterClassMapForType(IReadOnlyCollection<Type> types)
        {
            new { types }.Must().NotBeNull().OrThrowFirstFailure();

            foreach (var type in types)
            {
                this.RegisterClassMapForType(type);
            }
        }

        /// <summary>
        /// Method to register the specified type and all derivative types in the same assembly.
        /// </summary>
        /// <param name="types">Types to register.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterClassMapForTypeAndSubclassTypes(IReadOnlyCollection<Type> types)
        {
            new { types }.Must().NotBeNull().OrThrowFirstFailure();

            var allTypes = types.SelectMany(_ => this.GetSubclassTypes(_)).Distinct().ToList();

            this.RegisterClassMapForType(allTypes);
        }

        /// <summary>
        /// Method to register the specified type and all derivative types in the same assembly.
        /// </summary>
        /// <param name="types">Types to register.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterClassMapForTypeAndSubclassTypesWithAutomaticMapping(IReadOnlyCollection<Type> types)
        {
            new { types }.Must().NotBeNull().OrThrowFirstFailure();

            var allTypes = types.SelectMany(_ => this.GetSubclassTypes(_)).Distinct().ToList();

            this.RegisterClassMapForTypeWithAutomaticMapping(allTypes);
        }

        /// <summary>
        /// Register class maps by automatically inferring usage from types; can optionally be filtered to a set of properties.
        /// </summary>
        /// <typeparam name="T">Type to register.</typeparam>
        /// <param name="constrainToProperties">Optional list of properties to constrain type members to (null or 0 will mean all).</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Want this to be used with generic type declaration.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterClassMapForTypeWithAutomaticMapping<T>(IReadOnlyCollection<string> constrainToProperties = null)
        {
            this.RegisterClassMapForTypeWithAutomaticMapping(typeof(T), constrainToProperties);
        }

        /// <summary>
        /// Method to use relection and call <see cref="BsonClassMap.RegisterClassMap{TClass}()"/> using the <see cref="Type"/> as a parameter.
        /// </summary>
        /// <param name="types">Types to register.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterClassMapForTypeWithAutomaticMapping(IReadOnlyCollection<Type> types)
        {
            new { types }.Must().NotBeNull().OrThrowFirstFailure();

            foreach (var type in types)
            {
                this.RegisterClassMapForTypeWithAutomaticMapping(type);
            }
        }

        /// <summary>
        /// Register class maps by automatically inferring usage from types; can optionally be filtered to a set of properties.
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="constrainToProperties">Optional list of properties to constrain type members to (null or 0 will mean all).</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Like this structure.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected void RegisterClassMapForTypeWithAutomaticMapping(Type type, IReadOnlyCollection<string> constrainToProperties = null)
        {
            new { type }.Must().NotBeNull().OrThrowFirstFailure();

            var bsonClassMap = this.AutomaticallyBuildBsonClassMap(type, constrainToProperties);

            BsonClassMap.RegisterClassMap(bsonClassMap);
        }

        /// <summary>
        /// Configures a <see cref="BsonClassMap"/> automatically based on the members of the provided type.
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="constrainToProperties">Optional list of properties to constrain type members to (null or 0 will mean all).</param>
        /// <returns>Configured <see cref="BsonClassMap"/>.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Like this structure.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected BsonClassMap AutomaticallyBuildBsonClassMap(Type type, IReadOnlyCollection<string> constrainToProperties)
        {
            new { type }.Must().NotBeNull().OrThrowFirstFailure();

            var bsonClassMap = new BsonClassMap(type);

            var constraintsAreNullOrEmpty = constrainToProperties == null || constrainToProperties.Count == 0;

            var allMembers = type.GetMembers(DefaultMongoBsonMemberSelectionBindingFlags)
                .Where(_ => _.MemberType == MemberTypes.Field || _.MemberType == MemberTypes.Property)
                .Where(_ => !_.CustomAttributes.Select(s => s.AttributeType).Contains(typeof(CompilerGeneratedAttribute))).ToList();

            var members = allMembers.Where(_ => constraintsAreNullOrEmpty || constrainToProperties.Contains(_.Name)).ToList();

            if (!constraintsAreNullOrEmpty)
            {
                var allMemberNames = allMembers.Select(_ => _.Name).ToList();
                constrainToProperties.Any(_ => !allMemberNames.Contains(_)).Named("constrainedPropertyDoesNotExistOnType").Must().BeFalse().OrThrowFirstFailure();
            }

            foreach (var member in members)
            {
                var memberType = member.GetUnderlyingType();
                memberType.Named(Invariant($"{member.Name}-{nameof(MemberInfo.DeclaringType)}")).Must().NotBeNull().OrThrowFirstFailure();

                try
                {
                    if (memberType.IsEnum)
                    {
                        bsonClassMap.MapMember(member).SetEnumStringSerializer();
                    }
                    else if (memberType == typeof(DateTime))
                    {
                        bsonClassMap.MapMember(member).SetDateTimeStringSerializer();
                    }
                    else if (memberType.IsArray && memberType.GetElementType().IsEnum)
                    {
                        bsonClassMap.MapMember(member).SetEnumArraySerializer();
                    }
                    else if (memberType.IsGenericType && memberType.GetInterfaces().Where(_ => _.IsGenericType).Select(_ => _.GetGenericTypeDefinition())
                                 .Contains(typeof(IDictionary<,>)))
                    {
                        bsonClassMap.MapMember(member).SetDictionarySerializer();
                    }
                    else
                    {
                        bsonClassMap.MapMember(member);
                    }
                }
                catch (Exception ex)
                {
                    throw new BsonConfigurationException(Invariant($"Error automatically mapping; type: {type}, member: {member}"), ex);
                }
            }

            return bsonClassMap;
        }

        /// <summary>
        /// Get a list of the subclass types of the provided type and the provided type if <paramref name="includeSpecifiedTypeInReturnList"/> is true.
        /// </summary>
        /// <param name="classType">Type to find derivatives of.</param>
        /// <param name="includeSpecifiedTypeInReturnList">Optional value indicating whether or not to include the provided type in the return list; DEFAULT is true.</param>
        /// <returns>List of the subclass types of the provided type and the provided type if <paramref name="includeSpecifiedTypeInReturnList"/> is true.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Want to be used from derivatives using 'this.'")]
        protected IReadOnlyCollection<Type> GetSubclassTypes(Type classType, bool includeSpecifiedTypeInReturnList = true)
        {
            new { classType }.Must().NotBeNull().OrThrowFirstFailure();
            new { classType.IsClass }.Must().BeTrue().OrThrowFirstFailure();

            var derivativeTypes = classType.Assembly.GetTypes().Where(_ => _.IsSubclassOf(classType)).ToList();

            if (includeSpecifiedTypeInReturnList)
            {
                derivativeTypes.Add(classType);
            }

            return derivativeTypes.Distinct().ToList();
        }
    }

    /// <summary>
    /// Null implementation of <see cref="BsonConfigurationBase"/>.
    /// </summary>
    public sealed class NullBsonConfiguration : BsonConfigurationBase
    {
        /// <inheritdoc cref="BsonConfigurationBase"/>
        protected override void CustomConfiguration()
        {
            /* no-op */
        }
    }
}