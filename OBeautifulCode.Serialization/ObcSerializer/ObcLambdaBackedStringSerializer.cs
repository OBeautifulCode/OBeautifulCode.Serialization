// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcLambdaBackedStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;

    /// <summary>
    /// String serializer that is backed by <see cref="Func{T1,TResult}" />.
    /// </summary>
    public class ObcLambdaBackedStringSerializer : IStringSerializeAndDeserialize
    {
        private readonly Func<object, string> serializeString;
        private readonly Func<string, Type, object> deserializeString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcLambdaBackedStringSerializer"/> class.
        /// </summary>
        /// <param name="serializeString">Serialize object to string.</param>
        /// <param name="deserializeString">Deserialize object from string.</param>
        public ObcLambdaBackedStringSerializer(
            Func<object, string> serializeString,
            Func<string, Type, object> deserializeString)
        {
            if (serializeString == null)
            {
                throw new ArgumentNullException(nameof(serializeString));
            }

            if (deserializeString == null)
            {
                throw new ArgumentNullException(nameof(deserializeString));
            }

            this.serializeString = serializeString;
            this.deserializeString = deserializeString;
        }

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            var result = this.serializeString(objectToSerialize);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            var result = (T)this.Deserialize(serializedString, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = this.deserializeString(serializedString, type);

            return result;
        }
    }
}