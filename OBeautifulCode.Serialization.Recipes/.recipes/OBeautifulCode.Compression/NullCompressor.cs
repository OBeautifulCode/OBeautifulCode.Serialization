﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullCompressor.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Compression.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Compression.Recipes
{
    /// <summary>
    /// Null implementation of <see cref="ICompressAndDecompress"/>.
    /// </summary>
#if !OBeautifulCodeCompressionRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Compression.Recipes", "See package version number")]
    internal
#else
    public
#endif
    class NullCompressor : ICompressAndDecompress
    {
        /// <inheritdoc cref="ICompressAndDecompress"/>
        public CompressionKind CompressionKind => CompressionKind.None;

        /// <inheritdoc cref="ICompressAndDecompress"/>
        public byte[] CompressBytes(
            byte[] uncompressedBytes)
        {
            var result = uncompressedBytes;

            return result;
        }

        /// <inheritdoc cref="ICompressAndDecompress"/>
        public byte[] DecompressBytes(
            byte[] compressedBytes)
        {
            var result = compressedBytes;

            return result;
        }
    }
}
