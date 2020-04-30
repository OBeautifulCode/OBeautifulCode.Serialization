﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializationConfigurationBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson.Serialization;

    using OBeautifulCode.Serialization;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcBsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class BsonSerializationConfigurationBase : SerializationConfigurationBase
    {
        private void ProcessTypeToRegisterForBson(
            TypeToRegisterForBson typeToRegisterForBson,
            SerializationConfigurationType registeringSerializationConfigurationType)
        {
            var type = typeToRegisterForBson.Type;

            var serializerBuilderFunc = typeToRegisterForBson.SerializerBuilderFunc;

            if (registeringSerializationConfigurationType == this.SerializationConfigurationType)
            {
                if (serializerBuilderFunc == null)
                {
                    try
                    {
                        if (type.IsClass)
                        {
                            var bsonClassMap = AutomaticallyBuildBsonClassMap(type, typeToRegisterForBson.PropertyNameWhitelist);

                            BsonClassMap.RegisterClassMap(bsonClassMap);
                        }
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
}