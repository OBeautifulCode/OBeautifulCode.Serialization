// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializerForTypes.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Specifies a <see cref="IStringSerializeAndDeserialize"/> that should be used for a specified set of types.
    /// </summary>
    public class StringSerializerForTypes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSerializerForTypes"/> class.
        /// </summary>
        /// <param name="serializerBuilderFunc">A func that builds the <see cref="IStringSerializeAndDeserialize"/>.</param>
        /// <param name="handledTypes">The set of types that should be handled by the <see cref="IStringSerializeAndDeserialize"/>.</param>
        public StringSerializerForTypes(
            Func<IStringSerializeAndDeserialize> serializerBuilderFunc,
            IReadOnlyCollection<Type> handledTypes)
        {
            new { serializerBuilderFunc }.AsArg().Must().NotBeNull();
            new { handledTypes }.AsArg().Must().NotBeNull().And().NotBeEmptyEnumerable();

            this.SerializerBuilderFunc = serializerBuilderFunc;
            this.HandledTypes = handledTypes;
        }

        /// <summary>
        /// Gets a func that builds the <see cref="IStringSerializeAndDeserialize"/>.
        /// </summary>
        public Func<IStringSerializeAndDeserialize> SerializerBuilderFunc { get; }

        /// <summary>
        /// Gets the set of types that should be handled by the <see cref="IStringSerializeAndDeserialize"/>.
        /// </summary>
        public IReadOnlyCollection<Type> HandledTypes { get; }
    }
}