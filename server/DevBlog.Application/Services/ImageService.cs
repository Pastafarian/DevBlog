using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace DevBlog.Application.Services
{
	public class ImageService : IImageService
	{
		private const int MaxImageSize = 1100;

		public Image<Rgba32> LoadImage(string base64String)
		{
			base64String = base64String.Substring(base64String.IndexOf(",", StringComparison.Ordinal) + 1);
			var imageBytes = Convert.FromBase64String(base64String);
			using var stream = new MemoryStream(imageBytes, 0, imageBytes.Length);
			return ConstrainSize(Image.Load<Rgba32>(stream), MaxImageSize, MaxImageSize);
		}

		public Image<Rgba32> LoadImage(Stream stream)
		{
			var image = Image.Load<Rgba32>(stream);
			return ConstrainSize(image, MaxImageSize, MaxImageSize);
		}

		public Image<Rgba32> LoadImage(byte[] content)
		{
			var image = Image.Load<Rgba32>(content);
			return ConstrainSize(image, MaxImageSize, MaxImageSize);
		}

		public Image<Rgba32> ConstrainSize(Image<Rgba32> image, int maxWidth, int maxHeight)
		{
			if (image.Height > maxHeight || image.Width > maxWidth)
				image.Mutate(x => x.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(maxWidth, maxHeight) }));

			return image;
		}
	}
}