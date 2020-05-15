﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.79.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;

    using global::FakeItEasy;

    using global::OBeautifulCode.AutoFakeItEasy;
    using global::OBeautifulCode.Compression;
    using global::OBeautifulCode.Representation.System;
    using global::OBeautifulCode.Serialization;

    /// <summary>
    /// The default (code generated) Dummy Factory.
    /// Derive from this class to add any overriding or custom registrations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("OBeautifulCode.CodeGen.ModelObject", "1.0.79.0")]
    public abstract class DefaultSerializationDummyFactory : IDummyFactory
    {
        public DefaultSerializationDummyFactory()
        {
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new DescribedSerialization(
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<string>(),
                                 A.Dummy<SerializerRepresentation>(),
                                 A.Dummy<SerializationFormat>()));

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new DynamicTypePlaceholder());

            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new SerializerRepresentation(
                                 A.Dummy<SerializationKind>(),
                                 A.Dummy<TypeRepresentation>(),
                                 A.Dummy<CompressionKind>(),
                                 A.Dummy<IReadOnlyDictionary<string, string>>()));
        }

        /// <inheritdoc />
        public Priority Priority => new FakeItEasy.Priority(1);

        /// <inheritdoc />
        public bool CanCreate(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public object Create(Type type)
        {
            return null;
        }
    }
}