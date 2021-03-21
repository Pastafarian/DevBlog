using System;
using System.IO;
using Google.Apis.Auth.OAuth2;

namespace DevBlog.Application.Factories
{
	public class GoogleCredentialFactory : IGoogleCredentialFactory
	{
		public GoogleCredential Build()
		{
			return GoogleCredential.FromJson(GetGoogleCredentialJson());
		}

		public static string GetGoogleCredentialJson()
		{
			var path = AppDomain.CurrentDomain.BaseDirectory + "account-key.json";
			return File.ReadAllText(path);
		}
	}
}