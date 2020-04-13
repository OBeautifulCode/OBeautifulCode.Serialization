// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBaseTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    using FluentAssertions;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Serialization.Bson;

    using Xunit;

    public static class BsonSerializationConfigurationBaseTest
    {
        [Fact]
        public static void RegisterClassMapsTypeFullyAutomatic___Type_with_id_member___Works()
        {
            // Arrange
            var type = typeof(TestWithId);
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType(type);
            var expectedMemberNames = BsonSerializationConfigurationBase.GetMembersToAutomap(type).Select(_ => _.Name).OrderBy(_ => _).ToList();

            // Act
            var classMap = RunTestCode(configuration);

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
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType(type);
            var expectedMemberNames = BsonSerializationConfigurationBase.GetMembersToAutomap(type).Select(_ => _.Name).OrderBy(_ => _).ToList();

            // Act
            var classMap = RunTestCode(configuration);

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
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType(typeof(TestMapping), constraints);

            // Act
            var classMap = RunTestCode(configuration);

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
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType(typeof(TestMapping), new[] { "monkey" });
            Action action = () => RunTestCode(configuration);

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
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType(null);
            Action action = () => RunTestCode(configuration);

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
            var configuration = new BsonSerializationConfigurationTestAutoConstrainedType(null, new[] { "monkeys" });
            Action action = () => RunTestCode(configuration);

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
                                        typeof(TestConfigureActionBaseFromSub), typeof(TestConfigureActionInheritedSub),
                                        typeof(TestConfigureActionSingle),
                                        typeof(TestConfigureActionFromInterface),
                                        typeof(TestConfigureActionBaseFromAuto), typeof(TestConfigureActionInheritedAuto), typeof(TestConfigureActionFromAuto),
                                    };

            var configType = typeof(TestVariousTypeOverloadsConfig);

            // Act
            var config = SerializationConfigurationManager.GetOrAddSerializationConfiguration(configType.ToSerializationConfigurationType());

            // Assert
            config.RegisteredTypeToSerializationConfigurationTypeMap.Keys.Intersect(expectedTypes).Should().BeEquivalentTo(expectedTypes);
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
            var configType = typeof(RegisterOnlyWithDiscoveryBsonSerializationConfiguration<TestTracking>);

            // Act
            var config = SerializationConfigurationManager.GetOrAddSerializationConfiguration(configType.ToSerializationConfigurationType());

            // Assert
            config.RegisteredTypeToSerializationConfigurationTypeMap.Keys.Should().Contain(testType);
        }

        private static BsonClassMap RunTestCode(object configuration)
        {
            try
            {
                return (BsonClassMap)configuration.GetType().GetMethod("TestCode", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(configuration, null);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            throw new NotSupportedException();
        }
    }
}
