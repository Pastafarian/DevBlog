using Microsoft.AspNetCore.Http;
using System.IO;

namespace DevBlog.Api.Extensions
{
	public static class HttpExtensions
	{
		public static byte[] ReadBytes(this IFormFile formFile)
		{
			using var reader = new BinaryReader(formFile.OpenReadStream());
			return reader.ReadBytes((int)formFile.Length);
		}
	}
}