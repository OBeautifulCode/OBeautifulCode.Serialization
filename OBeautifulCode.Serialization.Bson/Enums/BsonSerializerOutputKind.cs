// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializerOutputKind.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using MongoDB.Bson.Serialization;

    /// <summary>
    /// Specifies the kind of output of a <see cref="IBsonSerializer"/>.
    /// </summary>
    public enum BsonSerializerOutputKind
    {
        /// <summary>
        /// Completely unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Outputs what BSON will consider a string.
        /// </summary>
        String,

        /// <summary>
        /// Outputs what BSON will consider an object.
        /// </summary>
        Object,
    }
}