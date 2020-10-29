// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.NamedString.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    public partial class ObcPropertyBagSerializer : INamedPropertyBagStringValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> SerializeToNamedPropertyBagWithStringValues(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var propertyNameToObjectMap = this.SerializeToNamedPropertyBagWithObjectValues(objectToSerialize);

            var result = propertyNameToObjectMap.ToDictionary(
                _ => _.Key,
                _ => _.Value == null ? null : this.MakeStringFromPropertyValue(_.Value));

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<string, string> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<string, string> serializedPropertyBag,
            Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (serializedPropertyBag == null)
            {
                return SerializationConfigurationBase.NullSerializedStringValue;
            }

            if (!serializedPropertyBag.Any())
            {
                if (type.HasDefaultConstructor())
                {
                    return type.Construct();
                }
                else
                {
                    throw new SerializationException(Invariant($"Found no properties for type {type} and it also does not have a parameterless constructor."));
                }
            }

            var result = this.ConstructAndFillProperties(type, serializedPropertyBag);

            return result;
        }
    }
}