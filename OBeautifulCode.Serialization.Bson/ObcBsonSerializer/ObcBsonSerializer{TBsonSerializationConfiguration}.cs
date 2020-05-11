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
        public ObcBsonSerializer()
            : base(typeof(TBsonSerializationConfiguration).ToBsonSerializationConfigurationType())
        {
        }
    }
}
