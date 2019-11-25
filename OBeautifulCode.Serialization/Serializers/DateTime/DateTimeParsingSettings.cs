// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeParsingSettings.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Settings for parsing a <see cref="DateTime"/>.
    /// </summary>
    internal class DateTimeParsingSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="DateTimeKind" /> to parse into.
        /// </summary>
        public DateTimeKind DateTimeKind { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTimeStyles"/> to use when parsing the string.
        /// </summary>
        public DateTimeStyles DateTimeStyles { get; set; }

        /// <summary>
        /// Gets or sets a format specifier that defines the required format for the string being parsed.
        /// </summary>
        public string FormatString { get; set; }
    }
}
