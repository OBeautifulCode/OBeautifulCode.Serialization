// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcDateTimeStringSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using OBeautifulCode.Serialization.Internal;
    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// String serializer for <see cref="DateTime" />.
    /// </summary>
    public class ObcDateTimeStringSerializer : IStringSerializeAndDeserialize
    {
        private const string LocalDateTimeKindRegexPattern = @"[-+]\d\d:\d\d$";

        private static readonly Regex MatchLocalRegex = new Regex(LocalDateTimeKindRegexPattern, RegexOptions.Compiled);

        private static readonly IReadOnlyDictionary<DateTimeKind, string> DateTimeKindToFormatStringMap =
            new Dictionary<DateTimeKind, string>
            {
                // ReSharper disable once StringLiteralTypo
                { DateTimeKind.Utc, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'" },

                // ReSharper disable once StringLiteralTypo
                { DateTimeKind.Unspecified, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff''" },

                // ReSharper disable once StringLiteralTypo
                // note that the K here expands to the offset (e.g. "-05:00")
                { DateTimeKind.Local, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK" },
            };

        private static readonly IReadOnlyDictionary<DateTimeKind, DateTimeStyles> DateTimeKindToStylesMap =
            new Dictionary<DateTimeKind, DateTimeStyles>
            {
                { DateTimeKind.Utc, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal },
                { DateTimeKind.Unspecified, DateTimeStyles.None },
                { DateTimeKind.Local, DateTimeStyles.AssumeLocal },
            };

        private static readonly IReadOnlyDictionary<SerializedDateTimeKind, DateTimeParsingSettings> SerializedDateTimeKindToParsingSettingsMap =
            new Dictionary<SerializedDateTimeKind, DateTimeParsingSettings>
            {
                {
                    SerializedDateTimeKind.Unspecified, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Unspecified,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Unspecified],
                        FormatString = DateTimeKindToFormatStringMap[DateTimeKind.Unspecified],
                    }
                },
                {
                    SerializedDateTimeKind.Local, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Local,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Local],
                        FormatString = DateTimeKindToFormatStringMap[DateTimeKind.Local],
                    }
                },
                {
                    SerializedDateTimeKind.Utc, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],
                        FormatString = DateTimeKindToFormatStringMap[DateTimeKind.Utc],
                    }
                },
                {
                    SerializedDateTimeKind.UtcSixFs, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],

                        // ReSharper disable once StringLiteralTypo
                        FormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff'Z'",
                    }
                },
                {
                    SerializedDateTimeKind.UtcFiveFs, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],

                        // ReSharper disable once StringLiteralTypo
                        FormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffff'Z'",
                    }
                },
                {
                    SerializedDateTimeKind.UtcFourFs, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],

                        // ReSharper disable once StringLiteralTypo
                        FormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffff'Z'",
                    }
                },
                {
                    SerializedDateTimeKind.UtcThreeFs, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],

                        // ReSharper disable once StringLiteralTypo
                        FormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    }
                },
                {
                    SerializedDateTimeKind.UtcTwoFs, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],

                        // ReSharper disable once StringLiteralTypo
                        FormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'",
                    }
                },
                {
                    SerializedDateTimeKind.UtcOneFs, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],
                        FormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'f'Z'",
                    }
                },
                {
                    SerializedDateTimeKind.UtcZeroFs, new DateTimeParsingSettings
                    {
                        DateTimeKind = DateTimeKind.Utc,
                        DateTimeStyles = DateTimeKindToStylesMap[DateTimeKind.Utc],

                        // ReSharper disable once StringLiteralTypo
                        FormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                    }
                },
            };

        private static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;

        /// <summary>
        /// Serializes a <see cref="DateTime"/> to a string.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <returns>
        /// The serialized string.
        /// </returns>
        public static string SerializeToString(
            DateTime value)
        {
            var formatString = DateTimeKindToFormatStringMap[value.Kind];

            var result = value.ToString(formatString, FormatProvider);

            return result;
        }

        /// <summary>
        /// Deserializes a string into a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="serializedString">Serialized string.</param>
        /// <returns>
        /// The deserialized <see cref="DateTime"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static DateTime DeserializeToDateTime(
            string serializedString)
        {
            if (serializedString == null)
            {
                throw new ArgumentNullException(nameof(serializedString));
            }

            if (string.IsNullOrWhiteSpace(serializedString))
            {
                throw new ArgumentException(Invariant($"'{nameof(serializedString)}' is white space"));
            }

            var exceptionMessage = Invariant($"Provided {nameof(serializedString)}: {serializedString} is malformed; it's not in a supported format and cannot be deserialized.");

            var serializedDateTimeKind = DetermineSerializedDateTimeKind(serializedString);

            if (serializedDateTimeKind == SerializedDateTimeKind.Unknown)
            {
                throw new InvalidOperationException(exceptionMessage);
            }

            var parsingSettings = SerializedDateTimeKindToParsingSettingsMap[serializedDateTimeKind];

            DateTime result;

            try
            {
                result = DateTime.ParseExact(serializedString, parsingSettings.FormatString, FormatProvider, parsingSettings.DateTimeStyles);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(exceptionMessage, ex);
            }

            if (result.Kind != parsingSettings.DateTimeKind)
            {
                throw new InvalidOperationException(Invariant($"Provided {nameof(serializedString)}: {serializedString} deserialized into a {nameof(DateTime)} who's {nameof(DateTimeKind)} is {nameof(DateTimeKind)}.{result.Kind}, however {nameof(DateTimeKind)}.{parsingSettings.DateTimeKind} was expected based on the format of the string."));
            }

            return result;
        }

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                throw new ArgumentNullException(nameof(objectToSerialize));
            }

            if (objectToSerialize.GetType() != typeof(DateTime))
            {
                throw new ArgumentException(Invariant($"{nameof(objectToSerialize)}.GetType() != typeof({nameof(DateTime)}); '{nameof(objectToSerialize)}' is of type '{objectToSerialize.GetType().ToStringReadable()}'"));
            }

            var result = SerializeToString((DateTime)objectToSerialize);

            return result;
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            var result = (T)this.Deserialize(serializedString, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            string serializedString,
            Type type)
        {
            if (serializedString == null)
            {
                throw new ArgumentNullException(nameof(serializedString));
            }

            if (string.IsNullOrWhiteSpace(serializedString))
            {
                throw new ArgumentException(Invariant($"'{nameof(serializedString)}' is white space"));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type != typeof(DateTime))
            {
                throw new ArgumentException(Invariant($"{nameof(type)} != typeof({nameof(DateTime)}); '{nameof(type)}' is of type '{type.ToStringReadable()}'"));
            }

            var result = DeserializeToDateTime(serializedString);

            return result;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        private static SerializedDateTimeKind DetermineSerializedDateTimeKind(
            string serializedString)
        {
            SerializedDateTimeKind result;

            if (serializedString.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
            {
                if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.Utc].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.Utc;
                }
                else if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.UtcSixFs].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.UtcSixFs;
                }
                else if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.UtcFiveFs].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.UtcFiveFs;
                }
                else if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.UtcFourFs].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.UtcFourFs;
                }
                else if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.UtcThreeFs].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.UtcThreeFs;
                }
                else if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.UtcTwoFs].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.UtcTwoFs;
                }
                else if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.UtcOneFs].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.UtcOneFs;
                }
                else if (serializedString.Length == SerializedDateTimeKindToParsingSettingsMap[SerializedDateTimeKind.UtcZeroFs].FormatString.Count(_ => _ != '\''))
                {
                    result = SerializedDateTimeKind.UtcZeroFs;
                }
                else
                {
                    result = SerializedDateTimeKind.Unknown;
                }
            }
            else if (MatchLocalRegex.Match(serializedString).Success)
            {
                result = SerializedDateTimeKind.Local;
            }
            else
            {
                result = SerializedDateTimeKind.Unspecified;
            }

            return result;
        }
    }
}