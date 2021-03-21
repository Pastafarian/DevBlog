using System;

namespace DevBlog.Domain.Entities
{
	public class Post : Entity
	{
		public string Title { get; set; }
		public string Slug { get; set; }
		public string Body { get; set; }
		public string HeaderImage { get; set; }
		public DateTime? PublishDate { get; set; }
		public int ReadMinutes { get; set; }
	}
}
