using DevBlog.Application.Enums;
using DevBlog.Application.Extensions;
using DevBlog.Application.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevBlog.Application.Services
{
	public class ImageStorageService : IImageStorageService
	{
		private readonly IImageService imageService;
		private readonly IFileNamingService fileNamingService;
		private readonly IFileService fileService;
		private readonly AppSettings appSettings;

		public ImageStorageService(IImageService imageService, IFileNamingService fileNamingService, IFileService fileService, AppSettings appSettings)
		{
			this.imageService = imageService;
			this.fileNamingService = fileNamingService;
			this.fileService = fileService;
			this.appSettings = appSettings;
		}

		public async Task<string> StoreImage(string base64String, string title, CancellationToken cancellationToken)
		{
			var bytes = imageService.LoadImage(base64String).ToByteArray(ImageFormat.Png);
			return await SaveImageFile(bytes, title, ImageFormatInformation[ImageFormat.Png], cancellationToken);
		}

		public Task<string> StoreImage(byte[] content, string fileName, CancellationToken cancellationToken)
		{
			return SaveImageFile(content, fileName, GetImageFormatInfoFromFileName(fileName), cancellationToken);
		}

		private async Task<string> SaveImageFile(byte[] content, string title, ImageFormatInfo imageFormat, CancellationToken cancellationToken)
		{
			var fileName = fileNamingService.GenerateFileName(title, imageFormat.Extension);
			var response = await fileService.UploadFromByteArray(content, fileName, imageFormat.MimeType, cancellationToken);
			return appSettings.StorageAccountUri + response.Bucket + "/" + response.Name;
		}

		private static ImageFormatInfo GetImageFormatInfoFromFileName(string fileName)
		{
			var extension = Path.GetExtension(fileName).TrimStart('.').ToLower();
			return ImageFormatInformation.Single(x => x.Value.Extension == extension).Value;
		}


		private static readonly Dictionary<ImageFormat, ImageFormatInfo> ImageFormatInformation =
			new Dictionary<ImageFormat, ImageFormatInfo>
			{
				{ ImageFormat.Jpg, new ImageFormatInfo("jpg", "image/jpeg") },
				{ ImageFormat.Png, new ImageFormatInfo("png", "image/png")}
			};

		private class ImageFormatInfo
		{
			public string MimeType { get; }
			public string Extension { get; }

			public ImageFormatInfo(string extension, string mimeType)
			{
				Extension = extension;
				MimeType = mimeType;
			}
		}
	}
}