// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationBase.Virtual.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System.Collections.Generic;

    using OBeautifulCode.Serialization;

    public abstract partial class PropertyBagSerializationConfigurationBase
    {
        /// <summary>
        /// Gets the key value delimiter to use for string serialization of the property bag.
        /// </summary>
        public virtual string StringSerializationKeyValueDelimiter { get; } = ObcDictionaryStringStringSerializer.DefaultKeyValueDelimiter;

        /// <summary>
        /// Gets the line delimiter to use for string serialization of the property bag.
        /// </summary>
        public virtual string StringSerializationLineDelimiter { get; } = ObcDictionaryStringStringSerializer.DefaultLineDelimiter;

        /// <summary>
        /// Gets the null value encoding to use for string serialization of the property bag.
        /// </summary>
        public virtual string StringSerializationNullValueEncoding { get; } = ObcDictionaryStringStringSerializer.DefaultNullValueEncoding;

        /// <summary>
        /// Gets the <see cref="PropertyBagSerializationConfigurationBase"/>s that are needed for the current implementation of <see cref="PropertyBagSerializationConfigurationBase"/>.  Optionally overrideable, DEFAULT is empty collection.
        /// </summary>
        protected virtual IReadOnlyCollection<PropertyBagSerializationConfigurationType> DependentPropertyBagSerializationConfigurationTypes => new PropertyBagSerializationConfigurationType[0];

        /// <summary>
        /// Gets the types to register for property bag serialization.
        /// </summary>
        protected virtual IReadOnlyCollection<TypeToRegisterForPropertyBag> TypesToRegisterForPropertyBag { get; } = new TypeToRegisterForPropertyBag[0];
    }
}