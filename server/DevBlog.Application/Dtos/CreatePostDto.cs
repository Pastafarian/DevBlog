using System;

namespace DevBlog.Application.Dtos
{
	public class CreatePostDto
	{
		public string Title { get; set; }
		public string Body { get; set; }
		public string HeaderImage { get; set; }

		public string Slug
		{
			get => slug.ToLower();
			set => slug = value.ToLower();
		}
		
		private string slug;

		public DateTime PublishDate { get; set; }
	}
}