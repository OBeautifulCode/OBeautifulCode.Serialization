﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredTypeBsonSerializer{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Bson
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// The serializer that is used when <see cref="TypeToRegisterForBson"/> specifies a <see cref="BsonSerializerBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type that the backing serializer was registered with.</typeparam>
    /// <remarks>
    /// This class simply wraps the serializer that is generated by <see cref="BsonSerializerBuilder"/>.
    /// The behavior of <see cref="RelatedTypesToInclude"/> is such that that <see cref="BsonSerializerBuilder"/>
    /// could be specified for one type, but then get applied to other types.  For example, if
    /// <see cref="RelatedTypesToInclude.Descendants"/> is specified and the type has descendants, then those
    /// descendants will get registered with the specified serializer.  However, an <see cref="IBsonSerializer"/>
    /// only supports a single type in <see cref="IBsonSerializer.ValueType"/>.  We are not really sure how this
    /// property is used and whether it even matters, because the serializer will get registered for each
    /// individual type (e.g. once for the original type and once for each descendant) via
    /// <see cref="BsonSerializer.RegisterSerializer"/>.  Is there an problem if the type being registered doesn't
    /// match the type of the serializer?  This class exists for safety/to avoid any cases where it does matter.
    /// </remarks>
    public class RegisteredTypeBsonSerializer<T> : SerializerBase<T>
    {
        private readonly IBsonSerializer backingSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredTypeBsonSerializer{T}"/> class.
        /// </summary>
        /// <param name="backingSerializer">The backing serializer.</param>
        public RegisteredTypeBsonSerializer(
            IBsonSerializer backingSerializer)
        {
            new { backingSerializer }.AsArg().Must().NotBeNull();

            this.backingSerializer = backingSerializer;
        }

        /// <inheritdoc />
        public override T Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            var result = (T)this.backingSerializer.Deserialize(context, args);

            return result;
        }

        /// <inheritdoc />
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            T value)
        {
            this.backingSerializer.Serialize(context, args, value);
        }
    }
}