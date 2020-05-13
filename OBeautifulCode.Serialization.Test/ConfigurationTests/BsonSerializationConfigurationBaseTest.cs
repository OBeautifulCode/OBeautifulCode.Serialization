// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Bson;

    using Xunit;

    public static class BsonSerializationConfigurationBaseTest
    {
        [Fact]
        public static void RegisterClassMapsTypeFullyAutomatic___Type_with_id_member___Works()
        {
            // Arrange
            var type = typeof(TestWithId);
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType().Setup(type);
            var expectedMemberNames = type.GetMembersToAutomap().Select(_ => _.Name).OrderBy(_ => _).ToList();

            // Act
            var classMap = configuration.RunAutomaticallyBuildBsonClassMapOnSetupTypeAndConstrainedProperties();

            // Assert
            classMap.Should().NotBeNull();

            classMap.IdMemberMap.MemberType.Should().Be(typeof(string));
            classMap.IdMemberMap.MemberName.Should().Be(nameof(TestWithId.Id));

            var actualMemberNames = classMap.DeclaredMemberMaps.Select(_ => _.MemberName).OrderBy(_ => _).ToList();
            actualMemberNames.Should().Equal(expectedMemberNames);
        }

        [Fact]
        public static void RegisterClassMapsTypeFullyAutomatic___Type_with_no_constraints___Works()
        {
            // Arrange
            var type = typeof(TestMapping);
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType().Setup(type);
            var expectedMemberNames = type.GetMembersToAutomap().Select(_ => _.Name).OrderBy(_ => _).ToList();

            // Act
            var classMap = configuration.RunAutomaticallyBuildBsonClassMapOnSetupTypeAndConstrainedProperties();

            // Assert
            classMap.Should().NotBeNull();

            classMap.IdMemberMap.Should().BeNull();

            var actualMemberNames = classMap.DeclaredMemberMaps.Select(_ => _.MemberName).OrderBy(_ => _).ToList();
            actualMemberNames.Should().Equal(expectedMemberNames);
        }

        [Fact]
        public static void RegisterClassMapsTypeFullyAutomatic___Type_with_valid_constraints___Works()
        {
            // Arrange
            var constraints = new[] { nameof(TestMapping.GuidProperty), nameof(TestMapping.StringIntMap) };
            var expectedMemberNames = constraints.ToList();
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType().Setup(typeof(TestMapping), constraints);

            // Act
            var classMap = configuration.RunAutomaticallyBuildBsonClassMapOnSetupTypeAndConstrainedProperties();

            // Assert
            classMap.Should().NotBeNull();

            classMap.IdMemberMap.Should().BeNull();

            var actualMemberNames = classMap.DeclaredMemberMaps.Select(_ => _.MemberName).OrderBy(_ => _).ToList();
            actualMemberNames.Should().Equal(expectedMemberNames);
        }

        [Fact]
        public static void RegisterClassMapsTypeFullyAutomatic___Type_with_invalid_constraints___Throws()
        {
            // Arrange
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType().Setup(typeof(TestMapping), new[] { "monkey" });
            Action action = () => configuration.RunAutomaticallyBuildBsonClassMapOnSetupTypeAndConstrainedProperties();

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be("Provided value (name: 'constrainedPropertyDoesNotExistOnType') is not false.  Provided value is 'True'.");
        }

        [Fact]
        public static void RegisterClassMapsTypeFullyAutomatic___All_null_type___Throws()
        {
            // Arrange
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType().Setup(null);
            Action action = () => configuration.RunAutomaticallyBuildBsonClassMapOnSetupTypeAndConstrainedProperties();

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'type') is null.");
        }

        [Fact]
        public static void RegisterClassMapsTypeFullyAutomatic___Constrained_null_type___Throws()
        {
            // Arrange
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType().Setup(null, new[] { "monkeys" });
            Action action = () => configuration.RunAutomaticallyBuildBsonClassMapOnSetupTypeAndConstrainedProperties();

            // Act
            var exception = Record.Exception(action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Be("Provided value (name: 'type') is null.");
        }

        [Fact]
        public static void Configure___Override_collections___All_types_get_registered_as_expected()
        {
            var expectedTypes = new[]
            {
                typeof(TestConfigureActionBaseFromSub),
                typeof(TestConfigureActionInheritedSub),
                typeof(TestConfigureActionSingle),
                typeof(TestConfigureActionFromInterface),
                typeof(TestConfigureActionBaseFromAuto),
                typeof(TestConfigureActionInheritedAuto),
                typeof(TestConfigureActionFromAuto),
            };

            var configType = typeof(TestVariousTypeOverloadsConfig);

            // Act
            var config = SerializationConfigurationManager.GetOrAddSerializationConfiguration(configType.ToBsonSerializationConfigurationType());

            // Assert
            expectedTypes.Select(_ => config.IsRegisteredType(_)).AsTest().Must().Each().BeTrue();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "configs", Justification = "Name/spelling is correct.")]
        [Fact]
        public static void Configure___Provided_with_dependent_configs___Configures_dependents()
        {
            void Action() => SerializationConfigurationManager.GetOrAddSerializationConfiguration<DependsOnCustomThrowsConfig>();

            // Act
            var exception = Record.Exception(Action);

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().Be(CustomThrowsConfig.ExceptionMessage);
        }

        [Fact]
        public static void IsSubclassOf___Does_not_return_for_interfaces___Confirmation_test_for_internal_use()
        {
            typeof(TestConfigureActionFromInterface).IsSubclassOf(typeof(ITestConfigureActionFromInterface)).Should().BeFalse();
        }

        [Fact]
        public static void AllTrackedTypeContainers___Post_registration___Returns_fully_loaded_set()
        {
            // Arrange
            var testType = typeof(TestTracking);
            var configType = typeof(TypesToRegisterBsonSerializationConfiguration<TestTracking>);

            // Act
            var config = SerializationConfigurationManager.GetOrAddSerializationConfiguration(configType.ToBsonSerializationConfigurationType());

            // Assert
            config.IsRegisteredType(testType).Should().BeTrue();
        }
    }
}
