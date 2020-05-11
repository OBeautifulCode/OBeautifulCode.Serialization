// --------------------------------------------------------------------------------------------------------------------
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

    using OBeautifulCode.Assertion.Recipes;
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
        private readonly SerializationConfigurationBase serializationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonDiscriminatorConvention"/> class.
        /// </summary>
        /// <param name="serializationConfiguration">A reference to the serialization configuration instance that created this discriminator.</param>
        public ObcBsonDiscriminatorConvention(
            SerializationConfigurationBase serializationConfiguration)
        {
            new { serializationConfiguration }.AsArg().Must().NotBeNull();

            this.serializationConfiguration = serializationConfiguration;
        }

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
            // Note that this isn't a 100% complete solution.  Ideally this discriminator would know
            // which serializer (and hence which serialization configuration) is being used for deserializing,
            // because it is passed as a parameter to this method.  Because that's not an option in Mongo,
            // we have to construct this discriminator with a config and thus we have to choose which single config
            // to use.  See notes in BsonSerializationConfigurationBase.  Upon the first attempt to serialize
            // or de-serialize, we will consider THAT serializer's config to be the top-level config,
            // we will iterate thru all registered types, creating discriminators and use that config when
            // constructing those discriminators.  In that way, we ensure that ThrowOnUnregisteredTypeIfAppropriate
            // is being called on the "front-door"/oldest-ancestor config.  This has two problems:
            //    1. Another serializer can be instantiated with an even older ancestor config after the fact,
            //       and we have no way to re-point to that config here.
            //    2. Another serializer can be instantiated with a config that is in a completely different
            //       inheritance tree than this.serializationConfiguration, but still registers the same
            //       interface or base class type that this discriminator was created for.
            // To avoid these problems, we disallow serializers from requesting two different configs in
            // SerializationConfigurationManager.  First one wins, others will throw.
            this.serializationConfiguration.ThrowOnUnregisteredTypeIfAppropriate(result, SerializationDirection.Deserialize, null);

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
