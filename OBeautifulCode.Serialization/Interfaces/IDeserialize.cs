// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeserialize.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Interface to deserialize to and from a byte array or string.
    /// </summary>
    public interface IDeserialize : IStringDeserialize, IBinaryDeserialize, IHaveSerializationKind
    {
    }
}
