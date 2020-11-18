// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

namespace SixLabors.ImageSharp.Formats.Experimental.WebP
{
    /// <summary>
    /// Info about the webp format used.
    /// </summary>
    public enum WebPFormatType
    {
        /// <summary>
        /// Unknown webp format.
        /// </summary>
        Unknown,

        /// <summary>
        /// The lossless webp format.
        /// </summary>
        Lossless,

        /// <summary>
        /// The lossy webp format.
        /// </summary>
        Lossy,
    }
}