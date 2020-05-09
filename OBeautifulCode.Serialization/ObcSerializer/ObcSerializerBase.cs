// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSerializerBase.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Policy;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Serializer that utilizes a fully configured <see cref="SerializationConfigurationBase"/>.
    /// </summary>
    public abstract class ObcSerializerBase : ISerializeAndDeserialize
    {
        private static ConcurrentDictionary<Type, List<MemberInfo>> typeToAllFieldsAndPropertiesMemberInfoMap = new ConcurrentDictionary<Type, List<MemberInfo>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSerializerBase"/> class.
        /// </summary>
        /// <param name="serializationConfigurationType">The serialization configuration type to use.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">
        /// Strategy of how to handle a type that has never been registered.
        /// If <see cref="UnregisteredTypeEncounteredStrategy.Default"/>:
        ///     If type is an <see cref="IImplementNullObjectPattern" /> then the <see cref="UnregisteredTypeEncounteredStrategy.Default"/> strategy will be used.
        ///     Otherwise, the <see cref="UnregisteredTypeEncounteredStrategy.Throw" /> strategy is used.
        /// </param>
        protected ObcSerializerBase(
            SerializationConfigurationType serializationConfigurationType,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy)
        {
            new { serializationConfigurationType }.AsArg().Must().NotBeNull();

            if (unregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Default)
            {
                unregisteredTypeEncounteredStrategy =
                    serializationConfigurationType.ConcreteSerializationConfigurationDerivativeType.IsAssignableTo(typeof(IImplementNullObjectPattern))
                        ? UnregisteredTypeEncounteredStrategy.Attempt
                        : UnregisteredTypeEncounteredStrategy.Throw;
            }

            this.SerializationConfigurationType = serializationConfigurationType;
            this.UnregisteredTypeEncounteredStrategy = unregisteredTypeEncounteredStrategy;
            this.SerializationConfiguration = SerializationConfigurationManager.GetOrAddSerializationConfiguration(serializationConfigurationType);
        }

        /// <summary>
        /// Gets the strategy of how to handle a type that has never been registered.
        /// </summary>
        public UnregisteredTypeEncounteredStrategy UnregisteredTypeEncounteredStrategy { get; }

        /// <summary>
        /// Gets the serialization configuration.
        /// </summary>
        public SerializationConfigurationBase SerializationConfiguration { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType { get; }

        /// <inheritdoc />
        public abstract SerializationKind SerializationKind { get; }

        /// <inheritdoc />
        public abstract byte[] SerializeToBytes(
            object objectToSerialize);

        /// <inheritdoc />
        public abstract string SerializeToString(
            object objectToSerialize);

        /// <inheritdoc />
        public abstract T Deserialize<T>(
            string serializedString);

        /// <inheritdoc />
        public abstract object Deserialize(
            string serializedString,
            Type type);

        /// <inheritdoc />
        public abstract T Deserialize<T>(
            byte[] serializedBytes);

        /// <inheritdoc />
        public abstract object Deserialize(
            byte[] serializedBytes,
            Type type);

        /// <summary>
        /// Throw an <see cref="UnregisteredTypeAttemptException" /> if appropriate.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="serializationDirection">The serialization direction.</param>
        /// <param name="objectToSerialize">Optional object to serialize if serializing.  DEFAULT is null, assumes deserialization.</param>
        protected void ThrowOnUnregisteredTypeIfAppropriate(
            Type type,
            SerializationDirection serializationDirection,
            object objectToSerialize)
        {
            var visitedTypes = new HashSet<Type>();

            if (type == null)
            {
                // this must be supported for serializing null
                // if type == null then objectToSerialize == null
                return;
            }

            if (type.ContainsGenericParameters)
            {
                throw new InvalidOperationException(Invariant($"Cannot serialize or deserialize an open type: {type.ToStringReadable()}."));
            }

            this.InternalThrowOnUnregisteredTypeIfAppropriate(type, type, serializationDirection, objectToSerialize, visitedTypes);
        }

        private void InternalThrowOnUnregisteredTypeIfAppropriate(
            Type originalType,
            Type typeToValidate,
            SerializationDirection serializationDirection,
            object objectToSerialize,
            HashSet<Type> visitedTypes)
        {
            // for serialization we are dealing with validating runtime types:
            //     so we are holding an object and it's object.GetType(), we need to validate that type
            //     as having been registered (in doing so, we are effectively validating the declared types
            //     of the type's properties and fields, because if the type is registered then those property
            //     and field types would have been registered during initialization).  a registered type means
            //     we can serialize it.  But we need to get the runtime types of all of the properties and fields
            //     too, and recurse this validation process.  For arrays, collections and dictionaries we need to recurse
            //     into the runtime types of the elements/keys/values.  after that process, we'll be 100% certain that
            //     all types involved with the object being serialized, have been registered.
            // for deserialization we are dealing with validating declared types:
            //     if the declared type is registered then, upon deserialization, the only unregistered
            //     types that we may encounter in the payload are where the declared type is an interface
            //     or base class and the concrete type is a generic.  at serialization time, writing the
            //     payload for an interface or abstract type requires injecting the assembly qualified name
            //     of the concrete type into the payload to facilitate de-serialization.  and because
            //     serialization (per above) checks all of the runtime types and throws if one if not configured,
            //     the concrete type is already verified as registered (although it would be nice to just check again in
            //     deserialization, for completeness and to guard against using different serialization configuration types
            //     to serialize and deserialize, and to guard against highly constrained RelatedTypesToInclude and MemberTypesToInclude,
            //     but that's more difficult to accomplish at this time) except for generics which happen just-in-time
            //     via RegisterClosedGenericTypePostInitialization.  Upon deserialization, the serializer/wrapped-framework
            //     (e.g. Mongo, Newtonsoft) will perform a just-in-time registration of the concrete generic type (e.g. ObcBsonDiscriminatorConvention).
            //     Note that if we are deserializing into a closed generic type (the top-level type),
            //     we'll check that type here and register it via RegisterClosedGenericTypePostInitialization() if needed.
            //     Otherwise, if the type we are deserializing into has a field or property whose declared types is a closed generic,
            //     not a interface or base class, then checking that that top-level type is registered guarantees that
            //     the closed generic field or property is already registered.
            if (!visitedTypes.Contains(typeToValidate))
            {
                this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, typeToValidate);

                visitedTypes.Add(typeToValidate);
            }

            if (serializationDirection == SerializationDirection.Serialize)
            {
                if (!typeToValidate.IsSystemType())
                {
                    this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, objectToSerialize, visitedTypes);
                }
            }
            else if (serializationDirection == SerializationDirection.Deserialize)
            {

            }
        }


        private void Copy(
            Type originalType,
            Type typeToValidate,
            object objectToSerialize,
            HashSet<Type> visitedTypes)
        {
            if (!typeToAllFieldsAndPropertiesMemberInfoMap.ContainsKey(typeToValidate))
            {
                var memberInfosToAdd = typeToValidate
                    .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(_ => !_.IsCompilerGenerated())
                    .Where(_ => (_ is PropertyInfo) || (_ is FieldInfo))
                    .ToList();

                typeToAllFieldsAndPropertiesMemberInfoMap.TryAdd(typeToValidate, memberInfosToAdd);
            }

            var memberInfos = typeToAllFieldsAndPropertiesMemberInfoMap[typeToValidate];

            foreach (var memberInfo in memberInfos)
            {
                var memberType = memberInfo.GetUnderlyingType();

                if (objectToSerialize != null)
                {
                    // validate the declared type of the member
                    this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, memberType);

                    object memberObject;

                    if (memberInfo is PropertyInfo propertyInfo)
                    {
                        memberObject = propertyInfo.GetValue(objectToSerialize);
                    }
                    else if (memberInfo is FieldInfo fieldInfo)
                    {
                        memberObject = fieldInfo.GetValue(objectToSerialize);
                    }
                    else
                    {
                        throw new NotSupportedException(Invariant($"This type of {nameof(MemberInfo)} is not supported: {memberInfo.GetType().ToStringReadable()}."));
                    }

                    var memberObjectType = memberObject?.GetType();

                    if (memberObjectType != null)
                    {
                        this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, memberObjectType, SerializationDirection.Serialize, memberObject, visitedTypes);

                        if (memberObjectType.IsArray || memberObjectType.IsClosedSystemCollectionType())
                        {
                            var enumerableObject = (IEnumerable)memberObject;
                            foreach (var elementObject in enumerableObject)
                            {
                                if (elementObject != null)
                                {
                                    var elementObjectType = elementObject.GetType();

                                    this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, elementObjectType, SerializationDirection.Serialize, elementObject, visitedTypes);
                                }
                            }
                        }
                        else if (memberObjectType.IsClosedSystemDictionaryType())
                        {
                            var dictionaryObject = (IDictionary)memberObject;
                            foreach (var keyObject in dictionaryObject.Keys)
                            {
                                if (keyObject != null)
                                {
                                    var keyObjectType = keyObject.GetType();

                                    this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, keyObjectType, SerializationDirection.Serialize, keyObject, visitedTypes);
                                }
                            }

                            foreach (var valueObject in dictionaryObject.Values)
                            {
                                if (valueObject != null)
                                {
                                    var valueObjectType = valueObject.GetType();

                                    this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, valueObjectType, SerializationDirection.Serialize, valueObject, visitedTypes);
                                }
                            }
                        }
                    }
                }
            }
        }



        private void InternalThrowOnUnregisteredTypeIfAppropriate(
            Type originalType,
            object objectToSerialize,
            HashSet<Type> visitedTypes)
        {
            // objectType won't be null here; it will get short-circuited by the methods above
            var objectToSerializeType = objectToSerialize.GetType();

            if (!typeToAllFieldsAndPropertiesMemberInfoMap.ContainsKey(objectToSerializeType))
            {
                var memberInfosToAdd = objectToSerializeType
                    .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(_ => !_.IsCompilerGenerated())
                    .Where(_ => (_ is PropertyInfo) || (_ is FieldInfo))
                    .ToList();

                typeToAllFieldsAndPropertiesMemberInfoMap.TryAdd(objectToSerializeType, memberInfosToAdd);
            }

            var memberInfos = typeToAllFieldsAndPropertiesMemberInfoMap[objectToSerializeType];

            foreach (var memberInfo in memberInfos)
            {
                object memberObject;

                if (memberInfo is PropertyInfo propertyInfo)
                {
                    memberObject = propertyInfo.GetValue(objectToSerialize);
                }
                else if (memberInfo is FieldInfo fieldInfo)
                {
                    memberObject = fieldInfo.GetValue(objectToSerialize);
                }
                else
                {
                    throw new NotSupportedException(Invariant($"This type of {nameof(MemberInfo)} is not supported: {memberInfo.GetType().ToStringReadable()}."));
                }

                var memberObjectType = memberObject?.GetType();

                if (memberObjectType != null)
                {
                    this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, memberObjectType, SerializationDirection.Serialize, memberObject, visitedTypes);

                    if (memberObjectType.IsArray || memberObjectType.IsClosedSystemCollectionType())
                    {
                        var enumerableObject = (IEnumerable)memberObject;
                        foreach (var elementObject in enumerableObject)
                        {
                            if (elementObject != null)
                            {
                                var elementObjectType = elementObject.GetType();

                                this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, elementObjectType, SerializationDirection.Serialize, elementObject, visitedTypes);
                            }
                        }
                    }
                    else if (memberObjectType.IsClosedSystemDictionaryType())
                    {
                        var dictionaryObject = (IDictionary)memberObject;
                        foreach (var keyObject in dictionaryObject.Keys)
                        {
                            if (keyObject != null)
                            {
                                var keyObjectType = keyObject.GetType();

                                this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, keyObjectType, SerializationDirection.Serialize, keyObject, visitedTypes);
                            }
                        }

                        foreach (var valueObject in dictionaryObject.Values)
                        {
                            if (valueObject != null)
                            {
                                var valueObjectType = valueObject.GetType();

                                this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, valueObjectType, SerializationDirection.Serialize, valueObject, visitedTypes);
                            }
                        }
                    }
                }
            }
        }

        private void InternalThrowOnUnregisteredTypeIfAppropriate(
            Type originalType,
            Type typeToValidate)
        {
            if (typeToValidate.IsArray)
            {
                this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, typeToValidate.GetElementType());
            }
            else if (typeToValidate.IsGenericType)
            {
                // is the closed type registered?  if so, nothing to do
                if (!this.SerializationConfiguration.IsRegisteredType(typeToValidate))
                {
                    if (typeToValidate.IsSystemType())
                    {
                        // the closed system type is not registered, confirm that the generic type arguments are registered
                        // so this will inspect the element of type of lists, the key/value types of dictionaries, etc.
                        // we do NOT need to do this for non-System types; the code below will call
                        // RegisterClosedGenericTypePostInitialization which, using MemberTypesToExplore.All, will
                        // recurse through the types nested in the generic.
                        foreach (var genericArgumentType in typeToValidate.GenericTypeArguments)
                        {
                            this.InternalThrowOnUnregisteredTypeIfAppropriate(originalType, genericArgumentType);
                        }
                    }
                    else
                    {
                        // for non-System generic types that are not registered, the generic type definition should be registered
                        var genericTypeDefinition = typeToValidate.GetGenericTypeDefinition();

                        this.ThrowUnregisteredTypeEncounteredStrategyIfAppropriate(originalType, genericTypeDefinition);

                        var registeringSerializationConfigurationType = this.SerializationConfiguration.GetRegisteringSerializationConfigurationType(genericTypeDefinition);

                        var registeringSerializationConfiguration = SerializationConfigurationManager.GetOrAddSerializationConfiguration(registeringSerializationConfigurationType);

                        registeringSerializationConfiguration.RegisterClosedGenericTypePostInitialization(typeToValidate);
                    }
                }
            }
            else if (typeToValidate.IsSystemType())
            {
            }
            else
            {
                this.ThrowUnregisteredTypeEncounteredStrategyIfAppropriate(originalType, typeToValidate);
            }
        }

        private void ThrowUnregisteredTypeEncounteredStrategyIfAppropriate(
            Type originalType,
            Type typeToValidate)
        {
            if ((this.UnregisteredTypeEncounteredStrategy == UnregisteredTypeEncounteredStrategy.Throw) && (!this.SerializationConfiguration.IsRegisteredType(typeToValidate)))
            {
                if (originalType == typeToValidate)
                {
                    throw new UnregisteredTypeAttemptException(Invariant($"Attempted to perform operation on unregistered type '{originalType.ToStringReadable()}'."), originalType);
                }
                else
                {
                    throw new UnregisteredTypeAttemptException(Invariant($"Attempted to perform operation on type '{originalType.ToStringReadable()}', which contains the unregistered type '{typeToValidate.ToStringReadable()}'."), originalType);
                }
            }
        }
    }
}
