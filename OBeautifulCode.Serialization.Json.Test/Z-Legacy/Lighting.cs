// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Lighting.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json.Test
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class Lighting
    {
    }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is used via reflection and code analysis cannot detect that.")]
    internal class NoLighting : Lighting
    {
    }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is used via reflection and code analysis cannot detect that.")]
    internal class Incandescent : Lighting
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public int Watts { get; set; }
    }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is used via reflection and code analysis cannot detect that.")]
    internal class Led : Lighting
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public int Watts { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public int WattageEquivalent { get; set; }
    }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is used via reflection and code analysis cannot detect that.")]
    internal class SmartLed : Lighting
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public int Watts { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public int WattageEquivalent { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public string Features { get; set; }
    }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is used via reflection and code analysis cannot detect that.")]
    internal class CompactFluorescent : Lighting
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public int Watts { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Property is used via reflection and code analysis cannot detect that.")]
        public int WattageEquivalent { get; set; }
    }

    internal class LightingJsonSerializationConfiguration : JsonSerializationConfigurationBase
    {
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson => new[]
        {
            new TypeToRegisterForJson(typeof(Lighting), MemberTypesToInclude.All, RelatedTypesToInclude.Descendants, null, null),
        };
    }
}
