using System;
using Google.Cloud.Storage.V1;
using DevBlog.Application.Settings;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace DevBlog.Application.Services
{
	public class FileService : IFileService
	{
		private readonly StorageClient storageClient;
		private readonly AppSettings appSettings;

		public FileService(StorageClient storageClient, AppSettings appSettings)
		{
			this.storageClient = storageClient;
			this.appSettings = appSettings;
		}

		public async Task<Object> UploadFromByteArray(byte[] content, string fileName, string contentType, CancellationToken cancellationToken)
		{
			await using var stream = new MemoryStream(content);

			return await UploadFromStream(stream, fileName, contentType, cancellationToken);
		}

		public async Task<Object> UploadFromStream(Stream stream, string fileName, string contentType, CancellationToken cancellationToken)
		{
            return await storageClient.UploadObjectAsync(appSettings.BucketName, fileName, contentType, stream, new UploadObjectOptions(), cancellationToken);
        }
	}
}

