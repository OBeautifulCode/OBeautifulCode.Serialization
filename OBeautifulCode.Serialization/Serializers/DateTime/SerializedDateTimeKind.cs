// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializedDateTimeKind.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    /// <summary>
    /// Specifies the kind of serialized date time.
    /// </summary>
    internal enum SerializedDateTimeKind
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Universal kind, with seven decimal places after seconds.
        /// </summary>
        Utc,

        /// <summary>
        /// Utc with only six (not seven) decimal places after seconds.
        /// </summary>
        UtcSixFs,

        /// <summary>
        /// Utc with only five (not seven) decimal places after seconds.
        /// </summary>
        UtcFiveFs,

        /// <summary>
        /// Utc with only four (not seven) decimal places after seconds.
        /// </summary>
        UtcFourFs,

        /// <summary>
        /// Utc with only three (not seven) decimal places after seconds.
        /// </summary>
        UtcThreeFs,

        /// <summary>
        /// Utc with only two (not seven) decimal places after seconds.
        /// </summary>
        UtcTwoFs,

        /// <summary>
        /// Utc with only one (not seven) decimal places after seconds.
        /// </summary>
        UtcOneFs,

        /// <summary>
        /// Utc with zero (not seven) decimal places after seconds.
        /// </summary>
        UtcZeroFs,

        /// <summary>
        /// Unspecified kind.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Local kind.
        /// </summary>
        Local,
    }
}
