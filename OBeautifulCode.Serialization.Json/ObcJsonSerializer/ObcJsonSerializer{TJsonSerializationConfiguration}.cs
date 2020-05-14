// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcJsonSerializer{TJsonSerializationConfiguration}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    /// <inheritdoc />
    public sealed class ObcJsonSerializer<TJsonSerializationConfiguration> : ObcJsonSerializer
        where TJsonSerializationConfiguration : JsonSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObcJsonSerializer{TJsonSerializationConfiguration}"/> class.
        /// </summary>
        public ObcJsonSerializer()
            : base(typeof(TJsonSerializationConfiguration).ToJsonSerializationConfigurationType())
        {
        }
    }
}
