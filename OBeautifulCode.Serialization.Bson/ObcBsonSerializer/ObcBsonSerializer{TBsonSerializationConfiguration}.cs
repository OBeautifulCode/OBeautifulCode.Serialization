// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonSerializer{TBsonSerializationConfiguration}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    /// <inheritdoc />
    public sealed class ObcBsonSerializer<TBsonSerializationConfiguration> : ObcBsonSerializer
        where TBsonSerializationConfiguration : BsonSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonSerializer{TBsonSerializationConfiguration}"/> class.
        /// </summary>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        public ObcBsonSerializer(
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
            : base(typeof(TBsonSerializationConfiguration).ToBsonSerializationConfigurationType(), unregisteredTypeEncounteredStrategy)
        {
        }
    }
}
