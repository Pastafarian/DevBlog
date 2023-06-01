//using Google.Cloud.Storage.V1;

//namespace DevBlog.Application.Factories
//{
//	public class StorageClientFactory : IStorageClientFactory
//	{
//		private readonly IGoogleCredentialFactory googleCredentialFactory;

//		public StorageClientFactory(IGoogleCredentialFactory googleCredentialFactory)
//		{
//			this.googleCredentialFactory = googleCredentialFactory;
//		}

//		public StorageClient Build()
//		{
//			var credential = googleCredentialFactory.Build();
//			return StorageClient.Create(credential);
//		}
//	}
//}