// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObcSimplifyingSerializer.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Where possible, simplifies the serialization process by short-cutting certain trivial types
    /// to improve for time (using a more direct approach, e.g. int.Parse) and space (removing superfluous encoding);
    /// otherwise falls-back on a specified serializer.
    /// </summary>
    /// <remarks>
    /// This serializer shortcuts these types to/from string: string, Guid, sbyte, byte, short, ushort, int, uint, long, ulong.
    /// The serializer shortcuts byte[] to/from byte[].
    /// </remarks>
    public class ObcSimplifyingSerializer : ISerializer
    {
        /// <summary>
        /// The types that this serializer will shortcut.
        /// </summary>
        public static readonly IReadOnlyCollection<Type> StringSimplifiedTypes = new[]
        {
            typeof(string),
            typeof(Guid),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
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
        public object Deserialize(
            string serializedString,
            Type type)
        {
            if (type == typeof(string))
            {
                return serializedString;
            }
            else if (type == typeof(Guid))
            {
                return Guid.Parse(serializedString);
            }
            else if (type == typeof(sbyte))
            {
                return sbyte.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(byte))
            {
                return byte.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(short))
            {
                return short.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(ushort))
            {
                return ushort.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(int))
            {
                return int.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(uint))
            {
                return uint.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(long))
            {
                return long.Parse(serializedString, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(ulong))
            {
                return ulong.Parse(serializedString, CultureInfo.InvariantCulture);
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
            var result = type == typeof(byte[])
                ? serializedBytes
                : this.FallbackSerializer.Deserialize(serializedBytes, type);

            return result;
        }

        /// <inheritdoc />
        public byte[] SerializeToBytes(
            object objectToSerialize)
        {
            var result = objectToSerialize is byte[] objectToSerializeAsBytes
                ? objectToSerializeAsBytes
                : this.FallbackSerializer.SerializeToBytes(objectToSerialize);

            return result;
        }

        /// <inheritdoc />
        public string SerializeToString(
            object objectToSerialize)
        {
            if (objectToSerialize is string objectToSerializeString)
            {
                return objectToSerializeString;
            }
            else if (objectToSerialize is Guid objectToSerializeGuid)
            {
                return objectToSerializeGuid.ToString();
            }
            else if (objectToSerialize is sbyte objectToSerializeSignedByte)
            {
                return objectToSerializeSignedByte.ToString(CultureInfo.InvariantCulture);
            }
            else if (objectToSerialize is byte objectToSerializeByte)
            {
                return objectToSerializeByte.ToString(CultureInfo.InvariantCulture);
            }
            else if (objectToSerialize is short objectToSerializeShort)
            {
                return objectToSerializeShort.ToString(CultureInfo.InvariantCulture);
            }
            else if (objectToSerialize is ushort objectToSerializeUnsignedShort)
            {
                return objectToSerializeUnsignedShort.ToString(CultureInfo.InvariantCulture);
            }
            else if (objectToSerialize is int objectToSerializeInt)
            {
                return objectToSerializeInt.ToString(CultureInfo.InvariantCulture);
            }
            else if (objectToSerialize is uint objectToSerializeUnsignedInt)
            {
                return objectToSerializeUnsignedInt.ToString(CultureInfo.InvariantCulture);
            }
            else if (objectToSerialize is long objectToSerializeLong)
            {
                return objectToSerializeLong.ToString(CultureInfo.InvariantCulture);
            }
            else if (objectToSerialize is ulong objectToSerializeUnsignedLong)
            {
                return objectToSerializeUnsignedLong.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return this.FallbackSerializer.SerializeToString(objectToSerialize);
            }
        }
    }
}
