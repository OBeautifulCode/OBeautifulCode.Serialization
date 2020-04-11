// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationType{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    /// <inheritdoc />
    /// <typeparam name="T">The type of concrete <see cref="JsonSerializationConfigurationBase"/> derivative.</typeparam>
    public class JsonSerializationConfigurationType<T> : JsonSerializationConfigurationType
        where T : JsonSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationConfigurationType{T}"/> class.
        /// </summary>
        public JsonSerializationConfigurationType()
            : base(typeof(T))
        {
        }
    }
}