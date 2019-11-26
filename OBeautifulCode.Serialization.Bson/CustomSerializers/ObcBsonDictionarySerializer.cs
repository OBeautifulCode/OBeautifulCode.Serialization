// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcBsonDictionarySerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Options;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type.Recipes;

    /// <summary>
    /// Custom dictionary serializer to do the right thing for System dictionary types.
    /// See <see cref="TypeExtensions.IsSystemDictionaryType(System.Type)"/>.
    /// </summary>
    /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
    /// <typeparam name="TKey">The type of the key of the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the value of the dictionary.</typeparam>
    public class ObcBsonDictionarySerializer<TDictionary, TKey, TValue> : SerializerBase<TDictionary>
        where TDictionary : class, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly DictionaryInterfaceImplementerSerializer<Dictionary<TKey, TValue>> underlyingSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcBsonDictionarySerializer{TDictionary,TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionaryRepresentation">The dictionary representation.</param>
        /// <param name="keySerializer">The key serializer.</param>
        /// <param name="valueSerializer">The value serializer.</param>
        public ObcBsonDictionarySerializer(
            DictionaryRepresentation dictionaryRepresentation,
            IBsonSerializer keySerializer,
            IBsonSerializer valueSerializer)
        {
            typeof(TDictionary).IsSystemDictionaryType().AsArg("typeof(TDictionary).IsSystemDictionaryType()").Must().BeTrue();

            this.underlyingSerializer = new DictionaryInterfaceImplementerSerializer<Dictionary<TKey, TValue>>(dictionaryRepresentation, keySerializer, valueSerializer);
        }

        /// <inheritdoc />
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            TDictionary value)
        {
            new { context }.AsArg().Must().NotBeNull();

            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                var valueAsDictionary = value as Dictionary<TKey, TValue> ?? value.ToDictionary(_ => _.Key, _ => _.Value);

                // We HAVE to set the NominalType to IDictionary<TKey, TValue>,
                // otherwise the BSON framework serializes in a way that, upon deserialization,
                // doesn't used the specified key and value serializers.
                var argsNominalType = args.NominalType;
                args.NominalType = typeof(Dictionary<TKey, TValue>);

                this.underlyingSerializer.Serialize(context, args, valueAsDictionary);

                // restore NominalType
                args.NominalType = argsNominalType;
            }
        }

        /// <inheritdoc />
        public override TDictionary Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            new { context }.AsArg().Must().NotBeNull();

            TDictionary result;

            if (context.Reader.State != BsonReaderState.Type && context.Reader.CurrentBsonType == BsonType.Null)
            {
                context.Reader.ReadNull();

                result = null;
            }
            else
            {
                // set NominalType
                var argsNominalType = args.NominalType;
                args.NominalType = typeof(Dictionary<TKey, TValue>);

                var dictionary = this.underlyingSerializer.Deserialize(context, args);

                // restore NominalType
                args.NominalType = argsNominalType;

                var deserializedType = typeof(TDictionary);

                if (deserializedType == typeof(ReadOnlyDictionary<TKey, TValue>))
                {
                    result = new ReadOnlyDictionary<TKey, TValue>(dictionary) as TDictionary;
                }
                else if (deserializedType == typeof(ConcurrentDictionary<TKey, TValue>))
                {
                    result = new ConcurrentDictionary<TKey, TValue>(dictionary) as TDictionary;
                }
                else
                {
                    result = dictionary as TDictionary;
                }
            }

            return result;
        }
    }
}