// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Null object pattern implementation of a serializer factory.
    /// </summary>
    public class NullSerializerFactory : SerializerFactoryBase
    {
        private static readonly ISerializer CachedSerializer = new ObcNullSerializer();

        /// <inheritdoc />
        public override ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            var result = CachedSerializer;

            return result;
        }
    }
}
