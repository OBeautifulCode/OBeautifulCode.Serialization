// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CamelStrictConstructorContractResolver.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using NewtonsoftFork.Json;
    using NewtonsoftFork.Json.Serialization;

    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Resolves member mappings for a type using camel casing property names.
    /// Also requires that constructor parameters are defined in the json string
    /// when deserializing, for types with non-default constructors.
    /// </summary>
    /// <remarks>
    /// See. <a href="https://stackoverflow.com/questions/37416233/json-net-should-not-use-default-values-for-constructor-parameters-should-use-de"/>
    ///
    /// As of 7.0.1, Json.NET suggests using a static instance for "stateless" contract resolvers, for performance reasons.
    /// We CANNOT do that because we need to pass in the registered types.
    /// <a href="http://www.newtonsoft.com/json/help/html/ContractResolver.htm"/>
    /// <a href="http://www.newtonsoft.com/json/help/html/M_Newtonsoft_Json_Serialization_DefaultContractResolver__ctor_1.htm"/>
    /// "Use the parameter-less constructor and cache instances of the contract resolver within your application for optimal performance."
    /// Also. <a href="https://stackoverflow.com/questions/33557737/does-json-net-cache-types-serialization-information"/>
    /// </remarks>
    public class CamelStrictConstructorContractResolver
        : CamelCasePropertyNamesContractResolver
    {
        private readonly Func<IReadOnlyDictionary<Type, RegistrationDetails>> getRegisteredTypesFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CamelStrictConstructorContractResolver"/> class.
        /// </summary>
        /// <param name="getRegisteredTypesFunc">
        /// A func that gets the registered types mapped to the registration details.
        /// Note that this is a func so that we can always get the latest registered types.
        /// That set can get mutated with post-initialization registrations.
        /// </param>
        public CamelStrictConstructorContractResolver(
            Func<IReadOnlyDictionary<Type, RegistrationDetails>> getRegisteredTypesFunc)
        {
            if (getRegisteredTypesFunc == null)
            {
                throw new ArgumentNullException(nameof(getRegisteredTypesFunc));
            }

            // this will cause dictionary keys to be lowercased if set to true (or you can set to true and set a dictionary key resolver).
            this.NamingStrategy.ProcessDictionaryKeys = false;

            this.getRegisteredTypesFunc = getRegisteredTypesFunc;
        }

        /// <inheritdoc />
        /// <remarks>
        /// We are overriding this method to provide better support for objects with non-default constructors.
        ///
        /// Out-of-the-box, Newtonsoft throws when deserializing an object with multiple constructors.
        /// The solution is to add the [JsonConstructor] attribute on the constructor to use, but that
        /// pollutes the model and requires a reference to Newtonsoft in the project.
        /// The RIGHT way to do this is to do what BSON does - find the constructor with the maximum number
        /// of parameters having a corresponding property.  This a little cumbersome so we are using a less optimal
        /// but totally sufficient heuristic: we choose the constructor with the maximum number of parameters.
        /// However we could fix this up in the future.
        ///
        /// Another deficiency is support for models containing
        /// (a) initialized getter-only (e.g. public string MyProperty { get; } = "whatever") or
        /// (b) expression bodied getter-only (e.g. public string MyProperty => "whatever) properties.
        ///
        /// These properties are serialized and so, upon deserialization, Newtonsoft attempts to serialize
        /// them and throws if it can't, despite the fact that these properties cannot be set and thus
        /// should just be ignored.  WE HAVE NOT BEEN ABLE TO FIX-UP THIS DEFICIENCY.  We tried two approaches:
        /// 1. In the call to result.CreatorParameters.AddRange(...) only add properties that correspond to constructor
        ///    parameters.  This effectively does nothing because CreateConstructorParameters already filters-out unused
        ///    properties.  Newtonsoft still attempts to deserialize these properties before calling the constructor and throws.
        /// 2. Remove the getter-only properties from result.Properties.  In this case Newtonsoft does not throw, however
        ///    only models with expression-bodied properties would roundtrip.  Models with initialized getter-only properties
        ///    didn't call the initializer and thus those properties were set to null.  Also, in all cases the property
        ///    was NOT written to the payload upon serialization, which was a non-starter for us.  We expect all properties
        ///    to be written to the JSON payload, regardless of whether they are required for deserialization.
        /// </remarks>
        protected override JsonObjectContract CreateObjectContract(
            Type objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            var result = base.CreateObjectContract(objectType);

            // only apply the heuristic on registered types
            var registeredTypeToRegistrationDetailsMap = this.getRegisteredTypesFunc();

            if (registeredTypeToRegistrationDetailsMap.TryGetValue(objectType, out RegistrationDetails registrationDetails))
            {
                // if a converter is registered then we DON'T want to augment the contract
                var typeToRegisterForJson = (TypeToRegisterForJson)registrationDetails.TypeToRegister;

                if (typeToRegisterForJson.JsonConverterBuilder == null)
                {
                    var createdType = Nullable.GetUnderlyingType(objectType) ?? objectType;

                    var isInstantiable = !createdType.IsAbstract;

                    // this is somewhat reverse engineered from DefaultContractResolver.CreateObjectContract
                    // the object must be instantiable, and Newtonsoft did not find a constructor attributed with [JsonConstructor] (OverrideConstructor)
                    // nor did it find a single parameterized constructor (ParametrizedConstructor)
                    #pragma warning disable 618
                    if (isInstantiable && (result.OverrideConstructor == null) && (result.ParametrizedConstructor == null))
                    #pragma warning restore 618
                    {
                        var constructors = objectType.GetConstructorsFiltered(MemberRelationships.DeclaredOrInherited, MemberOwners.Instance, MemberAccessModifiers.Public);

                        // if constructors.Count == 1 then it must be the default constructor because we ruled-out
                        // a single parameterized constructor.  we don't think constructors.Count == 0 is possible.
                        if (constructors.Count > 1)
                        {
                            var maxParameterCount = constructors.Max(_ => _.GetParameters().Length);

                            var constructorsWithMaxParameterCount = constructors.Where(_ => _.GetParameters().Length == maxParameterCount).ToList();

                            if (constructorsWithMaxParameterCount.Count > 1)
                            {
                                throw new JsonSerializationException(Invariant($"Cannot deserialize registered type {objectType.ToStringReadable()} because it has multiple candidate constructors to use.  Looking for a single constructor with the maximum number of parameters, however found {constructorsWithMaxParameterCount.Count} constructors with {maxParameterCount} parameters."));
                            }

                            var parameterizedConstructor = constructorsWithMaxParameterCount.Single();

                            #pragma warning disable 618
                            result.ParametrizedConstructor = parameterizedConstructor;
                            #pragma warning restore 618

                            // CreatorParameters is empty regardless of whether one of the constructors is the default constructor
                            // We do not need to scope the properties down to just the constructor properties because CreateConstructorParameters will handle that.
                            result.CreatorParameters.Clear();
                            result.CreatorParameters.AddRange(this.CreateConstructorParameters(parameterizedConstructor, result.Properties));

                            // DefaultCreator is != null when an object has a default constructor,
                            // but also has one or more parameterized constructors, so we need to clear it out here
                            // so that it doesn't get used.
                            result.DefaultCreator = null;
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "This method is largely a copy-paste of the method it overrides, and that method does not validate arguments.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "This method is largely a copy-paste of the method it overrides, and that method does not validate arguments.")]
        protected override IList<JsonProperty> CreateConstructorParameters(
            ConstructorInfo constructor,
            JsonPropertyCollection memberProperties)
        {
            var constructorParameters = constructor.GetParameters();

            var result = new JsonPropertyCollection(constructor.DeclaringType);

            foreach (var parameterInfo in constructorParameters)
            {
                var matchingMemberProperty = (parameterInfo.Name != null) ? memberProperties.GetClosestMatchProperty(parameterInfo.Name) : null;

                // Constructor type must be assignable from property type.
                // Note that this is the only difference between this method and the method it overrides in DefaultContractResolver.
                // In DefaultContractResolver, the types must match exactly.
                if (matchingMemberProperty != null)
                {
                    var memberType = matchingMemberProperty.PropertyType;
                    var memberTypeIsGeneric = memberType.IsGenericType;
                    var memberGenericArguments = memberType.GenericTypeArguments;
                    var parameterTypeIsArray = parameterInfo.ParameterType.IsArray;
                    var parameterElementType = parameterInfo.ParameterType.GetElementType();
                    if (parameterTypeIsArray
                        && memberTypeIsGeneric
                        && memberGenericArguments.Length == 1
                        && memberType.IsAssignableTo(typeof(IEnumerable<>).MakeGenericType(parameterElementType)))
                    {
                        // NO-OP
                        // this allows for the constructor parameter to be a "params" array while still using a collection property as the source.
                    }
                    else if (memberType.IsAssignableTo(parameterInfo.ParameterType))
                    {
                        // NO-OP
                        // The property type and the constructor parameter type are equal.
                        // OR
                        // The property type is assignable to the constructor parameter type.
                        // In this case, the constructor is taking a less derived type and converting it
                        // to a more derived type before assigning to the property
                        // (e.g. constructor takes IEnumerable<string>, but property is an IReadOnlyCollection<string>
                        //       because the constructor calls .ToList() before assigning to the property).
                    }
                    else if (parameterInfo.ParameterType.IsAssignableTo(memberType))
                    {
                        // NO-OP
                        // The constructor parameter type is assignable to the property type.
                        // In this case, the constructor is taking a more derived type and assigning
                        // it to the property that is less derived.
                        // (e.g. constructor takes IReadOnlyCollection<string> and assigns it to a property
                        //       of type IEnumerable<string>).
                    }
                    else
                    {
                        // no way to do this so null out and the let the next step error with a clean message.
                        matchingMemberProperty = null;
                    }
                }

                if (matchingMemberProperty != null || parameterInfo.Name != null)
                {
                    var property = this.CreatePropertyFromConstructorParameterWithConstructorInfo(matchingMemberProperty, parameterInfo, constructor);

                    if (property != null)
                    {
                        result.AddProperty(property);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override JsonProperty CreatePropertyFromConstructorParameter(
            JsonProperty matchingMemberProperty,
            ParameterInfo parameterInfo)
        {
            var result = this.CreatePropertyFromConstructorParameterWithConstructorInfo(matchingMemberProperty, parameterInfo);

            return result;
        }

        private JsonProperty CreatePropertyFromConstructorParameterWithConstructorInfo(
            JsonProperty matchingMemberProperty,
            ParameterInfo parameterInfo,
            ConstructorInfo constructor = null)
        {
            if (matchingMemberProperty == null)
            {
                var constructorMessage = constructor == null
                    ? string.Empty
                    : constructor.DeclaringType?.FullName;

                var parameterMessage = parameterInfo == null
                    ? "all parameters"
                    : string.Format(CultureInfo.InvariantCulture, "parameter '{0}'", parameterInfo.Name);

                var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Constructor for '{0}' requires {1}; but it was not found in the json",
                    constructorMessage,
                    parameterMessage);

                throw new JsonSerializationException(message);
            }

            var result = base.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);

            if (result != null)
            {
                var required = matchingMemberProperty.Required;
                if (required == Required.Default)
                {
                    var parameterTypeAssignableToNull = parameterInfo.ParameterType.IsClosedTypeAssignableToNull();

                    if (parameterInfo.HasDefaultValue && ((!parameterTypeAssignableToNull) || (parameterInfo.DefaultValue == null)))
                    {
                        // See this for the difference between HasDefaultValue and IsOptional:
                        // https://stackoverflow.com/questions/16186694/difference-between-parameterinfo-isoptional-and-parameterinfo-hasdefaultvalue
                        // The constructor parameter is optional, and so it does not need to be defined in JSON.
                        // This allows us to add properties to models over time.  If a property (and associated constructor parameter)
                        // is added, then old payloads won't have that property and the model cannot be deserialized unless we
                        // set Required = Required.Default.  We need to set DefaultValue and DefaultValueHandling, otherwise Newtonsoft uses default(T).
                        // Additionally, there is a BUG in NewtonsoftFork.Json 9.0.1 such that this approach only works for types that cannot be assigned
                        // to null (e.g. intParam = 0) -OR- types that can be assigned to null where the constructor parameter's default value is null
                        // (e.g. nullableIntParam = null).  If the constructor parameter can be assigned to null but it's default value is not null
                        // (e.g. nullableIntParam = 10), and the value in the payload is null, Newtonsoft uses the default value instead of the null payload value.
                        // Note that the only types that are assignable to null but can be defaulted to a non-null value are Nullable types and string;
                        // all other types that are assignable to null can only be defaulted to null (try it out!  compiler does not allow it).
                        // The issue is with JsonSerializerInternalReader.CreateObjectUsingCreatorWithParameters line 1959:
                        // when null is in the payload, context.Presence == PropertyPresence.Null, so we enter the if block and grab the parameter's default value.
                        result.DefaultValue = parameterInfo.DefaultValue;

                        // note that the Include flag = 0 so really we are just setting Populate here.
                        // Meaning, by default serialization includes all members;
                        // to specifically ignore you have to set the Ignore flag which = 1.
                        result.DefaultValueHandling = DefaultValueHandling.Include | DefaultValueHandling.Populate;
                    }
                    else if (matchingMemberProperty.PropertyType != null &&
                             matchingMemberProperty.PropertyType.IsValueType &&
                             Nullable.GetUnderlyingType(matchingMemberProperty.PropertyType) == null)
                    {
                        // the property must be defined in JSON and cannot be null
                        required = Required.Always;
                    }
                    else
                    {
                        // this does NOT mean that the parameter is not required
                        // the property must be defined in JSON, but can be null
                        required = Required.AllowNull;
                    }

                    result.Required = matchingMemberProperty.Required = required;
                }
            }

            return result;
        }
    }
}
