// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForJsonTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using FakeItEasy;

    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Serialization.Json.Test.Internal;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    using static System.FormattableString;

    public static class TypeToRegisterForJsonTest
    {
        [Fact]
        public static void CreateSpawnedTypeToRegister___Should_return_spawned_TypeToRegisterForJson_with_KeyInDictionaryStringSerializer_set_null___When_typeToIncludeOrigin_is_GettingMemberTypes()
        {
            // Arrange
            var expected = A.Dummy<TestModel>();

            // Act & Assert
            // if the dictionary key string serializer is used for members, then ModelThatCanBeConvertedToStringSerializer
            // throws when serializing OtherModel in OtherModelToStringMap, because OtherModel will be registered with
            // that dictionary key string serializer.
            expected.RoundtripSerializeViaJsonWithBeEqualToAssertion(typeof(DictionaryKeyConvertingSerializationConfiguration));
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class TestModel : IEquatable<TestModel>
        {
            public IReadOnlyDictionary<ModelThatCanBeConvertedToString, ModelThatCanBeConvertedToString> ModelToModelMap { get; set; }

            public IReadOnlyDictionary<OtherModel, string> OtherModelToStringMap { get; set; }

            public static bool operator ==(TestModel left, TestModel right)
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

            public static bool operator !=(TestModel left, TestModel right) => !(left == right);

            public bool Equals(TestModel other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.ModelToModelMap.IsEqualTo(other.ModelToModelMap)
                          && this.OtherModelToStringMap.IsEqualTo(other.OtherModelToStringMap);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as TestModel);

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ObcSuppressBecause.CA1065_DoNotRaiseExceptionsInUnexpectedLocations_ThrowNotImplementedExceptionWhenForcedToSpecifyMemberThatWillNeverBeUsedInTesting)]
            public override int GetHashCode() => throw new NotImplementedException("should not get used");
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class OtherModel : IEquatable<OtherModel>
        {
            public OtherModel(
                Guid id)
            {
                this.Id = id;
            }

            public Guid Id { get; private set; }

            public static bool operator ==(OtherModel left, OtherModel right)
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

            public static bool operator !=(OtherModel left, OtherModel right) => !(left == right);

            public bool Equals(OtherModel other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = this.Id == other.Id;

                return result;
            }

            public override bool Equals(object obj) => this == (obj as OtherModel);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Id).Value;
        }

        [Serializable]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = ObcSuppressBecause.CA1034_NestedTypesShouldNotBeVisible_VisibleNestedTypeRequiredForTesting)]
        public class ModelThatCanBeConvertedToString : IEquatable<ModelThatCanBeConvertedToString>
        {
            public ModelThatCanBeConvertedToString(
                int value,
                int value2,
                OtherModel otherModel)
            {
                this.Value = value;
                this.Value2 = value2;
                this.OtherModel = otherModel;
            }

            public int Value { get; private set; }

            public int Value2 { get; private set; }

            public OtherModel OtherModel { get; private set; }

            public static bool operator ==(ModelThatCanBeConvertedToString left, ModelThatCanBeConvertedToString right)
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

            public static bool operator !=(ModelThatCanBeConvertedToString left, ModelThatCanBeConvertedToString right) => !(left == right);

            public bool Equals(ModelThatCanBeConvertedToString other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                var result = (this.Value == other.Value) && (this.Value2 == other.Value2) && (this.OtherModel == other.OtherModel);

                return result;
            }

            public override bool Equals(object obj) => this == (obj as ModelThatCanBeConvertedToString);

            public override int GetHashCode() => HashCodeHelper.Initialize().Hash(this.Value).Hash(this.Value2).Hash(this.OtherModel).Value;
        }

        private class ModelThatCanBeConvertedToStringSerializer : IStringSerializeAndDeserialize
        {
            public string SerializeToString(object objectToSerialize)
            {
                if (objectToSerialize == null)
                {
                    return null;
                }

                string result;

                if (objectToSerialize is ModelThatCanBeConvertedToString model)
                {
                    result = model.Value + "," + model.Value2 + "," + model.OtherModel.Id;
                }
                else
                {
                    throw new ArgumentException(Invariant($"{nameof(objectToSerialize)} is not a {nameof(ModelThatCanBeConvertedToString)}"));
                }

                return result;
            }

            public T Deserialize<T>(string serializedString)
            {
                var result = (T)this.Deserialize(serializedString, typeof(T));

                return result;
            }

            public object Deserialize(string serializedString, Type type)
            {
                if (serializedString == null)
                {
                    return null;
                }

                var tokens = serializedString.Split(',');

                var value = int.Parse(tokens[0]);

                var value2 = int.Parse(tokens[1]);

                var id = Guid.Parse(tokens[2]);

                var result = new ModelThatCanBeConvertedToString(value, value2, new OtherModel(id));

                return result;
            }
        }

        private class DictionaryKeyConvertingSerializationConfiguration : JsonSerializationConfigurationBase
        {
            private static readonly ModelThatCanBeConvertedToStringSerializer ModelThatCanBeConvertedToStringSerializer = new ModelThatCanBeConvertedToStringSerializer();

            protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
            {
                // ordering of the below does not matter for the test case;
                // OtherModel is picked-up first because it's a property of ModelThatCanBeConvertedToString
                // In TestModel, the system first sees Dictionary<OtherModel, string> so it takes 2 rounds
                // to hit OtherModel
                typeof(TestModel).ToTypeToRegisterForJson(),
                typeof(ModelThatCanBeConvertedToString).ToTypeToRegisterForJsonUsingKeyInDictionaryStringSerializer(ModelThatCanBeConvertedToStringSerializer),
            };
        }
    }
}
