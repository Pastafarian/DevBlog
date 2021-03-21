using Google.Apis.Storage.v1.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DevBlog.Application.Services
{
	public interface IFileService
	{
		Task<Object> UploadFromByteArray(byte[] content, string fileName, string contentType, CancellationToken cancellationToken);
		Task<Object> UploadFromStream(Stream stream, string fileName, string contentType, CancellationToken cancellationToken);
	}
}