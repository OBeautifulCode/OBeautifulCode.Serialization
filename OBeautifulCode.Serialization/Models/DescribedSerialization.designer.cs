﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.75.0)
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

    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Representation.System;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    public partial class DescribedSerialization : IModel<DescribedSerialization>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="DescribedSerialization"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(DescribedSerialization left, DescribedSerialization right)
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
        /// Determines whether two objects of type <see cref="DescribedSerialization"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(DescribedSerialization left, DescribedSerialization right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(DescribedSerialization other)
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
                      && this.SerializedPayload.Equals(other.SerializedPayload, StringComparison.Ordinal)
                      && this.SerializerDescription.IsEqualTo(other.SerializerDescription);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as DescribedSerialization);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.PayloadTypeRepresentation)
            .Hash(this.SerializedPayload)
            .Hash(this.SerializerDescription)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public DescribedSerialization DeepClone()
        {
            var result = new DescribedSerialization(
                                 this.PayloadTypeRepresentation?.DeepClone(),
                                 this.SerializedPayload?.Clone().ToString(),
                                 this.SerializerDescription?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="PayloadTypeRepresentation" />.
        /// </summary>
        /// <param name="payloadTypeRepresentation">The new <see cref="PayloadTypeRepresentation" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="DescribedSerialization" /> using the specified <paramref name="payloadTypeRepresentation" /> for <see cref="PayloadTypeRepresentation" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public DescribedSerialization DeepCloneWithPayloadTypeRepresentation(TypeRepresentation payloadTypeRepresentation)
        {
            var result = new DescribedSerialization(
                                 payloadTypeRepresentation,
                                 this.SerializedPayload?.Clone().ToString(),
                                 this.SerializerDescription?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="SerializedPayload" />.
        /// </summary>
        /// <param name="serializedPayload">The new <see cref="SerializedPayload" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="DescribedSerialization" /> using the specified <paramref name="serializedPayload" /> for <see cref="SerializedPayload" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public DescribedSerialization DeepCloneWithSerializedPayload(string serializedPayload)
        {
            var result = new DescribedSerialization(
                                 this.PayloadTypeRepresentation?.DeepClone(),
                                 serializedPayload,
                                 this.SerializerDescription?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="SerializerDescription" />.
        /// </summary>
        /// <param name="serializerDescription">The new <see cref="SerializerDescription" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="DescribedSerialization" /> using the specified <paramref name="serializerDescription" /> for <see cref="SerializerDescription" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public DescribedSerialization DeepCloneWithSerializerDescription(SerializerDescription serializerDescription)
        {
            var result = new DescribedSerialization(
                                 this.PayloadTypeRepresentation?.DeepClone(),
                                 this.SerializedPayload?.Clone().ToString(),
                                 serializerDescription);

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Invariant($"OBeautifulCode.Serialization.DescribedSerialization: PayloadTypeRepresentation = {this.PayloadTypeRepresentation?.ToString() ?? "<null>"}, SerializedPayload = {this.SerializedPayload?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, SerializerDescription = {this.SerializerDescription?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}