using System;
using DevBlog.Application.Enums;
using DevBlog.Application.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevBlog.Application.Services
{
	public class ImageStorageService : IImageStorageService
	{
		private readonly IImageService _imageService;
		private readonly IFileNamingService _fileNamingService;
		private readonly IFileSystemService _fileService;
        
		public ImageStorageService(IImageService imageService, IFileNamingService fileNamingService, IFileSystemService fileService)
		{
			_imageService = imageService;
			_fileNamingService = fileNamingService;
			_fileService = fileService;
        }

		public async Task<string> StoreImage(string base64String, string title, CancellationToken cancellationToken)
		{
            try
            {
                var bytes = _imageService.LoadImage(base64String).ToByteArray(ImageFormat.Png);
                return await SaveImageFile(bytes, title, ImageFormatInformation[ImageFormat.Png], cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

		public Task<string> StoreImage(byte[] content, string fileName, CancellationToken cancellationToken)
		{
			return SaveImageFile(content, fileName, GetImageFormatInfoFromFileName(fileName), cancellationToken);
		}

		private async Task<string> SaveImageFile(byte[] content, string title, ImageFormatInfo imageFormat, CancellationToken cancellationToken)
		{
			var fileName = _fileNamingService.GenerateFileName(title, imageFormat.Extension);
			var response = await _fileService.UploadFromByteArray(content, fileName, cancellationToken);
		
			return response;
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