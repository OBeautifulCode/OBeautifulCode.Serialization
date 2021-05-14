// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritedTypeJsonConverterBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;

    using NewtonsoftFork.Json;

    /// <summary>
    /// A converter that handles inherited types.
    /// </summary>
    internal abstract class InheritedTypeJsonConverterBase : JsonConverter
    {
        /// <summary>
        /// The concrete type token name constant.
        /// </summary>
        protected const string ConcreteTypeTokenName = "$concreteType";

        private readonly Func<ConcurrentDictionary<Type, object>> getNonAbstractBaseClassTypesToHandleFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedTypeJsonConverterBase"/> class.
        /// </summary>
        /// <param name="getNonAbstractBaseClassTypesToHandleFunc">
        /// A func that returns the set non-abstract base class types that, when encountered, should trigger usage of the converter.
        /// These are the types that the converter cannot programmatically identify as inherited.
        /// Note that this is a func so that we can always get the latest types to handle.  That set can get mutated
        /// with post-initialization registrations.
        /// </param>
        /// <param name="getTypesWithConvertersFunc">A func that returns the set of types having converters.</param>
        protected InheritedTypeJsonConverterBase(
            Func<ConcurrentDictionary<Type, object>> getNonAbstractBaseClassTypesToHandleFunc,
            Func<ConcurrentDictionary<Type, object>> getTypesWithConvertersFunc)
        {
            if (getNonAbstractBaseClassTypesToHandleFunc == null)
            {
                throw new ArgumentNullException(nameof(getNonAbstractBaseClassTypesToHandleFunc));
            }

            if (getTypesWithConvertersFunc == null)
            {
                throw new ArgumentNullException(nameof(getTypesWithConvertersFunc));
            }

            this.getNonAbstractBaseClassTypesToHandleFunc = getNonAbstractBaseClassTypesToHandleFunc;
            this.GetTypesWithConvertersFunc = getTypesWithConvertersFunc;
        }

        /// <summary>
        /// Gets a func that returns the set of types having converters.
        /// </summary>
        protected Func<ConcurrentDictionary<Type, object>> GetTypesWithConvertersFunc { get; private set; }

        /// <summary>
        /// Determines if the specified type should be handled by this converter.
        /// </summary>
        /// <param name="objectType">The type.</param>
        /// <returns>
        /// true if the specified type should be handled by this converter; false otherwise.
        /// </returns>
        protected bool ShouldBeHandledByThisConverter(
            Type objectType)
        {
            bool result;

            if (objectType == typeof(object))
            {
                result = true;
            }
            else if (this.GetTypesWithConvertersFunc().ContainsKey(objectType))
            {
                result = false;
            }
            else if (objectType.IsRestrictedType())
            {
                result = false;
            }
            else if (objectType.IsAbstract)
            {
                // Abstract class or interface?
                result = true;
            }
            else if (this.getNonAbstractBaseClassTypesToHandleFunc().ContainsKey(objectType))
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
