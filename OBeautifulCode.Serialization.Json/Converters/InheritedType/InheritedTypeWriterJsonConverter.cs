// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritedTypeWriterJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;

    using NewtonsoftFork.Json;
    using NewtonsoftFork.Json.Linq;

    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// An <see cref="InheritedTypeJsonConverterBase"/> that handles writes/serialization.
    /// </summary>
    internal class InheritedTypeWriterJsonConverter : InheritedTypeJsonConverterBase
    {
        private static readonly ConcurrentDictionary<Type, Type> CachedTypeToObjectWrapperTypeMap = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedTypeWriterJsonConverter"/> class.
        /// </summary>
        /// <param name="getNonAbstractBaseClassTypesToHandleFunc">
        /// A func that returns the set non-abstract base class types that, when encountered, should trigger usage of the converter.
        /// These are the types that the converter cannot programmatically identify as inherited.
        /// </param>
        /// <param name="getTypesWithConvertersFunc">A func that returns the set of types having converters.</param>
        public InheritedTypeWriterJsonConverter(
            Func<ConcurrentDictionary<Type, object>> getNonAbstractBaseClassTypesToHandleFunc,
            Func<ConcurrentDictionary<Type, object>> getTypesWithConvertersFunc)
            : base(getNonAbstractBaseClassTypesToHandleFunc, getTypesWithConvertersFunc)
        {
        }

        /// <inheritdoc />
        public override bool CanRead => false;

        /// <inheritdoc />
        public override bool CanWrite => true;

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType,
            Type declaredType)
        {
            if (declaredType == null)
            {
                throw new ArgumentNullException(nameof(declaredType));
            }

            var result = this.ShouldBeHandledByThisConverter(declaredType);

            return result;
        }

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            throw new NotSupportedException("This is a write-only converter");
        }

        /// <inheritdoc />
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer,
            Type declaredType)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // If the value's declared type is typeof(object), then we need to wrap the value in an ObjectWrapper<T>
            // and serialize that wrapper object.  That will result in a $concreteType being written with T as
            // as the runtime type of the value, which can be pulled out by InheritedTypeReaderJsonConverter
            // and property deserialized.
            if (declaredType == typeof(object))
            {
                var valueRuntimeType = value.GetType();

                if (!CachedTypeToObjectWrapperTypeMap.TryGetValue(valueRuntimeType, out var objectWrapperType))
                {
                    objectWrapperType = typeof(ObjectWrapper<>).MakeGenericType(valueRuntimeType);

                    CachedTypeToObjectWrapperTypeMap.TryAdd(valueRuntimeType, objectWrapperType);
                }

                value = objectWrapperType.Construct(value);
            }

            var runtimeType = value.GetType();

            var typeName = runtimeType.ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();

            var jsonObject = JObject.FromObject(value, serializer, runtimeType);

            jsonObject.Add(ConcreteTypeTokenName, typeName);

            jsonObject.WriteTo(writer);
        }
    }
}