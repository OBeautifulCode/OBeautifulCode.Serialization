// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializerForTypes.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Specifies a <see cref="IBsonSerializer"/> that should be used for a specified set of types.
    /// </summary>
    public class BsonSerializerForTypes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializerForTypes"/> class.
        /// </summary>
        /// <param name="serializerBuilderFunc">A func that builds the <see cref="IBsonSerializer"/>.</param>
        /// <param name="handledTypes">The set of types that should be handled by the <see cref="IBsonSerializer"/>.</param>
        public BsonSerializerForTypes(
            Func<IBsonSerializer> serializerBuilderFunc,
            IReadOnlyCollection<Type> handledTypes)
        {
            new { serializerBuilderFunc }.AsArg().Must().NotBeNull();
            new { handledTypes }.AsArg().Must().NotBeNull().And().NotBeEmptyEnumerable();

            this.SerializerBuilderFunc = serializerBuilderFunc;
            this.HandledTypes = handledTypes;
        }

        /// <summary>
        /// Gets a func that builds the <see cref="IBsonSerializer"/>.
        /// </summary>
        public Func<IBsonSerializer> SerializerBuilderFunc { get; }

        /// <summary>
        /// Gets the set of types that should be handled by the <see cref="IBsonSerializer"/>.
        /// </summary>
        public IReadOnlyCollection<Type> HandledTypes { get; }
    }
}