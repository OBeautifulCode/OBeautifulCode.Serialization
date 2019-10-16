// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcStringSerializerAttribute.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    using OBeautifulCode.Assertion.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Attribute to specify the type of <see cref="IStringSerializeAndDeserialize" /> to use for this type during serializations that support this override.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ObcStringSerializerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcStringSerializerAttribute"/> class.
        /// </summary>
        /// <param name="serializerType">Type of <see cref="IStringSerializeAndDeserialize" /> to use when string serializing where supported.</param>
        public ObcStringSerializerAttribute(Type serializerType)
        {
            new { serializerType }.AsArg().Must().NotBeNull();

            serializerType.HasParameterlessConstructor().AsArg(Invariant($"Type specified {serializerType} must have a paramerterless constructor.")).Must()
                .BeTrue();
            serializerType.ImplementsInterface<IStringSerializeAndDeserialize>().AsArg(
                Invariant($"Type specified {serializerType} was not an implementer of {typeof(IStringSerializeAndDeserialize)}")).Must().BeTrue();

            this.SerializerType = serializerType;
        }

        /// <summary>
        /// Gets the type of <see cref="IStringSerializeAndDeserialize" />.
        /// </summary>
        public Type SerializerType { get; private set; }
    }

    /// <summary>
    /// Attribute to specify the type of <see cref="IStringSerializeAndDeserialize" /> to use for elements in a collection or array.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ObcElementStringSerializerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcElementStringSerializerAttribute"/> class.
        /// </summary>
        /// <param name="elementSerializerType">Type of <see cref="IStringSerializeAndDeserialize" /> to use when string serializing where supported.</param>
        public ObcElementStringSerializerAttribute(Type elementSerializerType)
        {
            elementSerializerType.HasParameterlessConstructor()
                .AsArg(Invariant($"Type specified {elementSerializerType} must have a paramerterless constructor.")).Must().BeTrue();
            elementSerializerType.ImplementsInterface<IStringSerializeAndDeserialize>().AsArg(
                Invariant($"Type specified {elementSerializerType} was not an implementer of {typeof(IStringSerializeAndDeserialize)}")).Must().BeTrue();

            this.ElementSerializerType = elementSerializerType;
        }

        /// <summary>
        /// Gets the type of <see cref="IStringSerializeAndDeserialize" /> for elements.
        /// </summary>
        public Type ElementSerializerType { get; private set; }
    }
}