// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredContractResolver.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    using Newtonsoft.Json.Serialization;

    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Json converter to use.
    /// </summary>
    public class RegisteredContractResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredContractResolver"/> class.
        /// </summary>
        /// <param name="contractResolverBuilderFunction">Builder function.</param>
        public RegisteredContractResolver(Func<IContractResolver> contractResolverBuilderFunction)
        {
            new { contractResolverBuilderFunction }.AsArg().Must().NotBeNull();

            this.ContractResolverBuilderFunction = contractResolverBuilderFunction;
        }

        /// <summary>
        /// Gets the builder function.
        /// </summary>
        public Func<IContractResolver> ContractResolverBuilderFunction { get; private set; }
    }
}