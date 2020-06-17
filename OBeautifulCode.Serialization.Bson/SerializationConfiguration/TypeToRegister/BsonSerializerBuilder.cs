// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializerBuilder.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Builds an <see cref="IBsonSerializer"/>.
    /// </summary>
    public class BsonSerializerBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializerBuilder"/> class.
        /// </summary>
        /// <param name="bsonSerializerBuilderFunc">A func that builds an <see cref="IBsonSerializer"/>.</param>
        /// <param name="outputKind">The output kind of the serializer.</param>
        public BsonSerializerBuilder(
            Func<IBsonSerializer> bsonSerializerBuilderFunc,
            BsonSerializerOutputKind outputKind)
        {
            new { bsonSerializerBuilderFunc }.AsArg().Must().NotBeNull();
            new { outputKind }.AsArg().Must().NotBeEqualTo(BsonSerializerOutputKind.Unknown);

            this.BsonSerializerBuilderFunc = bsonSerializerBuilderFunc;
            this.OutputKind = outputKind;
        }

        /// <summary>
        /// Gets a func that builds the <see cref="IBsonSerializer"/>.
        /// </summary>
        public Func<IBsonSerializer> BsonSerializerBuilderFunc { get; }

        /// <summary>
        /// Gets the output kind of the serializer.
        /// </summary>
        /// <remarks>
        /// If a custom serializer outputs a string, that string cannot be the root-level item
        /// in the BSON, because that isn't supported and Mongo will throw.
        /// In those cases, we'll wrap the object in another type and serialize that type.
        /// </remarks>
        public BsonSerializerOutputKind OutputKind { get; }
    }
}