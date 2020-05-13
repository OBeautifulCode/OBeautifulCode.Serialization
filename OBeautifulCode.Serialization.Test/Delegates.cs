// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    public delegate void RoundtripSerializationCallback<in T>(
        DescribedSerialization yieldedDescribedSerialization,
        T deserializedObject);
}
