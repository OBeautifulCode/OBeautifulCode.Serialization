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

        private readonly Dictionary<Type, object> typesWithCustomStringSerializers = new Dictionary<Type, object>();

        /// <summary>
        /// Gets the types that have custom string serializers.
        /// </summary>
        public IReadOnlyDictionary<Type, object> TypesWithCustomStringSerializers => this.typesWithCustomStringSerializers;

        private void ProcessTypeToRegisterForBson(
            TypeToRegisterForBson typeToRegisterForBson,
            SerializationConfigurationType registeringSerializationConfigurationType)
        {
            var type = typeToRegisterForBson.Type;

            var bsonSerializerBuilder = typeToRegisterForBson.BsonSerializerBuilder;

            var propertyNameWhitelist = typeToRegisterForBson.PropertyNameWhitelist;

            if (registeringSerializationConfigurationType == this.SerializationConfigurationType)
            {
                if (bsonSerializerBuilder == null)
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
                    // see comments in RegisteredTypeBsonSerializer<T> for why we are wrapping the serializer
                    var registeredTypeBsonSerializer = RegisteredTypeBsonSerializer.Build(type, bsonSerializerBuilder.BsonSerializerBuilderFunc());
                    BsonSerializer.RegisterSerializer(type, registeredTypeBsonSerializer);
                }
            }

            if (bsonSerializerBuilder != null)
            {
                this.typesWithCustomSerializerOrPropertyNamesWhitelist.Add(type, null);

                if (bsonSerializerBuilder.OutputKind == BsonSerializerOutputKind.String)
                {
                    this.typesWithCustomStringSerializers.Add(type, null);
                }
            }

            if (propertyNameWhitelist != null)
            {
                this.typesWithCustomSerializerOrPropertyNamesWhitelist.Add(type, null);
            }
        }
    }
}