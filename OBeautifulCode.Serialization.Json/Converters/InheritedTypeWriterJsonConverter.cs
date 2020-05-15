// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritedTypeWriterJsonConverter.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// An <see cref="InheritedTypeJsonConverterBase"/> that handles writes/serialization.
    /// </summary>
    internal class InheritedTypeWriterJsonConverter : InheritedTypeJsonConverterBase
    {
        private bool writeJsonCalled;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritedTypeWriterJsonConverter"/> class.
        /// </summary>
        /// <param name="getTypesToHandleFunc">A func that gets the types that, when encountered, should trigger usage of the converter.</param>
        public InheritedTypeWriterJsonConverter(
            Func<ConcurrentDictionary<Type, object>> getTypesToHandleFunc)
            : base(getTypesToHandleFunc)
        {
        }

        /// <inheritdoc />
        public override bool CanRead => false;

        /// <inheritdoc />
        public override bool CanWrite => true;

        /// <inheritdoc />
        public override bool CanConvert(
            Type objectType)
        {
            // objectType will be the runtime type, NOT the declared type.
            // so if you have an abstract Animal and a derived Dog and you have a property of type Animal,
            // objectType will never be Animal, you'll only "see" Dog.

            // WriteJson needs to use the JsonSerializer passed to the method so that the various
            // settings in the serializer are utilized for writing.  Using the serializer as-is,
            // however, will cause infinite recursion because this Converter is utilized by the serializer.
            // We cannot modify the serializer to remove this converter because the object to serialize
            // might contain types that require this converter.  The only way to manage this to store
            // some state when WriteJson is called, detect that state here, and tell json.net that we
            // cannot convert the type.
            if (this.writeJsonCalled)
            {
                this.writeJsonCalled = false;

                return false;
            }

            var result = this.ShouldBeHandledByThisConverter(objectType);

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
            JsonSerializer serializer)
        {
            // We write the concrete type regardless of whether the declared type is an interface/base class
            // or a derivative class.  That's because, as noted in CanConvert(), we aren't told what the declared
            // type is.  So, unlike Mongo (which handles this through a Discriminator), we have no way of knowing
            // whether we need to write the concrete type.  It always gets written if the runtime type participates
            // in a hierarchy, even if the declared type IS the runtime type and thus the extra concrete type could
            // be excluded.
            new { value }.AsArg().Must().NotBeNull();

            var typeName = value.GetType().ToRepresentation().RemoveAssemblyVersions().BuildAssemblyQualifiedName();

            this.writeJsonCalled = true;

            var jsonObject = JObject.FromObject(value, serializer);

            jsonObject.Add(ConcreteTypeTokenName, typeName);

            jsonObject.WriteTo(writer);
        }
    }
}