// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuppressBecause.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Build source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Internal
{
    using global::System.CodeDom.Compiler;
    using global::System.Diagnostics.CodeAnalysis;
    
    /// <summary>
    /// Standard justifications for analysis suppression.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("OBeautifulCode.Build.Analyzers", "See package version number")]
    internal static class ObcSuppressBecause
    {
        /// <summary>
        /// See the other suppression message(s) applied within the same context.
        /// </summary>
        public const string CA_ALL_SeeOtherSuppressionMessages = "See the other suppression messages applied within the same context.";

        /// <summary>
        /// We agree with the assessment.  This code needs refactoring but we cannot justify spending time on this right now.
        /// </summary>
        public const string CA_ALL_AgreeWithAssessmentAndNeedsRefactoring = "We agree with the assessment.  This code needs refactoring but we cannot justify spending time on this right now.";

        /// <summary>
        /// A static property returns an instance of the generic class that contains the property.  The property exists for convenience in creating and configuring the instance.  It is most discoverable where it is, in-context of the class being instantiated.
        /// </summary>
        public const string CA1000_DoNotDeclareStaticMembersOnGenericTypes_StaticPropertyReturnsInstanceOfContainingGenericClassAndIsConvenientAndMostDiscoverableWhereDeclared = "A static property returns an instance of the generic class that contains the property.  The property exists for convenience in creating and configuring the instance.  It is most discoverable where it is, in-context of the class being instantiated.";

        /// <summary>
        /// The only input(s) to the method are Types.  It's more concise to call the method using generic types parameters instead of a parameter signature with parameters of type Type (e.g. MyMethod&lt;MyType&gt;() instead of MyMethod(Type myType)).
        /// </summary>
        public const string CA1004_GenericMethodsShouldProvideTypeParameter_OnlyInputsToMethodAreTypesAndItsMoreConciseToCallMethodUseGenericTypeParameters = "The only input(s) to the method are Types.  It's more concise to call the method using generic types parameters instead of a parameter signature with parameters of type Type (e.g. MyMethod<MyType>() instead of MyMethod(Type myType)).";

        /// <summary>
        /// The specified paramters are required to achieve the needed functionality.
        /// </summary>
        public const string CA1005_AvoidExcessiveParametersOnGenericTypes_SpecifiedParametersRequiredForNeededFunctionality = "The specified paramters are required to achieve the needed functionality.";

        /// <summary>
        /// Console executable does not need the [assembly: CLSCompliant(true)] as it should not be shared as an assembly for reference.
        /// </summary>
        public const string CA1014_MarkAssembliesWithClsCompliant_ConsoleExeDoesNotNeedToBeClsCompliant = "Console executable does not need the [assembly: CLSCompliant(true)] as it should not be shared as an assembly for reference.";

        /// <summary>
        /// We are optimizing for the logical grouping of types rather than the number of types in a namepace.
        /// </summary>
        public const string CA1020_AvoidNamespacesWithFewTypes_OptimizeForLogicalGroupingOfTypes = "We are optimizing for the logical grouping of types rather than the number of types in a namepace.";

        /// <summary>
        /// This is not a flags enum.  Enum values are purposefully non-contiguous.
        /// </summary>
        public const string CA1027_MarkEnumsWithFlags_EnumValuesArePurposefullyNonContiguous = "This is not a flags enum.  Enum values are purposefully non-contiguous.";

        /// <summary>
        /// A visible nested type is required in unit tests.
        /// </summary>
        public const string CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting = "A visible nested type is required in unit tests.";

        /// <summary>
        /// The type exists for unit tests that require a comparable type, but do not use the type to perform any comparisons.
        /// </summary>
        public const string CA1036_OverrideMethodsOnComparableTypes_TypeCreatedForTestsThatRequireComparableTypeButDoNotUseTypeToPerformComparisons = "The type exists for unit tests that require a comparable type, but do not use the type to perform any comparisons.";

        /// <summary>
        /// When we need to identify a group of types, we prefer the use of an empty interface over an attribute because it's easier to use and results in cleaner code.
        /// </summary>
        public const string CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute = "When we need to identify a group of types, we prefer the use of an empty interface over an attribute because it's easier to use and results in cleaner code.";

        /// <summary>
        /// The type is used for test code that requires the instance field to be visible.
        /// </summary>
        public const string CA1051_DoNotDeclareVisibleInstanceFields_TypeUsedInTestingThatRequiresInstanceFieldToBeVisible = "The type is used for test code that requires the instance field to be visible.";

        /// <summary>
        /// It's ok to throw NotSupportedException for an unreachable code path.
        /// </summary>
        public const string CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotSupportedExceptionForUnreachableCodePath = "It's ok to throw NotSupportedException for an unreachable code path.";

        /// <summary>
        /// It's ok to throw NotImplementedException when a base type or implementing an interface forces us to create a member that will never be used in testing.
        /// </summary>
        public const string CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting = "It's ok to throw NotImplementedException when a base type or implementing an interface forces us to create a member that will never be used in testing.";

        /// <summary>
        /// We prefer to read <see cref="global::System.Guid" />'s string representation as lowercase.
        /// </summary>
        public const string CA1308_NormalizeStringsToUppercase_PreferGuidLowercase = "We prefer to read System.Guid's string representation as lowercase.";

        /// <summary>
        /// We disagree with the assessment that this method as excessively complex.
        /// </summary>
        public const string CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment = "We disagree with the assessment that this method as excessively complex.";

        /// <summary>
        /// The analyzer is incorrectly detecting compound words in a unit test method name.
        /// </summary>
        public const string CA1702_CompoundWordsShouldBeCasedCorrectly_AnalyzerIsIncorrectlyDetectingCompoundWordsInUnitTestMethodName = "The analyzer is incorrectly detecting compound words in a unit test method name.";

        /// <summary>
        /// The spelling of the identifier is correct in-context of the domain.
        /// </summary>
        public const string CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain = "The spelling of the identifier is correct in-context of the domain.";

        /// <summary>
        /// The identifier is suffixed with the name of the Type that it directly extends or implements to improves readability and comprehension of unit tests whre the Type is a primary concern of those tests.
        /// </summary>
        public const string CA1710_IdentifiersShouldHaveCorrectSuffix_NameDirectlyExtendedOrImplementedTypeAddedAsSuffixForTestsWhereTypeIsPrimaryConcern = "The identifier is suffixed with the name of the Type that it directly extends or implements to improves readability and comprehension of unit tests whre the Type is a primary concern of those tests.";

        /// <summary>
        /// The identifier is suffixed with it's Type name to improve readability and comprehension of unit tests where the Type is a primary concern of those tests.
        /// </summary>
        public const string CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern = "The identifier is suffixed with it's Type name to improve readability and comprehension of unit tests where the Type is a primary concern of those tests.";

        /// <summary>
        /// The name is plural.
        /// </summary>
        public const string CA1714_FlagsEnumsShouldHavePluralNames_TheNameIsPlural = "The name is plural.";

        /// <summary>
        /// The type name adds clarity to the identifier and there is no good alternative.
        /// </summary>
        public const string CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndNoGoodAlternative = "The type name adds clarity to the identifier and there is no good alternative.";

        /// <summary>
        /// The type name adds clarity to the identifier and the alternatives degrade the clarity of the identifier.
        /// </summary>
        public const string CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity = "The type name adds clarity to the identifier and the alternatives degrade the clarity of the identifier.";

        /// <summary>
        /// The identifier includes it's Type name to improve readability and comprehension of unit tests where the Type is a primary concern of those tests.
        /// </summary>
        public const string CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern = "The identifier includes it's Type name to improve readability and comprehension of unit tests where the Type is a primary concern of those tests.";

        /// <summary>
        /// The identifier includes 'Flags' to improve readability and comprehension of unit tests where the kind of Enum is a primary concern of those tests.
        /// </summary>
        public const string CA1726_UsePreferredTerms_FlagsAddedForTestsWhereEnumKindIsPrimaryConcern = "The identifier includes 'Flags' to improve readability and comprehension of unit tests where the kind of Enum is a primary concern of those tests.";

        /// <summary>
        /// The name of the Type of the identifier uses the term 'Flags' and so it's appropriate to use that term in the the name of the identifier.
        /// </summary>
        public const string CA1726_UsePreferredTerms_NameOfTypeOfIdentifierUsesTheTermFlags = "The name of the Type of the identifier uses the term 'Flags' and so it's appropriate to use that term in the the name of the identifier.";

        /// <summary>
        /// The static field(s) are declared in a code generated partial test class that should not be alterated.
        /// </summary>
        public const string CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass = "The static field(s) are declared in a code generated partial test class that should not be alterated.";

        /// <summary>
        /// The method is wired into CLAP, a framework for command-line parsing, as a verb.
        /// </summary>
        public const string CA1811_AvoidUncalledPrivateCode_MethodIsWiredIntoClapAsVerb = "The method is wired into CLAP, a framework for command-line parsing, as a verb.";

        /// <summary>
        /// The property exists for completeness.
        /// </summary>
        public const string CA1811_AvoidUncalledPrivateCode_PropertyExistsForCompleteness = "The property exists for completeness.";

        /// <summary>
        /// The class is wired into CLAP, a framework for command-line parsing, in Program.cs.
        /// </summary>
        public const string CA1812_AvoidUninstantiatedInternalClasses_ClassIsWiredIntoClapInProgramCs = "The class is wired into CLAP, a framework for command-line parsing, in Program.cs.";

        /// <summary>
        /// The class exists because it's Type is used in unit tests.
        /// </summary>
        public const string CA1812_AvoidUninstantiatedInternalClasses_ClassExistsToUseItsTypeInUnitTests = "The class exists so that it's Type can be used in unit tests.";

        /// <summary>
        /// The type is being used in testing and we explicitly do not want the type to be equatable because it has bearing on the tests.
        /// </summary>
        public const string CA1815_OverrideEqualsAndOperatorEqualsOnValueTypes_TypeUsedForTestsThatRequireTypeToNotBeEquatable = "The type is being used in testing and we explicitly do not want the type to be equatable because it has bearing on the tests.";

        /// <summary>
        /// The type is immutable.
        /// </summary>
        public const string CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable = "The type is immutable.";

        /// <summary>
        /// The reserved exception is being used in unit test code; there is no real caller that will be impacted.
        /// </summary>
        public const string CA2201_DoNotRaiseReservedExceptionTypes_UsedForUnitTesting = "The reserved exception is being used in unit test code; there is no real caller that will be impacted.";

        /// <summary>
        /// The analyzer is incorectly flagging an object as being disposed multiple times.
        /// </summary>
        public const string CA2202_DoNotDisposeObjectsMultipleTimes_AnalyzerIsIncorrectlyFlaggingObjectAsBeingDisposedMultipleTimes = "The analyzer is incorectly flagging an object as being disposed multiple times.";

        /// <summary>
        /// The public interface of the system associated with this object never exposes this object.
        /// </summary>
        public const string CA2227_CollectionPropertiesShouldBeReadOnly_PublicInterfaceNeverExposesTheObject = "The public interface of the system associated with this object never exposes this object.";

        /// <summary>
        /// The type is used in unit tests with no intention to serialize.
        /// </summary>
        public const string CA2237_MarkISerializableTypesWithSerializable_UsedForTestingWithNoIntentionToSerialize = "The type is used in unit tests with no intention to serialize.";
    }
}
