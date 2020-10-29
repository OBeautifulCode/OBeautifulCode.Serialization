// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.OrdinalString.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;

    public partial class ObcPropertyBagSerializer : IOrdinalPropertyBagStringValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<int, string> SerializeToOrdinalPropertyBagWithStringValues(
            object objectToSerialize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<int, string> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<int, string> serializedPropertyBag,
            Type type)
        {
            throw new NotImplementedException();
        }
    }
}