﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.152.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Globalization;
    using global::System.Linq;

    using global::OBeautifulCode.Cloning.Recipes;
    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Representation.System;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class StringDescribedSerialization : IModel<StringDescribedSerialization>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="StringDescribedSerialization"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(StringDescribedSerialization left, StringDescribedSerialization right)
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

        /// <summary>
        /// Determines whether two objects of type <see cref="StringDescribedSerialization"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(StringDescribedSerialization left, StringDescribedSerialization right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(StringDescribedSerialization other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.PayloadTypeRepresentation.IsEqualTo(other.PayloadTypeRepresentation)
                      && this.SerializerRepresentation.IsEqualTo(other.SerializerRepresentation)
                      && this.SerializedPayload.IsEqualTo(other.SerializedPayload, StringComparer.Ordinal);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as StringDescribedSerialization);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.PayloadTypeRepresentation)
            .Hash(this.SerializerRepresentation)
            .Hash(this.SerializedPayload)
            .Value;

        /// <inheritdoc />
        public new StringDescribedSerialization DeepClone() => (StringDescribedSerialization)this.DeepCloneInternal();

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public override DescribedSerializationBase DeepCloneWithPayloadTypeRepresentation(TypeRepresentation payloadTypeRepresentation)
        {
            var result = new StringDescribedSerialization(
                                 payloadTypeRepresentation,
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.SerializedPayload?.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public override DescribedSerializationBase DeepCloneWithSerializerRepresentation(SerializerRepresentation serializerRepresentation)
        {
            var result = new StringDescribedSerialization(
                                 this.PayloadTypeRepresentation?.DeepClone(),
                                 serializerRepresentation,
                                 this.SerializedPayload?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="SerializedPayload" />.
        /// </summary>
        /// <param name="serializedPayload">The new <see cref="SerializedPayload" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="StringDescribedSerialization" /> using the specified <paramref name="serializedPayload" /> for <see cref="SerializedPayload" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public StringDescribedSerialization DeepCloneWithSerializedPayload(string serializedPayload)
        {
            var result = new StringDescribedSerialization(
                                 this.PayloadTypeRepresentation?.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 serializedPayload);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override DescribedSerializationBase DeepCloneInternal()
        {
            var result = new StringDescribedSerialization(
                                 this.PayloadTypeRepresentation?.DeepClone(),
                                 this.SerializerRepresentation?.DeepClone(),
                                 this.SerializedPayload?.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Serialization.StringDescribedSerialization: PayloadTypeRepresentation = {this.PayloadTypeRepresentation?.ToString() ?? "<null>"}, SerializerRepresentation = {this.SerializerRepresentation?.ToString() ?? "<null>"}, SerializedPayload = {this.SerializedPayload?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}.");

            return result;
        }
    }
}