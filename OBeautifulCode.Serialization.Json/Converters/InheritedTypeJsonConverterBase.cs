// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritedTypeJsonConverterBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;

    using Newtonsoft.Json;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// A converter that handles inherited types.
    /// </summary>
    internal abstract class InheritedTypeJsonConverterBase : JsonConverter
    {
        /// <summary>
        /// The concrete type token name constant.
        /// </summary>
        protected const string ConcreteTypeTokenName = "$concreteType";

        private readonly Func<ConcurrentDictionary<Type, object>> getTypesToHandleFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedTypeJsonConverterBase"/> class.
        /// </summary>
        /// <param name="getTypesToHandleFunc">
        /// A func that returns the set types that, when encountered, should trigger usage of the converter.
        /// Note that this is a func so that we can always get the latest types to handle.  That set can get mutated
        /// with post-initialization registrations.
        /// </param>
        protected InheritedTypeJsonConverterBase(
            Func<ConcurrentDictionary<Type, object>> getTypesToHandleFunc)
        {
            new { getTypesToHandleFunc }.AsArg().Must().NotBeNull();

            this.getTypesToHandleFunc = getTypesToHandleFunc;
        }

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
            var result = this.getTypesToHandleFunc().ContainsKey(objectType);

            return result;
        }
    }
}
