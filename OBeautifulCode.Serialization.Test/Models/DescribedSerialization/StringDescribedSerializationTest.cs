// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringDescribedSerializationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class StringDescribedSerializationTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StringDescribedSerializationTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StringDescribedSerialization>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'payloadTypeRepresentation' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StringDescribedSerialization>();

                            var result = new StringDescribedSerialization(
                                                 null,
                                                 referenceObject.SerializerRepresentation,
                                                 referenceObject.SerializedPayload);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "payloadTypeRepresentation" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StringDescribedSerialization>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'serializerRepresentation' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StringDescribedSerialization>();

                            var result = new StringDescribedSerialization(
                                                 referenceObject.PayloadTypeRepresentation,
                                                 null,
                                                 referenceObject.SerializedPayload);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "serializerRepresentation" },
                    });
        }
    }
}