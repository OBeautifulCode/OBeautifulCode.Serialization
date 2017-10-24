// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewtonsoftJsonSerializerSettingsFactory.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Serialization.Json
{
    using System;

    using Naos.Serialization.Domain;

    using Newtonsoft.Json;

    using Spritely.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Factory to build <see cref="JsonSerializerSettings" />.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Newtonsoft", Justification = "Spelling/name is correct.")]
    public static class NewtonsoftJsonSerializerSettingsFactory
    {
        /// <summary>
        ///  Gets the settings to use from the <see cref="SerializationKind" /> and optional configuration <see cref="Type" /> provided.
        /// </summary>
        /// <param name="serializationKind">Kind to determine the settings.</param>
        /// <param name="configurationType">Optional configuration Type.</param>
        /// <returns><see cref="JsonSerializerSettings" /> to use with <see cref="Newtonsoft" /> when serializing.</returns>
        public static JsonSerializerSettings BuildSettings(SerializationKind serializationKind, Type configurationType = null)
        {
            new { serializationKind }.Must().NotBeEqualTo(SerializationKind.Invalid).OrThrowFirstFailure();

            if (serializationKind == SerializationKind.Custom)
            {
                if (configurationType == null)
                {
                    throw new ArgumentException(Invariant($"Must specify {nameof(configurationType)} if using {nameof(serializationKind)} of {nameof(SerializationKind)}.{SerializationKind.Custom}"));
                }

                throw new NotImplementedException("Still need to implement custom Type creation of settings.");
            }
            else
            {
                return GetSettingsBySerializationKind(serializationKind);
            }
        }

        /// <summary>
        /// Gets the settings to use from the <see cref="SerializationKind" /> provided.
        /// </summary>
        /// <param name="serializationKind">Kind to determine the settings.</param>
        /// <returns><see cref="JsonSerializerSettings" /> to use with <see cref="Newtonsoft" /> when serializing.</returns>
        public static JsonSerializerSettings GetSettingsBySerializationKind(SerializationKind serializationKind)
        {
            switch (serializationKind)
            {
                case SerializationKind.Default: return JsonConfiguration.DefaultSerializerSettings;
                case SerializationKind.Compact: return JsonConfiguration.CompactSerializerSettings;
                case SerializationKind.Minimal: return JsonConfiguration.MinimalSerializerSettings;
                default: throw new NotSupportedException(Invariant($"Value of {nameof(SerializationKind)} - {serializationKind} is not currently supported."));
            }
        }
    }
}