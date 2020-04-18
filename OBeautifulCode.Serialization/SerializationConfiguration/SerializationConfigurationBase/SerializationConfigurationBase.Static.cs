// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBase.Static.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using OBeautifulCode.Compression;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    public abstract partial class SerializationConfigurationBase
    {
        /// <summary>
        /// Gets the string representation of null.
        /// </summary>
        public const string NullSerializedStringValue = "null";

        private static readonly Dictionary<Assembly, IReadOnlyCollection<Type>> AssemblyToTypesToConsiderForRegistrationMap = new Dictionary<Assembly, IReadOnlyCollection<Type>>();

        private static readonly HashSet<Type> RelatedTypesBlacklist = new HashSet<Type>(
            new[]
            {
                // all types are assignable to these types, so filter them out.
                typeof(string),
                typeof(object),
                typeof(ValueType),
                typeof(Enum),
                typeof(Array),
            });

        /// <summary>
        /// Gets the types that need to be registered for any and all serialization.
        /// </summary>
        protected static IReadOnlyCollection<Type> InternallyRequiredTypes => new[]
        {
            // OBC.Type
            typeof(UtcDateTimeRangeInclusive),

            // OBC.Compression
            typeof(CompressionKind),

            // OBC.Representation
            typeof(AssemblyRepresentation),
            typeof(ElementInitRepresentation),
            typeof(MemberBindingRepresentationBase),
            typeof(ExpressionRepresentationBase),
            typeof(TypeRepresentation),
            typeof(ConstructorInfoRepresentation),
            typeof(MemberInfoRepresentation),
            typeof(MethodInfoRepresentation),

            // OBC.Serialization:
            typeof(SerializerDescription),
            typeof(DescribedSerialization),
            typeof(DynamicTypePlaceholder),
        };

        private static IReadOnlyCollection<Type> GetRelatedTypesToInclude(
            Type type,
            RelatedTypesToInclude relatedTypesToInclude)
        {
            if (relatedTypesToInclude == RelatedTypesToInclude.None)
            {
                return new Type[0];
            }

            if (IsSystemType(type))
            {
                return new Type[0];
            }

            var allTypesToConsiderForRegistration = GetAllTypesToConsiderForRegistration();

            IReadOnlyCollection<Type> result;

            // note: we are PURPOSELY not using OBeautifulCode.Reflection.Recipes.TypeHelper.IsAssignableTo
            // because of performance issues related to the OBeautifulCode.Validation calls in that method.
            switch (relatedTypesToInclude)
            {
                case RelatedTypesToInclude.Ancestors:
                    result = allTypesToConsiderForRegistration.Where(_ => _.IsAssignableFrom(type)).ToList();
                    break;
                case RelatedTypesToInclude.Descendants:
                    // ReSharper disable once ConvertClosureToMethodGroup
                    result = allTypesToConsiderForRegistration.Where(_ => type.IsAssignableFrom(_)).ToList();
                    break;
                case RelatedTypesToInclude.AncestorsAndDescendants:
                    result = allTypesToConsiderForRegistration.Where(_ => type.IsAssignableFrom(_) || _.IsAssignableFrom(type)).ToList();
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(RelatedTypesToInclude)} is not supported: {relatedTypesToInclude}"));
            }

            result = result.Where(_ => _ != type).ToList();

            return result;
        }

        private static IReadOnlyCollection<Type> GetMemberTypesToInclude(
            Type type,
            MemberTypesToInclude memberTypesToInclude)
        {
            if (memberTypesToInclude == MemberTypesToInclude.None)
            {
                return new Type[0];
            }

            var result = new List<Type>();

            if (memberTypesToInclude.HasFlag(MemberTypesToInclude.GenericArguments))
            {
                if (type.IsGenericType)
                {
                    result.AddRange(type.GenericTypeArguments);
                }
            }

            if (memberTypesToInclude.HasFlag(MemberTypesToInclude.ArrayElement))
            {
                if (type.IsArray)
                {
                    result.Add(type.GetElementType());
                }
            }

            if (!IsSystemType(type))
            {
                bool IsCompilerGenerated(MemberInfo memberInfo) => memberInfo.CustomAttributes.Select(s => s.AttributeType).Contains(typeof(CompilerGeneratedAttribute));

                result.AddRange(
                    type
                        .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(_ => !IsCompilerGenerated(_))
                        .SelectMany(
                            _ =>
                            {
                                if ((_ is PropertyInfo propertyInfo) && memberTypesToInclude.HasFlag(MemberTypesToInclude.DeclaredProperties))
                                {
                                    return new[] { propertyInfo.PropertyType };
                                }

                                if ((_ is FieldInfo fieldInfo) && memberTypesToInclude.HasFlag(MemberTypesToInclude.DeclaredFields))
                                {
                                    return new[] { fieldInfo.FieldType };
                                }

                                return new Type[0];
                            }));
            }

            // result = result.Where(_ => _.IsClosedNonAnonymousClassType()).ToList() // older versions of Serialization filtered to closed non-anonymous class types but we believe this is algorithmically incomplete
            result = result
                .Where(_ => IsTypeThatCanBeRegistered(_, allowSystemType: true))
                .Where(_ => _ != type)
                .ToList();

            return result;
        }

        private static IReadOnlyCollection<Type> GetAllTypesToConsiderForRegistration()
        {
            var assemblies = AssemblyLoader.GetLoadedAssemblies();

            foreach (var assembly in assemblies)
            {
                if (!AssemblyToTypesToConsiderForRegistrationMap.ContainsKey(assembly))
                {
                    var typesToConsiderForThisAssembly = new[] { assembly }
                        .GetTypesFromAssemblies()
                        .Where(_ => IsTypeThatCanBeRegistered(_, allowSystemType: false)) // && _.IsClass  In an old version of Serialization we only included class types but twe think this is algorithmically incorrect because in certain situations it will exclude types that should be identified as "related" in GetRelatedTypesToInclude
                        .ToList();

                    AssemblyToTypesToConsiderForRegistrationMap.Add(assembly, typesToConsiderForThisAssembly);
                }
            }

            var result = AssemblyToTypesToConsiderForRegistrationMap.Values.SelectMany(_ => _).ToList();

            return result;
        }

        private static string BuildIdIgnoringOrigin(
            TypeToRegister typeToRegister)
        {
            var result = typeToRegister.Type.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName() + "_" + typeToRegister.RelatedTypesToInclude + "_" + (int)typeToRegister.MemberTypesToInclude;

            return result;
        }

        private static bool IsSystemType(
            Type type)
        {
            var result = type.Namespace?.StartsWith(nameof(System), StringComparison.Ordinal) ?? false;

            return result;
        }

        private static bool IsTypeThatCanBeRegistered(
            TypeToRegister typeToRegister)
        {
            var type = typeToRegister.Type;

            if (!IsTypeThatCanBeRegistered(type, allowSystemType: true))
            {
                return false;
            }

            if (IsSystemType(type) && (!typeToRegister.IsOriginatingType))
            {
                return false;
            }

            if (type.IsArray || type.IsClosedSystemDictionaryType() || type.IsClosedSystemCollectionType() || type.IsClosedSystemEnumerableType())
            {
                return false;
            }

            return true;
        }

        private static bool IsTypeThatCanBeRegistered(
            Type type,
            bool allowSystemType)
        {
            if (type.ContainsGenericParameters)
            {
                return false;
            }

            if (RelatedTypesBlacklist.Contains(type))
            {
                return false;
            }

            if (type.IsClosedAnonymousType())
            {
                return false;
            }

            if ((!allowSystemType) && IsSystemType(type))
            {
                return false;
            }

            return true;
        }
    }
}