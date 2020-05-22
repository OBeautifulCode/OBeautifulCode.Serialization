// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelWithNullableProperties.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;

    [Serializable]
    public class ModelWithNullableProperties
    {
        public DateTime? NullableDateTime { get; set; }

        public Guid? NullableGuid { get; set; }

        public int? NullableInt { get; set; }
    }
}
