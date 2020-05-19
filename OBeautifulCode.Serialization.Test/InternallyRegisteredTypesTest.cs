// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternallyRegisteredTypesTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Linq;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using Xunit;

    public static class InternallyRegisteredTypesTest
    {
        [Fact]
        public static void Deserialize___Should_roundtrip_all_serialized_internally_required_types___When_called()
        {
            // Arrange
            // this is required to load all of the internal types
            var throwaway = SerializationConfigurationBase.InternallyRequiredTypes;

            // NOTE: as new open generic types are added to internally required types, specify some corresponding closed types
            // this is not something we can automate
            var closedGenericTypes = new[]
            {
                typeof(ConstantExpressionRepresentation<string>),
                typeof(ConstantExpressionRepresentation<DateTime>),
            };

            var modelTypes = AssemblyLoader
                .GetLoadedAssemblies()
                .GetTypesFromAssemblies()
                .Where(_ => !_.ContainsGenericParameters)
                .Where(_ => _.IsAssignableTo(typeof(IModel)))
                .Where(_ => _ != typeof(IModel))
                .Where(_ => _ != typeof(DynamicTypePlaceholder))
                .Concat(closedGenericTypes)
                .ToList();

            // Act, Assert
            foreach (var modelType in modelTypes)
            {
                var expected = AD.ummy(modelType);

                var bsonConfigType = typeof(ThrowOnUnregisteredTypeBsonSerializationConfiguration<NullBsonSerializationConfiguration>);

                var jsonConfigType = typeof(ThrowOnUnregisteredTypeJsonSerializationConfiguration<NullJsonSerializationConfiguration>);

                expected.RoundtripSerializeWithEquatableAssertion(bsonConfigType, jsonConfigType);
            }
        }
    }
}