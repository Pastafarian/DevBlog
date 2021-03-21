using Google.Apis.Auth.OAuth2;

namespace DevBlog.Application.Factories
{
	public interface IGoogleCredentialFactory
	{
		GoogleCredential Build();
	}
}