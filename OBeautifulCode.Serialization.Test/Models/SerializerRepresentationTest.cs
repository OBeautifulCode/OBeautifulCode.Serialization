// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerRepresentationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Test.Internal;

    public static partial class SerializerRepresentationTest
    {
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static SerializerRepresentationTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SerializerRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'serializationKind' is SerializationKind.Invalid scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerRepresentation>();

                            var result = new SerializerRepresentation(
                                SerializationKind.Invalid,
                                A.Dummy<TypeRepresentation>(),
                                referenceObject.CompressionKind,
                                referenceObject.Metadata);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "serializationKind", "Invalid" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SerializerRepresentation>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'compressionKind' is CompressionKind.Invalid scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerRepresentation>();

                            var result = new SerializerRepresentation(
                                referenceObject.SerializationKind,
                                A.Dummy<TypeRepresentation>(),
                                CompressionKind.Invalid,
                                referenceObject.Metadata);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "compressionKind", "Invalid" },
                    });

            ConstructorPropertyAssignmentTestScenarios
                .AddScenario(() =>
                    new ConstructorPropertyAssignmentTestScenario<SerializerRepresentation>
                    {
                        Name = "ConfigurationTypeRepresentation should return null passed to constructor parameter 'serializationConfigType' when getting",
                        SystemUnderTestExpectedPropertyValueFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerRepresentation>();

                            var result = new SystemUnderTestExpectedPropertyValue<SerializerRepresentation>
                            {
                                SystemUnderTest = new SerializerRepresentation(
                                    referenceObject.SerializationKind,
                                    null,
                                    referenceObject.CompressionKind,
                                    referenceObject.Metadata),
                                ExpectedPropertyValue = null,
                            };

                            return result;
                        },
                        PropertyName = nameof(SerializerRepresentation.SerializationConfigType),
                    })
                .AddScenario(() =>
                    new ConstructorPropertyAssignmentTestScenario<SerializerRepresentation>
                    {
                        Name = "Metadata should return empty Dictionary<string, string> when null is passed to constructor parameter 'metadata' when getting",
                        SystemUnderTestExpectedPropertyValueFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerRepresentation>();

                            var result = new SystemUnderTestExpectedPropertyValue<SerializerRepresentation>
                            {
                                SystemUnderTest = new SerializerRepresentation(
                                    referenceObject.SerializationKind,
                                    referenceObject.SerializationConfigType,
                                    referenceObject.CompressionKind,
                                    null),
                                ExpectedPropertyValue = new Dictionary<string, string>(),
                            };

                            return result;
                        },
                        PropertyName = nameof(SerializerRepresentation.Metadata),
                        CompareActualToExpectedUsing = CompareActualToExpectedUsing.ValueEquality,
                    });
        }
    }
}