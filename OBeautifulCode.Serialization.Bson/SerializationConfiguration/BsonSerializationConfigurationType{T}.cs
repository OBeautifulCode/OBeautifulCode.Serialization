// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationType{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    /// <inheritdoc />
    /// <typeparam name="T">The type of concrete <see cref="BsonSerializationConfigurationBase"/> derivative.</typeparam>
    public class BsonSerializationConfigurationType<T> : BsonSerializationConfigurationType
        where T : BsonSerializationConfigurationBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializationConfigurationType{T}"/> class.
        /// </summary>
        public BsonSerializationConfigurationType()
            : base(typeof(T))
        {
        }
    }
}