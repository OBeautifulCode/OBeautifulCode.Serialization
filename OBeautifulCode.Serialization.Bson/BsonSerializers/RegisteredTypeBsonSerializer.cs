// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredTypeBsonSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Helper methods for creating a <see cref="RegisteredTypeBsonSerializer{T}"/>.
    /// </summary>
    internal static class RegisteredTypeBsonSerializer
    {
        /// <summary>
        /// Builds a <see cref="RegisteredTypeBsonSerializer{T}"/> for the specified type and backing serializer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="backingSerializer">The backing serializer.</param>
        /// <returns>
        /// A newly constructed <see cref="RegisteredTypeBsonSerializer{T}"/> for the specified type and backing serializer.
        /// </returns>
        public static IBsonSerializer Build(
            Type type,
            IBsonSerializer backingSerializer)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = (IBsonSerializer)typeof(RegisteredTypeBsonSerializer<>).MakeGenericType(type).Construct(backingSerializer);

            return result;
        }
    }
}