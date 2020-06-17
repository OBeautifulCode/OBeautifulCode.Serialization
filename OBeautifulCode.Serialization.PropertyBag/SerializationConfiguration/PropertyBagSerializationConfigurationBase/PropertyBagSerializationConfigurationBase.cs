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
        private readonly Dictionary<Type, IStringSerializeAndDeserialize> typeToSerializerMap = new Dictionary<Type, IStringSerializeAndDeserialize>();

        private readonly Dictionary<Type, object> typesWithCustomSerializers = new Dictionary<Type, object>();

        /// <summary>
        /// Builds a map of type to serializer.
        /// </summary>
        /// <returns>
        /// Map of type to specific serializer.
        /// </returns>
        public IReadOnlyDictionary<Type, IStringSerializeAndDeserialize> BuildConfiguredTypeToSerializerMap()
        {
            var result = this.typeToSerializerMap;

            return result;
        }

        private void ProcessTypeToRegisterForPropertyBag(
            TypeToRegisterForPropertyBag typeToRegisterForPropertyBag)
        {
            var type = typeToRegisterForPropertyBag.Type;

            var stringSerializerBuilderFunc = typeToRegisterForPropertyBag.StringSerializerBuilderFunc;

            if (stringSerializerBuilderFunc != null)
            {
                this.typeToSerializerMap.Add(type, stringSerializerBuilderFunc());

                this.typesWithCustomSerializers.Add(type, null);
            }
        }
    }
}