// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionBsonSerializer.cs" company="OBeautifulCode">
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
    /// See: <see cref="TypeExtensions.IsClosedSystemCollectionType(System.Type)"/>.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the collection.</typeparam>
    public class CollectionBsonSerializer<TCollection, TElement> : SerializerBase<TCollection>
        where TCollection : class, IEnumerable<TElement>
    {
        private readonly ReadOnlyCollectionSerializer<TElement> underlyingSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionBsonSerializer{TCollection,TElement}"/> class.
        /// </summary>
        /// <param name="elementSerializer">The element serializer.</param>
        public CollectionBsonSerializer(
            IBsonSerializer<TElement> elementSerializer)
        {
            typeof(TCollection).IsClosedSystemCollectionType().AsArg("typeof(TCollection).IsSystemCollectionType()").Must().BeTrue();

            this.underlyingSerializer = elementSerializer == null
                ? new ReadOnlyCollectionSerializer<TElement>()
                : new ReadOnlyCollectionSerializer<TElement>(elementSerializer);
        }

        /// <inheritdoc />
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            TCollection value)
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
                var argsNominalType = args.NominalType;
                args.NominalType = typeof(ReadOnlyCollection<TElement>);

                this.underlyingSerializer.Serialize(context, args, wrappedValue);

                // restore the NominalType
                args.NominalType = argsNominalType;
            }
        }

        /// <inheritdoc />
        public override TCollection Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
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
                // set Nominal Type
                var argsNominalType = args.NominalType;
                args.NominalType = typeof(ReadOnlyCollection<TElement>);

                var readOnlyCollection = this.underlyingSerializer.Deserialize(context, args);

                // restore nominal type
                args.NominalType = argsNominalType;

                var deserializedType = typeof(TCollection);

                if ((deserializedType == typeof(List<TElement>)) ||
                    (deserializedType == typeof(IList<TElement>)) ||
                    (deserializedType == typeof(ICollection<TElement>)))
                {
                    result = readOnlyCollection.ToList() as TCollection;
                }
                else if (deserializedType == typeof(Collection<TElement>))
                {
                    result = new Collection<TElement>(readOnlyCollection.ToList()) as TCollection;
                }
                else
                {
                    ////typeof(ReadOnlyCollection<>),
                    ////typeof(IReadOnlyCollection<>),
                    ////typeof(IReadOnlyList<>),
                    result = readOnlyCollection as TCollection;
                }
            }

            return result;
        }
    }
}