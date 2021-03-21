using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace DevBlog.Application.Services
{
	public interface IImageService
	{
		Image<Rgba32> LoadImage(string base64String);
		Image<Rgba32> LoadImage(Stream stream);
		Image<Rgba32> LoadImage(byte[] content);
		Image<Rgba32> ConstrainSize(Image<Rgba32> image, int maxWidth, int maxHeight);
	}
}