// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelWithEnumerableOfEnumerableInterfaces.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    [Serializable]
    public class ModelWithEnumerableOfEnumerableInterfaces
    {
        public IReadOnlyDictionary<string, IReadOnlyCollection<IValue>> StringToValueCollectionMap { get; set; }

        public IReadOnlyDictionary<IReadOnlyCollection<IValue>, string> ValueCollectionToStringMap { get; set; }

        public IReadOnlyCollection<IReadOnlyCollection<IValue>> ValueCollectionCollection { get; set; }
    }
}
