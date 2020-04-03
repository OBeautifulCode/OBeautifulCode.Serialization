// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISerialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Interface to serialize to and from a byte array or string.
    /// </summary>
    public interface ISerialize : IStringSerialize, IBinarySerialize, IHaveSerializationKind
    {
    }
}
