﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerializeAndDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Interface to serialize and deserialize to and from a byte array or string.
    /// </summary>
    public interface ISerializeAndDeserialize : ISerialize, IDeserialize, IStringSerializeAndDeserialize, IBinarySerializeAndDeserialize
    {
    }
}