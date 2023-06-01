using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using DevBlog.Application.Enums;
using System;
using System.IO;

namespace DevBlog.Application.Extensions
{
	public static class ImageExtensions
	{
		public static byte[] ToByteArray(this Image<Rgba32> image, ImageFormat format)
		{
			using var memoryStream = new MemoryStream();

			switch (format)
			{

				case ImageFormat.Jpg:
					image.SaveAsJpeg(memoryStream, new JpegEncoder { Quality = 90 });
					break;
				case ImageFormat.Png:
					image.SaveAsPng(memoryStream, new PngEncoder { CompressionLevel = PngCompressionLevel.Level6 });
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(format), format, null);
			}

			memoryStream.Position = 0;
			return memoryStream.ToArray();
		}
	}
}