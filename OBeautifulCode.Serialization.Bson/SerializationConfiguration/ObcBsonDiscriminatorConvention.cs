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
        /// An instance of this discriminator.
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
            // to register our own via RegisterClosedGenericTypePostInitialization, Mongo throws.
            // We need an earlier hook point.
            // -------------------------------------------
            // see comments in RegisterClosedGenericTypePostInitialization()
            // ObcSerializerBase.InternalThrowOnUnregisteredTypeIfAppropriate will call
            // RegisterClosedGenericTypePostInitialization() on the top-most (the type being serialized
            // thru the front door) type if it's a closed generic, which will recurse thru the related
            // and member types (all of which will be closed at that point).
            //
            // If we are getting here, it means that either:
            //    the top-most type is a closed generic (BSON always uses a discriminator for the top-most type),
            //    in which case this call will have no net effect since it was already made by
            //    InternalThrowOnUnregisteredTypeIfAppropriate()
            // OR
            //    we are serializing a property of the top-most type that is declared as an abstract class or
            //    an interface (hence the need for a discriminator) and the runtime type is a closed generic
            //    which may not have been previously registered.
            // if (actualType.IsClosedGenericType())
            // {
            //    // this.SerializationConfiguration.RegisterClosedGenericTypePostInitialization(actualType);
            // }
            return result;
        }
    }
}
