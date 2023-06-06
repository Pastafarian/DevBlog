using System.Threading;
using System.Threading.Tasks;

namespace DevBlog.Application.Services;

public interface ISendGridService
{
    Task<bool> SendEmailAsync(string name, string email, string message, CancellationToken cancellationToken);
}