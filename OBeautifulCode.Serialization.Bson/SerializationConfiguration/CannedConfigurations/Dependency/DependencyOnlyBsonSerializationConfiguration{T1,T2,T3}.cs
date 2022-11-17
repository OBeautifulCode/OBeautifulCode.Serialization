// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyOnlyBsonSerializationConfiguration{T1,T2,T3}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.CodeAnalysis.Recipes;

    /// <summary>
    /// A BSON serialization configuration that populates <see cref="DependentBsonSerializationConfigurationTypes"/> with typeof(<typeparamref name="T1"/>), typeof(<typeparamref name="T2"/>), and typeof(<typeparamref name="T3"/>).
    /// </summary>
    /// <typeparam name="T1">The first dependent BSON serialization configuration type.</typeparam>
    /// <typeparam name="T2">The second dependent BSON serialization configuration type.</typeparam>
    /// <typeparam name="T3">The third dependent BSON serialization configuration type.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = ObcSuppressBecause.CA1005_AvoidExcessiveParametersOnGenericTypes_SpecifiedParametersRequiredForNeededFunctionality)]
    public sealed class DependencyOnlyBsonSerializationConfiguration<T1, T2, T3> : BsonSerializationConfigurationBase
        where T1 : BsonSerializationConfigurationBase
        where T2 : BsonSerializationConfigurationBase
        where T3 : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new[]
        {
            typeof(T1).ToBsonSerializationConfigurationType(),
            typeof(T2).ToBsonSerializationConfigurationType(),
            typeof(T3).ToBsonSerializationConfigurationType(),
        };
    }
}