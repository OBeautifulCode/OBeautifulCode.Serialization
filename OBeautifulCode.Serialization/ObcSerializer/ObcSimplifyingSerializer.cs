// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSimplifyingSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.String.Recipes;

    /// <summary>
    /// Where possible, simplifies the serialization process by short-cutting certain trivial types
    /// to improve for time (using a more direct approach, e.g. int.Parse) and space (removing superfluous encoding);
    /// otherwise falls-back on a specified serializer.
    /// </summary>
    /// <remarks>
    /// The serializer shortcuts null to/from string and byte.
    /// This serializer shortcuts these types (including Nullable where applicable) to/from string: string, Guid, bool, DateTime, sbyte, byte, short, ushort, int, uint, long, ulong, float, double, decimal.
    /// The serializer shortcuts byte[] to/from byte[].
    /// </remarks>
    public class ObcSimplifyingSerializer : ISerializer
    {
        /// <summary>
        /// The types that this serializer will shortcut.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = ObcSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly IReadOnlyCollection<Type> StringSimplifiedTypes = new[]
        {
            typeof(string),
            typeof(Guid),
            typeof(Guid?),
            typeof(bool),
            typeof(bool?),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(sbyte),
            typeof(sbyte?),
            typeof(byte),
            typeof(byte?),
            typeof(short),
            typeof(short?),
            typeof(ushort),
            typeof(ushort?),
            typeof(int),
            typeof(int?),
            typeof(uint),
            typeof(uint?),
            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),
            typeof(float),
            typeof(float?),
            typeof(double),
            typeof(double?),
            typeof(decimal),
            typeof(decimal?),
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ObcSimplifyingSerializer"/> class.
        /// </summary>
        /// <param name="fallbackSerializer">The serializer to use when the serializer cannot shortcut a type.</param>
        public ObcSimplifyingSerializer(
            ISerializer fallbackSerializer)
        {
            if (fallbackSerializer == null)
            {
                throw new ArgumentNullException(nameof(fallbackSerializer));
            }

            this.FallbackSerializer = fallbackSerializer;
        }

        /// <summary>
        /// Gets the serializer to use when the serializer cannot shortcut a type.
        /// </summary>
        public ISerializer FallbackSerializer { get; }

        /// <inheritdoc />
        public SerializationConfigurationType SerializationConfigurationType => this.FallbackSerializer.SerializationConfigurationType;

        /// <inheritdoc />
        public SerializationKind SerializationKind => this.FallbackSerializer.SerializationKind;

        /// <inheritdoc />
        public SerializerRepresentation SerializerRepresentation => this.FallbackSerializer.SerializerRepresentation;

        /// <inheritdoc />
        public T Deserialize<T>(
            string serializedString)
        {
            var result = (T)this.Deserialize(serializedString, typeof(T));

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public object Deserialize(
            string serializedString,
            Type type)
        {
            if (serializedString == null)
            {
                return null;
            }
            else if (type == typeof(string))
            {
                return serializedString;
            }
            else if ((type == typeof(Guid)) || (type == typeof(Guid?)))
            {
                return Guid.Parse(serializedString);
            }
            else if ((type == typeof(bool)) || (type == typeof(bool?)))
            {
                return bool.Parse(serializedString);
            }
            else if ((type == typeof(DateTime)) || (type == typeof(DateTime?)))
            {
                return ObcDateTimeStringSerializer.DeserializeToDateTime(serializedString);
            }
            else if ((type == typeof(sbyte)) || (type == typeof(sbyte?)))
            {
                return sbyte.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(byte)) || (type == typeof(byte?)))
            {
                return byte.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(short)) || (type == typeof(short?)))
            {
                return short.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(ushort)) || (type == typeof(ushort?)))
            {
                return ushort.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(int)) || (type == typeof(int?)))
            {
                return int.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(uint)) || (type == typeof(uint?)))
            {
                return uint.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(long)) || (type == typeof(long?)))
            {
                return long.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(ulong)) || (type == typeof(ulong?)))
            {
                return ulong.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(float)) || (type == typeof(float?)))
            {
                return float.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(double)) || (type == typeof(double?)))
            {
                return double.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if ((type == typeof(decimal)) || (type == typeof(decimal?)))
            {
                return decimal.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else
            {
                return this.FallbackSerializer.Deserialize(serializedString, type);
            }
        }

        /// <inheritdoc />
        public T Deserialize<T>(
            byte[] serializedBytes)
        {
            var result = (T)this.Deserialize(serializedBytes, typeof(T));

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(
            byte[] serializedBytes,
            Type type)
        {
            if (serializedBytes == null)
            {
                return null;
            }

            var result = type == typeof(byte[])
                ? serializedBytes
                : this.FallbackSerializer.Deserialize(serializedBytes, type);

            return result;
        }

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }

            var result = objectToSerialize is byte[] objectToSerializeAsBytes
                ? objectToSerializeAsBytes
                : this.FallbackSerializer.SerializeToBytes(objectToSerialize);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public string SerializeToString(
            object objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                return null;
            }
            else if (objectToSerialize is string objectToSerializeString)
            {
                return objectToSerializeString;
            }
            else if (objectToSerialize is Guid objectToSerializeGuid)
            {
                return objectToSerializeGuid.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is bool objectToSerializeBool)
            {
                return objectToSerializeBool.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is DateTime objectToSerializeDateTime)
            {
                return ObcDateTimeStringSerializer.SerializeToString(objectToSerializeDateTime);
            }
            else if (objectToSerialize is sbyte objectToSerializeSignedByte)
            {
                return objectToSerializeSignedByte.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is byte objectToSerializeByte)
            {
                return objectToSerializeByte.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is short objectToSerializeShort)
            {
                return objectToSerializeShort.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is ushort objectToSerializeUnsignedShort)
            {
                return objectToSerializeUnsignedShort.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is int objectToSerializeInt)
            {
                return objectToSerializeInt.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is uint objectToSerializeUnsignedInt)
            {
                return objectToSerializeUnsignedInt.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is long objectToSerializeLong)
            {
                return objectToSerializeLong.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is ulong objectToSerializeUnsignedLong)
            {
                return objectToSerializeUnsignedLong.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is float objectToSerializeFloat)
            {
                return objectToSerializeFloat.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is double objectToSerializeDouble)
            {
                return objectToSerializeDouble.ToStringInvariantPreferred();
            }
            else if (objectToSerialize is decimal objectToSerializeDecimal)
            {
                return objectToSerializeDecimal.ToStringInvariantPreferred();
            }
            else
            {
                return this.FallbackSerializer.SerializeToString(objectToSerialize);
            }
        }
    }
}
