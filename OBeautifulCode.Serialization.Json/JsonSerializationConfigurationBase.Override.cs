// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializationConfigurationBase.Override.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Base class to use for creating <see cref="ObcJsonSerializer" /> configuration.
    /// </summary>
    public abstract partial class JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<TypeToRegister> TypesToRegister => this.TypesToRegisterForJson;

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DefaultDependentSerializationConfigurationTypes => new[]
        {
            typeof(InternallyRequiredTypesWithDiscoveryJsonSerializationConfiguration).ToJsonSerializationConfigurationType(),
        };

        /// <inheritdoc />
        protected sealed override IReadOnlyCollection<SerializationConfigurationType> DependentSerializationConfigurationTypes => this.DependentJsonSerializationConfigurationTypes;

        /// <inheritdoc />
        protected sealed override void ProcessRegistrationDetailsPriorToRegistration(
            RegistrationDetails registrationDetails)
        {
            if (registrationDetails.TypeToRegister is TypeToRegisterForJson typeToRegisterForJson)
            {
                var type = typeToRegisterForJson.Type;

                var jsonConverterBuilder = typeToRegisterForJson.JsonConverterBuilder;

                if (jsonConverterBuilder != null)
                {
                    this.TypesWithConverters.Add(type);

                    if (jsonConverterBuilder.OutputKind == JsonConverterOutputKind.String)
                    {
                        this.TypesWithStringConverters.Add(type);
                    }

                    if (this.JsonConverterBuilders.All(_ => _.Id != jsonConverterBuilder.Id))
                    {
                        this.JsonConverterBuilders.Add(jsonConverterBuilder);
                    }
                }
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(registrationDetails)}.{nameof(RegistrationDetails.TypeToRegister)} is expected to be of type {nameof(TypeToRegisterForJson)}, but found this type: {registrationDetails.TypeToRegister.GetType().ToStringReadable()}."));
            }
        }

        /// <inheritdoc />
        protected sealed override void FinalizeInitialization()
        {
            this.AddHierarchyParticipatingTypes(this.RegisteredTypeToRegistrationDetailsMap.Keys.ToList());
        }

        private void AddHierarchyParticipatingTypes(
            IReadOnlyCollection<Type> types)
        {
            var inheritedTypeConverterTypes = types.Where(t =>
                (!InheritedTypeConverterBlackList.Contains(t)) &&
                (t.IsAbstract || t.IsInterface || types.Any(a => (a != t) && (t.IsAssignableTo(a) || a.IsAssignableTo(t))))).Distinct().ToList();

            // TODO: what info do we want to capture here? should we give registration details?
            this.HierarchyParticipatingTypes.AddRange(inheritedTypeConverterTypes.Except(this.TypesWithConverters));
        }
    }
}