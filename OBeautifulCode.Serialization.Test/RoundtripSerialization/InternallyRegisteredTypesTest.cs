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
    using OBeautifulCode.Serialization.Recipes;
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
            Console.Write(SerializationConfigurationBase.InternallyRequiredTypes);

            // NOTE: as new open generic types are added to internally required types, specify some corresponding closed types
            // this is not something we can automate
            var closedGenericTypes = new[]
            {
                // from OBC.Type
                typeof(ExecuteOpRequestedEvent<Version, GetProtocolOp>),
                typeof(ExecuteOpRequestedEvent<GetProtocolOp>),
                typeof(NullEvent),
                typeof(NullEvent<Version>),
                typeof(EventBase<Version>),
                typeof(NullReturningOp<Version>),
                typeof(ThrowOpExecutionAbortedExceptionOp<Version>),
                typeof(ReturningOperationBase<Version>),
                typeof(NamedValue<Version>),

                // Purposefully not testing this; many of the methods needed to test roundtrip serialization (like equality) throw NotImplementedException.
                // typeof(FakeModel<Version>),

                // from OBC.Representation
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
                .Where(_ => _.Namespace != typeof(InternallyRegisteredTypesTest).Namespace)
                .Concat(closedGenericTypes)
                .ToList();

            // Act, Assert
            foreach (var modelType in modelTypes)
            {
                var expected = AD.ummy(modelType);

                var bsonConfigType = typeof(ThrowOnUnregisteredTypeBsonSerializationConfiguration<NullBsonSerializationConfiguration>);

                var jsonConfigType = typeof(ThrowOnUnregisteredTypeJsonSerializationConfiguration<NullJsonSerializationConfiguration>);

                expected.RoundtripSerializeWithBeEqualToAssertion(bsonConfigType, jsonConfigType);
            }
        }
    }
}