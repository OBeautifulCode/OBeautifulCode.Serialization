// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredContractResolver.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Json
{
    using System;

    /// <summary>
    /// Json converter to use.
    /// </summary>
    public class RegisteredContractResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredContractResolver"/> class.
        /// </summary>
        /// <param name="contractResolverBuilder">A contract resolver builder.</param>
        public RegisteredContractResolver(
            ContractResolverBuilder contractResolverBuilder)
        {
            if (contractResolverBuilder == null)
            {
                throw new ArgumentNullException(nameof(contractResolverBuilder));
            }

            this.ContractResolverBuilder = contractResolverBuilder;
        }

        /// <summary>
        /// Gets the builder function.
        /// </summary>
        public ContractResolverBuilder ContractResolverBuilder { get; }
    }
}