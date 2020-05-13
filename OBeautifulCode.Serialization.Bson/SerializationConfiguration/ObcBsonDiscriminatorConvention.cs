﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonDiscriminatorConvention.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// A discriminator (some information added when serializing a type that is declared as an interface type
    /// or is a type that is an ancestor to one or more other types) to specify the runtime type for use
    /// in deserialization.  Out-of-the-box Mongo uses type.Name, which is insufficient for generic types
    /// because it doesn't specify the generic type arguments.
    /// </summary>
    public class ObcBsonDiscriminatorConvention : IDiscriminatorConvention
    {
        /// <summary>
        /// Gets a static instance of this class.
        /// </summary>
        public static readonly ObcBsonDiscriminatorConvention Instance = new ObcBsonDiscriminatorConvention();

        /// <inheritdoc />
        public string ElementName => "_t";

        /// <inheritdoc />
        public Type GetActualType(
            IBsonReader bsonReader,
            Type nominalType)
        {
            var bookmark = bsonReader.GetBookmark();

            bsonReader.ReadStartDocument();

            Type result;

            if (bsonReader.FindElement(this.ElementName))
            {
                var value = bsonReader.ReadString();

                try
                {
                    var assemblyQualifiedName = value.ToTypeRepresentationFromAssemblyQualifiedName();

                    result = assemblyQualifiedName.ResolveFromLoadedTypes();
                }
                catch (ArgumentException)
                {
                    // previously persisted documents will have used Type.Name
                    // in that case ToTypeRepresentationFromAssemblyQualifiedName will throw.
                    // this is here for backward compatibility.
                    result = TypeNameDiscriminator.GetActualType(value);

                    if (result == null)
                    {
                        throw new InvalidOperationException(Invariant($"Found discriminator '{value}' when deserializing into {nameof(nominalType)} '{nominalType.ToStringReadable()}', but could not get the actual type using {nameof(TypeNameDiscriminator)}.{nameof(TypeNameDiscriminator.GetActualType)}(); it returned null."));
                    }
                }
            }
            else
            {
                // if _t is not in the payload then a discriminator wasn't needed
                result = nominalType;
            }

            bsonReader.ReturnToBookmark(bookmark);

            // See notes in ThrowOnUnregisteredTypeIfAppropriate for the need to make this call.
            // Note that this is a sub-par solution.  Ideally this discriminator would know
            // which serializer (and hence which serialization configuration) is being used for deserializing,
            // because it is passed as a parameter to this method.  Because that's not an option in Mongo,
            // we have to use ObcBsonSerializer.SerializationConfigurationInUseForDeserialization, which
            // tracks the thread being used for the deserialize operation and associates the serialization
            // configuration in-use with the thread.
            ObcBsonSerializer.SerializationConfigurationInUseForDeserialization.ThrowOnUnregisteredTypeIfAppropriate(result, SerializationDirection.Deserialize, null);

            return result;
        }

        /// <inheritdoc />
        public BsonValue GetDiscriminator(
            Type nominalType,
            Type actualType)
        {
            var result = actualType.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();

            // The below approach didn't work because, prior to calling this method, Mongo
            // registers the missing class map for the closed generic type.  So when we attempt
            // to register our own via ThrowOnUnregisteredTypeIfAppropriate, Mongo throws.
            // We need an earlier hook point.  This is, among other reasons, why SerializationConfigurationBase
            // recurses thru the runtime types of the object being serialized and registers any closed
            // generic types it encounters.
            // if (actualType.IsClosedGenericType())
            // {
            //    // this.serializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(actualType);
            // }
            return result;
        }
    }
}
