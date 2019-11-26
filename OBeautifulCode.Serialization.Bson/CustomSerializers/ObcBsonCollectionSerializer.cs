﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonCollectionSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    /// <summary>
    /// Custom collection serializer to do the right thing for all System collection types.
    /// See: <see cref="TypeExtensions.IsSystemCollectionType(System.Type)"/>.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the collection.</typeparam>
    public class ObcBsonCollectionSerializer<TCollection, TElement> : SerializerBase<TCollection>
        where TCollection : class, IEnumerable<TElement>
    {
        private readonly ReadOnlyCollectionSerializer<TElement> underlyingSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonCollectionSerializer{TCollection,TElement}"/> class.
        /// </summary>
        /// <param name="elementSerializer">The element serializer.</param>
        public ObcBsonCollectionSerializer(
            IBsonSerializer<TElement> elementSerializer)
        {
            typeof(TCollection).IsSystemCollectionType().AsArg("typeof(TCollection).IsSystemCollectionType()").Must().BeTrue();

            this.underlyingSerializer = elementSerializer == null
                ? new ReadOnlyCollectionSerializer<TElement>()
                : new ReadOnlyCollectionSerializer<TElement>(elementSerializer);
        }

        /// <inheritdoc />
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TCollection value)
        {
            new { context }.AsArg().Must().NotBeNull();

            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                ReadOnlyCollection<TElement> wrappedValue;

                if (value is IList<TElement> valueAsIList)
                {
                    wrappedValue = new ReadOnlyCollection<TElement>(valueAsIList);
                }
                else
                {
                    wrappedValue = new ReadOnlyCollection<TElement>(value.ToList());
                }

                // We HAVE to set the NominalType to ReadOnlyCollection<TElement>,
                // otherwise the BSON framework serializes in a way that, upon deserialization,
                // doesn't used the specified elementSerializer.
                args.NominalType = typeof(ReadOnlyCollection<TElement>);

                this.underlyingSerializer.Serialize(context, args, wrappedValue);
            }
        }

        /// <inheritdoc />
        public override TCollection Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();

            TCollection result;

            if ((context.Reader.State != BsonReaderState.Type) && (context.Reader.CurrentBsonType == BsonType.Null))
            {
                context.Reader.ReadNull();

                result = null;
            }
            else
            {
                var readOnlyCollection = this.underlyingSerializer.Deserialize(context, args);

                var deserializedType = typeof(TCollection);

                if (deserializedType == typeof(List<TElement>))
                {
                    result = readOnlyCollection.ToList() as TCollection;
                }
                else if (deserializedType == typeof(Collection<TElement>))
                {
                    result = new Collection<TElement>(readOnlyCollection) as TCollection;
                }
                else
                {
                    result = readOnlyCollection as TCollection;
                }
            }

            return result;
        }
    }
}