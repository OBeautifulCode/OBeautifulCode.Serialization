// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcPropertyBagSerializer.NamedObject.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.PropertyBag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OBeautifulCode.Representation.System;

    using static System.FormattableString;

    public partial class ObcPropertyBagSerializer : INamedPropertyBagObjectValuesSerializeAndDeserialize
    {
        /// <inheritdoc />
        public IReadOnlyDictionary<string, object> SerializeToNamedPropertyBagWithObjectValues(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var objectType = objectToSerialize.GetType();

            var propertyNames = objectType.GetProperties(GetPropertiesOfConcernBindingFlags);

            var result = propertyNames.ToDictionary(
                _ => _.Name,
                _ =>
                {
                    var propertyInfo = objectType.GetProperty(_.Name, GetPropertiesOfConcernBindingFlags);

                    if (propertyInfo == null)
                    {
                        throw new ArgumentNullException(Invariant($"Could not find {nameof(PropertyInfo)} on type: {objectType} by name: {_.Name}"));
                    }

                    var propertyValue = propertyInfo.GetValue(objectToSerialize);

                    return propertyValue;
                });

            result.Add(ReservedKeyForToString, objectToSerialize.ToString());

            var specifiedTypeVersionlessAssemblyQualifiedName = objectType.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();

            result.Add(ReservedKeyForTypeVersionlessAssemblyQualifiedName, specifiedTypeVersionlessAssemblyQualifiedName);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            IReadOnlyDictionary<string, object> serializedPropertyBag)
        {
            var result = (T)this.Deserialize(serializedPropertyBag, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            IReadOnlyDictionary<string, object> serializedPropertyBag,
            Type type)
        {
            throw new NotImplementedException();
        }
    }
}
