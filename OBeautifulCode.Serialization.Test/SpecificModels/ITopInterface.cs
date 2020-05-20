// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITopInterface.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "required for test")]
    public interface ITopInterface
    {
        string Species { get; set; }
    }

    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "required for test")]
    public interface IMiddleInterface : ITopInterface
    {
        string Name { get; set; }
    }

    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "required for test")]
    public class BottomClass : IMiddleInterface
    {
        public string Species { get; set; }

        public string Name { get; set; }
    }
}
