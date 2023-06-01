using DevBlog.Application.Settings;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevBlog.Application.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly AppSettings _appSettings;

        public FileSystemService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<string> UploadFromByteArray(byte[] content, string fileName, CancellationToken cancellationToken)
        {
            await using var stream = new MemoryStream(content);

            return await UploadFromStream(stream, fileName, cancellationToken);
        }

        public async Task<string> UploadFromStream(Stream stream, string fileName, CancellationToken cancellationToken)
        {
            await using var fileStream = File.Create(_appSettings.FileStoragePath + fileName);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream, cancellationToken);

            return _appSettings.ImagePath + fileName;
        }
    }
}

