// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Serialization;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcBsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class BsonSerializationConfigurationBase : SerializationConfigurationBase
    {
        private readonly Dictionary<Type, object> typesWithCustomSerializerOrPropertyNamesWhitelist = new Dictionary<Type, object>();

        private void ProcessTypeToRegisterForBson(
            TypeToRegisterForBson typeToRegisterForBson,
            SerializationConfigurationType registeringSerializationConfigurationType)
        {
            var type = typeToRegisterForBson.Type;

            var serializerBuilderFunc = typeToRegisterForBson.SerializerBuilderFunc;

            var propertyNameWhitelist = typeToRegisterForBson.PropertyNameWhitelist;

            if (registeringSerializationConfigurationType == this.SerializationConfigurationType)
            {
                if (serializerBuilderFunc == null)
                {
                    try
                    {
                        if (type.IsClass)
                        {
                            var bsonClassMap = AutomaticallyBuildBsonClassMap(type, propertyNameWhitelist);

                            BsonClassMap.RegisterClassMap(bsonClassMap);
                        }

                        // we are not 100% sure whether interface types or abstract types need to be registered
                        // but there doesn't seem to be any harm in doing so.
                        BsonSerializer.RegisterDiscriminatorConvention(type, ObcBsonDiscriminatorConvention.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new BsonSerializationConfigurationException(Invariant($"Failed to run {nameof(BsonClassMap.RegisterClassMap)} on {type.FullName}"), ex);
                    }
                }
                else
                {
                    BsonSerializer.RegisterSerializer(type, serializerBuilderFunc());
                }
            }

            if (serializerBuilderFunc != null)
            {
                this.typesWithCustomSerializerOrPropertyNamesWhitelist.Add(type, null);
            }

            if (propertyNameWhitelist != null)
            {
                this.typesWithCustomSerializerOrPropertyNamesWhitelist.Add(type, null);
            }
        }
    }
}