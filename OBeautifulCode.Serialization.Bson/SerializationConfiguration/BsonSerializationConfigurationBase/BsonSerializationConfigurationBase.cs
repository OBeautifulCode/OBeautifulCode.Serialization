// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;

    using OBeautifulCode.Serialization;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcBsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class BsonSerializationConfigurationBase : SerializationConfigurationBase
    {
        private IDiscriminatorConvention obcBsonDiscriminatorConvention;

        private void ProcessTypeToRegisterForBson(
            TypeToRegisterForBson typeToRegisterForBson)
        {
            var type = typeToRegisterForBson.Type;

            var serializerBuilderFunc = typeToRegisterForBson.SerializerBuilderFunc;

            if (serializerBuilderFunc == null)
            {
                try
                {
                    if (type.IsClass)
                    {
                        var bsonClassMap = AutomaticallyBuildBsonClassMap(type, typeToRegisterForBson.PropertyNameWhitelist);

                        BsonClassMap.RegisterClassMap(bsonClassMap);
                    }

                    // we are not 100% sure whether interface types or abstract types need to be registered
                    // but there doesn't seem to be any harm in doing so.
                    if (this.obcBsonDiscriminatorConvention == null)
                    {
                        this.obcBsonDiscriminatorConvention = new ObcBsonDiscriminatorConvention(this);
                    }

                    BsonSerializer.RegisterDiscriminatorConvention(type, this.obcBsonDiscriminatorConvention);
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
    }
}