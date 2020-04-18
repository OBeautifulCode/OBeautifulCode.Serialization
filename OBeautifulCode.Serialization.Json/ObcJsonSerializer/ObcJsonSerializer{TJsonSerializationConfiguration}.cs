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
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <param name="formattingKind">Optional type of formatting to use; DEFAULT is <see cref="JsonFormattingKind.Default" />.</param>
        public ObcJsonSerializer(
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default,
            JsonFormattingKind formattingKind = JsonFormattingKind.Default)
            : base(typeof(TJsonSerializationConfiguration).ToJsonSerializationConfigurationType(), unregisteredTypeEncounteredStrategy, formattingKind)
        {
        }
    }
}
