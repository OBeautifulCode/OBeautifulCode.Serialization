// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetDrawingBsonSerializationConfiguration.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// A default serialization configuration that ensures proper handling of <see cref="Color"/>.
    /// </summary>
    public sealed class NetDrawingBsonSerializationConfiguration : BsonSerializationConfigurationBase, IIgnoreDefaultDependencies
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializerForTypes> TypesToRegisterWithSerializer => new[]
        {
            new BsonSerializerForTypes(() => new ObcBsonColorSerializer(), new[] { typeof(Color) }),
        };
    }
}