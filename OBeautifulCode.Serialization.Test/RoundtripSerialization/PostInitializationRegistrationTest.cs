﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostInitializationRegistrationTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    public static class PostInitializationRegistrationTest
    {
        [Fact]
        public static void RoundtripSerialize___Should_activate_post_initialization_registration___When_type_is_a_concrete_closed_generic_having_a_generic_ancestor()
        {
            // Arrange
            var expected = new ModelPrivateSetGenericParentGenericChild<Version, Guid>(
                A.Dummy<IReadOnlyDictionary<Version, Guid>>(), A.Dummy<IReadOnlyDictionary<Version, Guid>>());

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationJsonConfiguration));
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationBsonConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_activate_post_initialization_registration___When_type_is_a_concrete_closed_generic_declared_as_a_generic_ancestor()
        {
            // Arrange
            ModelPrivateSetGenericParent<Version, Guid> expected = new ModelPrivateSetGenericParentGenericChild<Version, Guid>(
                A.Dummy<IReadOnlyDictionary<Version, Guid>>(), A.Dummy<IReadOnlyDictionary<Version, Guid>>());

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationJsonConfiguration));
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationBsonConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_activate_post_initialization_registration___When_type_is_a_concrete_closed_generic_declared_having_concrete_and_abstract_generic_members()
        {
            // Arrange
            var expected = new Wrapper<Version, Guid>(
                A.Dummy<ModelPrivateSetGenericParentGenericChild<Version, Guid>>(), A.Dummy<ModelPrivateSetGenericParentGenericChild<Version, Guid>>());

            // Act, Assert
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationJsonConfiguration));
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationBsonConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_activate_post_initialization_registration___When_type_contains_dictionary_whose_values_are_declared_as_a_generic_concrete_type()
        {
            var expected = new ModelPrivateSetGenericParentGenericChild<string, ModelPublicSetNonGenericParentGenericChild<Version>>(
                new Dictionary<string, ModelPublicSetNonGenericParentGenericChild<Version>>
                {
                    { "abc", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" } },
                    { "def", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" } },
                },
                new Dictionary<string, ModelPublicSetNonGenericParentGenericChild<Version>>
                {
                    { "abc", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" } },
                    { "def", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" } },
                });

            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationJsonConfiguration));
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationBsonConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_activate_post_initialization_registration___When_type_contains_dictionary_whose_keys_are_declared_as_a_generic_concrete_type()
        {
            var expected = new ModelPrivateSetGenericParentGenericChild<ModelPublicSetNonGenericParentGenericChild<Version>, string>(
                new Dictionary<ModelPublicSetNonGenericParentGenericChild<Version>, string>
                {
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" }, "abc" },
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" }, "def" },
                },
                new Dictionary<ModelPublicSetNonGenericParentGenericChild<Version>, string>
                {
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" }, "abc" },
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" }, "def" },
                });

            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationJsonConfiguration));
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationBsonConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_activate_post_initialization_registration___When_type_contains_dictionary_whose_value_is_declared_as_non_generic_base_class_having_generic_inheritors()
        {
            var expected = new ModelPrivateSetGenericParentGenericChild<string, ModelPublicSetNonGenericParent>(
                new Dictionary<string, ModelPublicSetNonGenericParent>
                {
                    { "abc", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" } },
                    { "def", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" } },
                },
                new Dictionary<string, ModelPublicSetNonGenericParent>
                {
                    { "abc", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" } },
                    { "def", new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" } },
                });

            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationJsonConfiguration));
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationBsonConfiguration));
        }

        [Fact]
        public static void RoundtripSerialize___Should_activate_post_initialization_registration___When_type_contains_dictionary_whose_key_is_declared_as_non_generic_base_class_having_generic_inheritors()
        {
            var expected = new ModelPrivateSetGenericParentGenericChild<ModelPublicSetNonGenericParent, string>(
                new Dictionary<ModelPublicSetNonGenericParent, string>
                {
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" }, "abc" },
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" }, "def" },
                },
                new Dictionary<ModelPublicSetNonGenericParent, string>
                {
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "abc-details" }, "abc" },
                    { new ModelPublicSetNonGenericParentGenericChild<Version> { Detail = "def-details" }, "def" },
                });

            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationJsonConfiguration));
            expected.RoundtripSerializeViaBsonWithBeEqualToAssertion(typeof(PostInitializationRegistrationBsonConfiguration));
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class Wrapper<TFirst, TSecond>
            where TSecond : struct
        {
            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            public Wrapper(
                ModelPrivateSetGenericParent<TFirst, TSecond> privateSetGenericParent,
                ModelPrivateSetGenericParentGenericChild<TFirst, TSecond> privateSetGenericParentGenericChild)
            {
                this.PrivateSetGenericParent = privateSetGenericParent;
                this.PrivateSetGenericParentGenericChild = privateSetGenericParentGenericChild;
            }

            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            public ModelPrivateSetGenericParent<TFirst, TSecond> PrivateSetGenericParent { get; private set; }

            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            public ModelPrivateSetGenericParentGenericChild<TFirst, TSecond> PrivateSetGenericParentGenericChild { get; private set; }

            public static bool operator ==(Wrapper<TFirst, TSecond> left, Wrapper<TFirst, TSecond> right)
            {
                if (ReferenceEquals(left, right))
                {
                    return true;
                }

                if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                {
                    return false;
                }

                var result = left.Equals(right);

                return result;
            }

            public static bool operator !=(Wrapper<TFirst, TSecond> left, Wrapper<TFirst, TSecond> right) => !(left == right);

            public bool Equals(Wrapper<TFirst, TSecond> other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.PrivateSetGenericParent.IsEqualTo(other.PrivateSetGenericParent)
                          && this.PrivateSetGenericParentGenericChild.IsEqualTo(other.PrivateSetGenericParentGenericChild);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as Wrapper<TFirst, TSecond>);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => throw new NotImplementedException("should not get used");
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public abstract class ModelPrivateSetGenericParent<TFirst, TSecond>
            : IEquatable<ModelPrivateSetGenericParent<TFirst, TSecond>>
        {
            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            protected ModelPrivateSetGenericParent(
                IReadOnlyDictionary<TFirst, TSecond> parentGenericReadOnlyDictionaryProperty1)
            {
                this.ParentGenericReadOnlyDictionaryProperty1 = parentGenericReadOnlyDictionaryProperty1;
            }

            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            public IReadOnlyDictionary<TFirst, TSecond> ParentGenericReadOnlyDictionaryProperty1 { get; private set; }

            public static bool operator ==(ModelPrivateSetGenericParent<TFirst, TSecond> left, ModelPrivateSetGenericParent<TFirst, TSecond> right)
            {
                if (ReferenceEquals(left, right))
                {
                    return true;
                }

                if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                {
                    return false;
                }

                var result = left.Equals((object)right);

                return result;
            }

            public static bool operator !=(ModelPrivateSetGenericParent<TFirst, TSecond> left, ModelPrivateSetGenericParent<TFirst, TSecond> right) => !(left == right);

            public bool Equals(ModelPrivateSetGenericParent<TFirst, TSecond> other) => this == other;

            public abstract override bool Equals(object obj);

            public abstract override int GetHashCode();
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public partial class ModelPrivateSetGenericParentGenericChild<TFirst, TSecond>
            : ModelPrivateSetGenericParent<TFirst, TSecond>, IEquatable<ModelPrivateSetGenericParentGenericChild<TFirst, TSecond>>
        {
            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            public ModelPrivateSetGenericParentGenericChild(
                IReadOnlyDictionary<TFirst, TSecond> parentGenericReadOnlyDictionaryProperty1,
                IReadOnlyDictionary<TFirst, TSecond> childGenericReadOnlyDictionaryProperty1)
                : base(parentGenericReadOnlyDictionaryProperty1)
            {
                this.ChildGenericReadOnlyDictionaryProperty1 = childGenericReadOnlyDictionaryProperty1;
            }

            [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddedToIdentifierForTestsWhereTypeIsPrimaryConcern)]
            public IReadOnlyDictionary<TFirst, TSecond> ChildGenericReadOnlyDictionaryProperty1 { get; private set; }

            public static bool operator ==(ModelPrivateSetGenericParentGenericChild<TFirst, TSecond> left, ModelPrivateSetGenericParentGenericChild<TFirst, TSecond> right)
            {
                if (ReferenceEquals(left, right))
                {
                    return true;
                }

                if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                {
                    return false;
                }

                var result = left.Equals(right);

                return result;
            }

            public static bool operator !=(ModelPrivateSetGenericParentGenericChild<TFirst, TSecond> left, ModelPrivateSetGenericParentGenericChild<TFirst, TSecond> right) => !(left == right);

            public bool Equals(ModelPrivateSetGenericParentGenericChild<TFirst, TSecond> other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.ParentGenericReadOnlyDictionaryProperty1.IsEqualTo(other.ParentGenericReadOnlyDictionaryProperty1)
                          && this.ChildGenericReadOnlyDictionaryProperty1.IsEqualTo(other.ChildGenericReadOnlyDictionaryProperty1);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelPrivateSetGenericParentGenericChild<TFirst, TSecond>);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => throw new NotImplementedException("should not get used");
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public abstract class ModelPublicSetNonGenericParent : IEquatable<ModelPublicSetNonGenericParent>
        {
            public static bool operator ==(ModelPublicSetNonGenericParent left, ModelPublicSetNonGenericParent right)
            {
                if (ReferenceEquals(left, right))
                {
                    return true;
                }

                if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                {
                    return false;
                }

                var result = left.Equals((object)right);

                return result;
            }

            public static bool operator !=(ModelPublicSetNonGenericParent left, ModelPublicSetNonGenericParent right) => !(left == right);

            public bool Equals(ModelPublicSetNonGenericParent other) => this == other;

            public abstract override bool Equals(object obj);

            public abstract override int GetHashCode();
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ModelPublicSetNonGenericParentGenericChild<T> : ModelPublicSetNonGenericParent, IEquatable<ModelPublicSetNonGenericParentGenericChild<T>>
        {
            public string Detail { get; set; }

            public static bool operator ==(ModelPublicSetNonGenericParentGenericChild<T> left, ModelPublicSetNonGenericParentGenericChild<T> right)
            {
                if (ReferenceEquals(left, right))
                {
                    return true;
                }

                if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                {
                    return false;
                }

                var result = left.Equals(right);

                return result;
            }

            public static bool operator !=(ModelPublicSetNonGenericParentGenericChild<T> left, ModelPublicSetNonGenericParentGenericChild<T> right) => !(left == right);

            public bool Equals(ModelPublicSetNonGenericParentGenericChild<T> other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.Detail.IsEqualTo(other.Detail);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelPublicSetNonGenericParentGenericChild<T>);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => this.Detail.GetHashCode();
        }

        private class PostInitializationRegistrationBsonConfiguration : BsonSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
            {
                typeof(Wrapper<,>).ToTypeToRegisterForBson(),
                typeof(ModelPrivateSetGenericParent<,>).ToTypeToRegisterForBson(),
                typeof(ModelPrivateSetGenericParentGenericChild<,>).ToTypeToRegisterForBson(),
                typeof(ModelPublicSetNonGenericParent).ToTypeToRegisterForBson(),
                typeof(ModelPublicSetNonGenericParentGenericChild<>).ToTypeToRegisterForBson(),
            };
        }

        private class PostInitializationRegistrationJsonConfiguration : JsonSerializationConfigurationBase
        {
            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                typeof(Wrapper<,>).ToTypeToRegisterForJson(),
                typeof(ModelPrivateSetGenericParent<,>).ToTypeToRegisterForJson(),
                typeof(ModelPrivateSetGenericParentGenericChild<,>).ToTypeToRegisterForJson(),
                typeof(ModelPublicSetNonGenericParent).ToTypeToRegisterForJson(),
                typeof(ModelPublicSetNonGenericParentGenericChild<>).ToTypeToRegisterForJson(),
            };
        }
    }
}
