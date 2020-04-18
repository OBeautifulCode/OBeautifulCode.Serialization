// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBagSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization;

    /// <summary>
    /// Base class to use for creating a <see cref="ObcPropertyBagSerializer" /> configuration.
    /// </summary>
    public abstract partial class PropertyBagSerializationConfigurationBase : SerializationConfigurationBase
    {
        /// <summary>
        /// Gets the map of type to specific serializer.
        /// </summary>
        private Dictionary<Type, IStringSerializeAndDeserialize> TypeToSerializerMap { get; } = new Dictionary<Type, IStringSerializeAndDeserialize>();

        /// <summary>
        /// Builds a map of type to serializer.
        /// </summary>
        /// <returns>
        /// Map of type to specific serializer.
        /// </returns>
        public IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> BuildConfiguredTypeToSerializerMap()
        {
            var result = this.TypeToSerializerMap;

            return result;
        }

        private void ProcessTypeToRegisterForPropertyBag(
            TypeToRegisterForPropertyBag typeToRegisterForPropertyBag)
        {
            var type = typeToRegisterForPropertyBag.Type;

            var stringSerializerBuilderFunc = typeToRegisterForPropertyBag.StringSerializerBuilderFunc;

            if (stringSerializerBuilderFunc != null)
            {
                this.TypeToSerializerMap.Add(type, stringSerializerBuilderFunc());
            }
        }
    }
}