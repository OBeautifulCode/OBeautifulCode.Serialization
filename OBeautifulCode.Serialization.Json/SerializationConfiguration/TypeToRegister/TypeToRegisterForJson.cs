// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToRegisterForJson.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using Newtonsoft.Json;

    using static System.FormattableString;

    /// <summary>
    /// Specifies a type to register for JSON serialization.
    /// </summary>
    public class TypeToRegisterForJson : TypeToRegister
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForJson"/> class with a type that is it's own origin.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        /// <param name="jsonConverterBuilder">Builds a serializing and deserializing <see cref="JsonConverter"/>.</param>
        public TypeToRegisterForJson(
            Type type,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            JsonConverterBuilder jsonConverterBuilder)
            : this(type, type, type, memberTypesToInclude, relatedTypesToInclude, jsonConverterBuilder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToRegisterForJson"/> class, specifying the origin types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="recursiveOriginType">The type whose recursive processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> resulted in the creation of this <see cref="TypeToRegisterForJson"/>.</param>
        /// <param name="directOriginType">The type whose processing of <paramref name="memberTypesToInclude"/> and <paramref name="relatedTypesToInclude"/> directly resulted in the creation of this <see cref="TypeToRegisterForJson"/>.</param>
        /// <param name="memberTypesToInclude">Specifies which member types of <paramref name="type"/> that should also be registered.</param>
        /// <param name="relatedTypesToInclude">Specifies which types related to <paramref name="type"/> that should also be registered.</param>
        /// <param name="jsonConverterBuilder">Builds a serializing and deserializing <see cref="JsonConverter"/>.</param>
        public TypeToRegisterForJson(
            Type type,
            Type recursiveOriginType,
            Type directOriginType,
            MemberTypesToInclude memberTypesToInclude,
            RelatedTypesToInclude relatedTypesToInclude,
            JsonConverterBuilder jsonConverterBuilder)
            : base(type, recursiveOriginType, directOriginType, memberTypesToInclude, relatedTypesToInclude)
        {
            if (jsonConverterBuilder != null)
            {
                if (memberTypesToInclude != MemberTypesToInclude.None)
                {
                    throw new ArgumentException(Invariant($"{nameof(jsonConverterBuilder)} is specified, but {nameof(Serialization.MemberTypesToInclude)} is not {MemberTypesToInclude.None}."));
                }
            }

            this.JsonConverterBuilder = jsonConverterBuilder;
        }

        /// <summary>
        /// Gets an object that builds a serializing and deserializing <see cref="JsonConverter"/>.
        /// </summary>
        public JsonConverterBuilder JsonConverterBuilder { get; }

        /// <inheritdoc />
        public override TypeToRegister CreateSpawnedTypeToRegister(
            Type type)
        {
            var result = new TypeToRegisterForJson(type, this.RecursiveOriginType, this.Type, this.MemberTypesToInclude, this.RelatedTypesToInclude, this.JsonConverterBuilder);

            return result;
        }
    }
}
