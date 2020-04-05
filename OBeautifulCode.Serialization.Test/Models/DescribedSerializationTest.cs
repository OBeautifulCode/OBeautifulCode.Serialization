// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerializationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    using static System.FormattableString;

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

        [Fact]
        public static void AnonymousObject___Can_be_round_tripped_back_into_a_dynamic()
        {
            // Arrange
            var input = new { Item = "item", Items = new[] { "item1", "item2" } };
            var serializerDescriptionJson = new SerializerDescription(SerializationKind.Json, SerializationFormat.String);
            var serializerDescriptionBson = new SerializerDescription(SerializationKind.Bson, SerializationFormat.String);

            // Act
            var serializedJson = input.ToDescribedSerialization(serializerDescriptionJson);
            dynamic deserializedJson = serializedJson.DeserializePayload();

            var serializedBson = input.ToDescribedSerialization(serializerDescriptionBson);
            dynamic deserializedBson = serializedBson.DeserializePayload();

            // Assert
            ((string)deserializedJson.Item).Should().Be(input.Item);
            ((string)deserializedJson.Items[0]).Should().Be(input.Items[0]);
            ((string)deserializedJson.Items[1]).Should().Be(input.Items[1]);

            ((string)deserializedBson.Item).Should().Be(input.Item);
            ((string)deserializedBson.Items[0]).Should().Be(input.Items[0]);
            ((string)deserializedBson.Items[1]).Should().Be(input.Items[1]);
        }
    }
}