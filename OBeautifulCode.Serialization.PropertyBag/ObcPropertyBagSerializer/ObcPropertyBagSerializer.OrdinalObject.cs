// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.OrdinalObject.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;

    public partial class ObcPropertyBagSerializer : IOrdinalPropertyBagObjectValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<int, object> SerializeToOrdinalPropertyBagWithObjectValues(
            object objectToSerialize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<int, object> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<int, object> serializedPropertyBag,
            Type type)
        {
            throw new NotImplementedException();
        }
    }
}