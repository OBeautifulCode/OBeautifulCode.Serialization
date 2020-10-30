// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationConfigurationBase.Static.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using OBeautifulCode.Collection.Recipes;
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

        private static readonly Dictionary<Assembly, HashSet<Assembly>> AssemblyToRecursivelyReferencedAssemblyMap = new Dictionary<Assembly, HashSet<Assembly>>();

        private static readonly HashSet<Assembly> AssembliesThatHaveBeenProcessedForRelatedTypes = new HashSet<Assembly>();

        private static readonly Dictionary<Type, HashSet<Type>> TypeToAncestorTypesMap = new Dictionary<Type, HashSet<Type>>(VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance);

        private static readonly Dictionary<Type, HashSet<Type>> TypeToDescendantTypesMap = new Dictionary<Type, HashSet<Type>>(VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance);

        private static readonly HashSet<Type> TypesToExploreBlacklist = new HashSet<Type>(
            new[]
            {
                // all types are assignable to these types, so filter them out.
                typeof(string),
                typeof(object),
                typeof(ValueType),
                typeof(Enum),
                typeof(Array),
            });

        private static Dictionary<string, Assembly[]> loadedAssemblyFullNameToAssembliesMap = null;

        /// <summary>
        /// Gets the types that need to be registered for any and all serialization.
        /// </summary>
        /// <remarks>
        /// All of the model types within these subsystems will be picked-up as
        /// descendants of <see cref="IModel"/>, which is a public interface type
        /// in OBC.Type, and thus specified below.
        /// </remarks>
        public static IReadOnlyCollection<Type> InternallyRequiredTypes =>
            new Type[0]
            .Concat(new[] { typeof(IModel) })
            .Concat(OBeautifulCode.Type.ProjectInfo.Assembly.GetPublicEnumTypes())
            .Concat(OBeautifulCode.Compression.ProjectInfo.Assembly.GetPublicEnumTypes())
            .Concat(OBeautifulCode.Representation.System.ProjectInfo.Assembly.GetPublicEnumTypes())
            .ToList();

        /// <summary>
        /// Gets the namespace prefix filters to use for the <see cref="InternallyRequiredTypes"/>.
        /// </summary>
        public static IReadOnlyCollection<string> InternallyRequiredNamespacePrefixFilters =>
            new[]
            {
                OBeautifulCode.Type.ProjectInfo.Namespace,
                OBeautifulCode.Compression.ProjectInfo.Namespace,
                OBeautifulCode.Representation.System.ProjectInfo.Namespace,
                OBeautifulCode.Serialization.ProjectInfo.Namespace,
            };

        private static void SeedAncestorsAndDescendants(
            SerializationConfigurationType serializationConfigurationType)
        {
            var serializationConfigurationConcreteType = serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType;

            var assembliesToProcess = new HashSet<Assembly> { serializationConfigurationConcreteType.Assembly };

            // The universe of types that we are interested in are scoped to the dependency tree of assemblies of the serialization
            // configuration type's assembly.  The serialization configuration will depend on other serialization configuration
            // types and on domain types, which themselves will depend on other serialization configuration and domain types, and so on.
            assembliesToProcess.AddRange(GetRecursivelyReferencedAssemblies(serializationConfigurationConcreteType.Assembly));

            // However, when the serialization configuration type is NOT in-subsystem, then there's the potential to miss some
            // assemblies.  For example, lets say the consumer uses TypesToRegisterBsonSerializationConfiguration<T> and specifies
            // a T in their sub-system.  TypesToRegisterBsonSerializationConfiguration<MyType> is in the OBC.Serialization.Bson assembly
            // and using the heuristic above, we won't traverse T's assembly.  So this code recursively unpacks the generic arguments
            // (if any) of the serialization configuration type and adds the assemblies of those types to the list of assemblies to process.
            // One flaw with this approach is that the consumer could have created their own non-generic "wrapper" serialization configuration
            // in some assembly other than an assembly in the sub-system in question.  For example, they could create a class
            // MyTypeToRegisterBsonSerializationConfiguration with a constructor that takes parameter of type Type.
            // So there's certainly an improvement that could be made to the code below that inspects all of the types referenced
            // by the serialization configuration type and not just the generic parameter types.
            assembliesToProcess.AddRange(GetRecursiveGenericArgumentAssemblies(serializationConfigurationConcreteType));

            var typesToProcess = new List<Type>();

            foreach (var assemblyToProcess in assembliesToProcess)
            {
                // add types in assemblies that we haven't processed yet
                if (!AssembliesThatHaveBeenProcessedForRelatedTypes.Contains(assemblyToProcess))
                {
                    var typesToConsiderForThisAssembly = new[] { assemblyToProcess }
                        .GetTypesFromAssemblies()
                        .Where(IsRelatedTypeCandidate)
                        .ToList();

                    foreach (var typeToConsiderForThisAssembly in typesToConsiderForThisAssembly)
                    {
                        typesToProcess.Add(typeToConsiderForThisAssembly);
                    }

                    AssembliesThatHaveBeenProcessedForRelatedTypes.Add(assemblyToProcess);
                }
            }

            DiscoverAncestorsAndDescendants(typesToProcess);
        }

        private static IReadOnlyCollection<Assembly> GetRecursivelyReferencedAssemblies(
            Assembly assembly)
        {
            if (AssemblyToRecursivelyReferencedAssemblyMap.ContainsKey(assembly))
            {
                return AssemblyToRecursivelyReferencedAssemblyMap[assembly];
            }

            if (loadedAssemblyFullNameToAssembliesMap == null)
            {
                loadedAssemblyFullNameToAssembliesMap =
                    AssemblyLoader
                        .GetLoadedAssemblies()
                        .GroupBy(_ => _.GetName().FullName)
                        .ToDictionary(_ => _.Key, _ => _.ToArray());
            }

            // System has circular dependencies so for any assembly that comes into this method
            // we are going to disregard it's referenced System assemblies.
            // Anyways, these types shouldn't be explored for ancestors or descendants.
            var referencedAssemblyNames = assembly.GetReferencedAssemblies().Where(_ => (!_.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase)) && (!_.FullName.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase))).ToList();

            var notLoadedReferencedAssemblyNames = referencedAssemblyNames.Where(_ => !loadedAssemblyFullNameToAssembliesMap.ContainsKey(_.FullName)).ToList();

            foreach (var notLoadedReferencedAssemblyName in notLoadedReferencedAssemblyNames)
            {
                try
                {
                    var loadedAssembly = Assembly.Load(notLoadedReferencedAssemblyName);

                    loadedAssemblyFullNameToAssembliesMap.Add(notLoadedReferencedAssemblyName.FullName, new[] { loadedAssembly });
                }
                catch (Exception)
                {
                }
            }

            var referencedAssemblies = referencedAssemblyNames
                .Where(_ => loadedAssemblyFullNameToAssembliesMap.ContainsKey(_.FullName))
                .SelectMany(_ => loadedAssemblyFullNameToAssembliesMap[_.FullName])
                .ToList();

            var result = new HashSet<Assembly>();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                result.Add(referencedAssembly);

                result.AddRange(GetRecursivelyReferencedAssemblies(referencedAssembly));
            }

            AssemblyToRecursivelyReferencedAssemblyMap.Add(assembly, result);

            return result;
        }

        private static HashSet<Assembly> GetRecursiveGenericArgumentAssemblies(
            Type type)
        {
            var result = new HashSet<Assembly>();

            if (type.IsGenericType)
            {
                // type is a serialization configuration type; it's guaranteed to be closed
                var genericArguments = type.GenericTypeArguments;

                foreach (var genericArgument in genericArguments)
                {
                    result.Add(genericArgument.Assembly);

                    result.AddRange(GetRecursiveGenericArgumentAssemblies(genericArgument));
                }
            }

            return result;
        }

        private static IReadOnlyCollection<Type> GetRelatedTypesToInclude(
            Type type,
            RelatedTypesToInclude relatedTypesToInclude)
        {
            relatedTypesToInclude = relatedTypesToInclude == RelatedTypesToInclude.Default
                ? type.ResolveDefaultIntoActionableRelatedTypesToInclude()
                : relatedTypesToInclude;

            if (relatedTypesToInclude == RelatedTypesToInclude.None)
            {
                return new Type[0];
            }

            // short-circuit System types.  We will see these types here because we want to explore them in
            // GetMemberTypesToInclude (e.g. List<MyModel>), but we are not interested in their related types
            // (e.g. IEnumerable).
            if (type.IsSystemType())
            {
                return new Type[0];
            }

            // In a past iteration of this code we checked type.IsAssignableFrom against all loaded types, and vice versa.
            // In that iteration we purposefully avoided TypeHelper.IsAssignableTo because of performance issues related
            // to the OBeautifulCode.Validation calls in that method.  Also, IsAssignableTo throws when parameter 'type'
            // is a generic type definition.
            // Unfortunately, IsAssignableFrom is not a complete solution for generic types.
            // For generic types, IsAssignableFrom sometimes find ancestors but not always, depending on how
            // the type's inheritance is structured/declared.  Consider:
            // class Parent<T> { }
            // class Child<T> : Parent<T> { }
            // typeof(Child<>).BaseType != typeof(Parent<>)
            // typeof(Parent<>).IsAssignableFrom(typeof(Child<>)) == false
            // typeof(Child<>).BaseType is actually not a loaded type.  So this "related type" relationship cannot be discovered
            // using the methodology of the prior iteration of this code.
            DiscoverAncestorsAndDescendants(type);

            IReadOnlyCollection<Type> result;

            switch (relatedTypesToInclude)
            {
                // note we don't need to check whether TypeToAncestorTypesMap or TypeToAncestorTypesMap contains type
                // because type is guaranteed to be keys in those dictionaries after the call to DiscoverAncestorsAndDescendants() above
                case RelatedTypesToInclude.Ancestors:
                    result = TypeToAncestorTypesMap[type].ToList();
                    break;
                case RelatedTypesToInclude.Descendants:
                    result = TypeToDescendantTypesMap[type].ToList();
                    break;
                case RelatedTypesToInclude.AncestorsAndDescendants:
                    result = new Type[0].Concat(TypeToAncestorTypesMap[type]).Concat(TypeToDescendantTypesMap[type]).ToList();
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(RelatedTypesToInclude)} is not supported: {relatedTypesToInclude}"));
            }

            // there will be open generic types that are not generic type definitions
            // we can only register the generic type definitions but we want to explore both sets of types
            // (e.g. the generic type definition of IDictionary<string, MyGenericClass<T>> is IDictionary<,>,
            // which misses MyGenericClass<>)
            var genericTypeDefinitions = result
                .Where(_ => _.IsGenericType && _.ContainsGenericParameters && (!_.IsGenericTypeDefinition))
                .Select(_ => _.GetGenericTypeDefinition())
                .ToList();

            result = result
                .Concat(genericTypeDefinitions)
                .ToList();

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
                // Contrary to what is implied in documentation, a constructed generic type can still
                // contain generic parameters!  Also, and likely related, type.GetGenericArguments() can still
                // return generic parameters!  consider...
                //
                // private class GenericClassToRegister<T>
                // {
                //     public IDictionary<string, MyGenericClass<T>> MyProperty { get; set; }
                // }
                //
                // private class MyGenericClass<T> { }
                //
                // MemberTypesToInclude.DeclaredProperties will pull-in the type of MyProperty
                // When calling this method on that type, we observe the following:
                //
                // propertyType.IsConstructedGenericType == true
                // propertyType.ContainsGenericParameters == true
                // propertyType.IsGenericParameter == false
                // propertyType.IsGenericTypeDefinition == false
                //
                // var dictionaryValueType = propertyType.GetGenericArguments().Last()
                // dictionaryValueType.IsConstructedGenericType == true
                // dictionaryValueType.ContainsGenericParameters == true
                // dictionaryValueType.IsGenericParameter == false
                // dictionaryValueType.IsGenericTypeDefinition == false
                //
                // var genericParameterType = dictionaryValueType.GetGenericArguments().First()
                // genericParameterType.IsConstructedGenericType == false
                // genericParameterType.ContainsGenericParameters == true
                // genericParameterType.IsGenericParameter == true
                // genericParameterType.IsGenericTypeDefinition == false
                if (type.IsGenericType)
                {
                    // generic parameters will get filtered out below, in ...Where(IsTypeThatCanBeExplored)
                    result.AddRange(type.GenericTypeArguments);
                }
            }

            if (memberTypesToInclude.HasFlag(MemberTypesToInclude.ArrayElement))
            {
                if (type.IsArray)
                {
                    // same considerations as in the comment above about generic arguments:
                    // private class GenericClassToRegister<T>
                    // {
                    //     public IDictionary<string, MyGenericClass<T>>[] MyProperty { get; set; }
                    // }
                    result.Add(type.GetElementType());
                }
            }

            // We want to pull generic arguments and array elements from System types (e.g. List<MyModel>)
            // but otherwise we are not interested in the fields and properties of those types, which will
            // contain a bunch of other System and .NET internal types.
            if (!type.IsSystemType())
            {
                var fieldAndPropertyTypes = type
                    .GetMembersFiltered(MemberRelationships.DeclaredInType, MemberOwners.Instance, MemberAccessModifiers.All, MemberKinds.Field | MemberKinds.Property)
                    .Where(_ =>
                        ((_ is PropertyInfo) && memberTypesToInclude.HasFlag(MemberTypesToInclude.DeclaredProperties)) ||
                        ((_ is FieldInfo) && memberTypesToInclude.HasFlag(MemberTypesToInclude.DeclaredFields)))
                    .Select(_ => _.GetUnderlyingType())
                    .ToList();

                result.AddRange(fieldAndPropertyTypes);
            }

            // there will be open generic types that are not generic type definitions
            // we can only register the generic type definitions but we want to explore both sets of types
            // (e.g. the generic type definition of IDictionary<string, MyGenericClass<T>> is IDictionary<,>,
            // which misses MyGenericClass<>)
            var genericTypeDefinitions = result
                .Where(_ => _.IsGenericType && _.ContainsGenericParameters && (!_.IsGenericTypeDefinition))
                .Select(_ => _.GetGenericTypeDefinition())
                .ToList();

            result.AddRange(genericTypeDefinitions);

            result = result
                .Where(IsTypeThatCanBeExplored)
                .ToList();

            return result;
        }

        private static void DiscoverAncestorsAndDescendants(
            Type type)
        {
            // is a closed generic type?
            // this only happens if the user registered a closed generic type in their serialization configuration
            // or if a member of a type is a closed generic type (pulled-in via GetMemberTypesToInclude())
            // Assemblies do not contain closed generic types.
            if (type.IsClosedGenericType())
            {
                // the concern here is that DiscoverAncestorsAndDescendants will get all ancestors,
                // but could miss derivatives.  Consider:
                // class Child<T> : Parent<T> { }
                // class GrandChild<T> : Child<T> { }
                // if type == Child<int>, then GrandChild<int> is a descendant
                // but because GrandChild<int> is not a loaded type, DiscoverAncestorsAndDescendants()
                // will never "see" this type and thus won't be able to assign it as a derivative of Child<int>
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                // the generic type definition should have already been processed
                var genericTypeDefinitionDescendants = TypeToDescendantTypesMap[genericTypeDefinition];

                // its difficult if not impossible to try to determine the descendants from
                // genericTypeDefinitionDescendants.  consider re-defining this type:
                // class GrandChild<T2, T1> : Child<T1>
                // how would we match-up GrandChild's T1 to Child's T?
                // consider further re-defining this type:
                // class GrandChild<T> : Child<string>
                // OR
                // class GrandChild : Child<string>
                // Here the type is NOT a descendant of Child<int>, but would have already been noted as a descendant
                // of Child<string> and NOT Child<T>
                // So truly if there are any descendants here (generic or not), then we are out of luck.
                if (genericTypeDefinitionDescendants.Any())
                {
                    // Commented out this throw because consumers were hitting it too many times.
                    // Ultimately if a type is not registered then the serialization front-door will throw so we are protected.
                    // Also, these derivative types we are punting on constructing are artificial, they don't really exist.
                    // throw new NotSupportedException(Invariant($"Cannot determine which types are related to {type.ToStringReadable()} because it is a closed generic type, having a generic type definition with the following known descendants.  It not clear how to use the generic arguments of the closed type along with these descendants of the generic type definition, to construct descendants of the specified closed generic type: {genericTypeDefinitionDescendants.Select(_ => _.ToStringReadable()).ToDelimitedString(" | ")}."));
                }
            }

            DiscoverAncestorsAndDescendants(new[] { type });
        }

        private static void DiscoverAncestorsAndDescendants(
            IReadOnlyCollection<Type> types)
        {
            var typesQueue = new Queue<Type>(types);

            // note: This algorithm does not directly identify types that are related to the specified
            // generic or array types in a co-variant or contra-variant manner.
            // For example, if we are exploring IDoSomething<Animal>, we will NOT identify IDoSomething<Dog>
            // as a related type, regardless of whether IDoSomething<Dog> is assignable to IDoSomething<Animal>.
            // The consumer CAN register IDoSomething<> and separately register Animal (with RelatedTypesToInclude.Descendants,
            // which will pickup Dog) and then IDoSomething<Dog> will be considered to be a registered type.
            while (typesQueue.Any())
            {
                var typeQueueItem = typesQueue.Dequeue();

                if (TypeToAncestorTypesMap.ContainsKey(typeQueueItem))
                {
                    continue;
                }

                var ancestors = new HashSet<Type>(VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance);

                TypeToAncestorTypesMap.Add(typeQueueItem, ancestors);

                if (!TypeToDescendantTypesMap.ContainsKey(typeQueueItem))
                {
                    TypeToDescendantTypesMap.Add(typeQueueItem, new HashSet<Type>(VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance));
                }

                var ancestorTypes = new Type[0]
                    .Concat(typeQueueItem.GetInheritancePath())
                    .Concat(typeQueueItem.GetInterfaces())
                    .ToList();

                foreach (var ancestorType in ancestorTypes)
                {
                    if (!IsRelatedTypeCandidate(ancestorType))
                    {
                        continue;
                    }

                    ancestors.Add(ancestorType);

                    typesQueue.Enqueue(ancestorType);

                    if (!TypeToDescendantTypesMap.ContainsKey(ancestorType))
                    {
                        TypeToDescendantTypesMap.Add(ancestorType, new HashSet<Type>(VersionlessOpenTypeConsolidatingTypeEqualityComparer.Instance));
                    }

                    TypeToDescendantTypesMap[ancestorType].Add(typeQueueItem);
                }
            }
        }

        private static string BuildIdIgnoringOrigin(
            TypeToRegister typeToRegister)
        {
            string result;

            if (typeToRegister.Type.ContainsGenericParameters && (!typeToRegister.Type.IsGenericTypeDefinition))
            {
                // this branch is needed because ToRepresentation() below will throw on this kind of type
                result = "OPEN_" + typeToRegister.Type;
            }
            else
            {
                result = typeToRegister.Type.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();
            }

            var typesToIncludeSuffix = "_" + typeToRegister.RelatedTypesToInclude + "_" + ((int)typeToRegister.MemberTypesToInclude).ToString(CultureInfo.InvariantCulture);

            result = result + typesToIncludeSuffix;

            return result;
        }

        private static bool IsTypeThatCanBeRegistered(
            TypeToRegister typeToRegister)
        {
            var type = typeToRegister.Type;

            if (!IsTypeThatCanBeExplored(type))
            {
                return false;
            }

            // open type is only allowed if it is a generic type definition
            if (type.ContainsGenericParameters && (!type.IsGenericTypeDefinition))
            {
                return false;
            }

            if (type.IsSystemType())
            {
                // needed to register serializer for System.Drawing.Color and others
                if (!typeToRegister.IsOriginatingType)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsTypeThatCanBeExplored(
            Type type)
        {
            if (type.IsGenericParameter)
            {
                return false;
            }

            if (type.Namespace == null || // anonymous types
                type.Namespace.StartsWith("Windows", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("Microsoft", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("JetBrains", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("MS", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("Internal", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("FakeItEasy", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("Newtonsoft", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("MongoDB", StringComparison.Ordinal) ||
                type.Namespace.StartsWith("Xunit", StringComparison.Ordinal) ||
                type.Name.StartsWith("<>c", StringComparison.Ordinal))
            {
                return false;
            }

            if (TypesToExploreBlacklist.Contains(type))
            {
                return false;
            }

            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();

                if (TypesToExploreBlacklist.Contains(genericTypeDefinition))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsRelatedTypeCandidate(
            Type type)
        {
            var result = IsTypeThatCanBeExplored(type) && (!type.IsSystemType());

            return result;
        }
    }
}