// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryJsonConverterBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using NewtonsoftFork.Json;

    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;

    /// <summary>
    /// Custom dictionary converter to do the right thing for System Dictionary types.
    /// See: <see cref="TypeExtensions.IsClosedSystemDictionaryType"/> for supported types.
    /// </summary>
    internal abstract class DictionaryJsonConverterBase : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryJsonConverterBase"/> class.
        /// </summary>
        /// <param name="typesThatSerializeToString">The types that are serialized as strings.</param>
        protected DictionaryJsonConverterBase(
            IReadOnlyCollection<Type> typesThatSerializeToString)
        {
            this.TypesThatSerializeToString = typesThatSerializeToString ?? new Type[0];
        }

        /// <summary>
        /// Gets the types that are serialized as strings.
        /// </summary>
        protected IReadOnlyCollection<Type> TypesThatSerializeToString { get; }

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType,
            Type declaredType)
        {
            bool result;

            if (objectType == null)
            {
                result = false;
            }
            else
            {
                // Note that we are NOT checking whether the type is assignable to a dictionary type,
                // we are specifically checking that the type is a System dictionary type.
                // If the consumer is deriving from a dictionary type, they should create a custom converter.
                if (objectType.IsClosedSystemDictionaryType())
                {
                    var keyType = objectType.GetClosedSystemDictionaryKeyType();

                    result = this.ShouldHandleKeyType(keyType);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the wrapped dictionary into the correct return type.
        /// </summary>
        /// <param name="returnType">Type to convert to.</param>
        /// <param name="wrappedDictionary">Wrapped dictionary to convert.</param>
        /// <param name="genericArguments">Generic arguments.</param>
        /// <returns>
        /// Converted dictionary to proper return object if necessary.
        /// </returns>
        protected static object ConvertResultAsNecessary(
            Type returnType,
            object wrappedDictionary,
            Type[] genericArguments)
        {
            object result;

            var unboundedGenericReturnType = returnType.GetGenericTypeDefinition();

            if ((unboundedGenericReturnType == typeof(IDictionary<,>)) || (unboundedGenericReturnType == typeof(Dictionary<,>)))
            {
                // nothing to do, the dictionary is already of the expected return type
                result = wrappedDictionary;
            }
            else if ((unboundedGenericReturnType == typeof(ReadOnlyDictionary<,>)) ||
                     (unboundedGenericReturnType == typeof(IReadOnlyDictionary<,>)))
            {
                result = typeof(ReadOnlyDictionary<,>).MakeGenericType(genericArguments).Construct(wrappedDictionary);
            }
            else if (unboundedGenericReturnType == typeof(ConcurrentDictionary<,>))
            {
                result = typeof(ConcurrentDictionary<,>).MakeGenericType(genericArguments).Construct(wrappedDictionary);
            }
            else
            {
                throw new InvalidOperationException("The following type was not expected: " + returnType);
            }

            return result;
        }

        /// <summary>
        /// Determines if this converter should handle the specified type of dictionary key.
        /// </summary>
        /// <param name="keyType">The type of the dictionary key.</param>
        /// <returns>
        /// A value indicating whether or not this converter should handle the specified type of dictionary key.
        /// </returns>
        protected abstract bool ShouldHandleKeyType(
            Type keyType);
    }
}
