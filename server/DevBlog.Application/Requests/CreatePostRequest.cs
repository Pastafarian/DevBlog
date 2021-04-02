using System;

namespace DevBlog.Application.Requests
{
	public class CreatePostRequest
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public string HeaderImage { get; set; }

		public string Slug
		{
			get => slug.ToLower().Replace(" ", "_");
			set => slug = value;
		}
		
		private string slug;

		public DateTime PublishDate { get; set; }
	}
}