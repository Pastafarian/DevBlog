namespace DevBlog.Application.Response
{
	public class ImageUploadedResponseDto
	{
		public string ImageUrl { get; set; }

		public ImageUploadedResponseDto(string imageUrl)
		{
			ImageUrl = imageUrl;
		}
	}
}
