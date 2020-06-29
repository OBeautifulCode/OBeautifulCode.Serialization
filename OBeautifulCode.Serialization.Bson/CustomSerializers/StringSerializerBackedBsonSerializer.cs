// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializerBackedBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Helper methods for creating a <see cref="StringSerializerBackedBsonSerializer{T}"/>.
    /// </summary>
    internal static class StringSerializerBackedBsonSerializer
    {
        /// <summary>
        /// Builds a <see cref="StringSerializerBackedBsonSerializer{T}"/> for the specified type and backing string serializer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="backingSerializer">The backing string serializer.</param>
        /// <returns>
        /// A newly constructed <see cref="StringSerializerBackedBsonSerializer{T}"/> for the specified type and backing string serializer.
        /// </returns>
        public static IBsonSerializer Build(
            Type type,
            IStringSerializeAndDeserialize backingSerializer)
        {
            new { type }.AsArg().Must().NotBeNull();

            var result = (IBsonSerializer)typeof(StringSerializerBackedBsonSerializer<>).MakeGenericType(type).Construct(backingSerializer);

            return result;
        }
    }
}