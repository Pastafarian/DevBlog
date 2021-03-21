using System.Threading;
using System.Threading.Tasks;

namespace DevBlog.Application.Services
{
	public interface IImageStorageService
	{
		Task<string> StoreImage(string base64String, string title, CancellationToken cancellationToken);
		Task<string> StoreImage(byte[] content, string fileName, CancellationToken cancellationToken);
	}
}