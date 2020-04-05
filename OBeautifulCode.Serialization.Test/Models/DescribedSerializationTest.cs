// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerializationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    using FakeItEasy;

    using OBeautifulCode.CodeGen.ModelObject.Recipes;

    public static partial class DescribedSerializationTest
    {
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
                                                 referenceObject.SerializerDescription);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "payloadTypeRepresentation" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<DescribedSerialization>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'serializerDescription' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<DescribedSerialization>();

                            var result = new DescribedSerialization(
                                                 referenceObject.PayloadTypeRepresentation,
                                                 referenceObject.SerializedPayload,
                                                 null);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "serializerDescription" },
                    });
        }
    }
}