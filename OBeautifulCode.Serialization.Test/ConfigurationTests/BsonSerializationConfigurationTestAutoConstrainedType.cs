// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationTestAutoConstrainedType.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Serialization.Bson;

    public class BsonSerializationConfigurationTestAutoConstrainedType : BsonSerializationConfigurationBase
    {
        public Type TypeToRegister { get; set; }

        public IReadOnlyCollection<string> ConstrainedProperties { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Only used in testing.")]
        public BsonSerializationConfigurationTestAutoConstrainedType Setup(Type type, IReadOnlyCollection<string> constrainedProperties = null)
        {
            this.TypeToRegister = type;
            this.ConstrainedProperties = constrainedProperties;

            return this;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Is actually called by reflection.")]
        public BsonClassMap RunAutomaticallyBuildBsonClassMapOnSetupTypeAndConstrainedProperties()
        {
            // AutomaticallyBuildBsonClassMap is not public and this is a shim to expose that method to test this functionality directly
            return AutomaticallyBuildBsonClassMap(this.TypeToRegister, this.ConstrainedProperties);
        }
    }

    public class DependsOnCustomThrowsConfig : BsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[] { typeof(CustomThrowsConfig).ToBsonSerializationConfigurationType() };
    }

    public class CustomThrowsConfig : BsonSerializationConfigurationBase
    {
        /// <summary>
        /// Gets the exception message being thrown.
        /// </summary>
        public const string ExceptionMessage = "Expected to be thrown.";

        protected override void FinalizeInitialization()
        {
            throw new ArgumentException(ExceptionMessage);
        }
    }

    public class CustomNoPublicConstructor : BsonSerializationConfigurationBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "notUsed", Justification = "Needed for testing.")]
        public CustomNoPublicConstructor(string notUsed)
        {
        }
    }

    public class TestVariousTypeOverloadsConfig : BsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new TypeToRegisterForBson[]
        {
            typeof(TestConfigureActionSingle).ToTypeToRegisterForBson(MemberTypesToInclude.None),
            typeof(ITestConfigureActionFromAuto).ToTypeToRegisterForBson(MemberTypesToInclude.None),
            typeof(TestConfigureActionBaseFromSub).ToTypeToRegisterForBson(MemberTypesToInclude.None),
            typeof(TestConfigureActionBaseFromAuto).ToTypeToRegisterForBson(MemberTypesToInclude.None),
            typeof(ITestConfigureActionFromInterface).ToTypeToRegisterForBson(MemberTypesToInclude.None),
        };
    }

    public class TestConfigWithSettableFields : BsonSerializationConfigurationBase
    {
#pragma warning disable SA1401 // Fields should be private
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "For testing.")]
        public IReadOnlyCollection<Type> SettableClassTypesToRegister = new Type[0];

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "For testing.")]
        public IReadOnlyCollection<Type> SettableTypesToAutoRegister = new Type[0];

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "For testing.")]
        public IReadOnlyCollection<Type> SettableClassTypesToRegisterAlongWithInheritors = new Type[0];

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "For testing.")]
        public IReadOnlyCollection<Type> SettableInterfaceTypesToRegisterImplementationOf = new Type[0];

#pragma warning restore SA1401 // Fields should be private

        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new TypeToRegisterForBson[0]
            .Concat(this.SettableClassTypesToRegister.Select(_ => _.ToTypeToRegisterForBson(MemberTypesToInclude.None)))
            .Concat(this.SettableTypesToAutoRegister.Select(_ => _.ToTypeToRegisterForBson(MemberTypesToInclude.None)))
            .Concat(this.SettableClassTypesToRegisterAlongWithInheritors.Select(_ => _.ToTypeToRegisterForBson(MemberTypesToInclude.None)))
            .Concat(this.SettableInterfaceTypesToRegisterImplementationOf.Select(_ => _.ToTypeToRegisterForBson(MemberTypesToInclude.None)))
            .ToList();
    }

    public class InvestigationConfiguration : BsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new TypeToRegisterForBson[]
        {
            typeof(IDeduceWhoLetTheDogsOut).ToTypeToRegisterForBson(MemberTypesToInclude.None),
            typeof(NamedInvestigator).ToTypeToRegisterForBson(MemberTypesToInclude.None),
            typeof(AnonymousInvestigator).ToTypeToRegisterForBson(MemberTypesToInclude.None),
        };
    }
}
