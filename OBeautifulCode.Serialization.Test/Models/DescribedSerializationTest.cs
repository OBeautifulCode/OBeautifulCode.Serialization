// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerializationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Serialization.Test.Internal;

    public static partial class DescribedSerializationTest
    {
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static DescribedSerializationTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<DescribedSerialization>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'payloadTypeRepresentation' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<DescribedSerialization>();

                            var result = new DescribedSerialization(
                                                 null,
                                                 referenceObject.SerializedPayload,
                                                 referenceObject.SerializerRepresentation,
                                                 referenceObject.SerializationFormat);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "payloadTypeRepresentation" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<DescribedSerialization>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'serializerRepresentation' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<DescribedSerialization>();

                            var result = new DescribedSerialization(
                                                 referenceObject.PayloadTypeRepresentation,
                                                 referenceObject.SerializedPayload,
                                                 null,
                                                 referenceObject.SerializationFormat);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "serializerRepresentation" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<DescribedSerialization>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'serializationFormat' is SerializationFormat.Invalid scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<DescribedSerialization>();

                            var result = new DescribedSerialization(
                                referenceObject.PayloadTypeRepresentation,
                                referenceObject.SerializedPayload,
                                referenceObject.SerializerRepresentation,
                                SerializationFormat.Invalid);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "serializationFormat", "Invalid" },
                    });
        }
    }
}