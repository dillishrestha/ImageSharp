// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.IO;

using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Tests.TestUtilities.ImageComparison;
using SixLabors.ImageSharp.Tests.TestUtilities.ReferenceCodecs;

using Xunit;

namespace SixLabors.ImageSharp.Tests.Formats.Tiff
{
    [Trait("Category", "Tiff")]
    public class TiffEncoderTests
    {
        private static readonly IImageDecoder ReferenceDecoder = new MagickReferenceDecoder();

        public static readonly TheoryData<string, TiffBitsPerPixel> TiffBitsPerPixelFiles =
            new TheoryData<string, TiffBitsPerPixel>
            {
                { TestImages.Tiff.Calliphora_BiColor, TiffBitsPerPixel.Pixel1 },
                { TestImages.Tiff.GrayscaleUncompressed, TiffBitsPerPixel.Pixel8 },
                { TestImages.Tiff.RgbUncompressed, TiffBitsPerPixel.Pixel24 },
            };

        [Theory]
        [MemberData(nameof(TiffBitsPerPixelFiles))]
        public void TiffEncoder_PreserveBitsPerPixel(string imagePath, TiffBitsPerPixel expectedBitsPerPixel)
        {
            // arrange
            var tiffEncoder = new TiffEncoder();
            var testFile = TestFile.Create(imagePath);
            using Image<Rgba32> input = testFile.CreateRgba32Image();
            using var memStream = new MemoryStream();

            // act
            input.Save(memStream, tiffEncoder);

            // assert
            memStream.Position = 0;
            using var output = Image.Load<Rgba32>(memStream);
            TiffMetadata meta = output.Metadata.GetTiffMetadata();
            Assert.Equal(expectedBitsPerPixel, meta.BitsPerPixel);
        }

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb, TiffEncoderCompression.Deflate);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_RgbUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeRgb_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.Rgb, TiffEncoderCompression.PackBits);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray, TiffEncoderCompression.Deflate);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_GrayscaleUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeGray_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel8, TiffEncodingMode.Gray, TiffEncoderCompression.PackBits);

        // TODO: this test fails, but the output looks correct. I thinks its due to the fact that a quantizer is used to create the palette.
        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.ColorPalette);

        // TODO: this test fails, but the output looks correct. I thinks its due to the fact that a quantizer is used to create the palette.
        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.ColorPalette, TiffEncoderCompression.Deflate);

        // TODO: this test fails, but the output looks correct. I thinks its due to the fact that a quantizer is used to create the palette.
        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_PaletteUncompressed, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeColorPalette_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.ColorPalette, TiffEncoderCompression.PackBits);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_BiColor, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel24, TiffEncodingMode.BiColor);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_BiColor, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithDeflateCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.Deflate);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_BiColor, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithPackBitsCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.PackBits);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_BiColor, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithCcittGroup3FaxCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.CcittGroup3Fax);

        [Theory]
        [WithFile(TestImages.Tiff.Calliphora_BiColor, PixelTypes.Rgba32)]
        public void TiffEncoder_EncodeBiColor_WithModifiedHuffmanCompression_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel> => TestTiffEncoderCore(provider, TiffBitsPerPixel.Pixel1, TiffEncodingMode.BiColor, TiffEncoderCompression.ModifiedHuffman);

        private static void TestTiffEncoderCore<TPixel>(
            TestImageProvider<TPixel> provider,
            TiffBitsPerPixel bitsPerPixel,
            TiffEncodingMode mode,
            TiffEncoderCompression compression = TiffEncoderCompression.None,
            bool useExactComparer = true,
            float compareTolerance = 0.01f)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using Image<TPixel> image = provider.GetImage();
            var encoder = new TiffEncoder { Mode = mode, Compression = compression };

            // Does DebugSave & load reference CompareToReferenceInput():
            image.VerifyEncoder(provider, "tiff", bitsPerPixel, encoder, useExactComparer ? ImageComparer.Exact : ImageComparer.Tolerant(compareTolerance), referenceDecoder: ReferenceDecoder);
        }
    }
}