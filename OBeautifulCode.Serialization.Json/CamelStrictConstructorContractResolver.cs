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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="CamelStrictConstructorContractResolver"/> class.
        /// </summary>
        /// <param name="registeredTypes">The registered types.</param>
        public CamelStrictConstructorContractResolver(
            IReadOnlyCollection<Type> registeredTypes)
        {
            new { registeredTypes }.AsArg().Must().NotBeNull().And().NotContainAnyNullElements();

            // this will cause dictionary keys to be lowercased if set to true (or you can set to true and set a dictionary key resolver.
            this.NamingStrategy.ProcessDictionaryKeys = false;

            this.RegisteredTypes = new HashSet<Type>(registeredTypes);
        }

        /// <summary>
        /// Gets the registered types.
        /// </summary>
        public HashSet<Type> RegisteredTypes { get; private set; }

        /// <inheritdoc />
        /// <remarks>
        /// We are overriding this method to support objects with multiple constructors.
        /// Out-of-the-box, Newtonsoft throws when deserializing an object with multiple constructors.
        /// The solution is to add the [JsonConstructor] attribute on the constructor to use, but that
        /// pollutes the model and requires a reference to Newtonsoft in the project.
        /// The RIGHT way to do this is to do what BSON does - find the constructor that matches
        /// the properties in the payload.  This would be very cumbersome (not possible?) in Newtonsoft
        /// so we are using a less optimal but totally sufficient heuristic: we choose the constructor
        /// with the maximum number of parameters.
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
            if (this.RegisteredTypes.Contains(objectType))
            {
                var createdType = Nullable.GetUnderlyingType(objectType) ?? objectType;

                var isInstantiable = (!createdType.IsInterface) && (!createdType.IsAbstract);

                // this is reverse engineered from DefaultContractResolver.CreateObjectContract
                // the object must be instantiable, and Newtonsoft did not find a constructor attributed with [JsonConstructor] (OverrideConstructor)
                // nor did it find a single constructor (ParametrizedConstructor)
                #pragma warning disable 618
                if (isInstantiable && (result.ParametrizedConstructor == null) && (result.OverrideConstructor == null))
                #pragma warning restore 618
                {
                    var constructors = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).ToList();

                    if (constructors.Any())
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

                        result.CreatorParameters.AddRange(this.CreateConstructorParameters(parameterizedConstructor, result.Properties));

                        // DefaultCreator is != null when an object has a default constructor
                        // so we need to clear it out here.
                        result.DefaultCreator = null;
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
                        // NO-OP - this allows for the constructor parameter to be a "params" array while still using a collection property as the source.
                    }
                    else if (memberType.IsAssignableTo(parameterInfo.ParameterType))
                    {
                        // NO-OP - vanilla assignable type to constructor check.
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
                    : constructor.DeclaringType.FullName;

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
                    if (matchingMemberProperty.PropertyType != null &&
                        matchingMemberProperty.PropertyType.IsValueType &&
                        Nullable.GetUnderlyingType(matchingMemberProperty.PropertyType) == null)
                    {
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
