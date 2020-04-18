// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBase.Virtual.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;

    public abstract partial class BsonSerializationConfigurationBase
    {
        /// <summary>
        /// Gets the <see cref="BsonSerializationConfigurationBase"/>s that are needed for the current implementation of <see cref="BsonSerializationConfigurationBase"/>.  Optionally overrideable, DEFAULT is empty collection.
        /// </summary>
        protected virtual IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes => new BsonSerializationConfigurationType[0];

        /// <summary>
        /// Gets the types to register for BSON serialization.
        /// </summary>
        protected virtual IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson { get; } = new TypeToRegisterForBson[0];
    }
}