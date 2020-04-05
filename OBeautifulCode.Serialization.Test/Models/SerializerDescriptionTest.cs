// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializerDescriptionTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;

    using FakeItEasy;

    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Representation.System;

    public static partial class SerializerDescriptionTest
    {
        static SerializerDescriptionTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SerializerDescription>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'serializationKind' is SerializationKind.Invalid scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerDescription>();

                            var result = new SerializerDescription(
                                SerializationKind.Invalid,
                                referenceObject.SerializationFormat,
                                A.Dummy<TypeRepresentation>(),
                                referenceObject.CompressionKind,
                                referenceObject.Metadata);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "serializationKind" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SerializerDescription>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'serializationFormat' is SerializationFormat.Invalid scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerDescription>();

                            var result = new SerializerDescription(
                                referenceObject.SerializationKind,
                                SerializationFormat.Invalid,
                                A.Dummy<TypeRepresentation>(),
                                referenceObject.CompressionKind,
                                referenceObject.Metadata);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "serializationFormat" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<SerializerDescription>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'compressionKind' is CompressionKind.Invalid scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerDescription>();

                            var result = new SerializerDescription(
                                referenceObject.SerializationKind,
                                referenceObject.SerializationFormat,
                                A.Dummy<TypeRepresentation>(),
                                CompressionKind.Invalid,
                                referenceObject.Metadata);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "compressionKind" },
                    });

            ConstructorPropertyAssignmentTestScenarios
                .AddScenario(() =>
                    new ConstructorPropertyAssignmentTestScenario<SerializerDescription>
                    {
                        Name = "ConfigurationTypeRepresentation should return null passed to constructor parameter 'configurationTypeRepresentation' when getting",
                        SystemUnderTestExpectedPropertyValueFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerDescription>();

                            var result = new SystemUnderTestExpectedPropertyValue<SerializerDescription>
                            {
                                SystemUnderTest = new SerializerDescription(
                                    referenceObject.SerializationKind,
                                    referenceObject.SerializationFormat,
                                    null,
                                    referenceObject.CompressionKind,
                                    referenceObject.Metadata),
                                ExpectedPropertyValue = null,
                            };

                            return result;
                        },
                        PropertyName = nameof(SerializerDescription.ConfigurationTypeRepresentation),
                    })
                .AddScenario(() =>
                    new ConstructorPropertyAssignmentTestScenario<SerializerDescription>
                    {
                        Name = "Metadata should return empty Dictionary<string, string> when null is passed to constructor parameter 'metadata' when getting",
                        SystemUnderTestExpectedPropertyValueFunc = () =>
                        {
                            var referenceObject = A.Dummy<SerializerDescription>();

                            var result = new SystemUnderTestExpectedPropertyValue<SerializerDescription>
                            {
                                SystemUnderTest = new SerializerDescription(
                                    referenceObject.SerializationKind,
                                    referenceObject.SerializationFormat,
                                    referenceObject.ConfigurationTypeRepresentation,
                                    referenceObject.CompressionKind,
                                    null),
                                ExpectedPropertyValue = new Dictionary<string, string>(),
                            };

                            return result;
                        },
                        PropertyName = nameof(SerializerDescription.Metadata),
                        CompareActualToExpectedUsing = CompareActualToExpectedUsing.ValueEquality,
                    });
        }
    }
}
