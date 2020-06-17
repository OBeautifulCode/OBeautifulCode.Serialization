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
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson => new[]
        {
            typeof(Color).ToTypeToRegisterForBson(MemberTypesToInclude.None, RelatedTypesToInclude.None, new BsonSerializerBuilder(() => new ObcBsonColorSerializer(), BsonSerializerOutputKind.String)),
        };

        /// <inheritdoc />
        protected override IReadOnlyCollection<string> TypeToRegisterNamespacePrefixFilters => new[] { typeof(Color).Namespace };
    }
}