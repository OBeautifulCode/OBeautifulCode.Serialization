// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationDummyFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Serialization.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Recipes
{
    using System;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;

    /// <summary>
    /// Creates dummy objects for <see cref="OBeautifulCode.Serialization" /> types.
    /// </summary>
#if !OBeautifulCodeSerializationRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Serialization.Recipes", "See package version number")]
    internal
#else
    public
#endif
    class SerializationDummyFactory : IDummyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationDummyFactory"/> class.
        /// </summary>
        public SerializationDummyFactory()
        {
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(SerializationKind.Invalid, SerializationKind.Proprietary);
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(SerializationFormat.Invalid);
            AutoFixtureBackedDummyFactory.ConstrainDummyToExclude(RegisteredJsonConverterOutputKind.Unknown);

#if OBeautifulCodeSerializationRecipesProject
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new TypeRepresentation(A.Dummy<string>(), A.Dummy<string>(), A.Dummy<string>(), new TypeRepresentation[0]));

            AutoFixtureBackedDummyFactory.UseRandomConcreteSubclassForDummy<OBeautifulCode.Serialization.Test.KeyOrValueObjectHierarchyBase>();
            AutoFixtureBackedDummyFactory.UseRandomConcreteSubclassForDummy<OBeautifulCode.Serialization.Test.TestBase>();
#endif
        }

        /// <inheritdoc />
        public Priority Priority => new Priority(1);

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