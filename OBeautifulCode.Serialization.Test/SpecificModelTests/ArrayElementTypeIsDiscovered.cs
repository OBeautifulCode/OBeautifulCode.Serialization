// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayElementTypeIsDiscovered.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;
    using OBeautifulCode.Serialization.Json;
    using Xunit;

    public static class ArrayElementTypeIsDiscovered
    {
        [Fact]
        public static void ElementTypeOfArrayIsOnlyTypeDiscovered()
        {
            // Arrange, Act
            var configured = SerializationConfigurationManager.GetOrAddSerializationConfiguration(typeof(TypesToRegisterJsonSerializationConfiguration<TypeWithObjectArray>).ToJsonSerializationConfigurationType());

            // Assert
            configured.IsRegisteredType(typeof(TypeWithObjectArray)).Should().BeTrue();
            configured.IsRegisteredType(typeof(TypeWithObjectArrayElementType)).Should().BeTrue();
            configured.IsRegisteredType(typeof(TypeWithObjectArrayElementType[])).Should().BeFalse();
        }
    }

    public class TypeWithObjectArray
    {
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Need an array for testing purposes.")]
        public TypeWithObjectArrayElementType[] Array { get; set; }
    }

    public class TypeWithObjectArrayElementType
    {
        public string Property { get; set; }
    }
}
