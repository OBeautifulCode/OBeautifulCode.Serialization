// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParamsConstructorCollectionProperty.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Serialization.Recipes;

    using Xunit;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Params", Justification = "Name/spelling is correct.")]
    public static class ParamsConstructorCollectionProperty
    {
        [Fact]
        public static void Values_in_collection_will_be_passed_to_constructor()
        {
            // Arrange
            var jsonConfigType = typeof(TypesToRegisterJsonSerializationConfiguration<ObjectWithParamsConstructor>);
            var bsonConfigType = typeof(TypesToRegisterBsonSerializationConfiguration<ObjectWithParamsConstructor>);

            var expected = A.Dummy<ObjectWithParamsConstructor>();

            void ThrowIfObjectsDiffer(string serialized, SerializationFormat format, ObjectWithParamsConstructor deserialized)
            {
                deserialized.Should().NotBeNull();
                deserialized.InputStuff.Should().Equal(expected.InputStuff);
            }

            // Act & Assert
            expected.RoundtripSerializeWithCallbackVerification(ThrowIfObjectsDiffer, bsonConfigType, jsonConfigType);
        }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Params", Justification = "Name/spelling is correct.")]
    public class ObjectWithParamsConstructor : IEquatable<ObjectWithParamsConstructor>
    {
        public ObjectWithParamsConstructor(params ObjectWithParamsConstructorElement[] inputStuff)
        {
            this.InputStuff = inputStuff;
        }

        public IReadOnlyCollection<ObjectWithParamsConstructorElement> InputStuff { get; set; }

        public bool Equals(ObjectWithParamsConstructor other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.InputStuff.SequenceEqual(other.InputStuff);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ObjectWithParamsConstructor)obj);
        }

        public override int GetHashCode()
        {
            return this.InputStuff != null ? this.InputStuff.GetHashCode() : 0;
        }
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Params", Justification = "Name/spelling is correct.")]
    public class ObjectWithParamsConstructorElement : IEquatable<ObjectWithParamsConstructorElement>
    {
        public string Property { get; set; }

        public bool Equals(ObjectWithParamsConstructorElement other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.Property, other.Property);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ObjectWithParamsConstructorElement)obj);
        }

        public override int GetHashCode()
        {
            return this.Property != null ? this.Property.GetHashCode() : 0;
        }
    }
}
