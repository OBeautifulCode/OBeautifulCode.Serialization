// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSerializerFactory.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using FakeItEasy;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Compression;
    using OBeautifulCode.Type;

    public class TestSerializerFactory : ISerializerFactory
    {
        // non-compressing
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Mutable type is acceptable in test project.")]
        public static readonly BuildSerializerParametersAndResult BuildSerializerTestParametersAndResult1 = new BuildSerializerParametersAndResult
        {
            SerializerId = "serializer-1",
            SerializerRepresentation = A.Dummy<SerializerRepresentation>().Whose(_ => _.CompressionKind == CompressionKind.None),
            AssemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion,
            ResultFunc = () => new ObcLambdaBackedSerializer(o => "serializer-1", (s, t) => A.Dummy<string>(), o => A.Dummy<byte[]>(), (b, t) => A.Dummy<byte[]>()),
        };

        // non-compressing, same serializer representation as above but different assembly version match strategy
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Mutable type is acceptable in test project.")]
        public static readonly BuildSerializerParametersAndResult BuildSerializerTestParametersAndResult2 = new BuildSerializerParametersAndResult
        {
            SerializerId = "serializer-2",
            SerializerRepresentation = BuildSerializerTestParametersAndResult1.SerializerRepresentation,
            AssemblyVersionMatchStrategy = VersionMatchStrategy.MaxVersion,
            ResultFunc = () => new ObcLambdaBackedSerializer(o => "serializer-2", (s, t) => A.Dummy<string>(), o => A.Dummy<byte[]>(), (b, t) => A.Dummy<byte[]>()),
        };

        // compressing
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Mutable type is acceptable in test project.")]
        public static readonly BuildSerializerParametersAndResult BuildSerializerTestParametersAndResult3 = new BuildSerializerParametersAndResult
        {
            SerializerId = "serializer-3",
            SerializerRepresentation = A.Dummy<SerializerRepresentation>().Whose(_ => _.CompressionKind == CompressionKind.DotNetZip),
            AssemblyVersionMatchStrategy = VersionMatchStrategy.MinVersion,
            ResultFunc = () => new ObcLambdaBackedSerializer(o => "serializer-3", (s, t) => A.Dummy<string>(), o => A.Dummy<byte[]>(), (b, t) => A.Dummy<byte[]>()),
        };

        // compressing, same version match strategy as above but different serializer representation
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Mutable type is acceptable in test project.")]
        public static readonly BuildSerializerParametersAndResult BuildSerializerTestParametersAndResult4 = new BuildSerializerParametersAndResult
        {
            SerializerId = "serializer-4",
            SerializerRepresentation = A.Dummy<SerializerRepresentation>().Whose(_ => _.CompressionKind == CompressionKind.DotNetZip),
            AssemblyVersionMatchStrategy = VersionMatchStrategy.MinVersion,
            ResultFunc = () => new ObcLambdaBackedSerializer(o => "serializer-4", (s, t) => A.Dummy<string>(), o => A.Dummy<byte[]>(), (b, t) => A.Dummy<byte[]>()),
        };

        // same as BuildSerializerTestParametersAndResult3 but non-compression
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Mutable type is acceptable in test project.")]
        public static readonly BuildSerializerParametersAndResult BuildSerializerTestParametersAndResult5 = new BuildSerializerParametersAndResult
        {
            SerializerId = "serializer-5",
            SerializerRepresentation = BuildSerializerTestParametersAndResult3.SerializerRepresentation.DeepCloneWithCompressionKind(CompressionKind.None),
            AssemblyVersionMatchStrategy = VersionMatchStrategy.MinVersion,
            ResultFunc = () => new ObcLambdaBackedSerializer(o => "serializer-5", (s, t) => A.Dummy<string>(), o => A.Dummy<byte[]>(), (b, t) => A.Dummy<byte[]>()),
        };

        // same as BuildSerializerTestParametersAndResult4 but non-compression
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Mutable type is acceptable in test project.")]
        public static readonly BuildSerializerParametersAndResult BuildSerializerTestParametersAndResult6 = new BuildSerializerParametersAndResult
        {
            SerializerId = "serializer-6",
            SerializerRepresentation = BuildSerializerTestParametersAndResult4.SerializerRepresentation.DeepCloneWithCompressionKind(CompressionKind.None),
            AssemblyVersionMatchStrategy = VersionMatchStrategy.MinVersion,
            ResultFunc = () => new ObcLambdaBackedSerializer(o => "serializer-6", (s, t) => A.Dummy<string>(), o => A.Dummy<byte[]>(), (b, t) => A.Dummy<byte[]>()),
        };

        // different serializer representation, different assembly version match strategy as any of the above.
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Mutable type is acceptable in test project.")]
        public static readonly BuildSerializerParametersAndResult BuildSerializerTestParametersAndResult7 = new BuildSerializerParametersAndResult
        {
            SerializerId = "serializer-7",
            SerializerRepresentation = A.Dummy<SerializerRepresentation>(),
            AssemblyVersionMatchStrategy = VersionMatchStrategy.SpecifiedVersion,
            ResultFunc = () => new ObcLambdaBackedSerializer(o => "serializer-7", (s, t) => A.Dummy<string>(), o => A.Dummy<byte[]>(), (b, t) => A.Dummy<byte[]>()),
        };

        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = ObcSuppressBecause.CA2104_DoNotDeclareReadOnlyMutableReferenceTypes_TypeIsImmutable)]
        public static readonly IReadOnlyList<BuildSerializerParametersAndResult> BuildSerializerTestParametersAndResults = new List<BuildSerializerParametersAndResult>
        {
            BuildSerializerTestParametersAndResult1,
            BuildSerializerTestParametersAndResult2,
            BuildSerializerTestParametersAndResult3,
            BuildSerializerTestParametersAndResult4,
            BuildSerializerTestParametersAndResult5,
            BuildSerializerTestParametersAndResult6,
            BuildSerializerTestParametersAndResult7,
        };

        public ISerializer BuildSerializer(
            SerializerRepresentation serializerRepresentation,
            VersionMatchStrategy assemblyVersionMatchStrategy = VersionMatchStrategy.AnySingleVersion)
        {
            var result = BuildSerializerTestParametersAndResults
                .Single(_ =>
                    (_.SerializerRepresentation == serializerRepresentation) &&
                    (_.AssemblyVersionMatchStrategy == assemblyVersionMatchStrategy))
                .ResultFunc();

            return result;
        }
    }

    public class BuildSerializerParametersAndResult
    {
        public string SerializerId { get; set; }

        public SerializerRepresentation SerializerRepresentation { get; set; }

        public VersionMatchStrategy AssemblyVersionMatchStrategy { get; set; }

        public Func<ISerializer> ResultFunc { get; set; }
    }
}